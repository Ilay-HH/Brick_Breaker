using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //addsounds then add title screen then make into an app
    private int ballCounter;
    private bool isGameOver = false;

    [SerializeField]
    int spawnOffSet = 40;
    [SerializeField]
    int lowerSpawnTime = -2;


    private Vector3 lengthOffSet = new Vector3(1.6f, 0, 0);
    private Vector3 heightOffSet = new Vector3(0, -0.6f, 0);
    private Vector3 initialSpawn = new Vector3(-7.4f, 4.2f, 0);
    private int rowSpawns = 9;

    [SerializeField]
    private GameObject[] spawnables;
    [SerializeField]
    private BallMovement ballMovement;

    private List<GameObject> bricks;

    private UIManager uiManager;
    // Start is called before the first frame update
    private void Awake()
    {
        bricks = new List<GameObject>();
        uiManager = FindObjectOfType<Canvas>().GetComponent<UIManager>();
        ballMovement.isFirstBall = true;
    }
    public void StartGame()
    {
        StartCoroutine(BrickSpawnRoutine());
    }
    void Start()
    {
        ballCounter = 1;

    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); 
    }
    void SpawnManager()
    {
        GameObject[] liveBricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach(GameObject brick in liveBricks)
        {
            brick.transform.position += heightOffSet;
            if (brick.transform.position.y <= -4.2f)
            {
                GameOverSequence();
            }
        }
    }
    private void LevelUp(int level)
    {
        GameObject ironBrick = spawnables[0];
        spawnables[level] = ironBrick;
    }
    IEnumerator BrickSpawnRoutine()
    {
        int rounds = 0;
        int level = 0;
        while (true)
        {
            rounds++;
            if (rounds % 5 == 0)
            {
                level++;
                LevelUp(level);
            }

            SpawnManager();
            for (int i = 0; i <= rowSpawns; i++)
            {
                int whichBrick = Random.Range(0, spawnables.Length);
                GameObject g = Instantiate(spawnables[whichBrick], initialSpawn + lengthOffSet * i, Quaternion.identity);
                g.transform.parent = GameObject.Find("Bricks").transform;
            }
            yield return new WaitForSeconds(spawnOffSet);
            spawnOffSet += lowerSpawnTime;
            if (spawnOffSet <= 10)
            {
                spawnOffSet = 10;
            }
        }

    }
    public void AddBalls(int n)
    {
        ballCounter += n;
    }
    public void RemoveBall()
    {
        ballCounter--;
        if (ballCounter == 0)
        {
            GameOverSequence();
        }
    }
    public void AddBrick(GameObject brick)
    {
        bricks.Add(brick);
    }
    public void RemoveBrick(GameObject brick)
    {
        bricks.Remove(brick);
    }
    public void GameOverSequence()
    {
        uiManager.GameOverRoutine();
        isGameOver = true;
        Time.timeScale = 0;
    }
}
