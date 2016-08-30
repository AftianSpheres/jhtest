using UnityEngine;
using System.Collections;

namespace TextMonger
{
    /// <summary>
    /// 
    /// </summary>
    public enum TextCanvasPrintMode
    {
        AllAtOnce,
        PrintCanvasPixels,
        PrintCanvasPixelsVertically,
        PrintCharacters,
        PrintCharacterPixels,
    }

    enum TextCanvasInputState
    {
        None,
        Waiting,
        Received
    }

    /// <summary>
    /// 
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TextCanvas : MonoBehaviour
    {
        public bool finishedProcessing { get; private set; } // this is set when we've finished displaying the text we were given - you can close the TextCanvas and return control to the player once it's true
        public bool isOpen { get; private set; }
        [SerializeField]
        private Color bgColor;
        private Color currentFontColor;
        [SerializeField]
        private BitmapFont font;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        private Texture2D texture;
        [SerializeField]
        private Texture2D bgTexture;
        private TextBlock[] blocks;
        private TextCanvasInputState inputState;
        [SerializeField]
        private TextCanvasPrintMode printMode;
        [SerializeField]
        private bool autoScroll;
        private bool dirty;
        [SerializeField]
        private bool openByDefault;
        private bool printing;
        [SerializeField]
        private bool printLeftToRight = true;
        [SerializeField]
        private bool printUpToDown = true;
        [SerializeField]
        private bool updateIsFrameDependent;
        private bool waitingForInput;
        [SerializeField]
        private float tickLength;
        private float time;
        [SerializeField]
        private int bottomMargin;
        [SerializeField]
        private int characterSpacing;
        private int currentBlockIndex;
        private int currentLineIndex;
        [SerializeField]
        private int eventsPerTick;
        [SerializeField]
        private int height;
        [SerializeField]
        private int leftMargin;
        [SerializeField]
        private int linesOnCanvas;
        [SerializeField]
        private int lineSpacing;
        private int permittedScrollLines = 0;
        private int printingTicks;
        [SerializeField]
        private int rightMargin;
        [SerializeField]
        private int scrollSpeed;
        [SerializeField]
        private int topMargin;
        [SerializeField]
        private int width;
        [SerializeField]
        private string defaultValue;

        void Awake ()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (font == null) font = ScriptableObject.CreateInstance<BitmapFont>();
            CreateTexture();
            texture.Apply();
            dirty = false;
        }

        void Start ()
        {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
            if (defaultValue != string.Empty)
            {
                if (openByDefault) ParseAndOpenOn(defaultValue);
                else ParseButDontOpen(defaultValue);
            }
            else if (Application.isPlaying)
            {
                if (openByDefault) OpenOnLastContentsIfAny();
                else Close();
            }
        }

        void Update ()
        {
            if (isOpen)
            {
                if (dirty)
                {
                    texture.Apply();
                    dirty = false;
                }
                if (printing)
                {
                    if (updateIsFrameDependent) printingTicks++;
                    else if (Time.realtimeSinceStartup - time >= (1.0f / 60.0f))
                    {
                        printingTicks += Mathf.RoundToInt((Time.realtimeSinceStartup - time) / (1.0f / 60.0f));
                        time = Time.realtimeSinceStartup;
                    }
                }
            }

        }

        /// <summary>
        /// Draws the background on all pixels within the texture's interior area.
        /// </summary>
        void ClearTexture ()
        {
            for (int x = leftMargin; x < width - rightMargin; x++)
            {
                for (int y = bottomMargin; y < height - topMargin; y++)
                {
                    DrawBGPixelAt(x, y);
                }
            }
        }

        /// <summary>
        /// Initializes TextCanvas.texture and draws the margins.
        /// </summary>
        void CreateTexture ()
        {
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            DrawMargins();
            ClearTexture();
        }

        /// <summary>
        /// Draws the background at specified texture coordinates.
        /// </summary>
        void DrawBGPixelAt(int x, int y)
        {
            Color c;
            if (bgTexture != null && x < bgTexture.width && y < bgTexture.height) c = bgTexture.GetPixel(x, y);
            else c = bgColor;
            texture.SetPixel(x, y, c);
            dirty = true;
        }

        /// <summary>
        /// Draws the specified character at the specified coordinates.
        /// </summary>
        void DrawCharacterAt (char _char, int _x, int _y, Color fontColor)
        {
            Sprite s = font.GetCharacter(_char).sprite;
            DrawCharacterAt(s, _x, _y, fontColor);
        }

