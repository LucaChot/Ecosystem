using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class GetMesh : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colours;
    MeshCollider collider;
    

    public void UpdateMesh(List<Vector3> v, List<int> t, List<Color> c)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        collider = GetComponent<MeshCollider>();
        mesh.Clear();

        mesh.vertices = v.ToArray();
        mesh.triangles = t.ToArray();
        mesh.colors = c.ToArray();
        mesh.RecalculateNormals();
        collider.sharedMesh = mesh;
    }
}
