using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityTileMap;
using UnityEditor.SceneManagement;

// For obtaining list of sorting layers.
using UnityEditorInternal;
using System.Reflection;

[Serializable]
[CustomEditor(typeof(TileMapBehaviour))]
public class TileMapBehaviourInspector : Editor
{
    private const float FLOAT_PICKER_MARGIN = 24f;
    private const float FLOAT_ITEM_MARGIN = 6f;

    [SerializeField]
    private bool m_showSortSettings = true;

    [SerializeField]
    private bool m_showMapSettings = true;

    [SerializeField]
    private bool m_showSprites;

    [SerializeField]
    private bool m_showPickerSettings;

    [SerializeField]
    private bool m_sortSpritesByName;

    [SerializeField]
    private bool m_showAnims;

    private readonly Dictionary<string, Texture2D> m_thumbnailCache = new Dictionary<string, Texture2D>();

    private TileMapBehaviour m_tileMap;
    private TileSheet m_tileSheet;
    private TileAnim[] m_tileAnims;

    private TilesetType m_tileset;

    private TileEntry[] m_tileAnimsRoots;
    private int m_tileAnimsLen;
    private int[] m_tileAnimsLens;
    private int[] m_tileAnimsIntervals;
    private int[][] m_tileAnimsFrames;
    // array of arrays containing sprite indices

    private int m_tilesX;
    private int m_tilesY;
    private int m_tileResolution;
    private float m_tileSize;
    private MeshMode m_meshMode;
    private TextureFormat m_textureFormat;
    private FilterMode m_textureFilterMode;
    private bool m_activeInEditMode;

    private Vector3 m_mouseHitPos = -Vector3.one;
    private int m_setTileID = -1;
    private Rect m_tilePickerPosition = new Rect(0f, 21f, 256f, 320f);
    private Vector2 m_tilePickerScroll = Vector2.zero;
    private int m_tilePickerXCount = 4;

    private void OnEnable()
    {
        m_tileMap = (TileMapBehaviour)target;
        if (m_tileMap.TileSheet == null)
        {
            m_tileMap.SetTileSheet(CreateInstance<TileSheet>());
        }
        m_tileSheet = m_tileMap.TileSheet;
        m_tileAnims = m_tileMap.TileAnims;
        m_tileset = m_tileMap.tileset;

        var meshSettings = m_tileMap.MeshSettings;
        if (meshSettings != null)
        {
            m_tilesX = meshSettings.TilesX;
            m_tilesY = meshSettings.TilesY;
            m_tileResolution = meshSettings.TileResolution;
            m_tileSize = meshSettings.TileSize;
            m_meshMode = meshSettings.MeshMode;
            m_textureFormat = meshSettings.TextureFormat;
        }
        m_activeInEditMode = m_tileMap.ActiveInEditMode;
        if (!Application.isPlaying) EditorApplication.update += m_tileMap.SpecialUpdateAnim;
    }

    private void OnDisable()
    {
        if (!Application.isPlaying) EditorApplication.update -= m_tileMap.SpecialUpdateAnim;
    }

