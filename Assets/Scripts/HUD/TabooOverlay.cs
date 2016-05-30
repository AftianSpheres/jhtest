using UnityEngine;
using System.Collections;

public class TabooOverlay : MonoBehaviour
{
    public WorldController world;
    new public SpriteRenderer renderer;
    public Sprite[] frames;
    private bool active = false;

	// Update is called once per frame
	void Update ()
    {
	    if (active != renderer.enabled)
        {
            renderer.enabled = active;
        }
	}

    void Close()
    {
        active = false;
        world.Unpause();
    }

    public void DisplayTabooGlyph(TabooType taboo)
    {
        StopAllCoroutines();
        switch(taboo)
        {
            case TabooType.Eyes:
                frames = Resources.LoadAll<Sprite>(GlobalStaticResourcePaths.p_TabooOverlay_Eyes);
                break;
            default:
                throw new System.Exception("Taboo overlay received invalid taboo of value: " + taboo.ToString());
        }
        StartCoroutine(in_DisplayTabooGlyph());

    }

    IEnumerator in_DisplayTabooGlyph()
    {
        if (world.paused == false)
        {
            world.Pause();
        }
        active = true;
        int i = 0;
        renderer.sprite = frames[3];
        while (i < 3)
        {
            i++;
            yield return null;
        }
        renderer.sprite = frames[2];
        while (i < 6)
        {
            i++;
            yield return null;
        }
        renderer.sprite = frames[1];
        while (i < 9)
        {
            i++;
            yield return null;
        }
        renderer.sprite = frames[0];
        while (i < 45)
        {
            i++;
            yield return null;
        }
        Close();
    }
}
