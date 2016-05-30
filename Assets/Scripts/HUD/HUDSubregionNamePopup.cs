using UnityEngine;
using System;
using System.Collections;

public class HUDSubregionNamePopup : MonoBehaviour
{
    public WorldController world;
    public TextMesh textMesh;
    new public MeshRenderer renderer;
    private SubregionType subregion = SubregionType.None;
    private string[] lines;
    private AudioClip clip;
	
    void Awake ()
    {
        clip = Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_SubregionPopupFanfare);
        lines = Resources.Load<TextAsset>(GlobalStaticResourcePaths.p_subregion_names).ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    }

	// Update is called once per frame
	void Update ()
    {
	    if (world.activeRoom != null && world.activeRoom.Subregion != subregion)
        {
            StopAllCoroutines();
            StartCoroutine(DisplaySubregionName());
        }
	}

    IEnumerator DisplaySubregionName ()
    {
        subregion = world.activeRoom.Subregion;
        world.BGM0.volume = 0.33f;
        world.BGS0.PlayOneShot(clip);
        renderer.enabled = true;
        textMesh.text = lines[(int)subregion];
        for (int i = 0; i < 180; i++)
        {
            yield return null;
        }
        renderer.enabled = false;
        for (float i = 0.33f; i <= 1.0f; i+= .11f)
        {
            if (i == .99)
            {
                i = 1f;
            }
            world.BGM0.volume = i;
        }
        world.BGM0.volume = 1.0f;
    }
}
