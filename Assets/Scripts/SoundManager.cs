using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;

    [Header("SFX")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource fridgeSource;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySFX(AudioClip audio)
    {
        SFXSource.PlayOneShot(audio);
    }

    public void PlayFridgeAmbience(bool play, AudioClip audio = null)
    {
        if(play)
        {
            fridgeSource.clip = audio;
            fridgeSource.loop = true;
            fridgeSource.Play();
        }
        else
        {
            fridgeSource.Stop();
        }
    }

    public void PlayMusic(AudioClip audio)
    {
        musicSource.Play();
    }
}
