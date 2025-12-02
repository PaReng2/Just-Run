using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public GameObject dirt;
    public GameObject Water;
    public GameObject Gress;
    public GameObject gold;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
    

    private int dirtHeight;
    public int waterHeight = 5;
    [SerializeField] private float noiseScale = 20f;
    void Start()
    {
        dirtHeight = maxHeight - 1;

        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);

                int h = Mathf.FloorToInt(noise * maxHeight);


                if (h <= 0) continue;

                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                    {
                        SetGress(x, y, z);

                    }
                    else
                    {
                        SetDirt(x, y, z);
                    }
                    
                    
                }
                for (int wh = h + 1; wh <= waterHeight; wh++)
                {
                    if (waterHeight >= wh)
                    {
                        SetWater(x, wh, z);
                    }
                }
                
            }
        }
    }
    public void PlaceTile(Vector3Int pos, BlockType type)
    {
        switch (type)
        {
            case BlockType.Dirt:
                SetDirt(pos.x, pos.y, pos.z);
                break;
            case BlockType.Grass:
                SetGress(pos.x, pos.y, pos.z);
                break;
            case BlockType.Gold:
                SetGress(pos.x, pos.y, pos.z);
                break;
        }
    }

    private void SetGress(int x, int y, int z)
    {
        var go = Instantiate(Gress, new Vector3(x, y, z), Quaternion.identity);
        go.name = $"B_{x}_{y}_{z}_G";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Grass;
        b.maxHp = 3;
        b.dropCount = 1;
        b.minable = true;
    }

    private void SetDirt(int x, int y, int z)
    {
        
            var go = Instantiate(dirt, new Vector3(x, y, z), Quaternion.identity);
            go.name = $"B_{x}_{y}_{z}_D";

            var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
            b.type = BlockType.Dirt;
            b.maxHp = 3;
            b.dropCount = 1;
            b.minable = true;
        
            


    }

    private void SetWater(int x, int y, int z)
    {
        var go = Instantiate(Water, new Vector3(x, y, z), Quaternion.identity);
        go.name = $"B_{x}_{y}_{z}_W";

        
    }

    private void SetGold(int x, int y, int z)
    {
        var go = Instantiate(gold, new Vector3(x, y, z), Quaternion.identity);
        go.name = $"B_{x}_{y}_{z}_G";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Gold;
        b.maxHp = 5;
        b.dropCount = 1;
        b.minable = true;
    }

    
    void Update()
    {
        
    }
}
