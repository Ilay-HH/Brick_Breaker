using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    private float movementSpeed = 3;
    private float extraSpeedPerHit = 0.2f;
    private float maxExtraSpeed = 9;
    private int hitCounter;
    private float speed;

    private Vector3 oldPosition = Vector3.zero;
    private float checkedSpeed;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    public bool isFirstBall = false;
    private Vector2 direction = new Vector2(0,0);

    [SerializeField]
    private Text countdownText;

    GameController gameController;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameController = GameObject.Find("Game Manager").GetComponent<GameController>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        switch (isFirstBall)
        {  
            case true:
                StartCoroutine(StartBall());
                break;
        }
    }
    private void FixedUpdate()
    {
        
        if ((transform.position.x >= 8.64f) || (transform.position.x <= -8.64f))
        {
            IncreaseHitCounter();
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            audioSource.Play();
        }
        else if (transform.position.y >= 4.738f)
        {
            IncreaseHitCounter();
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            audioSource.Play();
        }
        else if (transform.position.y <= -5.4f)
        {
            gameController.RemoveBall();
            Destroy(gameObject);
        }
        else if (transform.position.y >= 5)
        {
            Destroy(gameObject);
        }
        DirectionCheck();
        SpeedCheck();

    }
    private void SpeedCheck()
    {
        checkedSpeed = Vector3.Distance(oldPosition, transform.position) * 100f;
        if (checkedSpeed != speed)
        {
            MoveBall(rb.velocity);
        }
        oldPosition = transform.position;
    }
    private void DirectionCheck()
    {
        if (rb.velocity.y == 0 && !isFirstBall)
        {
            rb.velocity = new Vector2(rb.velocity.x, -0.3f);
        }
    }

    public IEnumerator StartBall()
    {
        transform.position = new Vector3(0, 0, 0);
        hitCounter = 0;
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);

        gameController.StartGame();
        MoveBall(new Vector2(0, -1));
        yield return new WaitForSeconds(2);
        isFirstBall = false;
    }
    public void MoveBall(Vector2 dir)
    {
        dir = dir.normalized;

        speed = movementSpeed + extraSpeedPerHit * hitCounter;

        
        rb.velocity = dir * speed;
    }
    public void IncreaseHitCounter()
    {
        if (hitCounter * extraSpeedPerHit <= maxExtraSpeed)
        {
            hitCounter++;
        }
    }
    void BounceFromRacket(Collision2D c)
    {
        Vector3 ballPosition = transform.position;
        Vector3 racketPosition = c.gameObject.transform.position;

        float racketLength = c.collider.bounds.size.x;
        float y = 1;
        float x = (ballPosition.x - racketPosition.x) / racketLength;

        IncreaseHitCounter();
        MoveBall(new Vector2(x, y));
        audioSource.Play();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IncreaseHitCounter();
        audioSource.Play();
        if (collision.gameObject.tag.Equals("Player"))
        {
            BounceFromRacket(collision);
        }
    }
}
