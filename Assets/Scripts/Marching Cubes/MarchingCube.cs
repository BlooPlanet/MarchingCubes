using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Marching_Cubes {
    public class MarchingCube {
        int width, height, depth;
        float[] density;
        public float surfaceLevel;
        
        public MarchingCube(int width, int height, int depth,float surfaceLevel,float initialDensity) {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.surfaceLevel = surfaceLevel;
            density = new float[(width + 1) * (height + 1) * (depth + 1)];
            for (int i = 0; i < density.Length; i++) {
                density[i] = initialDensity;
            }
        }
        
        int GetIndexFromCoord(Vector3Int densityCoord) {
            int x = densityCoord.x;
            int y = densityCoord.y;
            int z = densityCoord.z;
            return x + y * (width + 1) + z * (width + 1) * (height + 1);
        }
        
        public float GetDensity(Vector3Int densityCoord) {
            return density[GetIndexFromCoord(densityCoord)];
        }
        
        public void SetDensity(Vector3Int densityCoord, float value) {
            density[GetIndexFromCoord(densityCoord)] = value;
        }

        public int GetCubeIndex(Vector3Int cubeCoord) {
            int cubeIndex = 0;
            for (int i = 0; i < MarchingCubeConfigs.CornerTable.Length; i++) {
                Vector3Int corner = MarchingCubeConfigs.CornerTable[i] + cubeCoord;
                if (density[GetIndexFromCoord(corner)] > surfaceLevel) {
                    cubeIndex |= 1 << i;
                }
            }
            return cubeIndex;
        }

        public void MarchCube(Vector3Int cubeCoord, int cubeIndex,ref List<Vector3> vertices,ref List<int> triangles) {
            if (cubeIndex == 0 || cubeIndex == 255) return;
            for (int i = 0; i < 15; i++) {
                int edgeIndex = MarchingCubeConfigs.TriangleTable[cubeIndex,i];
                if (edgeIndex == -1) break;
                Vector3 cornerA = MarchingCubeConfigs.EdgeTable[edgeIndex, 0];
                Vector3 cornerB = MarchingCubeConfigs.EdgeTable[edgeIndex, 1];
                Vector3 midPoint = (cornerA + cornerB) / 2f;
                
                vertices.Add(midPoint + cubeCoord);
                triangles.Add(vertices.Count - 1);
            }
        }

        public Mesh ConstructMesh() {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < depth; z++) {
                    for (int y = 0; y < height; y++) {
                        Vector3Int cubeCoord = new Vector3Int(x, y, z);
                        int cubeIndex = GetCubeIndex(cubeCoord);
                        MarchCube(cubeCoord,cubeIndex,ref vertices,ref triangles);
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }
    }
}