using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SE
{
    Shot,
    Hit,
}

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip[] setclips;

    static AudioSource pAudioSource;
    static AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        pAudioSource = audioSource;
        clips = setclips;
    }

    public static void playSound(SE se)
    {
        pAudioSource.PlayOneShot(clips[(int)se]);
    }
}
