using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Data.Common;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRender : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;
    
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colours;
    

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

    void GenerateVoxelMesh( VoxelData data)
    {
        
        Debug.Log(data.Width);
        Color color;
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colours = new List<Color>();
        for (int z = 0; z < data.Depth; z++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                Debug.Log(data.GetCell(x,z));
                if(data.GetCell(x,z) == 0)
                {
                    if ((x+z) % 2 == 0)
                    {
                        color = Color.black;
                    }
                    else
                    {
                        color = Color.white;
                    }
                    MakeCube(adjustedScale, new Vector3((float)x * scale, 0, (float)z * scale), x, z, data, color); ;
                }
                
            }
        }
        

    }

    void MakeCube(float cubeScale, Vector3 cubePos, int x, int z, VoxelData data, Color color)
    {
        for (int i = 0; i < 5; i++)
        {
            if(data.GetNeighbour(x, z, (Direction)i) == 0)
            {
                MakeFace((Direction)i, cubeScale, cubePos);
                for (int f = 0; f < 4; f++)
                {
                    colours.Add(color);
                }
            } 
        }
    }

    void MakeFace(Direction dir, float faceScale, Vector3 cubePos)
    {
        //vertices.AddRange(CubeMeshBuilder.faceVertices((dir), faceScale, cubePos));
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

        Debug.Log(vertices.Count);
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colours.ToArray();
        mesh.RecalculateNormals();
        
    }
}
