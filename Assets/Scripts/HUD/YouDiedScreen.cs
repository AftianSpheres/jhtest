using UnityEngine;
using System.Collections;

enum YouDiedScreenState
{
    None,
    FirstDeployed,
    SecondDeployed,
    Undeployed
}

public class YouDiedScreen : MonoBehaviour
{
    public WorldController world;
    public Renderer spr0;
    public Renderer spr1;
    public AudioClip clip;
    public AudioSource source;
    YouDiedScreenState state;
    int ctr;
    static int stage0length = 60;
    static int stage1length = 180;

	// Use this for initialization
	void Start ()
    {
        state = YouDiedScreenState.Undeployed;
        spr0.enabled = spr1.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    switch (state)
        {
            case YouDiedScreenState.Undeployed:
                if (world.player.Dead == true)
                {
                    if (world.player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == PlayerAnimatorHashes.PlayerCorpse)
                    {
                        state = YouDiedScreenState.FirstDeployed;
                        world.reticle.renderer.enabled = false;
                        ctr = 0;
                        spr0.enabled = true;
                        world.BGM0.Stop();
                        source.PlayOneShot(clip);
                    }
                }
                break;
            case YouDiedScreenState.FirstDeployed:
                if (ctr > stage0length)
                {
                    state = YouDiedScreenState.SecondDeployed;
                    ctr = 0;
                    spr1.enabled = true;
                }
                else
                {
                    ctr++;
                }
                break;
            case YouDiedScreenState.SecondDeployed:
                if (ctr > stage1length)
                {
                    world.player.animator.Play(PlayerAnimatorHashes.PlayerDisintegrate);
                    spr0.enabled = false;
                    ctr = -1;
                }
                else if (ctr > -1)
                {
                    ctr++;
                }
                if (source.isPlaying == false)
                {
                    spr1.enabled = false;
                }
                if (world.player.Dead == false && world.Curtain.activeInHierarchy == false)
                {
                    state = YouDiedScreenState.Undeployed;
                    world.BGM0.Play();
                    world.reticle.renderer.enabled = true;
                }
                break;

        }
	}
}
