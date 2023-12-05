using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float size = 1f;
    [SerializeField] private Terrain mapTerrain;
    private int height;
    private int width;

    public Vector3 GetNearestPointOnGrid(Vector3 position) {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)xCount * size, (float)yCount * size, (float)zCount * size);
        result += transform.position;
        return result;
    }

    private void OnDrawGizmos(){
        Vector3 terrainSize = mapTerrain.terrainData.size;
        this.width = (int)terrainSize.z;
        this.height = (int)terrainSize.x;
        Gizmos.color = Color.yellow;
        for (float x = 0; x < this.height; x += size)
        {
            for (float z = 0; z < this.width; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x+mapTerrain.transform.position.x+1, 0f, z+mapTerrain.transform.position.z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}