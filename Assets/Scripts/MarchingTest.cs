using System;
using System.Collections.Generic;
using Marching_Cubes;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class MarchingTest : MonoBehaviour {
    MarchingCube marching;
    int cubeIndex = 0;
    
    public void Start() {
        marching = new MarchingCube(1,1,1,0,1);
        Debug.Log("cube config index " + marching.GetCubeIndex(Vector3Int.zero));
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            cubeIndex++;
            Debug.Log("Config index " + cubeIndex);
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
        
            marching.MarchCube(Vector3Int.zero,cubeIndex,ref vertices, ref triangles);
        
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