        /// <summary>
        /// Draws character represented by given sprite to specified coordinates.
        /// </summary>
        void DrawCharacterAt(Sprite s, int _x, int _y, Color fontColor)
        {
            for (int x = 0; x + _x < width - rightMargin && x < s.texture.width; x++)
            {
                DrawCharacterColumnAt(s, x, _x, _y, fontColor);
            }
        }

        /// <summary>
        /// Draws specified column of pixels from given character to specified coordinates.
        /// </summary>
        void DrawCharacterColumnAt (char _char, int column, int _x, int _y, Color fontColor)
        {
            Sprite s = font.GetCharacter(_char).sprite;
            DrawCharacterColumnAt(s, column, _x, _y, fontColor);
        }

        /// <summary>
        /// Draws specified column of pixels from character represented by given sprite to specified coordinates.
        /// </summary>
        void DrawCharacterColumnAt (Sprite s, int column, int _x, int _y, Color fontColor)
        {
            for (int y = 0; y + _y < height - topMargin && y < s.texture.height; y++)
            {
                if (s == null || s.texture.GetPixel(column, y) == Color.clear) DrawBGPixelAt(column + _x, y + _y);
                else texture.SetPixel(column + _x, y + _y, fontColor * s.texture.GetPixel(column, y));
            }
            dirty = true;
        }
        
        /// <summary>
        /// Draws small space between characters at specified coodinates.
        /// </summary>
        void DrawCharacterGapAt (int x, int _height)
        {
            for (int n = x; n < x + characterSpacing; n++)
            {
                DrawEmptyColumnAt(n, _height);
            }
        }

        /// <summary>
        /// Draws specified row of pixels from given character to specified coodinates.
        /// </summary>
        void DrawCharacterRowAt (char _char, int row, int _x, int _y, Color fontColor)
        {
            Sprite s = font.GetCharacter(_char).sprite;
            DrawCharacterRowAt(s, row, _x, _y, fontColor);
        }

        /// <summary>
        /// Draws specified row of pixels from character represented by given sprite to specified coordinates.
        /// </summary>
        void DrawCharacterRowAt (Sprite s, int row, int _x, int _y, Color fontColor)
        {
            for (int x = 0; x + _x < width - rightMargin && x < s.texture.width; x++)
            {
                if (s == null || s.texture.GetPixel(x, row) == Color.clear) DrawBGPixelAt(x + _x, row + _y);
                else texture.SetPixel(x + _x, row + _y, fontColor * s.texture.GetPixel(x, row));
            }
            dirty = true;
        }

        /// <summary>
        /// Draws a single-pixel empty colum of character height at specified coordinates.
        /// </summary>
        void DrawEmptyColumnAt (int x, int _height)
        {
            for (int y = _height; y < _height + font.spaceHeight; y++)
            {
                DrawBGPixelAt(x, y);
            }
        }

        /// <summary>
        /// Draws sprite s onto the canvas, starting at the specified coorinates.
        /// </summary>
        void DrawExternalSpriteAt (Sprite s, int _x, int _y)
        {
            for (int x = 0; x + _x < width - rightMargin && x < s.texture.width; x++)
            {
                for (int y = 0; y + _y < height - topMargin && y < s.texture.height; y++)
                {
                    if (s == null || s.texture.GetPixel(x, y) == Color.clear) DrawBGPixelAt(x + _x, y + _y);
                    else texture.SetPixel(x + _x, y + _y, s.texture.GetPixel(x, y));
                }
            }
            dirty = true;
        }

        /// <summary>
        /// Draws horizontal line of text onto the canvas at specified local height.
        /// </summary>
        void DrawLine (TextLine line, int _height)
        {
            Color currentFontColor = line.lineStartColor;
            int c = 0;
            int x = leftMargin;
            while (x < width - rightMargin)
            {
                if (c < line.text.Length)
                {
                    Sprite s = font.GetCharacter(line.text[c]).sprite;
                    if (s != null)
                    {
                        DrawCharacterAt(s, x, _height, currentFontColor);
                        x += s.texture.width;
                        DrawCharacterGapAt(x, _height);
                        x += characterSpacing;
                    }
                    else
                    {
                        DrawSpaceAt(x, _height);
                        x += font.spaceLength;
                    }
                }
                else
                {
                    x++;
                }
            }
        }

        /// <summary>
        /// Draws margin between lines onto the canvas at specified local height.
        /// </summary>
        void DrawLineMargin(int _height)
        {
            for (int x = leftMargin; x < width - rightMargin; x++)
            {
                for (int y = _height; y < _height + lineSpacing && y < height - topMargin; y++)
                {
                    DrawBGPixelAt(x, y);
                }
            }
        }

