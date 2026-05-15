using UnityEngine;

public class CubeWallBuilder : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    [SerializeField] private float spacing = 1.1f;

    [SerializeField] private float distanceFromTurret = 25f;

    private void Start()
    {
        BuildWall();
    }

    private void BuildWall()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float offsetX =
                    y % 2 == 0
                    ? 0f
                    : spacing * 0.5f;

                Vector3 spawnPosition = new Vector3(
                    x * spacing + offsetX,
                    y * spacing,
                    distanceFromTurret);

                GameObject cube = Instantiate(
                    cubePrefab,
                    spawnPosition,
                    Quaternion.identity,
                    transform);

                cube.AddComponent<CubeHighlight>();
            }
        }
    }
}