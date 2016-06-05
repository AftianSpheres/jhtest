using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public AudioSource BGM;
    public GameObject curtain;
    private bool inTransitionFromTitle = false;
    private HardwareInterfaceManager hardwareInterfaceManager;
    private float origVolume;

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
	    else if (hardwareInterfaceManager.Menu.BtnDown == true)
        {
            StartCoroutine(TransitionFromTitle(2));
        }
	}

    IEnumerator TransitionFromTitle (int sceneID)
    {
        int i = 0;
        while (i < 90)
        {
            BGM.volume = origVolume * ((90f - i) / 90f);
            Debug.Log(BGM.volume);
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
