using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.AI.Navigation;


public class DynamicNavMeshBaker : MonoBehaviour
{
    public GameObject floorPrefab;
    public int numberOfFloors = 5;

    private List<GameObject> instantiatedFloors = new List<GameObject>();

    void Start()
    {
        InstantiateFloors();
        StartCoroutine(BakeAllNavMeshes());
    }

    void InstantiateFloors()
    {
        for (int i = 0; i < numberOfFloors; i++)
        {
            Vector3 position = new Vector3(i * 10, 0, 0); 
            GameObject newFloor = Instantiate(floorPrefab, position, Quaternion.identity);
            newFloor.name = "Floor_" + i; 
            instantiatedFloors.Add(newFloor);
        }
    }

    System.Collections.IEnumerator BakeAllNavMeshes()
    {
        foreach (GameObject floor in instantiatedFloors)
        {
            NavMeshSurface navMeshSurface = floor.GetComponent<NavMeshSurface>();
            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
                Debug.Log("Baked NavMesh for floor: " + floor.name);
                yield return null;
            }
            else
            {
                Debug.LogError("NavMeshSurface component missing on floor prefab.");
            }
        }
    }
}
