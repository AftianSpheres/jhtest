using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource source;
    public AudioClip clip;
    public ScrollingLayer[] fgScrolls;
    public GameObject curtain;
    public GameObject titleMenu;
    public Renderer TextRenderer;
    public TextMesh pressStartText;
    public int TextFlashInterval;
    public bool preMenu = true;
    private bool inTransitionFromTitle = false;
    public HardwareInterfaceManager hardwareInterfaceManager;
    private float origVolume;
    private int ctr;

	// Use this for initialization
	void Start ()
    {
        GameObject hwIMobj = GameObject.Find("Universe/HardwareInterfaceManager");
        if (hwIMobj != null)
        {
            hardwareInterfaceManager = hwIMobj.GetComponent<HardwareInterfaceManager>();
        }    
        origVolume = BGM.volume;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (hardwareInterfaceManager == null)
        {
            GameObject hwIMobj = GameObject.Find("Universe/HardwareInterfaceManager");
            if (hwIMobj != null)
            {
                hardwareInterfaceManager = hwIMobj.GetComponent<HardwareInterfaceManager>();
            }
        }
	    else if (preMenu == true)
        {
            if (inTransitionFromTitle == false)
            {
                ctr++;
                if (ctr >= TextFlashInterval)
                {
                    TextRenderer.enabled = !TextRenderer.enabled;
                    ctr = 0;
                }
                if (hardwareInterfaceManager.Menu.BtnDown == true)
                {
                    preMenu = false;
                    source.PlayOneShot(clip);
                    titleMenu.SetActive(true);
                }
#if (DEVELOPMENT_BUILD || UNITY_EDITOR)
                else if (hardwareInterfaceManager.Fire1.BtnDown == true) // debug start
                {
                    StartCoroutine(TransitionFromTitle(2));
                    ctr = int.MinValue;
                    TextRenderer.enabled = true;
                    pressStartText.text = "Going to\ntest map";
                    preMenu = false;
                    source.PlayOneShot(clip);
                }
#endif
            }
        }
        else
        {
            ctr = TextFlashInterval;
            TextRenderer.enabled = false;
        }
	}

    IEnumerator TransitionFromTitle (int sceneID)
    {
        inTransitionFromTitle = true;
        int i = 0;
        while (i < 90)
        {
            if (i % 10 == 0)
            {
                fgScrolls[0].scrollingSpeed *= 2;
                fgScrolls[1].scrollingSpeed *= 2;
            }
            BGM.volume = origVolume * ((90f - i) / 90f);
            if (i >= 45)
            {
                curtain.SetActive(true);
            }
            i++;
            yield return null;
        }
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }
}
