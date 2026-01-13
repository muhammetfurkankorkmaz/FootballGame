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

    [SerializeField] TextMeshProUGUI blueExplanationText;
    [SerializeField] TextMeshProUGUI redExplanationText;

    [SerializeField] TextMeshProUGUI chooseOrderText;

    [SerializeField] Sprite[] faces;
    [SerializeField] string[] explanations;
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
}//Class
