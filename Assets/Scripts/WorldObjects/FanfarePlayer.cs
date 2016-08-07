using UnityEngine;
using System.Collections;

public class FanfarePlayer : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource source;
    public bool fanfarePlaying;
    public AudioClip lastClip = default(AudioClip);

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
        lastClip = clip;
        source.PlayOneShot(clip);
        fanfarePlaying = true;
    }
}
