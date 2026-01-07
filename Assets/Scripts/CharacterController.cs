using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //This is a character controller script 
    [Header("ControlScheme")]
    [SerializeField] ControlButtons controlButton;
    [Header("Variables")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationStepTime = 0.1f;

    float rotationTimer;
    float rotationDuration;
    Quaternion startRotation;
    Quaternion targetRotation;
    bool rotating;
    float currentTargetAngle;

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
    void LateUpdate()
    {
        if (!rotating) return;

        rotationTimer += Time.deltaTime;
        float t = rotationDuration > 0f ? rotationTimer / rotationDuration : 1f;

        transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            transform.rotation = targetRotation;
            rotating = false;
        }
    }
    void InputManager()
    {
        yInput = Input.GetKey(controlButton.upKeyCode) ? 1 : Input.GetKey(controlButton.downKeyCode) ? -1 : 0;
        xInput = Input.GetKey(controlButton.rightKeyCode) ? 1 : Input.GetKey(controlButton.leftKeyCode) ? -1 : 0;
    }
    void MovementManager()
    {
        Vector2 input = new Vector2(xInput, yInput);
        if (input.sqrMagnitude < 0.0001f) return;

        Vector3 moveDir = input.normalized;
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;

        float snappedAngle = GetSnappedAngle(moveDir);

        if (!rotating || Mathf.Abs(Mathf.DeltaAngle(currentTargetAngle, snappedAngle)) > 0.1f)
        {
            float currentAngle = transform.eulerAngles.z;
            StartRotation(currentAngle, snappedAngle);
        }
    }

    void StartRotation(float currentAngle, float targetAngle)
    {
        startRotation = transform.rotation; // current interpolated rotation
        targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        rotationDuration = GetRotationDuration(currentAngle, targetAngle);
        rotationTimer = 0f;

        rotating = true;
        currentTargetAngle = targetAngle;
    }
    float GetSnappedAngle(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Mathf.Round(angle / 45f) * 45f;
    }
    float GetRotationDuration(float currentAngle, float targetAngle)
    {
        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
        return (angleDelta / 45f) * 0.25f;
    }


}//Class
