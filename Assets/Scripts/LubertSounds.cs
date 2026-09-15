using UnityEngine;

public class LubertSounds : MonoBehaviour
{
    [SerializeField] private AudioClip bland;
    [SerializeField] private AudioClip stinky;
    [SerializeField] private AudioClip chopChop;
    [SerializeField] private AudioClip boring;
    [SerializeField] private AudioClip snoring;

    public void PlayBlandSound()
    {
        SoundManager.Singleton?.PlaySFX(bland);    
    }

    public void PlayStinkySound()
    {
        SoundManager.Singleton?.PlaySFX(stinky);
    }

    public void PlayChopChopSound()
    {
        SoundManager.Singleton?.PlaySFX(chopChop);
    }

    public void PlayBoringSound()
    {
        SoundManager.Singleton?.PlaySFX(boring);
    }

    public void PlaySnoringSound()
    {
        SoundManager.Singleton?.PlaySFX(snoring);
    }
}
