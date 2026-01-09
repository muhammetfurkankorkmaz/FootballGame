using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D col;
    Transform owner;

    [SerializeField] float followLerp = 0.2f;
    //[SerializeField] Vector2 localOffset = new Vector2(2.5f, 2.5f);
    [SerializeField] float forwardOffset = 0.01f;
    [SerializeField] float sideOffset = 0.01f; // optional, usually 0

    bool isBallCaptured = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (owner == null) return;

        transform.position =
            owner.position +
            owner.right * forwardOffset +
            owner.up * sideOffset;

        ResetVelocity();
    }

    void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void CaptureBall(Transform newOwner)
    {
        isBallCaptured = true;
        owner = newOwner;
        col.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void ReleaseBall()
    {
        isBallCaptured = false;
        owner = null;
        //col.enabled = true;
        StartCoroutine(EnableColliderNextFixedFrame());
    }

    public void ShootBall(Vector3 direction, float chargePercentage)
    {
        ReleaseBall();
        //owner = null;

        transform.localEulerAngles = direction;
        print(transform.localEulerAngles + "hehe");
        rb.AddForce(transform.right * 4 * chargePercentage, ForceMode2D.Impulse);
    }
    IEnumerator EnableColliderNextFixedFrame()
    {
        yield return new WaitForFixedUpdate();
        col.enabled = true;
    }
}//Class
