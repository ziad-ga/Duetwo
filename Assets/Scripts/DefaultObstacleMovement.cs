using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultObstacleMovement : MonoBehaviour
{

    public float ms;

    public Vector3 startPos;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;

        rb.velocity = new Vector2(0, -ms);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
