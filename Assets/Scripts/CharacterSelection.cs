using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    enum SelectionState
    {
        blueChoosing,
        redChoosing,
        bothSelected,
    }

    SelectionState currentSelectionState = SelectionState.blueChoosing;

    [SerializeField] GameObject blueFace;
    [SerializeField] GameObject redFace;

    //[SerializeField] GameObject blueCircle;
    //[SerializeField] GameObject redCircle;
    [SerializeField] GameObject triangleMarker;

    [SerializeField] TextMeshProUGUI blueExplanationText;
    [SerializeField] TextMeshProUGUI redExplanationText;

    [SerializeField] TextMeshProUGUI chooseOrderText;

    [SerializeField] Sprite[] faces;
    [SerializeField] string[] explanations;

    bool isAnimationPlaying;
    bool hasPlayedAnimation;
    private void Update()
    {
        if (!isAnimationPlaying)
        {
            if (currentSelectionState == SelectionState.blueChoosing)
            {
                triangleMarker.transform.position = blueFace.transform.position;
                StartCoroutine(CircleJumpAndLand());
            }
            else if (currentSelectionState == SelectionState.redChoosing)
            {
                StartCoroutine(CircleJumpAndLand());
            }
            else
            {
                if (triangleMarker.activeInHierarchy)
                {
                    triangleMarker.SetActive(false);
                }
            }
        }
    }
    public void CharacterSelectionButton(int buttonNumber)
    {
        if (currentSelectionState == SelectionState.bothSelected) return;
        if (currentSelectionState == SelectionState.blueChoosing)
        {
            Image img = blueFace.GetComponent<Image>();

            Color c = img.color;
            c.a = 1f;
            img.color = c;

            img.sprite = faces[buttonNumber];
            blueExplanationText.text = explanations[buttonNumber];
            currentSelectionState = SelectionState.redChoosing;
            PlayerPrefs.SetInt("blueface", buttonNumber);
            chooseOrderText.text = "reds turn";
            chooseOrderText.color = Color.red;
            triangleMarker.transform.position = redFace.transform.position;
        }
        else if (currentSelectionState == SelectionState.redChoosing)
        {
            Image img = redFace.GetComponent<Image>();

            Color c = img.color;
            c.a = 1f;
            img.color = c;

            img.sprite = faces[buttonNumber];
            redExplanationText.text = explanations[buttonNumber];
            currentSelectionState = SelectionState.bothSelected;
            PlayerPrefs.SetInt("redface", buttonNumber);
            chooseOrderText.text = "start";
            chooseOrderText.color = Color.black;
            chooseOrderText.fontSize = 64;
        }
    }
    public void StartButton()
    {
        if (currentSelectionState != SelectionState.bothSelected) return;
        SceneManager.LoadScene("GameScene");
    }
    private IEnumerator CircleJumpAndLand()
    {
        isAnimationPlaying = true;
        RectTransform rt = triangleMarker.GetComponent<RectTransform>();

        Vector3 startScale = Vector3.one;
        Vector3 pulseScale = Vector3.one * 1.12f;

        float elapsed = 0;
        float duration = 0.4f;

        Vector3 startRotation = rt.localEulerAngles;


        Vector3 targetRotation = startRotation + new Vector3(0, 0, 90);


        // Rotate + scale up
        while (elapsed <= duration)
        {
            float t = elapsed / duration;

            rt.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            rt.localScale = Vector3.Lerp(startScale, pulseScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0;

        // Scale back down
        while (elapsed <= 0.2f)
        {
            float t = elapsed / duration;
            elapsed += Time.deltaTime;
            rt.localScale = Vector3.Lerp(pulseScale, startScale, t);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.08f);
        rt.localScale = startScale;
        rt.localEulerAngles = targetRotation;
        isAnimationPlaying = false;
    }

}//Class
