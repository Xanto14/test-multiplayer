using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, Mesh };
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistances;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    [SerializeField]
    public TextAsset carte;

    private char[,] convertTextToArray(TextAsset asciiMap)
    {
        char[,] charMap = new char[mapChunkSize, mapChunkSize];
        Debug.Log(asciiMap.text.Length);
        int index = 0;
        for (int y = 0; y < charMap.GetLength(1); y++)
        {
            for (int x = 0; x < charMap.GetLength(0); x++)
            {
                while (asciiMap.text[index] == '\n' || asciiMap.text[index] == '\r')
                {
                    index++;
                }
                charMap[y, x] = asciiMap.text[index];
                index++;
            }
        }
        //Debug.Log(résultat.Length);
        //for (int i = 0; i < résultat.GetLength(1); i++)
        //{
        //    for (int j = 0; j < résultat.GetLength(0); j++)
        //    {
        //        var msg = "[" + i.ToString() + ", " + j.ToString() + "] = " + résultat[i, j].ToString();
        //        Debug.Log(msg);
        //    }
        //}
        return charMap;
    }
    public void GenerateMap()
    {
        char[,] texteCarte = convertTextToArray(carte);
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistances, lacunarity, offset, texteCarte);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        { display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
    }

    void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}

[System.Serializable]
public struct TerrainType
{
    public float height;
    public string name;
    public Color colour;
}