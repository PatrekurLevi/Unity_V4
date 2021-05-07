using UnityEngine;

/// <summary>
/// Will handle giving health to the character when they enter the trigger.
/// </summary>
public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other) //Collide scripta.
    {
        RubyController controller = other.GetComponent<RubyController>(); // Tengir Ruby við þessa scriptu.
        
        if (controller != null) // Ef Ruby er ekki með fullt líf, þá leyfir HealthCollectibles Ruby til að gleypa sig fyrir líf.
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1); // Gefur eitt líf.
                Destroy(gameObject); //Gleypir HealthCollectible.

                controller.PlaySound(collectedClip); //Hleypir hljóðinu í gegn.
            }
        }

    }
}