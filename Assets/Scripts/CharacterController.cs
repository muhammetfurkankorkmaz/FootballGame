using System.Collections;
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
    [SerializeField] float maxDashDuration = 1f;
    [SerializeField] float dashExtraSpeedMultiplier = 1f;

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

    public bool isBallCaptured { get; private set; } = false;

    bool isSettingDash = false;
    bool hasDashRotationSpeedRecalculated = false;

    Ball currentBallScript;
    Rigidbody2D rb;

    bool isDashing = false;
    float dashSetTimer;
    float dashingTimer;

    float stunTimer;
    bool isStunned = false;

    bool hasHittedEnemy = false;

    bool isColisionWithOtherPlayerOpen = true;



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
            dashSetTimer += Time.deltaTime;
        }

        if (isDashing)
        {
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= maxDashDuration)
            {
                dashingTimer = 0;
                isDashing = false;
                EndDash();
            }
        }

        if (isStunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= 0.33f)
            {
                isStunned = false;
                stunTimer = 0;
            }
        }
        ResetVelocity();

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
        if (isDashing || isStunned) return;
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
                rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime * dashExtraSpeedMultiplier);

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

    void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
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
        dashSetTimer = 0;
    }

    void ShootBall()
    {
        //Vector3 dir = transform.position - ballParentObject.transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //Vector3 final = new Vector3(0, 0, angle);
        //print(final);
        float chargePercent = Mathf.Clamp01(dashSetTimer / maxDashDuration);

        //dashExtraSpeedMultiplier = Mathf.Lerp(
        //    0,
        //    maxDashDuration,
        //    chargePercent
        //);


        currentBallScript.ShootBall(transform.localEulerAngles, chargePercent);
        isBallCaptured = false;
        ResetVelocity();
        //Vector2 shootDir = transform.right;

        //currentBallScript.ShootBall(shootDir);
        //isBallCaptured = false;
    }

    void Dash()
    {
        isDashing = true;

        float chargePercent = Mathf.Clamp01(dashSetTimer / maxDashDuration);

        dashExtraSpeedMultiplier = Mathf.Lerp(
            1.2f,
            4,
            chargePercent
        );
        //print("Dashed with " + chargePercent * 3.5f);
        //print("Dash timer is " + dashSetTimer);

        //dashExtraSpeedMultiplier = 3.5f * chargePercent;
    }
    void EndDash()
    {
        dashExtraSpeedMultiplier = 1;
        if (!hasHittedEnemy)
        {
            if (isBallCaptured) return;
            Stun();
        }
        //else
        //{
        //    //yes stun
        //}
    }

    public void Stun()
    {
        isStunned = true;
        ResetVelocity();
        xInput = 0;
        yInput = 0;
    }

    public Ball StealBall()
    {
        isBallCaptured = false;
        ResetVelocity();
        //currentBallScript.ReleaseBall();
        return currentBallScript;

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
        print("00000000");
        if (collision.gameObject.CompareTag("Ball"))
        {
            //if (!isDashing) return;
            print("11111");
            isBallCaptured = true;
            currentBallScript = collision.gameObject.GetComponent<Ball>();
            currentBallScript.CaptureBall(ballParentObject.transform);
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (isDashing && collision.gameObject.CompareTag("Player"))
        {
            if (!isColisionWithOtherPlayerOpen) return;
            print("hüloo" + gameObject.name);
            //if (!isDashing) return;
            CharacterController chController = collision.gameObject.GetComponent<CharacterController>();
            hasHittedEnemy = true;
            print("222222");

            if (chController.isBallCaptured)
            {
                //Take over the ball
                chController.Stun();
                Ball currentBall = chController.StealBall();
                if (currentBallScript == null)
                {
                    print("333333333");

                    currentBall.CaptureBall(ballParentObject.transform);
                }
                else
                {
                    print("444444444");

                    currentBallScript.CaptureBall(ballParentObject.transform);
                }
                isBallCaptured = true;
            }
            else//Both of them doesn't have the ball
            {
                print("555555555");

                Stun();
                chController.Stun();
            }
            isColisionWithOtherPlayerOpen = false;
            StartCoroutine(OpenCollisionWithOtherPlayer());
        }
    }

    IEnumerator OpenCollisionWithOtherPlayer()
    {
        yield return new WaitForFixedUpdate();
        isColisionWithOtherPlayerOpen = true;
    }
}//Class
