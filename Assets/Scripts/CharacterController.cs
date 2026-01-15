using CameraShake;
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

    [SerializeField] GameObject particle;

    [SerializeField] TeamType teamType;
    [SerializeField] SpriteRenderer faceSr;
    [SerializeField] Sprite[] faces;

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
    public Vector2 DashDirection { get; private set; }

    SoundManager SM;
    //CharacterAnimation chAnim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //chAnim = GetComponent<CharacterAnimation>();
        if (teamType == TeamType.left)
        {
            faceSr.sprite = faces[PlayerPrefs.GetInt("blueface")];
        }
        else
        {
            faceSr.sprite = faces[PlayerPrefs.GetInt("redface")];
        }
    }
    private void Start()
    {
        SM = SoundManager.Instance;
    }
    void Update()
    {
        if (GameManager.Instance.isGameStopped) return;
        InputManager();
        if (isSettingDash)
        {
            dashSetTimer += Time.deltaTime;
        }

        if (isDashing)
        {
            isSettingDash = false;
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= maxDashDuration)
            {
                dashingTimer = 0;
                isDashing = false;
                dashExtraSpeedMultiplier = 1;
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

        ballParentObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            ballParentObject.transform.rotation = targetRotation;
            isRotating = false;
        }
    }
    void InputManager()
    {
        if (isDashing || isStunned) return;
        yInput = Input.GetKey(controlButton.upKeyCode) ? 1 : Input.GetKey(controlButton.downKeyCode) ? -1 : 0;
        xInput = Input.GetKey(controlButton.rightKeyCode) ? 1 : Input.GetKey(controlButton.leftKeyCode) ? -1 : 0;

        if (Input.GetKey(controlButton.dashKeyCode) && !isDashing)
        {
            isSettingDash = true;

        }
        if (Input.GetKeyUp(controlButton.dashKeyCode))
        {
            isSettingDash = false;
            DashRelease();
            //chAnim.ChangeCharacterState(CharacterState.normal, 0);
        }
    }
    void MovementManager()
    {
        Vector2 input = new Vector2(xInput, yInput);

        if (input.sqrMagnitude > 0.0001f)
        {
            lastMoveDir = input.normalized;
            hasLastMoveDir = true;
        }

        if (!hasLastMoveDir)
        {
            return;
        }

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

        float snappedAngle = GetSnappedAngle(lastMoveDir);

        if (!isRotating || Mathf.Abs(Mathf.DeltaAngle(currentTargetAngle, snappedAngle)) > 0.1f)
        {
            float currentAngle = ballParentObject.transform.eulerAngles.z;
            StartRotation(currentAngle, snappedAngle);
        }
    }

    void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void StartRotation(float currentAngle, float targetAngle)
    {
        startRotation = ballParentObject.transform.rotation;
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
            SM.PlayBallKickSound();
            ShootBall();
        }
        else
        {
            SM.PlayDashSound(teamType);
            Dash();
        }
        dashSetTimer = 0;
    }

    void ShootBall()
    {
        float chargePercent = Mathf.Clamp01(dashSetTimer / maxDashDuration);

        currentBallScript.ShootBall(ballParentObject.transform.localEulerAngles, chargePercent);
        isBallCaptured = false;
        ResetVelocity();
    }

    void Dash()
    {
        isSettingDash = false;
        isDashing = true;

        DashDirection = hasLastMoveDir ? lastMoveDir.normalized : Vector2.right;
        float chargePercent = Mathf.Clamp01(dashSetTimer / maxDashDuration);
        //chAnim.ChangeCharacterState(CharacterState.dashing, chargePercent);
        //print(DashDirection);



        dashExtraSpeedMultiplier = Mathf.Lerp(
            1.2f,
            4,
            chargePercent
        );
    }
    void EndDash()
    {
        if (!hasHittedEnemy)
        {
            if (isBallCaptured) return;
            Stun();
        }
        dashExtraSpeedMultiplier = 1;
        isDashing = false; // Always ensure this is false when EndDash is called
        dashingTimer = 0;
    }
    public void Stun()
    {
        isStunned = true;
        isSettingDash = false;
        ResetVelocity();
        xInput = 0;
        yInput = 0;
    }

    public Ball StealBall()
    {
        isSettingDash = false;
        isBallCaptured = false;
        ResetVelocity();
        return currentBallScript;
    }

    public void ResetCharacter(Quaternion _targetRotation)
    {
        isSettingDash = false;
        isDashing = false;
        dashExtraSpeedMultiplier = 1;
        dashingTimer = 0;
        dashSetTimer = 0;
        isBallCaptured = false;
        xInput = 0;
        yInput = 0;

        ballParentObject.transform.rotation = _targetRotation;
        startRotation = _targetRotation;
        targetRotation = _targetRotation;
        currentTargetAngle = _targetRotation.eulerAngles.z;

        isRotating = false;
        rotationTimer = 0f;
        rotationDuration = 0f;

        hasLastMoveDir = false;
        lastMoveDir = Vector3.zero;

        ResetVelocity();
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
        //print("00000000");
        if (collision.gameObject.CompareTag("Ball"))
        {
            //if (!isDashing) return;
            //print("11111");
            isBallCaptured = true;

            currentBallScript = collision.gameObject.GetComponent<Ball>();
            currentBallScript.CaptureBall(ballParentObject.transform);
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            SM.PlayCaptureSound();
        }
        if (isDashing && collision.gameObject.CompareTag("Player"))
        {
            if (!isColisionWithOtherPlayerOpen) return;
            Instantiate(particle, transform.position, Quaternion.identity);

            CharacterController chController = collision.gameObject.GetComponent<CharacterController>();
            hasHittedEnemy = true;

            if (chController.isBallCaptured)
            {
                SM.PlayBallStealSound();
                chController.Stun();
                CameraShaker.Presets.ShortShake2D(0.1f, 0.12f, 25, 8);
                Ball currentBall = chController.StealBall();
                if (currentBallScript == null)
                {
                    currentBallScript = currentBall;
                    currentBall.CaptureBall(ballParentObject.transform);
                }
                else
                {
                    currentBallScript.CaptureBall(ballParentObject.transform);
                }
                isBallCaptured = true;
            }
            else//Both of them doesn't have the ball
            {
                SoundManager.Instance.PlayCollideSound();
                CameraShaker.Presets.ShortShake2D(0.06f, 0.08f, 30, 5);
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
