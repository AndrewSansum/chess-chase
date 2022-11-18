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

    public PlayerHealth playerhealth;
    // Start is called before the first frame update
    void Start()
    {
        //gamePlayer = FindObjectOfType<PlayerMovement>();
        cpSRCollider.enabled = false;
        bpSRCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn() 
    {
        gamePlayer.gameObject.SetActive(false);
        bpSRCollider.enabled = false;
        bpSR.sprite = blockDisabled;
        gamePlayer.transform.position = gamePlayer.respawnPoint;
        playerhealth.currentHealth = 3;
        gamePlayer.gameObject.SetActive(true);
    }

    public void Checkpoint()
    {
        cpSR.sprite = blockEnabled;
        cpSRCollider.enabled = true;
    }

    public void Blockpoint()
    {
        bpSR.sprite = blockEnabled;
        bpSRCollider.enabled = true;
    }

}
