using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;

    private void LateUpdate()
    {
        _target = GameObject.FindWithTag("Player").transform; 
        transform.position = new Vector3(_target.position.x, _target.position.y, -10);
    }
}
