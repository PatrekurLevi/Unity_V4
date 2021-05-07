using UnityEngine;

/// <summary>
/// Handle the projectile launched by the player to fix the robots.
/// </summary>
public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); // Geymir Rigidbodyið í breytu
    }

    public void Launch(Vector2 direction, float force) // Launch parameter miðað við Vector2 átt og force. Því hærra force því hraðar fer boltinn
    {
        rigidbody2d.AddForce(direction * force); // Setur upp átt og force afl á boltann
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);  // Eyðir upp boltanum miðað við tíma.
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>(); // Collision breyta fyrir óvin
        if (e != null)
        {
            e.Fix(); // Ef óvinurinn er ekki lagaður, þá lagar það hann
        }

        Destroy(gameObject); // Eyðir upp boltanum þegar hann lagar óvin
    }
}