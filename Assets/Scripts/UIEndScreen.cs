using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreen : MonoBehaviour
{
    [SerializeField] GameObject endScreenObject;
    [SerializeField] Image topCircleImage;
    [SerializeField] Image bottomCircleImage;
    [SerializeField] TextMeshProUGUI endText;

    [SerializeField] Image topFace;
    [SerializeField] Image bottomFace;
    [SerializeField] Sprite[] faces;

    [Header("Animation Settings")]
    [SerializeField] RectTransform topCircle;
    [SerializeField] RectTransform bottomCircle;
    [SerializeField] float jumpHeight = 120f;

    [Header("Win Text Idle Shake")]
    [SerializeField] float idleShakeAngle = 3f;
    [SerializeField] float idleShakeSpeed = 6f;

    float idleShakeTime;


    bool isAnimationPlaying = false;
    bool hasPlayedAnimation = false;

    private void Update()
    {
        if (hasPlayedAnimation && !isAnimationPlaying)
        {
            StartCoroutine(EndScreenAnimation());
        }
    }
    void LateUpdate()
    {
        if (!endScreenObject.activeSelf || endText == null)
            return;

        idleShakeTime += Time.deltaTime;

        float z = Mathf.Sin(idleShakeTime * idleShakeSpeed) * idleShakeAngle;
        endText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, z);
    }
    public void EnableEndScreen(TeamType _winnerTeam)
    {
        if (_winnerTeam == TeamType.left)
        {
            topCircleImage.color = Color.blue;
            bottomCircleImage.color = Color.red;
            topFace.sprite = faces[PlayerPrefs.GetInt("blueface")];
            bottomFace.sprite = faces[PlayerPrefs.GetInt("redface")];
            endText.color = Color.blue;
            endText.text = "blue wins!";
        }
        else
        {
            topCircleImage.color = Color.red;
            bottomCircleImage.color = Color.blue;
            bottomFace.sprite = faces[PlayerPrefs.GetInt("blueface")];
            topFace.sprite = faces[PlayerPrefs.GetInt("redface")];
            endText.color = Color.red;
            endText.text = "red wins!";
        }
        endScreenObject.SetActive(true);
        StartCoroutine(EndScreenAnimation());
    }


    private IEnumerator EndScreenAnimation()
    {
        yield return StartCoroutine(TopCircleJumpAndLand());
    }
    private IEnumerator TopCircleJumpAndLand()
    {
        isAnimationPlaying = true;
        hasPlayedAnimation = true;

        Vector3 startPos = topCircle.anchoredPosition;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + jumpHeight, 0);
        Vector3 originalScale = Vector3.one;
        Vector3 startScale = topCircle.transform.localScale; ;
        Vector3 stretchTargetScale = new Vector3(0.9f, 1.1f, 1);
        Vector3 squashTargetScale = new Vector3(1.2f, 0.85f, 1);

        float time = 0f;
        float upDuration = 0.3f;

        while (time < upDuration)
        {
            time += Time.deltaTime;
            float t = time / upDuration;
            float easedT = EaseOutSine(t);
            topCircle.localScale = Vector3.Lerp(startScale, stretchTargetScale, easedT);
            topCircle.anchoredPosition = Vector3.Lerp(startPos, targetPos, easedT);
            yield return null;
        }

        time = 0f;
        float downDuration = 0.3f;


        while (time < downDuration)
        {
            time += Time.deltaTime;
            float t = time / downDuration;

            float easedT = EaseInSine(t);
            topCircle.anchoredPosition = Vector3.Lerp(targetPos, startPos, easedT);
            //topCircle.localScale = Vector3.Lerp(stretchTargetScale, squashTargetScale, easedT);
            yield return null;
        }
        time = 0;
        float normalizeDuration = 0.1f;
        StartCoroutine(BottomCircleReaction());

        while (time < normalizeDuration)
        {
            time += Time.deltaTime;
            float t = time / normalizeDuration;
            float easedT = EaseOutSine(t);
            //float easedT = EaseOutBounce(t);
            topCircle.localScale = Vector3.Lerp(stretchTargetScale, squashTargetScale, easedT);
            yield return null;
        }
        time = 0;
        //while (time < normalizeDuration)
        //{
        //    time += Time.deltaTime;
        //    float t = time / normalizeDuration;
        //    //float easedT = EaseOutSine(t);
        //    float easedT = EaseOutBack(t);
        //    topCircle.localScale = Vector3.Lerp(squashTargetScale, originalScale, easedT);
        //    yield return null;
        //}

        topCircle.anchoredPosition = startPos;

        //yield return new WaitForSecondsRealtime(0.25f);

        //topCircle.localScale = originalScale;
        isAnimationPlaying = false;
    }

    private IEnumerator BottomCircleReaction()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.5f, 0.75f, 0);

        float time = 0f;
        float duration = 0.15f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float easedT = EaseOutSine(t);

            bottomCircle.localScale = Vector3.Lerp(originalScale, targetScale, easedT);

            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.1f);
        time = 0;
        duration = 0.4f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float easedT = EaseInSine(t);

            bottomCircle.localScale = Vector3.Lerp(targetScale, originalScale, easedT);

            yield return null;
        }

        bottomCircle.localScale = originalScale;
    }
    float EaseOutSine(float t)
    {
        return Mathf.Sin((t * Mathf.PI) * 0.5f);
    }

    float EaseInSine(float t)
    {
        return 1f - Mathf.Cos((t * Mathf.PI) * 0.5f);
    }

    //float EaseInOutSine(float t)
    //{
    //    return -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;
    //}
    float EaseOutBack(float t, float overshoot = 1.70158f)
    {
        t -= 1f;
        return 1f + (t * t * ((overshoot + 1f) * t + overshoot));
    }

}//Class
