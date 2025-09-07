using UnityEngine;
using System.Collections.Generic;

public class RayCastManipulator : MonoBehaviour
{
    public int numRaycasts = 10;

    CreateGrid createGrid;
    List<GameObject> hitCubes = new List<GameObject>();

    void Start()
    {
        createGrid = GetComponent<CreateGrid>();
    }

    void Update()
    {
        if (hitCubes.Count == createGrid.grid.Length)
        {
            foreach (GameObject cube in hitCubes)
            {
                cube.SetActive(true);
            }
            hitCubes = new List<GameObject>();
        }

        Vector3 cubeCenter = createGrid.grid[0].localPosition + new Vector3(createGrid.gridResolution / 2, createGrid.gridResolution / 2, createGrid.gridResolution / 2);
        for (int i = 0; i < numRaycasts; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(cubeCenter, Random.onUnitSphere, out hit, 100))
            {
                //Debug.DrawRay(cubeCenter, Random.onUnitSphere * hit.distance, Color.red,1);
                hit.transform.gameObject.SetActive(false);
                hitCubes.Add(hit.transform.gameObject);
            }
        }
    }
}
