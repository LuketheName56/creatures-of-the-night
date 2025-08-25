using UnityEngine;

public class Parallax : MonoBehaviour
{
    float distance;
    public Vector2 startPosition = Vector2.zero;

    [Range(0f, 0.5f)]
    public float speed = 0.2f;

    void Start()
    {
        transform.position = startPosition;
    }

    void Update()
    {
        distance += Time.deltaTime * speed;
        transform.Translate(Vector2.right * distance);
    }
}
