using UnityEngine;

public class HUDTabooIcon : MonoBehaviour
{
    new public SpriteRenderer renderer;
    public PlayerWeaponManager wpnManager;
    public Sprite[] frames;
    private TabooType tabooCache;
    private bool active = true;

    // Update is called once per frame
    void Update ()
    {
	    if (wpnManager.TabooReady != active)
        {
            active = wpnManager.TabooReady;
            renderer.enabled = wpnManager.TabooReady;
        }
        if (active == true && wpnManager.Taboo != tabooCache)
        {
            if (wpnManager.Taboo == TabooType.None)
            {
                renderer.enabled = false;
            }
            else
            {
                renderer.enabled = true;
                renderer.sprite = frames[(int)wpnManager.Taboo];
            }
            tabooCache = wpnManager.Taboo;
        }
	}
}
