using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] TeamType teamType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            GameManager.Instance.TeamScore(teamType);
        }
    }
}//Class
