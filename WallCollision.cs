using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    public GameObject[] connectedDoors;

    void Start()
    {
        Collider ownCollider = GetComponent<Collider>();
        ownCollider.enabled = false;

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            if (collider != ownCollider && collider.CompareTag("Wall"))
            {
                DestroyConnectedDoors();
                Destroy(gameObject);
                return;
            }
        }

        ownCollider.enabled = true;
    }

    void DestroyConnectedDoors()
    {
        foreach (GameObject door in connectedDoors)
        {
            if (door != null)
            {
                Destroy(door);
            }
        }
    }
}
