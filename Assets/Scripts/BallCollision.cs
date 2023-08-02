using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.PlayAudio("Collision");
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        
        GameManager.HandleCollision();
    }
}
