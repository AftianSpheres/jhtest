using UnityEngine;
using System.Collections;

public class TitleLogo : MonoBehaviour
{
    public Sprite[] frames;
    new public SpriteRenderer renderer;
    public SpriteRenderer logoRenderer;
    public bool animDone;
    public AudioSource titleBGM;
    public AudioSource ownSource;
    public AudioClip clip;
    private static int animLength = 30;
    private int i = 0;
	
	// Update is called once per frame
	void Update ()
    {
        if (animDone == false)
        {
            i++;
            if (i < animLength)
            {
                if (HardwareInterfaceManager.Instance != null && HardwareInterfaceManager.Instance.Menu.BtnDown == true)
                {
                    i = animLength - 1;
                }
                renderer.sprite = frames[i / (animLength / frames.Length)];

            }
            else
            {
                ownSource.PlayOneShot(clip);
                logoRenderer.enabled = true;
                titleBGM.Play();
                animDone = true;
            }
        }
	}
}
