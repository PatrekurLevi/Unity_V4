using UnityEngine;

/// <summary>
/// This class will apply continuous damage to the Player as long as it stay inside the trigger on the same object
/// </summary>
public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>(); // Tengir Ruby við scriptuna.

        if (controller != null)
        {
            controller.ChangeHealth(-1); // Slær 1 líf af Ruby ef Ruby kemur við Damageables.
        }
    }
}