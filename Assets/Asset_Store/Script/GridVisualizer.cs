using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public float gridSize = 1.0f;
    public int gridWidth = 10;
    public int gridHeight = 10;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 gridPosition = new Vector3(x * gridSize, 0f, z * gridSize);
                Gizmos.DrawWireCube(gridPosition, new Vector3(gridSize, 0.1f, gridSize));
            }
        }
    }
}
