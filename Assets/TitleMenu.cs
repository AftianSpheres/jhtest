using UnityEngine;
using System.Collections;

enum TitleMenuSelections
{
    NewGame,
    Continue,
    Options,
    Extras,
    Quit
}

public class TitleMenu : MonoBehaviour
{
    public TitleScreen master;
    public SpriteRenderer cursor;
    public AudioClip cursorUp;
    public AudioClip cursorDown;
    public AudioClip madeSelection;
    public AudioClip badOption;
    public int cursorSpeed;
    private Vector3 origCursorPos;
    private TitleMenuSelections selection;
    private int ctr;
    
	// Use this for initialization
	void Start ()
    {
        origCursorPos = cursor.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        cursor.transform.position = origCursorPos + (Vector3.down * 10 * (int)selection);
        if (ctr > 0)
        {
            ctr--;
        }
        else if (master.hardwareInterfaceManager.Down.Pressed == true)
        {
            if (selection < TitleMenuSelections.Quit)
            {
                selection++;
                master.source.PlayOneShot(cursorDown);
                ctr = 9;
            }
        }
        else if (master.hardwareInterfaceManager.Up.Pressed == true)
        {
            if (selection > TitleMenuSelections.NewGame)
            {
                selection--;
                master.source.PlayOneShot(cursorUp);
                ctr = 9;
            }
        }
        else if (master.hardwareInterfaceManager.Confirm.BtnDown == true)
        {
            switch (selection)
            {
                case TitleMenuSelections.NewGame:
                    master.source.PlayOneShot(badOption);
                    break;
                case TitleMenuSelections.Continue:
                    master.source.PlayOneShot(badOption);
                    break;
                case TitleMenuSelections.Options:
                    master.source.PlayOneShot(badOption);
                    break;
                case TitleMenuSelections.Extras:
                    master.source.PlayOneShot(badOption);
                    break;
                case TitleMenuSelections.Quit:
                    Application.Quit();
                    break;
            }
        }
        else if (master.hardwareInterfaceManager.Cancel.BtnDown == true)
        {
            master.preMenu = true;
            gameObject.SetActive(false);
        }
	}
}
