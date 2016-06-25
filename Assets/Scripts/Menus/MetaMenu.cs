using UnityEngine;
using System.Collections;

public class MetaMenu : MonoBehaviour
{
    public MenuSystem menuSystem;
    public GameStateManager gameStateManager;
    public RetroPrinterScriptBasic textbox;
    public bool open = false;

    void Awake ()
    {
        gameObject.SetActive(false);
    }

    public void Close ()
    {

    }

    public void Open ()
    {

    }

    public void PreOpen()
    {

    }
}
