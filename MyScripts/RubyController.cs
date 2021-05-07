using System;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f; // Setur upp hraðann

    public int maxHealth = 5; // Setur upp hámarks líf

    public GameObject projectilePrefab; // 

    public AudioClip throwSound; // Hljóðklippa fyrir að kasta boltanum
    public AudioClip hitSound; // Hljóðið þegar Ruby meiðir sig

    public int health { get { return currentHealth; } } // Gefur upp breytuna fyrir Lífið sem public
    int currentHealth; // Núverandi lífstaða á Ruby

    public float timeInvincible = 2.0f; // Setur Ruby ósýnilega í tvær sekúndur ef hún gengur inn í Damageables og stendur í þeim lengi.
    bool isInvincible; // Ef hún er 'ósýnileg'
    float invincibleTimer; // Tíminn sem hún er ósýnileg

    Rigidbody2D rigidbody2d; // Gefur upp rigidbody
    float horizontal; // Lárétt
    float vertical; // Lóðrétt

    Animator animator; // setur upp Animation breytu.
    Vector2 lookDirection = new Vector2(1, 0); // Breyta til að leyfa Ruby animationið að snúa sér með gönguáttinni.

    AudioSource audioSource; // Hljóðklippi breyta


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); // Fær Rigidbody
        animator = GetComponent<Animator>(); // Fær Animatorinn

        currentHealth = maxHealth; // Setur núverandi líf í hámark

        audioSource = GetComponent<AudioSource>(); // Setur hljóðið í gang þar sem það er merkt í Start
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        // Setur stýristefnu
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) 
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        // Lætur animationið fylgja eða snúa sér með Ruby
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Setur timer og leyfir Ruby að standa í Damageables í 2 sek áður en hún fær aftur slag á sig.
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        // Kastar frá sér boltanum
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        // Opnar upp dialogueið hjá Jambi.
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Raycast verður að vera í sömu augnlínu og Jambi er við Ruby svo það hleypi breytunni í gegn og opnar fyrir dialogueið hans
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    void FixedUpdate()
    {
        //Hreyfingarnar fyrir Ruby
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        // Breytan fyrir hljóðið ef að Ruby er að missa líf eða fer niður í 0 líf
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(hitSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Uppfærir healthbarið í UI miðað við lífið hennar
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    // classinn fyrir skrúfuna, kastar henni miðað við áttina sem Ruby snýr sér að
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300); // Fer ekki lengra en 300 units áður en boltinn eyðist upp

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }
    // spilar hljóðið Oneshot
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}