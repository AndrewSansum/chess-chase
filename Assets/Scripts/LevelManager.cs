using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float respawnDelay;
    public PlayerMovement gamePlayer;

    public SpriteRenderer cpSR;
    public SpriteRenderer bpSR;

    public Sprite blockEnabled;
    public Sprite blockDisabled;

    public BoxCollider2D cpSRCollider;
    public BoxCollider2D bpSRCollider;
    public BoxCollider2D blockTrigger;

    public PlayerHealth playerhealth;

    public Canvas gameOverScreen;

    public List<Enemy> firstSectionEnemies;

    public List<Enemy> secondSectionEnemies;

    // Start is called before the first frame update
    void Start()
    {
        //gamePlayer = FindObjectOfType<PlayerMovement>();
        cpSRCollider.enabled = false;
        bpSRCollider.enabled = false;
        gameOverScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver() {
        gameOverScreen.gameObject.SetActive(true);
    }

    public void Respawn() 
    {   
        gameOverScreen.gameObject.SetActive(false);
        if (!cpSRCollider.enabled) {
            foreach (var enemy in firstSectionEnemies)
            {
                enemy.Disable();
            }
        }
        
        if (bpSRCollider.enabled) {
            foreach (var enemy in secondSectionEnemies)
            {
                enemy.Disable();
            }
        }

        gamePlayer.gameObject.SetActive(false);
        bpSRCollider.enabled = false;
        blockTrigger.enabled = true;
        bpSR.sprite = blockDisabled;
        gamePlayer.transform.position = gamePlayer.respawnPoint;
        playerhealth.currentHealth = 3;
        playerhealth.gameObject.GetComponent<PlayerMovement>().enabled = true;

        if (firstSectionEnemies.Count > 0) {
            firstSectionEnemies[0].grid.ResetGrid();
        }
        
        if (secondSectionEnemies.Count > 0) {
            secondSectionEnemies[0].grid.ResetGrid();
        }

        if (!cpSRCollider.enabled) {
            foreach (var enemy in firstSectionEnemies)
            {
                enemy.Enable();
            }
        }

        gamePlayer.gameObject.SetActive(true);
    }

    public void Checkpoint()
    {
        cpSR.sprite = blockEnabled;
        cpSRCollider.enabled = true;

        foreach (var enemy in firstSectionEnemies)
        {
            enemy.Disable();
        }
    }

    public void Blockpoint()
    {
        bpSR.sprite = blockEnabled;
        bpSRCollider.enabled = true;

        foreach (var enemy in secondSectionEnemies) {
            enemy.Enable();
        }
    }

}
