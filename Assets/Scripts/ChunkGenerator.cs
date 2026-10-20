using UnityEngine;

public class ChunkGenerator : MonoBehaviour {

    public Material mat;
    
    public void Start() {
        Init(4,4);
    }

    void Init(int width, int depth) {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                Vector3Int chunkCoord = new Vector3Int(x * 64, 0, z * 64);
                GameObject chunkObj = new GameObject("Chunk " + chunkCoord);
                chunkObj.transform.parent = this.transform;
                chunkObj.transform.position = chunkCoord;
                Chunk chunk = chunkObj.AddComponent<Chunk>();
                chunk.Init();
                chunk.SetMaterial(mat);
            }
        }
    }
}