    // Taken from: http://answers.unity3d.com/questions/585108/how-do-you-access-sorting-layers-via-scripting.html
    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public override void OnInspectorGUI()
    {
        //		base.OnInspectorGUI();
        m_showMapSettings = EditorGUILayout.Foldout(m_showMapSettings, "Map Settings");
        if (m_showMapSettings)
        {
            m_activeInEditMode = EditorGUILayout.Toggle("Active In Edit Mode", m_activeInEditMode);

            EditorGUI.BeginChangeCheck();
            m_tileset = (TilesetType)EditorGUILayout.EnumPopup(new GUIContent("Tileset", "Tileset used by this map"), m_tileset);
            if (EditorGUI.EndChangeCheck() == true)
            {
                Undo.RecordObject(m_tileMap.gameObject, "Changed tilemap tileset");
                m_tileMap.tileset = m_tileset;
                LoadTileset();
            }

            m_tilesX = EditorGUILayout.IntField(
                new GUIContent("Tiles X", "The number of tiles on the X axis"),
                m_tilesX);
            m_tilesY = EditorGUILayout.IntField(
                new GUIContent("Tiles Y", "The number of tiles on the Y axis"),
                m_tilesY);
            m_tileResolution = EditorGUILayout.IntField(
                new GUIContent("Tile Resolution", "The number of pixels along each axis on one tile"),
                m_tileResolution);

            if (m_tileResolution != m_tileMap.MeshSettings.TileResolution)
            {
                EditorGUILayout.HelpBox("Changing tile resolution will clear the current tile setup.\n" +
                                        string.Format("Current tile resolution is {0}.", m_tileMap.MeshSettings.TileResolution), MessageType.Warning);
            }

            m_tileSize = EditorGUILayout.FloatField(
                new GUIContent("Tile Size", "The size of one tile in Unity units"),
                m_tileSize);
            m_meshMode = (MeshMode)EditorGUILayout.EnumPopup("Mesh Mode", m_meshMode);

            // these settings only apply to the single quad mode mesh
            if (m_meshMode == MeshMode.SingleQuad)
            {
                m_textureFormat = (TextureFormat)EditorGUILayout.EnumPopup("Texture Format", m_textureFormat);
                m_textureFilterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", m_textureFilterMode);
            }

            if (GUILayout.Button("Create/Recreate Mesh"))
            {
                bool canDelete = true;

                if (m_tileResolution != m_tileMap.MeshSettings.TileResolution)
                {
                    canDelete = ShowTileDeletionWarning();
                }

                if (canDelete)
                {
                    m_tileMap.MeshSettings = new TileMeshSettings(m_tilesX, m_tilesY, m_tileResolution, m_tileSize, m_meshMode, m_textureFormat);
                    
                    // if settings didnt change the mesh wouldnt be created, force creation
                    if (!m_tileMap.HasMesh)
                    {
                        m_tileMap.CreateMesh();
                    }
                    
                    m_activeInEditMode = true;
                }
            }
            if (GUILayout.Button("Destroy Mesh (keep data)"))
            {
                m_tileMap.DestroyMesh();

                m_activeInEditMode = false;
            }

            if (GUILayout.Button("Clear"))
            {
                if (ShowTileDeletionWarning())
                {
                    m_tileMap.DestroyMesh();
                    m_tileMap.ClearTiles();
                    
                    m_activeInEditMode = false;
                }
            }

            if (m_tileMap.ActiveInEditMode != m_activeInEditMode)
            {
                m_tileMap.ActiveInEditMode = m_activeInEditMode;
                OnSceneGUI(); // force redraw map editor box
            }
        }

        m_showSortSettings = EditorGUILayout.Foldout(m_showSortSettings, "Sort Settings");
        if (m_showSortSettings)
        {
            TileMeshBehaviour mesh = m_tileMap.GetComponentInChildren<TileMeshBehaviour>();

            // When first creating tile map or after deleting the mesh,
            // it will not be accessible until it's created by user.
            if (mesh != null && mesh.GetComponent<Renderer>() != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();

                string[] sortingLayers = GetSortingLayerNames();

                int currentLayer = 0;

                bool isLayerSet = mesh.GetComponent<Renderer>().sortingLayerName.Length > 0;

                if (! isLayerSet)
                {
                    currentLayer = FindStringIndex(ref sortingLayers, "Default");
                }
                else
                {
                    currentLayer = FindStringIndex(ref sortingLayers, mesh.GetComponent<Renderer>().sortingLayerName);
                }

                int chosenLayer = EditorGUILayout.Popup("Sorting Layer Name", Mathf.Max(currentLayer, 0), sortingLayers);

                if (EditorGUI.EndChangeCheck() || ! isLayerSet)
                {
                    mesh.GetComponent<Renderer>().sortingLayerName = sortingLayers[chosenLayer];
                }
                
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                
                int order = EditorGUILayout.IntField("Sorting Order", mesh.GetComponent<Renderer>().sortingOrder);
                
                if (EditorGUI.EndChangeCheck())
                {
                    mesh.GetComponent<Renderer>().sortingOrder = order;
                }
                
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox("Mesh has not been created.\nPlease use 'Create/Recreate Mesh' button in the Map Settings.", MessageType.Info, true);
            }
        }

        m_showPickerSettings = EditorGUILayout.Foldout(m_showPickerSettings, "Tile Picker Settings");
        if (m_showPickerSettings)
        {
            m_tilePickerXCount = Mathf.Clamp(EditorGUILayout.IntField(
                new GUIContent("Items Per Row", "The number of items to draw in a row in the Tile Picker Window."),
                m_tilePickerXCount), 1, int.MaxValue);

            m_tilePickerPosition.width = Mathf.Clamp(EditorGUILayout.FloatField(
                new GUIContent("Picker Width", "The width of the Tile Picker Window."),
                m_tilePickerPosition.width), 64f, float.MaxValue);

            m_tilePickerPosition.height = Mathf.Clamp(EditorGUILayout.FloatField(
                new GUIContent("Picker Height", "The height of the Tile Picker Window."),
                m_tilePickerPosition.height), 128f, float.MaxValue);
        }

        bool prominentImportArea = m_tileSheet.Ids.Count() == 0;
        m_showSprites = EditorGUILayout.Foldout(m_showSprites || prominentImportArea, "Sprites");
        if (m_showSprites || prominentImportArea)
        {
            ShowImportDropArea();
        }
        if (m_showSprites && !prominentImportArea)
        {
            if (GUILayout.Button("Delete all"))
            {
                foreach (var id in m_tileMap.TileSheet.Ids)
                    m_tileMap.TileSheet.Remove(id);
                m_thumbnailCache.Clear();
                SaveTileset();
            }

            if (GUILayout.Button("Refresh thumbnails"))
                m_thumbnailCache.Clear();

            m_sortSpritesByName = GUILayout.Toggle(m_sortSpritesByName, "Sort sprites by name");

            var ids = m_tileSheet.Ids.ToList();
            ids.Sort();
            for (int i = 0; i < ids.Count; i++)
            {
                var sprite = m_tileSheet.Get(ids[i]);
                ShowSprite(sprite);

                // add separators, except below the last one
                // could probably find a better looking one
                if (i < (ids.Count - 1))
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }
        }
        EditorGUI.BeginChangeCheck();
        m_showAnims = EditorGUILayout.Foldout(m_showAnims, "Tile Animations");
        if (m_showAnims)
        {
            if (m_tileAnims == null)
            {
                if (m_tileMap.TileAnims != null)
                {
                    m_tileAnims = m_tileMap.TileAnims;
                }
                else
                {
                    m_tileAnims = new TileAnim[m_tileAnimsLen];
                }
            }
            m_tileAnimsLen = EditorGUILayout.IntField(new GUIContent("No. of tile anims", "Number of animated tiles tied to this tilemap."), m_tileAnims.Length);
            // populate lens
            m_tileAnimsLens = new int[m_tileAnimsLen];
            // populate intervals
            m_tileAnimsIntervals = new int[m_tileAnimsLen];
            for (int i = 0; i < m_tileAnimsLen; i++)
            {
                if (i < m_tileAnims.Length && m_tileAnims[i] != null)
                {
                    m_tileAnimsIntervals[i] = m_tileAnims[i].frameInterval;
                }
                else
                {
                    m_tileAnimsIntervals[i] = 0;
                }
            }
            // populate roots
            m_tileAnimsRoots = new TileEntry[m_tileAnimsLen];
            for (int i = 0; i < m_tileAnimsLen; i++)
            {
                if ( i < m_tileAnims.Length && m_tileAnims[i] != null)
                {
                    m_tileAnimsRoots[i] = m_tileSheet.GetEntry(m_tileAnims[i].Id);
                }
                else
                {
                    m_tileAnimsRoots[i] = m_tileSheet.GetEntry(0);
                }
            }
            // populate frames
            m_tileAnimsFrames = new int[m_tileAnimsLen][];
            for (int i = 0; i < m_tileAnimsLen; i++)
            {
                if (i < m_tileAnims.Length && m_tileAnims[i] != null)
                {
                    m_tileAnimsFrames[i] = m_tileAnims[i].frameIds;
                }
                else
                {
                    m_tileAnimsFrames[i] = new int[0];
                }
                m_tileAnimsLens[i] = m_tileAnimsFrames[i].Length;
            }
            // start tile anims block
            EditorGUILayout.LabelField("______________________________________________________________________________________");
            for (int i = 0; i < m_tileAnimsLen; i++)
            {
                EditorGUILayout.LabelField("Anim " + i.ToString());
                m_tileAnimsLens[i] = EditorGUILayout.IntField(new GUIContent("Anim " + i.ToString() + " length", "Number of frames in this animation."), m_tileAnimsLens[i]);
                int[] mt = m_tileAnimsFrames[i];
                m_tileAnimsFrames[i] = new int[m_tileAnimsLens[i]];
                for (int ix = 0; ix < m_tileAnimsFrames[i].Length; ix++)
                {
                    if (ix >= mt.Length)
                    {
                        m_tileAnimsFrames[i][ix] = 0;
                    }
                    else
                    {
                        m_tileAnimsFrames[i][ix] = mt[ix];
                    }
                }
                m_tileAnimsIntervals[i] = EditorGUILayout.IntField(new GUIContent("Anim " + i.ToString() + " interval", "Number of frames between sprite changes."), m_tileAnimsIntervals[i]);
                // root
                if (m_tileAnimsRoots[i] == null)
                {
                    m_tileAnimsRoots[i] = m_tileSheet.GetEntry(0);
                }
                Sprite s = m_tileAnimsRoots[i].Sprite;
                GUILayout.Label(new GUIContent(s.name, GetThumbnail(s), s.textureRect.ToString()));
                m_tileAnimsRoots[i] = m_tileSheet.GetEntry(EditorGUILayout.IntField(new GUIContent("Root tile ID: " + m_tileAnimsRoots[i].Id.ToString(), "Tilesheet ID of anim root"), m_tileAnimsRoots[i].Id));
                // the other sprites
                for (int ix = 0; ix < m_tileAnimsFrames[i].Length; ix++)
                {
                    s = m_tileSheet.Get(m_tileAnimsFrames[i][ix]);
                    GUILayout.Label(new GUIContent(GetThumbnail(s), s.textureRect.ToString()));
                    m_tileAnimsFrames[i][ix] = EditorGUILayout.IntField(m_tileAnimsFrames[i][ix], GUILayout.Width(32));
                }
                EditorGUILayout.LabelField("______________________________________________________________________________________");
            }
            m_tileAnims = new TileAnim[m_tileAnimsLen];
            for (int i = 0; i < m_tileAnimsLen; i++)
            {
                m_tileAnims[i] = new TileAnim();
                m_tileAnims[i].Id = m_tileAnimsRoots[i].Id;
                m_tileAnims[i].frameInterval = m_tileAnimsIntervals[i];
                m_tileAnims[i].frameIds = m_tileAnimsFrames[i];
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_tileMap, "Changed tileanims");
            m_tileMap.SetAnims(m_tileAnims);
            SaveTileset();
        }
    }

