using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject normalTilePrefab;
    public GameObject burningTilePrefab;
    public GameObject suppliesTilePrefab;
    public GameObject boostTilePrefab;
    public GameObject stickyTilePrefab;
    public GameObject obstaclePrefab;  
    public float tileLength = 10f;      public int numberOfTiles = 5;      private float spawnZ = 0f;  
    private float[] lanePositions = { -3f, 0f, 3f };      public Transform player;  
    public List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
                 for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnInitialNormalTileRow();
        }
    }

    void Update()
    {
                 if (player.position.z - 30 > spawnZ - (numberOfTiles * tileLength))
        {
            SpawnTileRow();
            DeleteTileRow();
        }
    }

    void SpawnInitialNormalTileRow()
    {
                 foreach (float lanePosition in lanePositions)
        {
            GameObject newTile = Instantiate(normalTilePrefab, new Vector3(lanePosition, 0, spawnZ), Quaternion.identity);
            activeTiles.Add(newTile);
        }

        spawnZ += tileLength;      }

    void SpawnTileRow()
    {
        bool hasNormalTile = false;
        int normalTileLaneIndex = Random.Range(0, lanePositions.Length);          GameObject[] tileRow = new GameObject[lanePositions.Length];

                 for (int i = 0; i < lanePositions.Length; i++)
        {
            GameObject tileToSpawn = GetRandomTile();

                         if (tileToSpawn == normalTilePrefab)
            {
                hasNormalTile = true;
            }

                         tileRow[i] = tileToSpawn;

                         if (tileToSpawn != null)
            {
                GameObject newTile = Instantiate(tileToSpawn, new Vector3(lanePositions[i], 0, spawnZ), Quaternion.identity);
                activeTiles.Add(newTile);

                                 if (Random.Range(0f, 1f) > 0.9f)
                {
                    Vector3 obstaclePosition = new Vector3(lanePositions[i], 0.5f, spawnZ);
                    Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
                }
            }
            else
            {
                                 activeTiles.Add(null);
            }
        }

                 if (!hasNormalTile)
        {
            if (tileRow[normalTileLaneIndex] != null)
            {
                                 Destroy(activeTiles[activeTiles.Count - lanePositions.Length + normalTileLaneIndex]);
                activeTiles[activeTiles.Count - lanePositions.Length + normalTileLaneIndex] = null;
            }

                         GameObject normalTile = Instantiate(normalTilePrefab, new Vector3(lanePositions[normalTileLaneIndex], 0, spawnZ), Quaternion.identity);
            activeTiles[activeTiles.Count - lanePositions.Length + normalTileLaneIndex] = normalTile;
        }

        spawnZ += tileLength;      }

    GameObject GetRandomTile()
    {
                 int randomIndex = Random.Range(0, 6);
        switch (randomIndex)
        {
            case 0: return normalTilePrefab;              case 1: return burningTilePrefab;              case 2: return suppliesTilePrefab;              case 3: return boostTilePrefab;              case 4: return stickyTilePrefab;              case 5: return null;              default: return normalTilePrefab;
        }
    }

    void DeleteTileRow()
    {
                 if (activeTiles.Count < lanePositions.Length)
        {
            return;
        }

                 for (int i = 0; i < lanePositions.Length; i++)
        {
                         if (activeTiles[0] != null)
            {
                Destroy(activeTiles[0]);
            }
                         activeTiles.RemoveAt(0);
        }
    }
}