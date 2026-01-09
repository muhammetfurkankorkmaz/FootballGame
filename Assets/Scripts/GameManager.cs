using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager is empty!!!");

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    public float GameTimer { get; private set; } = 0;
    public bool isGameStopped = false;

    [SerializeField] GameObject leftPlayerPrefab;
    [SerializeField] GameObject rightPlayerPrefab;
    [SerializeField] GameObject ballPrefab;

    [SerializeField] Vector3 leftStartPosition;
    [SerializeField] Vector3 rightStartPosition;
    [SerializeField] Vector3 leftAfterGoalPosition;
    [SerializeField] Vector3 rightAfterGoalPosition;

    GameObject currentLeftPlayer;
    GameObject currentRightPlayer;
    GameObject currentBall;




    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStopped)
        {
            GameTimer += Time.deltaTime;
        }
        //if (GameTimer >= 5)
        //{

        //    GameStartAfterGoal(TeamType.right);
        //    GameTimer = 0;
        //}
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void TeamScore(TeamType teamType)
    {
        StopGame();
        GameStartAfterGoal(teamType);
    }

    void GameStart()
    {
        currentBall = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        currentLeftPlayer = Instantiate(leftPlayerPrefab, leftStartPosition, Quaternion.identity);
        currentRightPlayer = Instantiate(rightPlayerPrefab, leftStartPosition, Quaternion.Euler(0, 0, 180));

        currentLeftPlayer.transform.position = leftStartPosition;
        currentRightPlayer.transform.position = rightStartPosition;
    }

    void GameStartAfterGoal(TeamType teamType)
    {
        if (teamType == TeamType.left)
        {
            currentLeftPlayer.transform.position = leftStartPosition;
            currentRightPlayer.transform.position = rightAfterGoalPosition;
            currentLeftPlayer.GetComponent<CharacterController>().ResetCharacter(Quaternion.Euler(0,0,0));
            currentRightPlayer.GetComponent<CharacterController>().ResetCharacter(Quaternion.Euler(0, 0, 180));
            currentBall.GetComponent<Ball>().ResetBall();
            currentBall.transform.position = Vector3.zero;
        }
        else
        {

            currentLeftPlayer.transform.position = leftAfterGoalPosition;
            currentRightPlayer.transform.position = rightStartPosition;
            currentLeftPlayer.GetComponent<CharacterController>().ResetCharacter(Quaternion.Euler(0, 0, 0));
            currentRightPlayer.GetComponent<CharacterController>().ResetCharacter(Quaternion.Euler(0, 0, 180));
            currentBall.GetComponent<Ball>().ResetBall();
            currentBall.transform.position = Vector3.zero;
        }
    }

    void GameEnd()
    {

    }

}//Class
