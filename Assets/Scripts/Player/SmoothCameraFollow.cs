using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;
    public float delayTime = 0.3f;

    private static void SetResolution(int width, int height)
    {
        
    }

    private Vector2 currentVelocity = Vector2.zero;

    void Start()
    {
        SetResolution(480, 270);
    }
    

    void LateUpdate()
    {
        if (target)
        {
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref currentVelocity, delayTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            
        }
    }
}
