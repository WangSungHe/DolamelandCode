using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool damagePaused = false;

    public HealthBar healthBar;
    public PlayerLife playerLife;
    public GameObject warningBox;
    public float blinkInterval = 0.5f;
    private bool isBlinking = false;

    private Coroutine blinkCoroutine;
  

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        InvokeRepeating(nameof(DecreaseHealth), 0.2f, 1.5f);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            playerLife.Die();
        }

        if (currentHealth <= 30)
        {
            if (!isBlinking)
            {
                isBlinking = true;
                blinkCoroutine = StartCoroutine(BlinkWarning());
            }
        }
        else
        {
            isBlinking = false;
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);
            SetWarningBoxAlpha(0f);
        }
    }

    void DecreaseHealth()
    {
        if (!damagePaused)
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            int excessHealing = currentHealth - maxHealth;
            currentHealth = maxHealth;
            currentHealth -= excessHealing;
        }
        healthBar.SetHealth(currentHealth);
    }

    private IEnumerator BlinkWarning()
    {
        float alpha = 0f;
        bool increasing = true;
        float startTime = Time.time;

        while (isBlinking)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / blinkInterval;
            if (increasing)
                alpha = Mathf.Lerp(0f, 0.55f, t);
            else
                alpha = Mathf.Lerp(0.55f, 0f, t);

            SetWarningBoxAlpha(alpha);

            if (t >= 1f)
            {
                increasing = !increasing;
                startTime = Time.time;
            }

            yield return null;
        }
    }

    private void SetWarningBoxAlpha(float alpha)
    {
        if (warningBox != null && warningBox.GetComponent<Image>() != null)
        {
            var color = warningBox.GetComponent<Image>().color;
            color.a = alpha;
            warningBox.GetComponent<Image>().color = color;
        }
    }
}
