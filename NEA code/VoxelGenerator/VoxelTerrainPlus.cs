using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Data.Common;


public class VoxelTerrainPlus : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh ground;
    Mesh water;
    List<Vector3> grvertices, wavertices;
    List<int> grtriangles, watriangles;
    List<Color> grcolours, wacolours;

    public TerrainType[] regions;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    GameObject unwalkable;
    GameObject walkable;
    GetMesh walk;
    GetMesh unwalk;

    public VoxelData data = new VoxelData();

    public float scale = 1f;


    float adjustedScale;

    void Awake()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(data.Width, data.Depth, seed, noiseScale, octaves, persistance, lacunarity, offset);
        data.data = noiseMap;
        walkable = GameObject.FindGameObjectWithTag("walkable");
        unwalkable = GameObject.FindGameObjectWithTag("unwalkable");
        adjustedScale = scale * 0.5f;
        walk = walkable.GetComponent<GetMesh>();
        unwalk = unwalkable.GetComponent<GetMesh>();

        GenerateVoxelMesh(data);
        walk.UpdateMesh(grvertices, grtriangles, grcolours);
        unwalk.UpdateMesh(wavertices, watriangles, wacolours);
        Debug.Log("We win");

    }


    void Start()
    {
        
        
    }

    void GenerateVoxelMesh(VoxelData data)
    {
        
        Color color;
        grvertices = new List<Vector3>();
        wavertices = new List<Vector3>();
        grtriangles = new List<int>();
        watriangles = new List<int>();
        grcolours = new List<Color>();
        wacolours = new List<Color>();

        for (int z = 0; z < data.Depth; z++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                float currentHeight = data.GetCell(x, z);
                color = regions[0].colour;
                bool water = true;
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        color = regions[i].colour;

                        break;
                    }
                }
                if (currentHeight > regions[1].height)
                {
                    water = false;
                }
                MakeCube(adjustedScale, new Vector3((float)x * scale, 0, (float)z * scale), x, z, data, color, water); ;

            }
        }


    }

    void MakeCube(float cubeScale, Vector3 cubePos, int x, int z, VoxelData data, Color color, bool water)
    {
        if (water)
        {
            for (int i = 0; i < 5; i++)
            {
                if (data.GetNeighbour(x, z, (Direction)i) == 2)
                {
                    MakeFace((Direction)i, cubeScale, cubePos, water);
                    for (int f = 0; f < 4; f++)
                    {
                        wacolours.Add(color);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                float neighbour = data.GetNeighbour(x, z, (Direction)i);
                if (neighbour == 2 || neighbour <= 0.4)
                {
                    MakeFace((Direction)i, cubeScale, cubePos, water);
                    for (int f = 0; f < 4; f++)
                    {
                        grcolours.Add(color);
                    }
                }
            }
        }

    }

    void MakeFace(Direction dir, float faceScale, Vector3 cubePos, bool water)
    {
        if (water)
        {
            wavertices.AddRange(CubeMeshBuilder.faceVertices((dir), faceScale, cubePos, water));
            int vCount = wavertices.Count;

            watriangles.Add(vCount - 4);
            watriangles.Add(vCount - 4 + 1);
            watriangles.Add(vCount - 4 + 2);
            watriangles.Add(vCount - 4 + 0);
            watriangles.Add(vCount - 4 + 2);
            watriangles.Add(vCount - 4 + 3);
        }
        else
        {
            grvertices.AddRange(CubeMeshBuilder.faceVertices((dir), faceScale, cubePos, water));
            int vCount = grvertices.Count;

            grtriangles.Add(vCount - 4);
            grtriangles.Add(vCount - 4 + 1);
            grtriangles.Add(vCount - 4 + 2);
            grtriangles.Add(vCount - 4 + 0);
            grtriangles.Add(vCount - 4 + 2);
            grtriangles.Add(vCount - 4 + 3);
        }
        
    }

    

   
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

}

