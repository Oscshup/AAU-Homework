using UnityEngine;

public class Transformations : MonoBehaviour
{
    CreateGrid CreateGrid;

    Vector3[] startPos;

    [Header("Movement controls")]
    public float moveFrequency =2f;
    public float moveAmplitude = 5f;
    public float MoveOffset = 0f;

    Vector3 startScale = Vector3.zero;

    [Header("Scale Controls")]
    public float scaleFrequency = 2f;
    public float scaleAmplitude = 5f;
    public float scaleOffset = 0f;

    [Header("Rotation controls")]
    public float rotaionSpeed = 1f;

    private void Start()
    {
        CreateGrid= GetComponent<CreateGrid>();
        startPos = CreateGrid.starPos;
        startScale = CreateGrid.grid[0].localScale;
    }

    void Update()
    {
        for (int i = 0, z= 0; z < CreateGrid.gridResolution; z++)
        {
            for ( int y = 0; y < CreateGrid.gridResolution; y++)
            {
                for (int x = 0; x < CreateGrid.gridResolution; x++, i++)
                {
                    CreateGrid.grid[i].localPosition = startPos[i] + Vector3.up * Mathf.Sin(Time.time * moveFrequency + MoveOffset) * moveAmplitude;

                    CreateGrid.grid[i].localScale = startScale * Mathf.Sin(Time.time * scaleFrequency) * scaleAmplitude + Vector3.one * scaleOffset;

                    CreateGrid.grid[i].Rotate(Vector3.forward * Time.deltaTime * rotaionSpeed);
                }
            }
        }
    }








}
