using UnityEngine;

public class Ball : MonoBehaviour
{
    CircleCollider2D col;
    Transform owner;

    [SerializeField] float followLerp = 0.2f;
    //[SerializeField] Vector2 localOffset = new Vector2(2.5f, 2.5f);
    [SerializeField] float forwardOffset = 0.01f;
    [SerializeField] float sideOffset = 0.01f; // optional, usually 0

    bool isBallCaptured = false;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (owner == null) return;

        transform.position =
            owner.position +
            owner.right * forwardOffset +
            owner.up * sideOffset;
    }



    public void CaptureBall(Transform newOwner)
    {
        isBallCaptured = true;
        owner = newOwner;
        col.enabled = false;
    }

    public void ReleaseBall()
    {
        isBallCaptured = false;
        owner = null;
        col.enabled = true;
    }

    
}
