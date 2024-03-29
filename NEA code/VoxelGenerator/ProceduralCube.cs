﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    public float scale = 1f;
    public int posX, posY, posZ;

    float adjustedScale;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjustedScale = scale * 0.5f;
    }


    void Start()
    {
        MakeCube(adjustedScale, new Vector3((float)posX*scale, (float)posY *scale, (float)posZ *scale));
        UpdateMesh();
    }

    void MakeCube(float cubeScale, Vector3 cubePos)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        for(int i = 0; i < 6; i++)
        {
            MakeFace(i, cubeScale, cubePos);
        }
    }

    void MakeFace(int dir, float faceScale, Vector3 cubePos)
    {
        //vertices.AddRange(CubeMeshBuilder.faceVertices((dir), faceScale, cubePos));
        int vCount = vertices.Count;

        triangles.Add(vCount-4);
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
        mesh.RecalculateNormals();
    }

}
