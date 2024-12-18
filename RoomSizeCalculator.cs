using UnityEngine;

public class RoomSizeCalculator : MonoBehaviour
{
    public Vector3 GetRoomSize()
    {
        Bounds combinedBounds = new Bounds(Vector3.zero, Vector3.zero);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (combinedBounds.size == Vector3.zero)
            {
                combinedBounds = renderer.bounds;
            }
            else
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
        }

        return combinedBounds.size;
    }
}
