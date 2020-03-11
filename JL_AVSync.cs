using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class JL_AVSync : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoLead;
    public AudioSource audioFollow;

    [Header("Adjustments")]
    [Tooltip("Correction occurs when audio and video are out of sync by this amount")]
    public float tolerance = 0.04f;

    [Tooltip("Use this audio source to debug how frequently the video/audio is going out of sync")]    
    public AudioSource errorSound;

    [Tooltip("Trim this much from the start of the video/audio")]
    public float trim = 0.0f;

    [Tooltip("Use this to offset the audio from the video")]
    public float audioOffset = 0.0f;

    private void Start()
    {
        //This is just for testing
        Play();
    }

    public void Play()
    {
        StartCoroutine(PlayVideo(videoLead, audioFollow));
    }

    public void Play(VideoPlayer vL, AudioSource aF)
    {
        StartCoroutine(PlayVideo(vL, aF));
    }

    IEnumerator PlayVideo(VideoPlayer lead, AudioSource follow)
    {
        lead.time = trim;
        lead.Prepare();

        while (!lead.isPrepared)
        {
            yield return null;
        }

        lead.Play();

        follow.time = trim + audioOffset;
        follow.Play();

        while (lead.isPlaying)
        {
            if (Mathf.Abs((float)lead.time - (follow.time - audioOffset)) > tolerance)
            {
                follow.time = (float)lead.time + audioOffset;
                if (errorSound)
                    errorSound.Play();
            }
            yield return null;
        }
        if (follow.isPlaying)
            follow.Stop();
        yield return null;
    }
}
