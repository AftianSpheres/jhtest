using UnityEngine;
using System;
using System.Collections.Generic;

public class HUDMainTextbox : MonoBehaviour
{
    public WorldController world;
    public MeshRenderer textMesh;
    public RetroPrinterScriptBasic Printer;
    public SpriteRenderer BG;
    public SpriteRenderer Cursor;
    public Queue<string> TextSeries;
    public float lowerY;
    public float higherY;
    private bool Active = false;
    private uint FrameCtr = 0;
    private float CursorInitialY;
	
    void Awake ()
    {
        CursorInitialY = Cursor.transform.localPosition.y;
        Close();
    }

	// Update is called once per frame
	void Update ()
    {
        if (Active == true)
        {
            if (world.HardwareInterfaceManager.Confirm.BtnDown == true && Printer.finishedTextBlock == true)
            {
                if (TextSeries.Count == 0)
                {
                    Close();
                }
                else
                {
                    PrintNextBlock();
                }
            }
        }
        if (FrameCtr % 60 == 0)
        {
            Cursor.transform.localPosition = new Vector3(Cursor.transform.localPosition.x, CursorInitialY + 1, Cursor.transform.localPosition.z);
        }
        else if (FrameCtr % 30 == 0)
        {
            Cursor.transform.localPosition = new Vector3(Cursor.transform.localPosition.x, CursorInitialY, Cursor.transform.localPosition.z);
        }
        FrameCtr++;
    }

    public void Close()
    {
        Active = false;
        textMesh.enabled = false;
        BG.enabled = false;
        Cursor.enabled = false;
        Printer.Stop();
        world.reticle.gameObject.SetActive(true);
        world.Unpause();
    }

    public void StartPrinting(TextAsset a)
    {
        List<string> allLines = new List<string>(a.ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
        FrameCtr = 0;
        Active = true;
        textMesh.enabled = true;
        BG.enabled = true;
        Cursor.enabled = true;
        TextSeries = new Queue<string>();
        int linePos = 0;
        string stringBuffer = string.Empty;
        for (int i = 0; i < allLines.Count; i++)
        {
            if (linePos < Printer.lineCount)
            {
                stringBuffer += allLines[i];
                stringBuffer += Environment.NewLine;
                linePos++;
            }
            else
            {
                TextSeries.Enqueue(stringBuffer);
                stringBuffer = string.Empty;
                stringBuffer += allLines[i];
                linePos = 1;
            }
        }
        if (linePos < Printer.lineCount)
        {
            TextSeries.Enqueue(stringBuffer);
        }
        if (world.player.collider.bounds.center.y - world.cameraController.rect.yMin >= world.cameraController.rect.yMax - world.player.collider.bounds.center.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, lowerY, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, higherY, transform.localPosition.z);
        }
        world.reticle.gameObject.SetActive(false);
        world.Pause();
        PrintNextBlock();
    }

    public void PrintNextBlock ()
    {
        Printer.Play(TextSeries.Dequeue());
    }
}
