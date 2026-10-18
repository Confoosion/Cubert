using UnityEngine;

public class DeskBell : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bellSFX;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(bellSFX);
    }
}
