using UnityEngine;

public class Movingaxe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform pointA;
    public Transform pointB;
    private Vector3 nextPos;
    public float speed = 2f;
    void Start()
    {
     nextPos = pointB.position;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

        if(transform.position == nextPos) {
            nextPos = (nextPos == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
