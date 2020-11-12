using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Data.Common;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelTerrain : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colours;

    public TerrainType[] regions;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;


    public float scale = 1f;


    float adjustedScale;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        adjustedScale = scale * 0.5f;
    }


    void Start()
    {

        GenerateVoxelMesh(new VoxelData());
        UpdateMesh();


    }

    void GenerateVoxelMesh(VoxelData data)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(data.Width, data.Depth, seed, noiseScale, octaves, persistance, lacunarity, offset);
        data.data = noiseMap;
        Debug.Log(data.Width);
        Color color;
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colours = new List<Color>();
        for (int z = 0; z < data.Depth; z++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                float currentHeight = noiseMap[x, z];
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

    void MakeCube(float cubeScale, Vector3 cubePos, int x, int z, VoxelData data, Color color, bool water )
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
                        colours.Add(color);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                float neighbour = data.GetNeighbour(x, z, (Direction)i);
                Debug.Log(neighbour);
                if (neighbour == 2 || neighbour <= 0.4 )
                {
                    MakeFace((Direction)i, cubeScale, cubePos, water);
                    for (int f = 0; f < 4; f++)
                    {
                        colours.Add(color);
                    }
                }
            }
        }
        
    }

    void MakeFace(Direction dir, float faceScale, Vector3 cubePos, bool water)
    {
        vertices.AddRange(CubeMeshBuilder.faceVertices((dir), faceScale, cubePos, water));
        int vCount = vertices.Count;

        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 0);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colours.ToArray();
        mesh.RecalculateNormals();
    }
}

/*[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

}*/
