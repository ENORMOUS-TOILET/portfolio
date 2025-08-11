using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    public Tilemap groundTileMap;
    public int width;
    public int hight;

    public int seed;
    public bool useRandomSeed;

    public float lacunarity;

    [Range(0,1f)]
    public float waterProbility;

    public TileBase groundTile;
    public TileBase waterTile;

    private float[,] mapData;


    public void GenerateMap()
    {
        GenerateMapData();

        GenerateTileMap();
    }

    private void GenerateMapData()
    {
        //应用种子
        if(useRandomSeed)
        {
            seed = Time.time.GetHashCode();
        }
        UnityEngine.Random.InitState(seed);

        mapData = new float[width, hight];

        float randomOffset = UnityEngine.Random.Range(-10000, 10000);

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * lacunarity + randomOffset, y * lacunarity + randomOffset);
                mapData[x, y] = noiseValue;

                if (noiseValue < minValue) minValue = noiseValue;
                if (noiseValue > maxValue) maxValue = noiseValue;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                mapData[x, y] = Mathf.InverseLerp(minValue, maxValue, mapData[x, y]);
            }
        } 

    }

    private void GenerateTileMap()
    {
        CleanMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                TileBase tile = mapData[x, y] > waterProbility ? groundTile : waterTile;
                groundTileMap.SetTile(new Vector3Int(x, y), tile);
            }
        }
    }

    public void CleanMap()
    {
        groundTileMap.ClearAllTiles();
    }
}
