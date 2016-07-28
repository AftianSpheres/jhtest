using UnityEngine;
using System.Collections;

public class FanfarePlayer : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource source;
    bool fanfarePlaying;

    void Update ()
    {
        if (bgm.isPlaying == true && source.isPlaying == true)
        {
            fanfarePlaying = false;
            source.Stop();
        }
        if (fanfarePlaying == true && source.isPlaying == false)
        {
            fanfarePlaying = false;
            bgm.Play();
        }
    }

    public void Play (AudioClip clip)
    {
        bgm.Stop();
        source.Stop();
        source.PlayOneShot(clip);
        fanfarePlaying = true;
    }
}
