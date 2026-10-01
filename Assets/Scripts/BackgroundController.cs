using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Transform cameraTransform;
    
    [Range(0f, 1f)]
    public float movementFactor = 0.5f;
    
    private float _startYImage;
    private float _startYCamera;

    void Start()
    {
        if (cameraTransform)
        {
            _startYImage = transform.position.y;
            _startYCamera = 6;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform)
        {
            float cameraDeltaY = cameraTransform.position.y - _startYCamera;
            
            float newY = _startYImage - (cameraDeltaY * movementFactor);
            
            transform.position = new Vector2(transform.position.x, newY);
        }
    }
}