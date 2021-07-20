using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick_Controller : MonoBehaviour
{
    [SerializeField]
    GameObject[] capsules;
    int brickHealth;
    int scoreForDestruction;
    private UIManager uiManager;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int id;
    [SerializeField]
    Sprite damagedBrick;

    private AudioSource audioSource;

    GameController gameController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("Game Manager").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiManager = FindObjectOfType<Canvas>().GetComponent<UIManager>();

        gameController.AddBrick(gameObject);
        switch (id)
        {
            case 0:
                brickHealth = 2;
                scoreForDestruction = 20;
                break;
            case 1:
                brickHealth = 1;
                scoreForDestruction = 10;
                break;
        }
    }
    
    private void TakeDamage()
    {
        audioSource.Play();
        brickHealth--;
        if (brickHealth == 1)
        {
            spriteRenderer.sprite = damagedBrick;
        }
        else if (brickHealth == 0)
        {
            uiManager.ScoreUp(scoreForDestruction);
            CapsuleDrop();
            gameController.RemoveBrick(gameObject);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Laser"))
        {
            Destroy(collision.gameObject);
            TakeDamage();
        }

    }
    private void CapsuleDrop()
    {
        int chance = Random.Range(1, 8);
        int capsuleID = Random.Range(0, 7);

        if(chance == 2)
        {
            Instantiate(capsules[capsuleID], transform.position, Quaternion.identity);
        }
    }
}