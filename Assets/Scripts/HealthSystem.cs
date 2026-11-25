using UnityEngine;
using TMPro;
using UnityEngine.UI;


//Script que controla la vida del protagonista y de los zombies
public class HealthSystem : MonoBehaviour
{   
    [SerializeField] private RectTransform healthBar;

    [SerializeField] private float health = 100;

    public float barWidth = 200f;
    public float barHeight = 50f;
    [SerializeField] private AudioClip deathSound;

    [SerializeField, Range(0f, 1f)] private float deathSoundVolume = 1f;

    private Animator anim;

    private CapsuleCollider capsHitbox;

    private bool deathTriggered;

    private string characterTag;

    public TMP_Text DeathMessage;

    public bool IsDead { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        capsHitbox = GetComponent<CapsuleCollider>();
        characterTag = gameObject.tag;


        /*if (healthBar != null)
        {
            if (barWidth <= 0f)
            {
                RectTransform parentBar = healthBar.parent as RectTransform;
                barWidth = parentBar != null ? parentBar.rect.width : healthBar.rect.width;
            }

            if (barHeight <= 0f)
            {
                RectTransform parentBar = healthBar.parent as RectTransform;
                barHeight = parentBar != null ? parentBar.rect.height : healthBar.rect.height;
            }
        }*/
    }

    void Update()
    {
        if (health <= 0 && !deathTriggered)
        {
            Die();
        }

        updateHealthBar(health);
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);

        if (health <= 0 && !deathTriggered)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        if (IsDead) return;
        health = Mathf.Clamp(health + amount, 0f, 100f);
        updateHealthBar(health);
    }

    public void updateHealthBar(float h)
    {
        if (healthBar == null)
        {
            return;
        }

        float newWidth = (Mathf.Clamp(h, 0f, 100f) / 100f) * barWidth;

        healthBar.sizeDelta = new Vector2(newWidth, barHeight);

    }

    private void Die()
    {
        IsDead = true;
        deathTriggered = true;
        anim.SetTrigger("Death");
        if (characterTag == "Player")
        {
            SendMessage("SetDead", true, SendMessageOptions.DontRequireReceiver);

            if (DeathMessage != null)
            {
                DeathMessage.gameObject.SetActive(true);
                DeathMessage.text = "You survived 67 rounds";
            }
        }
        if (characterTag == "Zombie")
        {
            SendMessage("SetDead", true, SendMessageOptions.DontRequireReceiver);
        }
        PlayDeathSound(deathSound);
        //Si la capsula no se desactiva, queda una pared invisible donde se murio el zombie,
        //pero si se desactiva, el zombie se pone a deslizarse en el piso.
        /*if (capsHitbox != null)
        {
            capsHitbox.enable = false;
        }*/
    }

    private void PlayDeathSound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, deathSoundVolume);
        }
    }
}
