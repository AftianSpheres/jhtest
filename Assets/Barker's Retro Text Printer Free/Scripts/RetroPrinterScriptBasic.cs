using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RetroPrinterScriptBasic : MonoBehaviour {

	#region Private fields
	
	private AudioSource Audio;
	
	private string mainText = "";
	
	private bool running = true;

	private TextMesh textComponent;
	
	#endregion
	
	#region Public fields

	public GameObject ObjectToUpdate = null;
	
	public float LetterInterval = 0.1f;
	public float EndLineWait = 0.0f;

	public List<string> FullText;
	
	public bool runOnStart = true;

    public int lineCount;
    public int CharsPerFrame = 1;
	
	
	#endregion

	void Start () {
		
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
		foreach(string s in FullText)
		{
			if(!running)
				yield break;
			
			yield return StartCoroutine( UpdateLine(s) );
			
		}
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
	
    public void Play(TextAsset a)
    {
        Stop();
        FullText = new List<String>(a.ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
        Run();
    }

    void Run()
	{
        for (int i = FullText.Count - 1; i >= lineCount; i--)
        {
            FullText.RemoveAt(i);
        }
		running = true;
		StartCoroutine( UpdateText() );
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
	}
}
