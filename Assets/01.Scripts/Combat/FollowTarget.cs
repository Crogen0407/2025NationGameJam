using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public bool x = true;
    public bool y = true;
    public bool z = true;

    private void LateUpdate()
    {
        transform.position = 
            new Vector3(
                x ? target.position.x : transform.position.x, 
                y ? target.position.y : transform.position.y, 
                z ? target.position.z : transform.position.z);
    }
}