        /// <summary>
        /// Draws the background at the canvas margins.
        /// This should only ever be called once per canvas - no text is ever drawn at the margins; they don't need to be cleared.
        /// </summary>
        void DrawMargins ()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < bottomMargin; y++) DrawBGPixelAt(x, y);
                for (int y = height - topMargin; y < height; y++) DrawBGPixelAt(x, y);
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < leftMargin; x++) DrawBGPixelAt(x, y);
                for (int x = width - rightMargin; x < width; x++) DrawBGPixelAt(x, y);
            }
            dirty = true;
        }

        /// <summary>
        /// Draws a standard space at the specified coordinates.
        /// </summary>
        void DrawSpaceAt (int x, int _height)
        {
            for (int n = x; n < x + font.spaceLength; n++)
            {
                DrawEmptyColumnAt(n, _height);
            }
        }

        void FinishBlock ()
        {
            currentBlockIndex++;
            if (currentBlockIndex >= blocks.Length) finishedProcessing = true;
        }

        /// <summary>
        /// Internal function belonging to PrintLine_Horizontal: prints single character and updates coroutine state.
        /// </summary>
        void _PrintLine_Horizontal_Character(ref TextLine line, ref Sprite s, ref int c, ref int x, ref int _height, bool negative = false)
        {
            int m;
            if (negative) m = -1;
            else m = 1;
            s = font.GetCharacter(line.text[c]).sprite;
            if (s != null)
            {
                DrawCharacterAt(s, x, _height, currentFontColor);
                x += s.texture.width * m;
                DrawCharacterGapAt(x, _height);
                x += characterSpacing * m;
            }
            else
            {
                DrawSpaceAt(x, _height);
                x += font.spaceLength * m;
            }
            c++;
        }

        /// <summary>
        /// Internal function belonging to PrintLine_Horizontal: prints single column of line and updates coroutine state.
        /// </summary>
        void _PrintLine_Horizontal_Column(ref TextLine line, ref Sprite s, ref int column, ref int c, ref int x, ref int _height, bool negative = false)
        {
            int m;
            if (negative) m = -1;
            else m = 1;
            if (s != null)
            {
                if (column < s.texture.width && column > -1) DrawCharacterColumnAt(s, column, x, _height, currentFontColor);
                else if (column < s.texture.width + characterSpacing) DrawEmptyColumnAt(x, _height);
                else
                {
                    c++;
                    if (c < line.text.Length)
                    {
                        s = font.GetCharacter(line.text[c]).sprite;
                        if (printLeftToRight) column = 0;
                        else
                        {
                            if (s != null) column = s.texture.width - 1 + characterSpacing;
                            else column = font.spaceLength - 1 + characterSpacing;
                        }
                    }
                }
            }
            else
            {
                if (column < font.spaceLength + characterSpacing && column > -1) DrawEmptyColumnAt(x, _height);
                else
                {
                    c++;
                    if (c < line.text.Length)
                    {
                        s = font.GetCharacter(line.text[c]).sprite;
                        if (printLeftToRight) column = 0;
                        else
                        {
                            if (s != null) column = s.texture.width - 1 + characterSpacing;
                            else column = font.spaceLength - 1 + characterSpacing;
                        }
                    }
                }
            }
            x += m;
            column += m;
        }

        IEnumerator PrintSingleCanvasByLines_Horizontal()
        {
            for (int l = 0; l < linesOnCanvas; l++)
            {
                yield return StartCoroutine(PrintLine_Horizontal(blocks[currentBlockIndex].lines[currentLineIndex], topMargin - font.spaceHeight - (l * (font.spaceHeight + lineSpacing))));
                DrawLineMargin(topMargin - ((font.spaceHeight + lineSpacing) * (l + 1)));
                currentLineIndex++;
                if (currentLineIndex >= blocks[currentBlockIndex].lines.Length)
                {
                    FinishBlock();
                    break;
                }
            }
        }

        /// <summary>
        /// Coroutine: prints a single line horizontally in accordance with canvas settings.
        /// </summary>
        IEnumerator PrintLine_Horizontal(TextLine line, int _height)
        {
            printing = true;
            currentFontColor = line.lineStartColor;
            int x;
            int column;
            Sprite s = font.GetCharacter(line.text[0]).sprite;
            if (printLeftToRight)
            {
                x = leftMargin;
                column = 0;
            }
            else
            {
                x = rightMargin;
                if (s != null) column = s.texture.width - 1 + characterSpacing;
                else column = font.spaceLength - 1 + characterSpacing;
            }
            int c = 0;
            int ticker = 0;
            while (printing)
            {
                if (line.colorChanges.ContainsKey(c)) currentFontColor = line.colorChanges[c];
                while (printingTicks > ticker)
                {
                    for (int n = 0; n < eventsPerTick; n++)
                    {
                        bool outOfRange;
                        if (printLeftToRight) outOfRange = x >= width - rightMargin;
                        else outOfRange = x <= leftMargin;
                        if (outOfRange)
                        {
                            printing = false;
                            break;
                        }
                        else if (c < line.text.Length)
                        {
                            if (printMode == TextCanvasPrintMode.PrintCharacters) _PrintLine_Horizontal_Character(ref line, ref s, ref c, ref x, ref _height, !printLeftToRight);
                            else _PrintLine_Horizontal_Column(ref line, ref s, ref column, ref c, ref x, ref _height, !printLeftToRight); // we treat canvas and character pixel-based printing the same way, and this method is never called for vertical printing
                        }
                        else
                        {
                            if (printLeftToRight) for (int i = x; i < width - rightMargin; i++) DrawEmptyColumnAt(i, _height);
                            else for (int i = x; i > leftMargin; i--) DrawEmptyColumnAt(i, _height);
                            printing = false;
                            break;
                        }
                    }
                    ticker++;
                    yield return null;
                }
                yield return null;
            }
            printingTicks = 0;
        }

        IEnumerator ProcessLines_Horizontal()
        {
            while(currentBlockIndex < blocks.Length)
            {
                yield return StartCoroutine(PrintSingleCanvasByLines_Horizontal());
                if (autoScroll) { }
                else yield return StartCoroutine(WaitForInput());
            }
        }

        /// <summary>
        /// Coroutine: draws and scrolls lines vertically out of the top of the TextCanvas.
        /// Use to bump lines you've already drawn up to clear space to print more text.
        /// </summary>
        IEnumerator ScrollDrawnLines(int lineCount, int scrollDist)
        {
            printing = true;
            int heightMod = 0;
            int ticker = 0;
            while (heightMod < scrollDist)
            {
                while (ticker < printingTicks)
                {
                    if (heightMod >= scrollDist) break;
                    for (int n = 0; n < scrollSpeed; n++)
                    {
                        if (heightMod >= scrollDist) break;
                        for (int i = 0; i < lineCount; i++)
                        {
                            ClearTexture();
                            if (currentLineIndex + i < blocks[currentBlockIndex].lines.Length)
                            {
                                DrawLine(blocks[currentBlockIndex].lines[currentLineIndex], topMargin - ((font.spaceHeight + lineSpacing) * i) + heightMod);
                                DrawLineMargin(topMargin - ((font.spaceHeight + lineSpacing) * i) - (lineSpacing + (i * lineSpacing)) + heightMod);
                            }
                        }
                        heightMod++;
                    }
                    ticker++;
                }
                yield return null;
            }
            printing = false;
        }

        /// <summary>
        /// Coroutine: sets the TextCanvas to waiting for input and only returns when it receives that input.
        /// </summary>
        IEnumerator WaitForInput ()
        {
            inputState = TextCanvasInputState.Waiting;
            while (inputState == TextCanvasInputState.Waiting)
            {
                yield return null;
            }
            inputState = TextCanvasInputState.None;
        }

        /// <summary>
        /// Tells the TextCanvas that it's okay
        /// to scroll down a certain number of lines,
        /// if not autoscrolling.
        /// </summary>
        public void AdvancePrint (int lines)
        {
            if (autoScroll == false)
            {
                permittedScrollLines = lines;
            }
        }

        /// <summary>
        /// Closes the TextCanvas, preventing it from updating or rendering.
        /// </summary>
        public void Close ()
        {
            inputState = TextCanvasInputState.None;
            spriteRenderer.enabled = false;
            isOpen = false;           
        }

        /// <summary>
        /// Opens the TextCanvas without updating its contents at all. Use to bring a prompt back onto the screen,
        /// or (if empty) to draw empty textboxes.
        /// </summary>
        public void OpenOnLastContentsIfAny()
        {
            spriteRenderer.enabled = true;
            isOpen = true;
        }

        public void ParseAndOpenOn (string s)
        {
            ParseButDontOpen(s);
            OpenOnLastContentsIfAny();
        }

        public void ParseButDontOpen (string s)
        {

        }

        /// <summary>
        /// Signals that the text canvas has received player input, if it's waiting for that.
        /// </summary>
        public void SendInput ()
        {
            if (inputState == TextCanvasInputState.Waiting) inputState = TextCanvasInputState.Received;
        }
    }
}