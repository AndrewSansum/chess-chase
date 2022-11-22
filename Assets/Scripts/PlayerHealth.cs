using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    public int maxHealth = 3;

    //public SpriteRenderer playerDeath;
    public PlayerMovement playerMovement;
    public SwordSwing swordSwing;
    public LevelManager levelManager;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg) 
    {
        currentHealth -= dmg;

        if (currentHealth <= 0) 
        {
            playerMovement.Stop();
            //swordSwing.enabled = false;
            levelManager.GameOver();

            // Death Animation?
            // Game over screen?
        }
    }

    public void LevelComplete() {
        playerMovement.Stop();
        playerMovement.enabled = false;
    }

    public void Heal(int amount)
    {
        // play sound / animation
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
