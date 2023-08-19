using UnityEngine;
using DG.Tweening;

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.PlayAudio("Collision");
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        if (PlayerPrefs.GetInt("VisualEffects", 1) == 1) GetComponent<ParticleSystem>().Play();
        
        GameManager.HandleCollision();
    }
}
