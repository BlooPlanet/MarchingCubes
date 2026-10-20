using System;
using Marching_Cubes;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]
public class Chunk : MonoBehaviour {
    int width = 16;
    int height = 32;
    int depth = 16;
    
    MarchingCube marching;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    void Start() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        Init();
    }

    public void Init() {
        marching = new MarchingCube(width,height,depth,0,1);
        Generate();
        Mesh mesh = marching.ConstructMesh();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Generate() {
        for (int x = 1; x < width; x++) {
            for (int z = 1; z < depth; z++) {
                //int amplitude = (int)(Mathf.PerlinNoise(x / 30f, z / 30f) * height);
                for (int y = 1; y < 18; y++) {
                    Vector3Int coord = new Vector3Int(x, y, z);
                    marching.SetDensity(coord,-1);
                }
            }
        }
    }

    public void UpdateMesh() {
        meshCollider.sharedMesh = null;
        Mesh mesh = marching.ConstructMesh();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void SetCube(Vector3Int coordinate) {
        for (int i = 0; i < MarchingCubeConfigs.CornerTable.Length; i++) {
            Vector3Int corner = MarchingCubeConfigs.CornerTable[i] + coordinate;
            marching.SetDensity(corner,1);
        }
        
    }

    Vector3 size = new Vector3(16, 32, 16);
    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(size / 2 + transform.position,size);
    }
}