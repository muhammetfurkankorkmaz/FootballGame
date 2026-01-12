using CameraShake;
using System.Collections;
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

    public int leftPlayerScore { get; private set; } = 0;
    public int rightPlayerScore { get; private set; } = 0;

    [SerializeField] UýManager UIManagerScript;
    [SerializeField] UIEndScreen UIEndScreenScript;


    bool isGameEnded = false;


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
            if (GameTimer >= 30000)//Checks if it is even
            {
                if (isGameEnded) return;
                if (leftPlayerScore == rightPlayerScore)
                {
                    isGameEnded = true;
                    UIManagerScript.EnableGoldenGoalText();
                }
                else
                {
                    GameEnd();
                }
            }
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
        //StopGame();
        isGameStopped = true;
        CameraShaker.Presets.Explosion2D(2, 5, 1.1f);
        if (isGameEnded)
        {
            if (teamType == TeamType.left)
            {
                rightPlayerScore++;
            }
            else
            {
                leftPlayerScore++;
            }
            GameEnd();
            return;
        }
        if (UIManagerScript == null) return;
        UIManagerScript.EnableScoreText(teamType);
        StartCoroutine(ResetCoroutine(teamType));
    }

    IEnumerator ResetCoroutine(TeamType teamType)
    {
        //StopGame();
        yield return new WaitForSecondsRealtime(2f);
        GameStartAfterGoal(teamType);
    }

    void GameStart()
    {
        currentBall = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        currentLeftPlayer = Instantiate(leftPlayerPrefab, leftStartPosition, Quaternion.identity);
        currentRightPlayer = Instantiate(rightPlayerPrefab, leftStartPosition, Quaternion.Euler(0, 0, 0));

        currentLeftPlayer.transform.position = leftStartPosition;
        currentRightPlayer.transform.position = rightStartPosition;
    }


    void GameStartAfterGoal(TeamType teamType)
    {
        Time.timeScale = 1;
        isGameStopped = false;
        print(teamType + " second");
        if (teamType == TeamType.right)
        {
            leftPlayerScore++;

            currentLeftPlayer.transform.position = leftStartPosition;
            currentRightPlayer.transform.position = rightAfterGoalPosition;
            currentLeftPlayer.GetComponent<CharacterController>().ResetCharacter(Quaternion.Euler(0, 0, 0));
            currentRightPlayer.GetComponent<CharacterController>().ResetCharacter(Quaternion.Euler(0, 0, 180));
            currentBall.GetComponent<Ball>().ResetBall();
            currentBall.transform.position = Vector3.zero;
        }
        else
        {
            rightPlayerScore++;

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
        isGameStopped = true;
        if (leftPlayerScore > rightPlayerScore)
        {
            UIEndScreenScript.EnableEndScreen(TeamType.left);
        }
        else if (rightPlayerScore > leftPlayerScore)
        {
            UIEndScreenScript.EnableEndScreen(TeamType.right);

        }
        else
        {
            //Draw exception
        }

        //Return to main menu after some time
    }

}//Class
