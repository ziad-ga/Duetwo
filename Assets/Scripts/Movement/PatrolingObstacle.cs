using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PatrolingObstacle : MonoBehaviour
{
    private PlayerMovement player;
    private bool isMoving = false;
    private bool inFinalPosition = false;
    private int direction;
    [SerializeField]
    [Tooltip("Absolute value of the position on the x axis")]
    private float pos; // set from the inspector
    [SerializeField]
    [Tooltip("Minimum distance between player and obstacle to start moving")]
    private float movingYThreshold = 5f; // set from the inspector
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
        transform.position = new Vector3(direction * pos, transform.position.y, transform.position.z);
    }
    private void Update()
    {
        if (Mathf.Abs(transform.position.y - player.transform.position.y) < movingYThreshold && !isMoving && !inFinalPosition)
        {
            isMoving = true;
            transform.DOMoveX(-transform.position.x, 0.5f).OnComplete(() => { isMoving = false; inFinalPosition = true; });
        }
        if (GetComponent<Rigidbody2D>().velocity.y > 0 && !isMoving)
        {
            isMoving = true;
            transform.DOMoveX(-transform.position.x, 1f).SetEase(Ease.InQuad).onComplete += () => { isMoving = false; inFinalPosition = false; };
        }

    }

}
