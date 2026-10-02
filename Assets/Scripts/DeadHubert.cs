using UnityEngine;

public class DeadHubert : MonoBehaviour
{
    [SerializeField] private HubertPiece[] hubertPieces;
    [SerializeField] private AudioSource hubertSource;
    [SerializeField] private AudioClip explosion;

    void Start()
    {
        hubertSource.PlayOneShot(explosion);
        foreach(HubertPiece piece in hubertPieces)
        {
            piece.Launch(hubertSource);
        }
    }
}
