using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RetroPrinterScriptBasic : MonoBehaviour {

	#region Private fields
	
	public AudioSource source;
    private WorldController world;
	
	private string mainText = "";
	
	private bool running = true;

	private TextMesh textComponent;
	
	#endregion
	
	#region Public fields

	public GameObject ObjectToUpdate = null;
    public TextMesh secondaryUpdateTextMesh;
	
	public float LetterInterval = 0.1f;
	public float EndLineWait = 0.0f;

	public List<string> FullText;
	
	public bool runOnStart = true;
    public bool finishedTextBlock = false;

    public int lineCount;
    public int CharsPerFrame = 1;
	
	
	#endregion

	void Start ()
    {
        world = GameObject.Find("World").GetComponent<WorldController>();	
		Init();

        if (runOnStart == true)
        {
            Run();
        }
	}

	void Update ()
    {
		UpdateProperty();
	}
	
	void Init()
	{
		textComponent = (ObjectToUpdate.GetComponent(typeof(TextMesh)) as TextMesh);
	}
	
	IEnumerator UpdateText()
	{
        finishedTextBlock = false;
		foreach(string s in FullText)
		{
			if(!running)
				yield break;
			
			yield return StartCoroutine( UpdateLine(s) );
			
		}
        finishedTextBlock = true;
	}
	
	IEnumerator UpdateLine(string line)
	{
		if(!running)
			yield break;

		if(string.IsNullOrEmpty(line))
		{
			mainText += "\n";
			yield return new WaitForSeconds(EndLineWait);
		}
		
		else
		{
			for(int i = 0; i < line.Length; i+= CharsPerFrame)
			{
				if(!running)
					yield break;

				else
				{
                    int i2 = 0;
                    if (source != null)
                    {
                        source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_TextPrintSFX), 0.25f);
                    }
                    while (i2 < CharsPerFrame && i + i2 < line.Length)
                    {
                        mainText += line[i + i2];
                        i2++;
                    }
					yield return new WaitForSeconds(LetterInterval);
				}
			}
			
			yield return new WaitForSeconds(EndLineWait);
			
			if(FullText.IndexOf(line) != (FullText.Count() - 1)) // make sure that we don't add an empty line at the end of our text
				mainText += "\n";
		}
		
	}

    public void Play(string s)
    {
        Stop();
        FullText = new List<string>(s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
        Run();
    }

    public void Play(TextAsset a)
    {
        Play(a.ToString());
    }

    void Run()
	{
        for (int i = FullText.Count - 1; i >= lineCount; i--)
        {
            FullText.RemoveAt(i);
        }
		running = true;
        if (gameObject.activeInHierarchy == true)
        {
            StartCoroutine(UpdateText());
        }
        else
        {
            Stop();
        }
	}
	
	public void Stop()
	{
		running = false;
        StopAllCoroutines();
        Clear();
    }
	
	void Clear()
	{
		mainText = string.Empty;
	}
	
	
	private void UpdateProperty()
	{
        textComponent.text = mainText;
        if (secondaryUpdateTextMesh != null)
        {
            secondaryUpdateTextMesh.text = mainText;
        }
	}
}
