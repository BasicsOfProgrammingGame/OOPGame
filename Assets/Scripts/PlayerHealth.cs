using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Настройки здоровья")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Отображение жизней")]
    [SerializeField] private GameObject live3;
    [SerializeField] private GameObject live2;
    [SerializeField] private GameObject live1;
    [SerializeField] private GameObject live0;

    [Header("Настройки игрока")]
    [SerializeField] private GameObject player; // Ссылка на игрока
    public float knockBackForce = 5f;
    public float invincibilityTime = 1.5f;

    private Rigidbody2D playerRb;
    private Transform playerTransform;
    private bool isInvincible = false;
    private Collider2D playerCollider;
    private int originalPlayerLayer;

    public GameObject deathScreenPanel;

    private void Start()
    {
        deathScreenPanel.SetActive(false);
        InitializeHealthDisplay();
        InitializePlayerReferences();
    }

    private void InitializeHealthDisplay()
    {
        live0.SetActive(false);
        live1.SetActive(false);
        live2.SetActive(false);
        live3.SetActive(true);
        currentHealth = maxHealth;
    }

    private void InitializePlayerReferences()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // если забыл
        }

        if (player != null)
        {
            playerTransform = player.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerCollider = player.GetComponent<Collider2D>();
            originalPlayerLayer = player.layer;
        }
        else
        {
            Debug.LogError("Ты куда игрока дел?!");
        }
    }

    public void TakeDamage(int damage, Transform damageSource)
    {
        if (isInvincible || player == null) return;

        currentHealth -= damage;
        UpdateHealthDisplay();

        ApplyKnockback(damageSource);
        StartCoroutine(ActivateInvincibility());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthDisplay()
    {
        GameObject[] lives = { live0, live1, live2, live3 };

        // Отключаем все жизни
        foreach (var life in lives)
        {
            if (life != null) life.SetActive(false);
        }

        // Включаем текущее количество жизней
        if (currentHealth >= 0 && currentHealth < lives.Length && lives[currentHealth] != null)
        {
            lives[currentHealth].SetActive(true);
        }
    }

    private void ApplyKnockback(Transform damageSource)
    {
        if (playerRb != null && playerTransform != null)
            playerRb.AddForce(new Vector3(0, 1) * knockBackForce, ForceMode2D.Impulse);
    }

    private IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // Включаем прохождение сквозь врагов
        if (playerCollider != null)
        {
            player.layer = LayerMask.NameToLayer("InvinciblePlayer");
            var playerSprite = player.GetComponent<SpriteRenderer>();
            float timer = 0;

            // Эффект мигания
            while (timer < invincibilityTime)
            {
                if (playerSprite != null)
                {
                    playerSprite.enabled = !playerSprite.enabled;
                }
                timer += 0.15f;
                yield return new WaitForSeconds(0.15f);
            }

            if (playerSprite != null) playerSprite.enabled = true;
            player.layer = originalPlayerLayer;
        }

        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Player died");

        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);

            foreach (Transform child in deathScreenPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Death screen panel not assigned!");
        }

        // Дополнительные действия при смерти
        if (player != null)
        {
            player.SetActive(false); // Деактивируем игрока
        }
    }

    // Для вызова из других скриптов
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthDisplay();
    }
}