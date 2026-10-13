using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("SFX Sources")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource fridgeSource;
    [SerializeField] private AudioSource switchRoomSource;

    [Header("Music Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] foodSounds;

    // PlayerPrefs
    private const string SFXKey = "SFXVolume";
    private const string DialogueKey = "DialogueVolume";

    public float SFX_Volume => PlayerPrefs.GetFloat(SFXKey, 0.75f);
    public float Dialogue_Volume => PlayerPrefs.GetFloat(DialogueKey, 0.75f);


    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXKey, 0.75f);
        float savedDialogueVolume = PlayerPrefs.GetFloat(DialogueKey, 0.75f);

        ApplySFXVolume(savedSFXVolume);
        ApplyDialogueVolume(savedDialogueVolume);
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

    public void SetSFXVolume(float value)
    {
        ApplySFXVolume(value);
        PlayerPrefs.SetFloat(SFXKey, value);
        PlayerPrefs.Save();
    }

    public void SetDialogueVolume(float value)
    {
        ApplyDialogueVolume(value);
        PlayerPrefs.SetFloat(DialogueKey, value);
        PlayerPrefs.Save();
    }

    private void ApplySFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    private void ApplyDialogueVolume(float value)
    {
        audioMixer.SetFloat("DialogueVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
}
