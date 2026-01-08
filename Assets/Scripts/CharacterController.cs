using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //This is a character controller script 
    [Header("ControlScheme")]
    [SerializeField] ControlButtons controlButton;
    [SerializeField] GameObject ballParentObject;
    [Header("Variables")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationStepTime = 0.1f;
    [SerializeField] float slowAmountOnDashPrepare;

    float rotationTimer;
    float rotationDuration;
    Quaternion startRotation;
    Quaternion targetRotation;
    bool isRotating;
    float currentTargetAngle;

    Vector3 lastMoveDir;
    bool hasLastMoveDir;

    float xInput;
    float yInput;

    bool isBallCaptured = false;

    bool isSettingDash = false;
    bool hasDashRotationSpeedRecalculated = false;

    Ball currentBallScript;

    Rigidbody2D rb;


    bool isDashing = false;
    float dashTimer;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }


    void Update()
    {
        InputManager();
        if (isSettingDash)
        {
            dashTimer += Time.deltaTime;
        }
        //MovementManager();
    }
    private void FixedUpdate()
    {
        MovementManager();
    }
    void LateUpdate()
    {
        if (!isRotating) return;

        if (!hasDashRotationSpeedRecalculated && isSettingDash)
        {
            hasDashRotationSpeedRecalculated = true;
            isRotating = false;
        }
        else if (hasDashRotationSpeedRecalculated && !isSettingDash)
        {
            hasDashRotationSpeedRecalculated = false;
            isRotating = false;
        }

        rotationTimer += Time.deltaTime;
        float t = rotationDuration > 0f ? rotationTimer / rotationDuration : 1f;

        transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
    void InputManager()
    {
        yInput = Input.GetKey(controlButton.upKeyCode) ? 1 : Input.GetKey(controlButton.downKeyCode) ? -1 : 0;
        xInput = Input.GetKey(controlButton.rightKeyCode) ? 1 : Input.GetKey(controlButton.leftKeyCode) ? -1 : 0;

        if (Input.GetKey(controlButton.dashKeyCode))
        {
            isSettingDash = true;
        }
        if (Input.GetKeyUp(controlButton.dashKeyCode))
        {
            isSettingDash = false;
            DashRelease();
        }
    }
    void MovementManager()
    {
        Vector2 input = new Vector2(xInput, yInput);

        // Record direction whenever input exists
        if (input.sqrMagnitude > 0.0001f)
        {
            lastMoveDir = input.normalized;
            hasLastMoveDir = true;
        }

        // If we have no direction yet, do nothing
        if (!hasLastMoveDir)
            return;

        // Movement ONLY when input exists (unchanged behavior)
        if (input.sqrMagnitude > 0.0001f)
        {
            if (isSettingDash)
            {
                rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime * slowAmountOnDashPrepare);
            }
            else
            {
                rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime);

            }
        }

        // Rotation ALWAYS uses last known direction
        float snappedAngle = GetSnappedAngle(lastMoveDir);

        if (!isRotating || Mathf.Abs(Mathf.DeltaAngle(currentTargetAngle, snappedAngle)) > 0.1f)
        {
            float currentAngle = transform.eulerAngles.z;
            StartRotation(currentAngle, snappedAngle);
        }
    }


    void StartRotation(float currentAngle, float targetAngle)
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, targetAngle);



        rotationDuration = GetRotationDuration(currentAngle, targetAngle);
        rotationTimer = 0f;

        isRotating = true;
        currentTargetAngle = targetAngle;
    }

    void DashRelease()
    {

        if (isBallCaptured)
        {
            ShootBall();
        }
        else
        {
            Dash();
        }
        dashTimer = 0;
    }

    void ShootBall()
    {
        //Vector3 dir = transform.position - ballParentObject.transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //Vector3 final = new Vector3(0, 0, angle);
        //print(final);

        currentBallScript.ShootBall(transform.localEulerAngles);
        print(transform.localEulerAngles);
        isBallCaptured = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        //Vector2 shootDir = transform.right;

        //currentBallScript.ShootBall(shootDir);
        //isBallCaptured = false;
    }

    void Dash()
    {
        isDashing = true;
    }

    float GetSnappedAngle(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Mathf.Round(angle / 45f) * 45f;
    }
    float GetRotationDuration(float currentAngle, float targetAngle)
    {
        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
        if (isSettingDash)
        {
            return (angleDelta / 45f) * 0.25f * (1 / slowAmountOnDashPrepare);
        }
        else
        {
            return (angleDelta / 45f) * 0.25f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //if (!isDashing) return;
            isBallCaptured = true;
            currentBallScript = collision.gameObject.GetComponent<Ball>();
            currentBallScript.CaptureBall(ballParentObject.transform);
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}//Class
