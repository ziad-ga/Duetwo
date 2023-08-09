using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float height = Defaults.BALL_HEIGHT;
    public Direction currDirection = Direction.CLOCKWISE;

    public float Angle { get { return transform.rotation.eulerAngles.z; } }
    private void Start()
    {
        rotationSpeed = Defaults.BALL_ROTATION_SPEED * GameManager.GameSpeed;
        StartCoroutine(UpdateRotationSpeed());

    }
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Rotate(Direction.COUNTERCLOCKWISE);

            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Rotate(Direction.CLOCKWISE);
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
                    Rotate(Direction.COUNTERCLOCKWISE);
                }
                if (lastTouch.position.x > Screen.width / 2)
                {
                    Rotate(Direction.CLOCKWISE);
                }

            }


        }

    }
    public void Rotate(Direction dir, float speed = 0)
    {
        currDirection = dir;
        float s = speed == 0 ? rotationSpeed : speed;
        transform.Rotate(0, 0, s * (int)dir * Time.deltaTime);

    }
    
    /// <summary>
    /// Update rotation speed according to game speed every game speed update interval
    /// </summary>
    private IEnumerator UpdateRotationSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.GameUpdateInterval);
            yield return null; // wait for game manager to update game speed            
            yield return new WaitUntil(() => !GameManager.IsResetting && GameManager.InPlayMode);

            rotationSpeed = Defaults.BALL_ROTATION_SPEED * GameManager.GameSpeed;
        }
    }
}
