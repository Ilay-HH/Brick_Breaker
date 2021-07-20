using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.05f;
    [SerializeField]
    private int powerUpTime = 8; //change later

    [SerializeField]
    private Button attackButton;

    private Rigidbody2D rb;
    private UIManager uiManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameController gameController;
    private AudioSource audioSource;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1;

    

    private Sprite normalRacket;
    [SerializeField]
    private Sprite smallerRacket;
    [SerializeField]
    private Sprite largerRacket;
    [SerializeField]
    private Sprite blasterRacket;
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private GameObject blasterShotPrefab;

    Vector3 mousePosition;
    Vector2 position = new Vector2(0, 0);

    private bool isBlasting = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("Game Manager").GetComponent<GameController>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        uiManager = FindObjectOfType<Canvas>().GetComponent<UIManager>();
        normalRacket = spriteRenderer.sprite;
        attackButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         mousePosition = Input.mousePosition;

        if (Input.touchCount > 1)
        {
            if (Input.touches[0].position.x > Input.touches[1].position.x)
            {
                mousePosition = Input.touches[0].position;
            }
            else
            {
                mousePosition = Input.touches[1].position;
            }
        }

         mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
         position = Vector2.Lerp(transform.position, mousePosition, speed);
    }
    private void FixedUpdate()
    {
        PlayerMovement(position);
    }
    public void Fire()
    {
        if (isBlasting && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            GameObject g = Instantiate(blasterShotPrefab, transform.position, Quaternion.identity);
            Destroy(g, 3);
        }
    }
    private void PlayerMovement(Vector3 pos)
    {

        
        if (pos.x >= 7.9f)
        {
            pos = new Vector3(7.9f, -4.4f, 0);
        }
        else if (pos.x <= -7.9f)
        {
            pos = new Vector3(-7.9f, -4.4f, 0);
        }
        
        rb.MovePosition(pos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Capsule"))
        {
            switch (collision.gameObject.GetComponent<Capsule_Controller>().GetID())
            {
                case 0:
                    uiManager.ScoreUp(100);
                    break;
                case 1:
                    StartCoroutine(SlowPowerRoutine());
                    break;
                case 2:
                    StartCoroutine(FastPowerRoutine());
                    break;
                case 3:
                    StartCoroutine(SmallerPowerRoutine());
                    break;
                case 4:
                    StartCoroutine(LargerPowerRoutine());
                    break;
                case 5:
                    MultiBallPower();
                    break;
                case 6:
                    StartCoroutine(BlasterPowerRoutine());
                    break;
            }
            audioSource.Play();
            Destroy(collision.gameObject);
        }
    }
    IEnumerator SlowPowerRoutine()
    {
        speed = 2.5f;
        yield return new WaitForSeconds(powerUpTime);
        speed = 5;
    }
    IEnumerator FastPowerRoutine()
    {
        speed = 0.1f;
        yield return new WaitForSeconds(powerUpTime);
        speed = 0.05f;
    }
    IEnumerator SmallerPowerRoutine()
    {
        animator.enabled = false;
        spriteRenderer.sprite = smallerRacket;
        yield return new WaitForSeconds(powerUpTime);
        spriteRenderer.sprite = normalRacket;
        animator.enabled = true;
    }
    IEnumerator LargerPowerRoutine()
    {
        animator.enabled = false;
        spriteRenderer.sprite = largerRacket;
        yield return new WaitForSeconds(powerUpTime);
        spriteRenderer.sprite = normalRacket;
        animator.enabled = true;
    }
    void MultiBallPower()
    {
        GameObject[] newBalls = new GameObject[3]
        {
            Instantiate(ballPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity),
            Instantiate(ballPrefab, transform.position + new Vector3(0.7f, 0.5f, 0), Quaternion.Euler(0, 0, -45)),
            Instantiate(ballPrefab, transform.position + new Vector3(-0.7f, 0.5f, 0), Quaternion.Euler(0, 0, 45))
        };
        newBalls[0].GetComponent<BallMovement>().MoveBall(new Vector2(0, 1));
        newBalls[1].GetComponent<BallMovement>().MoveBall(new Vector2(1, 1));
        newBalls[2].GetComponent<BallMovement>().MoveBall(new Vector2(-1, 1));

        gameController.AddBalls(3);
        
    }
    IEnumerator BlasterPowerRoutine()
    {
        isBlasting = true;
        attackButton.gameObject.SetActive(true);
        animator.enabled = false;
        spriteRenderer.sprite = blasterRacket;
        yield return new WaitForSeconds(powerUpTime);
        spriteRenderer.sprite = normalRacket;
        animator.enabled = true;
        attackButton.gameObject.SetActive(false);
        isBlasting = false;
    }
}
