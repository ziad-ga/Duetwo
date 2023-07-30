using UnityEngine;


public enum Direction { CLOCKWISE, COUNTERCLOCKWISE }

public class Movement : MonoBehaviour
{

    [HideInInspector]
    public float angle;

    [property: SerializeField]
    public float RotationSpeed { get { return MovementSync.rotationSpeed; } }

    [property: SerializeField]
    private float Radius { get { return MovementSync.radius; } }

    private float Height { get { return MovementSync.height; } }

    [HideInInspector]
    public Direction currDirection = Direction.CLOCKWISE;

    public void RotateClockWise()
    {
        currDirection = Direction.CLOCKWISE;

        transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * Radius, Mathf.Sin(Mathf.Deg2Rad * angle) * Radius + Height);
        angle -= Time.deltaTime * RotationSpeed;
        if (angle < 0) angle += 360;

    }


    public void RotateCounterClockWise()
    {
        currDirection = Direction.COUNTERCLOCKWISE;

        transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * Radius, Mathf.Sin(Mathf.Deg2Rad * angle) * Radius + Height);
        angle += Time.deltaTime * RotationSpeed;
        if (angle > 360) angle = 0;

    }


    void Start()
    {
        transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * Radius, Mathf.Sin(Mathf.Deg2Rad * angle) * Radius + Height);

    }

    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                RotateCounterClockWise();

            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                RotateClockWise();
            }

        }
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount != 0)
            {
                Touch[] touches = Input.touches;
                Touch lastTouch = touches[touches.Length - 1];

                if (lastTouch.position.x < Screen.width / 2)
                {
                    RotateCounterClockWise();
                }
                if (lastTouch.position.x > Screen.width / 2)
                {
                    RotateClockWise();
                }

            }


        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameManager.HandleCollision(transform.gameObject.name);
    }




}
