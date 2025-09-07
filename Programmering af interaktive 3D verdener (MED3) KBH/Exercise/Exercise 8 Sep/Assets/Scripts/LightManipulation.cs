using UnityEngine;

public class LightManipulation : MonoBehaviour
{
    public GameObject pointLight;

    public float amplitude = 5.0f;
    public float frequency = 1.0f;
    public float maxDistance = 5.0f;
    public float verticalAmplitude = 2.0f;


    void Update()
    {
        float time = Time.time * frequency;

        float x = amplitude * Mathf.Sin(time); // Horizontal swinging
        float y = Mathf.Cos(time) * verticalAmplitude; // Vertical oscillation
        float z = 0;

        pointLight.transform.position = new Vector3(x, y, z);
        float t = Mathf.Clamp01(Mathf.Abs(x) / amplitude);

        pointLight.GetComponent<Light>().color = Color.Lerp(Color.red, Color.blue, t);
    }
}