    private int FindStringIndex(ref string[] strings, string layer)
    {
        return Array.IndexOf(strings, layer);
    }

    private bool ShowTileDeletionWarning()
    {
        return EditorUtility.DisplayDialog("Really delete?", 
                                           String.Format("{0} manually set tile(s) will be removed.", m_tileMap.TileCount), 
                                           "Yes", "No");
    }

    public void ShowSprite(Sprite sprite)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent(sprite.name, GetThumbnail(sprite), sprite.textureRect.ToString()));

        GUILayout.FlexibleSpace();

        // TODO would be nice to vertically center this button when larger sprites are used
        if (GUILayout.Button("Remove"))
            m_tileSheet.Remove(m_tileSheet.Lookup(sprite.name));

        EditorGUILayout.EndHorizontal();
    }

    public void ShowImportDropArea()
    {
        Event evt = Event.current;
        Rect rect = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(rect, "Drag and drop Texture/Sprite here");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!rect.Contains(evt.mousePosition))
                    return;
                //			if (evt.type != EventType.DragPerform)
                //				return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                //			DragAndDrop.AcceptDrag();

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    // I dont know of a way to multiselect stuff in the asset view
                    // to drag and drop, so assume only one
                    var dropped = DragAndDrop.objectReferences.FirstOrDefault();
                    TryImport(dropped);
                    
                    Event.current.Use();
                    SaveTileset();
                }
                break;
        }
    }

    private void TryImport(object obj)
    {
        var texture = obj as Texture2D;
        var sprite = obj as Sprite;
        if (texture != null)
            ImportTexture(texture);
        else if (sprite != null)
            ImportSprite(sprite);
    }

    private void ImportTexture(Texture2D texture)
    {
        if (!IsTextureReadable(texture))
        {
            Debug.LogError(string.Format("Texture '{0}' must be marked as readable", texture.name));
            return;
        }
        var assetPath = AssetDatabase.GetAssetPath(texture);
        var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        var sprites = assets.OfType<Sprite>().ToList();
        if (sprites.Count > 0)
        {
            foreach (var sprite in sprites)
            {
                if (m_tileSheet.Contains(sprite.name))
                    continue;
                m_tileSheet.Add(sprite);
            }
            Debug.Log(string.Format("{0} sprites loaded from {1}", sprites.Count, assetPath));
        }
        else
        {
            Debug.LogWarning(string.Format("No sprites found on asset path: {0}", assetPath));
        }
    }

    private void ImportSprite(Sprite sprite)
    {
        if (!IsTextureReadable(sprite.texture))
        {
            Debug.LogError(string.Format("Texture '{0}' must be marked as readable", sprite.texture.name));
            return;
        }
        if (m_tileSheet.Contains(sprite.name))
            Debug.LogError(string.Format("TileSheet already contains a sprite named {0}", sprite.name));
        else
            m_tileSheet.Add(sprite);
    }

    private Texture GetThumbnail(Sprite sprite)
    {
        Texture2D texture = null;
        if (!m_thumbnailCache.TryGetValue(sprite.name, out texture))
        {
            var rect = sprite.textureRect;
            texture = new Texture2D((int)rect.width, (int)rect.height, m_textureFormat, false);
            texture.SetPixels(sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height));
            texture.Apply(false, true);

            m_thumbnailCache[sprite.name] = texture;
        }
        return texture;
    }

    private static bool IsTextureReadable(Texture2D texture)
    {
        var path = AssetDatabase.GetAssetPath(texture);
        var textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
        return textureImporter.isReadable;
    }

    private void OnSceneGUI()
    {
        TileMeshBehaviour mesh = m_tileMap.GetComponentInChildren<TileMeshBehaviour>();

        if (mesh != null)
        {
            m_tileMap.GetComponentInChildren<TileMeshBehaviour>().transform.localPosition = Vector3.zero;
        }

        // TODO the scene gui should only be enabled if m_tileMap.ActiveInEditMode, but I havent found a good way to toggle
        //if (!m_tileMap.ActiveInEditMode)
        //    return;

        // TODO ive found the EditorWindow base class which is probably a better approach to show this
        // http://docs.unity3d.com/ScriptReference/EditorWindow.html
        m_tilePickerPosition = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), m_tilePickerPosition,
                                                OnTilePickerWindow, new GUIContent("Select a Tile"));
        DrawGrid();

        if (m_activeInEditMode)
        {
            HandleMouseEvents();
        }
    }

    private void OnTilePickerWindow(int id)
    {
        int[] ids = m_tileSheet.Ids.ToArray();

        if (ids.Length > 0)
        {
            m_tilePickerScroll = EditorGUILayout.BeginScrollView(m_tilePickerScroll, GUIStyle.none, GUI.skin.verticalScrollbar);
            {
                float itemSize = ((m_tilePickerPosition.width - FLOAT_PICKER_MARGIN) / m_tilePickerXCount) - FLOAT_ITEM_MARGIN;
                for (int i = 0; i < ids.Length; i += m_tilePickerXCount)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int c = 0; c < m_tilePickerXCount; c++)
                    {
                        if (i + c >= ids.Length)
                            break;
                        
                        int index = i + c;
                        GUIContent content = new GUIContent(GetThumbnail(m_tileSheet.Get(ids[index])));
                        
                        if (GUILayout.Toggle(m_setTileID == ids[index], content, GUI.skin.button, GUILayout.Width(itemSize), GUILayout.Height(itemSize)))
                            m_setTileID = ids[index];
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();

            if (m_setTileID < 0)
            {
                EditorGUILayout.HelpBox("No tile selected.", MessageType.Warning);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No tiles available.\nPlease use 'Sprites' in the inspector to add tiles.", MessageType.Info);
        }

        //GUI.DragWindow(new Rect(0f, 0f, m_tilePickerPosition.width, 21f));
        GUI.DragWindow(new Rect(0f, 0f, m_tilePickerPosition.width, m_tilePickerPosition.height));
    }

    private void DrawGrid()
    {
        float gridWidth = m_tilesX * m_tileSize;
        float gridHeight = m_tilesY * m_tileSize;

        Vector2 position = m_tileMap.gameObject.transform.position;

        Handles.color = Color.gray;
        for (float i = 1; i < gridWidth; i+=m_tileSize)
        {
            Handles.DrawLine(new Vector3(i + position.x, position.y), new Vector3(i + position.x, gridHeight + position.y));
        }
        for (float i = 1; i < gridHeight; i += m_tileSize)
        {
            Handles.DrawLine(new Vector3(position.x, i + position.y), new Vector3(gridWidth + position.x, i + position.y));
        }
        Handles.color = Color.white;
        Handles.DrawLine(position, new Vector3(gridWidth + position.x, position.y));
        Handles.DrawLine(position, new Vector3(position.x, gridHeight + position.y));
        Handles.DrawLine(new Vector3(gridWidth + position.x, position.y), new Vector3(gridWidth + position.x, gridHeight + position.y));
        Handles.DrawLine(new Vector3(position.x, gridHeight + position.y), new Vector3(gridWidth + position.x, gridHeight + position.y));
    }

    private void HandleMouseEvents()
    {
        // Do not process if no tile is selected
        if (m_setTileID < 0)
        {
            return;
        }

        Event e = Event.current;
        if ((e.type == EventType.MouseMove && e.modifiers == EventModifiers.Shift) || (e.type == EventType.MouseDown && e.button == 1))
        {
            if (UpdateMouseHit())
            {
                int tileX = Mathf.FloorToInt(m_mouseHitPos.x / (m_tileSize * m_tileResolution));
                int tileY = Mathf.FloorToInt(m_mouseHitPos.y / (m_tileSize * m_tileResolution));
                m_tileMap[tileX, tileY] = m_setTileID;

                e.Use();
            }
        }
    }

    private void LoadTileset ()
    {
        TilesetAnimsContainer animsContainer;
        TileSheet tileSheet;
        switch (m_tileMap.tileset)
        {
            case TilesetType.Village:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_village");
                if (animsContainer == null) goto default;
                break;
            case TilesetType.Forest:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_forest");
                if (animsContainer == null) goto default;
                break;
            case TilesetType.Valley:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_valley");
                if (animsContainer == null) goto default;
                break;
            case TilesetType.Marina:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_marina");
                if (animsContainer == null) goto default;
                break;
            case TilesetType.Mandala:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_mandala");
                if (animsContainer == null) goto default;
                break;
            case TilesetType.Tower:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_tower");
                if (animsContainer == null) goto default;
                break;
            case TilesetType.Tutorial:
                animsContainer = Resources.Load<TilesetAnimsContainer>("GFX/Tilesets/Metadata/anims_tutorial");
                if (animsContainer == null) goto default;
                break;
            default:
                animsContainer = CreateInstance<TilesetAnimsContainer>();
                break;
        }
        switch (m_tileMap.tileset)
        {
            case TilesetType.Village:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_village");
                if (tileSheet == null) goto default;
                break;
            case TilesetType.Forest:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_forest");
                if (tileSheet == null) goto default;
                break;
            case TilesetType.Valley:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_valley");
                if (tileSheet == null) goto default;
                break;
            case TilesetType.Marina:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_marina");
                if (tileSheet == null) goto default;
                break;
            case TilesetType.Mandala:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_mandala");
                if (tileSheet == null) goto default;
                break;
            case TilesetType.Tower:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_tower");
                if (tileSheet == null) goto default;
                break;
            case TilesetType.Tutorial:
                tileSheet = Resources.Load<TileSheet>("GFX/Tilesets/Metadata/tiles_tutorial");
                if (tileSheet == null) goto default;
                break;
            default:
                tileSheet = CreateInstance<TileSheet>();
                break;
        }
        if (animsContainer.tileAnims != null)
        {
            m_tileMap.SetAnims(animsContainer.tileAnims);
            m_tileMap.SetTileSheet(tileSheet);
            m_tileMap.CreateMesh();
            EditorSceneManager.MarkSceneDirty(m_tileMap.gameObject.scene);
        }
    }

    private void SaveTileset ()
    {
        TilesetAnimsContainer tp = CreateInstance<TilesetAnimsContainer>();
        tp.tileAnims = m_tileMap.TileAnims;
        string animsPath = "Assets/Resources/GFX/Tilesets/Metadata/anims_unknown.asset";
        string sheetPath = "Assets/Resources/GFX/Tilesets/Metadata/tiles_unknown.asset";
        switch (m_tileMap.tileset)
        {
            case TilesetType.Village:
                animsPath = "Assets/Resources/GFX/Tilesets/Metadata/anims_village.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/Metadata/tiles_village.asset";
                break;
            case TilesetType.Forest:
                animsPath = "Assets/Resources/GFX/Tilesets/Metadata/anims_forest.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/Metadata/tiles_forest.asset";
                break;
            case TilesetType.Valley:
                animsPath = "Assets/Resources/GFX/Tilesets/Metadata/anims_valley.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/Metadata/tiles_valley.asset";
                break;
            case TilesetType.Marina:
                animsPath = "Assets/Resources/GFX/Tilesets/Metadata/anims_marina.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/Metadata/tiles_marina.asset";
                break;
            case TilesetType.Mandala:
                animsPath = "Assets/Resources/GFX/Tilesets/Metadata/anims_mandala.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/Metadata/tiles_mandala.asset";
                break;
            case TilesetType.Tower:
                animsPath = "Assets/Resources/GFX/Tilesets/anims_tower.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/tiles_tower.asset";
                break;
            case TilesetType.Tutorial:
                animsPath = "Assets/Resources/GFX/Tilesets/anims_tutorial.asset";
                sheetPath = "Assets/Resources/GFX/Tilesets/tiles_tutorial.asset";
                break;
        }
        if (AssetDatabase.LoadAssetAtPath(sheetPath, typeof(TileSheet)) == null)
        {
            AssetDatabase.CreateAsset(m_tileMap.TileSheet, sheetPath);
        }
        AssetDatabase.CreateAsset(tp, animsPath);
        AssetDatabase.SaveAssets();
    }

    private bool UpdateMouseHit()
    {
        var p = new Plane(m_tileMap.transform.TransformDirection(Vector3.forward), m_tileMap.transform.position);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        Vector3 hit = new Vector3();
        float distance;
        if (p.Raycast(ray, out distance))
            hit = ray.origin + (ray.direction.normalized * distance);

        m_mouseHitPos = m_tileMap.transform.InverseTransformPoint(hit);
        m_mouseHitPos = new Vector3(m_mouseHitPos.x * m_tileSize, m_mouseHitPos.y * m_tileSize, m_mouseHitPos.z);

        return (m_mouseHitPos.x >= 0 && m_mouseHitPos.x < (m_tilesX * m_tileSize * m_tileResolution) &&
                m_mouseHitPos.y >= 0 && m_mouseHitPos.y < (m_tilesY * m_tileSize * m_tileResolution));
    }
}
