using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;

    [Header("SFX Sources")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource fridgeSource;
    [SerializeField] private AudioSource switchRoomSource;

    [Header("Music Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] foodSounds;

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

    public void PlayEatSFX()
    {
        PlaySFX(foodSounds[Random.Range(0, foodSounds.Length)]);
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

    public void PlaySwitchRoom(AudioClip audio)
    {
        switchRoomSource.PlayOneShot(audio);
    }

    public void PlayMusic(AudioClip audio)
    {
        musicSource.Play();
    }
}
