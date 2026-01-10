using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UıManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameTimerText;
    [SerializeField] TextMeshProUGUI leftPlayerScoreText;
    [SerializeField] TextMeshProUGUI rightPlayerScoreText;
    void Start()
    {

    }

    void Update()
    {
        ChangeText();
        leftPlayerScoreText.text = GameManager.Instance.leftPlayerScore.ToString();
        rightPlayerScoreText.text = GameManager.Instance.rightPlayerScore.ToString();
    }
    void ChangeText()
    {
        if (gameTimerText == null) return;
        gameTimerText.text = string.Format("{0}:{1:00}",
    Mathf.FloorToInt(GameManager.Instance.GameTimer / 60),
    Mathf.FloorToInt(GameManager.Instance.GameTimer % 60));
    }
}//Class

