using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UıManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameTimerText;
    [SerializeField] TextMeshProUGUI leftPlayerScoreText;
    [SerializeField] TextMeshProUGUI rightPlayerScoreText;

    [SerializeField] GameObject scoreTextObject;
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
    }

    void Update()
    {
        ChangeText();
        leftPlayerScoreText.text = GameManager.Instance.leftPlayerScore.ToString();
        rightPlayerScoreText.text = GameManager.Instance.rightPlayerScore.ToString();
    }

    public void EnableScoreText(TeamType teamType)
    {
        scoreText.gameObject.SetActive(true);

        if (teamType == TeamType.left)
        {
            scoreText.color = Color.red;
            scoreText.text = "RED SCORES";
        }
        else
        {
            scoreText.color = Color.blue;
            scoreText.text = "BLUE SCORES";
        }

        StopAllCoroutines();
        StartCoroutine(PlayScoreTextAnimation());
    }
    IEnumerator CloseScoreText()
    {
        yield return new WaitForSecondsRealtime(2f);
        scoreTextObject.SetActive(false);
    }
    IEnumerator PlayScoreTextAnimation()
    {
        RectTransform rect = scoreText.rectTransform;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startPos + Vector2.up * 80f;

        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        Color startColor = scoreText.color;
        startColor.a = 1f;
        scoreText.color = startColor;

        float time = 0f;
        float popDuration = 0.25f;
        float floatDuration = 1.2f;

        rect.localScale = startScale;

        // POP IN
        while (time < popDuration)
        {
            time += Time.deltaTime;
            float t = time / popDuration;
            float eased = EaseOutBack(t);

            rect.localScale = Vector3.Lerp(startScale, targetScale, eased);
            yield return null;
        }

        rect.localScale = targetScale;

        // FLOAT + FADE
        time = 0f;
        while (time < floatDuration)
        {
            time += Time.deltaTime;
            float t = time / floatDuration;

            rect.anchoredPosition = Vector2.Lerp(
                startPos,
                targetPos,
                EaseOutSine(t)
            );

            Color c = scoreText.color;
            c.a = Mathf.Lerp(1f, 0f, EaseInSine(t));
            scoreText.color = c;

            yield return null;
        }

        rect.anchoredPosition = startPos;
        scoreText.gameObject.SetActive(false);
        StartCoroutine(CloseScoreText());
    }
    float EaseOutSine(float t)
    {
        return Mathf.Sin(t * Mathf.PI * 0.5f);
    }

    float EaseInSine(float t)
    {
        return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
    }

    float EaseOutBack(float t, float overshoot = 1.8f)
    {
        t -= 1f;
        return 1f + (t * t * ((overshoot + 1f) * t + overshoot));
    }
    void ChangeText()
    {
        if (gameTimerText == null) return;
        gameTimerText.text = string.Format("{0}:{1:00}",
    Mathf.FloorToInt(GameManager.Instance.GameTimer / 60),
    Mathf.FloorToInt(GameManager.Instance.GameTimer % 60));
    }
}//Class

