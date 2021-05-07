using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed; // Hraði
    public bool vertical; // Setur hann láréttann
    public float changeTime = 3.0f; // Gefur 3 sekúndur

    public ParticleSystem smokeEffect; // Smoke Animationið

    Rigidbody2D rigidbody2D; // Gefur upp Rigidbody fyrir scriptuna
    float timer; // Tími
    int direction = 1; // Áttin
    bool broken = true; // Setur hann sem broken til að byrja með, svo Ruby geti lagað hann.

    Animator animator; // Animation breytan

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); // Setur Rigidbody á hann strax í byrjun
        timer = changeTime;  // Setur upp tímann strax í byrjun
        animator = GetComponent<Animator>(); // Gefur honum Animation breytuna strax í byrjun
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return; // Ef óvinurinn er ekki broken þá hættir hann að hreyfa sig til hliðar.
        }

        timer -= Time.deltaTime;
        // Stöðvar tímabreytinguna sem voru 3 sekúndur sem hann labbaði fram og til baka. Núna er hann ekki með neinar Sekúndur til að telja.
        if (timer < 0)
        {
            direction = -direction; // Breytir stefnunni.
            timer = changeTime; // Breytir tímanum.
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }
        // Ef óvinurinn er ekki brotinn þá stoppar hann hreyfingarnar á X ás og Y ás og fer í animationið.
        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        // Tengir Ruby við scriptuna.
        // Ef Ruby kemur við óvininn þá missir hún eitt líf.
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        // Ef óvinurinn hefur verið lagaður þá mun hann setja broken í false, tekur rigidbodyið af og fer beint í dans animationið.
        broken = false;
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed"); // Fer úr 'broken' animation yfir í 'fixed' animation.

        smokeEffect.Stop(); // Stöðvar reyk animationið úr óvininum.
    }
}