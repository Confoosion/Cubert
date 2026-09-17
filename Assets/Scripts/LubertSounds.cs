using UnityEngine;

public class LubertSounds : MonoBehaviour
{
    [SerializeField] private AudioSource lubertSource;
    [SerializeField] private AudioClip bland;
    [SerializeField] private AudioClip stinky;
    [SerializeField] private AudioClip chopChop;
    [SerializeField] private AudioClip boring;
    [SerializeField] private AudioClip snoring;

    private float soundEndTime;

    public void PlayLubertSound(AudioClip sound)
    {
        if(!IsPlayingSound())
        {
            lubertSource.PlayOneShot(sound);
            soundEndTime = Time.time + sound.length;
        }
    }

    public void StopLubertSound()
    {
        lubertSource.Stop();
    }

    private bool IsPlayingSound()
    {
        return(Time.time < soundEndTime && lubertSource.isPlaying);
    }

    public void PlayBlandSound()
    {
        PlayLubertSound(bland);    
    }

    public void PlayStinkySound()
    {
        PlayLubertSound(stinky);
    }

    public void PlayChopChopSound()
    {
        PlayLubertSound(chopChop);
    }

    public void PlayBoringSound()
    {
        PlayLubertSound(boring);
    }

    public void PlaySnoringSound()
    {
        PlayLubertSound(snoring);
    }
}
