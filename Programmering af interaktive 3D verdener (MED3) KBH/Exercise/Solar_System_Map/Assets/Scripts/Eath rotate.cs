using UnityEngine;

public class Eathrotate : MonoBehaviour
{   
    public float speed = 10f;
    public float orbitSpeed = 50f;
    public float orbitRadius = 10f;
    public Transform sun; // Reference to the Sun's transform

    void FixedUpdate()
    {
        // Rotate the Earth around its own axis
        transform.Rotate(Vector3.up, speed * Time.deltaTime);

        // Orbit the Earth around the Sun
        if (sun != null)
        {
            transform.RotateAround(sun.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }



    
}
