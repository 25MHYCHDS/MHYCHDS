using UnityEngine;

public class EnemyHealthContainer : HealthContainer
{
    public int EnemyHealth = 20;
    public static EnemyHealthContainer instance;
    public GameObject e2p;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            SetHealth(EnemyHealth - 1);
            EnemyHealthContainer.instance.GetComponent<Animator>().SetBool("Damage", true);
        }
    }
    protected override void InitHealth()
    {
        SetHealth(EnemyHealth);
    }
    protected override void Die()
    {
        Debug.Log($"Enemy {gameObject.name} is dead");

        gameObject.transform.SetParent(e2p.transform, false);
        PlayerHealthContainer.instance.SetHealth(5);
        //EnemyCatch.instance.HealthUI.SetActive(false);
        SetHealth(10);
        //SoundManager.instance.PlaySfx("Victory");
    }
}
