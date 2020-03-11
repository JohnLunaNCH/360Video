using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class JL_AVSync : MonoBehaviour
{
    public VideoPlayer vid_lead;
    public AudioSource aud_follow;
    public float tolerance = 0.25f;
    public AudioSource errorSound;
    public float startTime = 0.0f;
    public float audioOffset = 0.0f;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        vid_lead.time = startTime;
        vid_lead.Prepare();

        while (!vid_lead.isPrepared)
        {
            yield return null;
        }

        vid_lead.Play();

        aud_follow.time = startTime + audioOffset;
        aud_follow.Play();

        while (vid_lead.isPlaying)
        {
            if (Mathf.Abs((float)vid_lead.time - (aud_follow.time - audioOffset)) > tolerance)
            {
                aud_follow.time = (float)vid_lead.time + audioOffset;
                if (errorSound)
                    errorSound.Play();
            }
            yield return null;
        }
        yield return null;
    }
}