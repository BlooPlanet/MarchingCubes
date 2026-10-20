using System;
using Marching_Cubes;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]
public class Chunk : MonoBehaviour {
    int width = 32;
    int height = 32;
    int depth = 32;
    
    MarchingCube marching;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    void Start() {

        // Init();
    }

    public void Init() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        
        marching = new MarchingCube(width,height,depth,0,1);
        Generate();
        Mesh mesh = marching.ConstructMesh();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Generate() {
        for (int x = 0; x < width + 1; x++) {
            for (int z = 0; z < depth + 1; z++) {
                int amplitude = (int)(Mathf.PerlinNoise((x + transform.position.x) / 50f, (z + transform.position.z) / 50f) * height);
                for (int y = 1; y < amplitude; y++) {
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

    public void SetCube(Vector3Int coordinate,float value) {
        for (int i = 0; i < MarchingCubeConfigs.CornerTable.Length; i++) {
            Vector3Int corner = MarchingCubeConfigs.CornerTable[i] + coordinate;
            marching.SetDensity(corner,value);
        }
        
    }

    Vector3 size = new Vector3(32, 32, 32);
    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(size / 2 + transform.position,size);
    }

    public void SetMaterial(Material material) {
        GetComponent<MeshRenderer>().material = material;
    }
}