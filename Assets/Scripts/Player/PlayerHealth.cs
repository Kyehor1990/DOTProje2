using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    
    public bool isInvincible = false;
    public float invincibilityTime = 1f;

    public GameObject panel;

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        
        currentHealth -= damage;
        Debug.Log("Player health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }
    
    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
    
    private void Die()
    {
        Debug.Log("Player died!");
        panel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void UpgradeHealth(int amount)
    {
        maxHealth += amount;
        Debug.Log("Upgraded health: " + maxHealth);
    }

}