using UnityEngine;
using System;
using System.Collections;

public class HUDSubregionNamePopup : MonoBehaviour
{
    public WorldController world;
    public TextMesh textMesh;
    new public MeshRenderer renderer;
    public TextMesh bgText;
    public MeshRenderer bgRenderer;
    public TextMesh aliasText;
    public MeshRenderer aliasRenderer;
    private SubregionType subregion = SubregionType.None;
    private string[] lines;
    private AudioClip clip;
    public AudioSource source;
	
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
        world.BGM0.Stop();
        subregion = world.activeRoom.Subregion;
        world.BGM0.volume = 0.33f;
        source.PlayOneShot(clip);
        renderer.enabled = true;
        bgRenderer.enabled = true;
        aliasRenderer.enabled = true;
        textMesh.text = bgText.text = aliasText.text = lines[(int)subregion];
        for (int i = 0; i < 180; i++)
        {
            yield return null;
        }
        renderer.enabled = false;
        bgRenderer.enabled = false;
        aliasRenderer.enabled = false;
        world.BGM0.volume = 1.0f;
        world.BGM0.Play();
    }
}
