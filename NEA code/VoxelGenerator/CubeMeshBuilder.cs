using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeMeshBuilder 
{
    public static Vector3[] vertices =
    {
        new Vector3 (2,0,2),
        new Vector3 (0,0,2),
        new Vector3 (0,-2,2),
        new Vector3 (2,-2,2),
        new Vector3 (0,0,0),
        new Vector3 (2,0,0),
        new Vector3 (2,-2,0),
        new Vector3 (0,-2,0)
    };
    public static Vector3[] water_vertices =
    {
        new Vector3 (2,-0.5f,2),
        new Vector3 (0,-0.5f,2),
        new Vector3 (0,-2,2),
        new Vector3 (2,-2,2),
        new Vector3 (0,-0.5f,0),
        new Vector3 (2,-0.5f,0),
        new Vector3 (2,-2,0),
        new Vector3 (0,-2,0)
    };
    public static int[][] faceTriangles =
    {
        new int[]{0,1,2,3},
        new int[]{5,0,3,6},
        new int[]{4,5,6,7},
        new int[]{1,4,7,2},
        new int[]{5,4,1,0},
        new int[]{3,2,7,6},
    };

    public static Vector3[] faceVertices(int dir, float scale, Vector3 pos, bool water)
    {
        Vector3[] fv = new Vector3[4];
        if (water)
        {
            for (int i = 0; i < fv.Length; i++)
            {
                fv[i] = (water_vertices[faceTriangles[dir][i]] * scale) + pos;
            }
        }
        else
        {
            for (int i = 0; i < fv.Length; i++)
            {
                fv[i] = (vertices[faceTriangles[dir][i]] * scale) + pos;
            }
        }
        
        return fv;
    }

    public static Vector3[] faceVertices(Direction dir, float scale, Vector3 pos, bool water)
    {
        Vector3[] fv = new Vector3[4];
        return faceVertices((int)dir, scale, pos, water);
    }
}
