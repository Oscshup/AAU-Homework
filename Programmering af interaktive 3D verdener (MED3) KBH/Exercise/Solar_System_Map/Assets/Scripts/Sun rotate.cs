using UnityEngine;

public class Sunrotate : MonoBehaviour
{   
    public float speed = 10f;

    void FixedUpdate()
    {
        
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
