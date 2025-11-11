using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //This is a character controller script 
    [Header("ControlScheme")]
    [SerializeField] ControlButtons controlButton;
    [Header("Variables")]
    [SerializeField] float moveSpeed;

    float xInput;
    float yInput;

    void Start()
    {

    }


    void Update()
    {
        InputManager();
        MovementManager();
    }

    void InputManager()
    {
        yInput = Input.GetKey(controlButton.upKeyCode) ? 1 : Input.GetKey(controlButton.downKeyCode) ? -1 : 0;
        xInput = Input.GetKey(controlButton.rightKeyCode) ? 1 : Input.GetKey(controlButton.leftKeyCode) ? -1 : 0;
    }
    void MovementManager()
    {
        Vector3 input = new Vector3(xInput, yInput, 0);
        if (input.sqrMagnitude < 0.0001f) return;

        Vector3 nextPos = transform.position + input.normalized * moveSpeed * Time.deltaTime;

        transform.position = nextPos;
    }
}//Class
