using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;

    [SerializeField] private AudioSource SFXSource;
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

    public void PlayMusic(AudioClip audio)
    {
        musicSource.Play();
    }
}
