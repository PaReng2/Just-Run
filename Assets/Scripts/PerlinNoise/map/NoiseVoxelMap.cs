using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    [Header("Block Prefabs")]
    public GameObject dirt;
    public GameObject Water;
    public GameObject Grass; // Gress -> Grass 오타 수정
    public GameObject gold;

    [Header("Map Settings")]
    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;
    public int waterHeight = 5;

    [Header("Generation Settings")]
    [SerializeField] private float noiseScale = 20f;
    [Range(0, 100)] public int goldChance = 10; // 금 생성 확률 변수 추가

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // 노이즈 계산
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) continue;

                // 지형 생성 (땅, 잔디)
                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                    {
                        // 물 높이보다 낮으면 모래나 흙이 더 자연스럽지만, 일단 기존 로직대로 잔디 배치
                        SetGrass(x, y, z);
                    }
                    else
                    {
                        SetDirt(x, y, z);
                    }
                }

                // 물 생성 (지형 위부터 수면까지)
                for (int wh = h + 1; wh <= waterHeight; wh++)
                {
                    SetWater(x, wh, z);
                }
            }
        }
    }

    public void PlaceTile(Vector3Int pos, ItemType type)
    {
        switch (type)
        {
            case ItemType.Dirt:
                SetDirt(pos.x, pos.y, pos.z);
                break;
            case ItemType.Grass:
                SetGrass(pos.x, pos.y, pos.z);
                break;
            case ItemType.Gold:
                SetGold(pos.x, pos.y, pos.z); // [버그 수정] SetGrass -> SetGold
                break;
        }
    }

    private void SetGrass(int x, int y, int z)
    {
        CreateBlock(Grass, x, y, z, ItemType.Grass, "G", 3);
    }

    private void SetDirt(int x, int y, int z)
    {
        // 0~100 사이 난수 발생
        int randomValue = Random.Range(0, 100);

        // goldChance 보다 작으면 금 생성
        if (randomValue < goldChance)
        {
            SetGold(x, y, z);
        }
        else
        {
            CreateBlock(dirt, x, y, z, ItemType.Dirt, "D", 3);
        }
    }

    private void SetWater(int x, int y, int z)
    {
        // 물은 Block 컴포넌트 설정이 없어서 따로 Instantiate
        var go = Instantiate(Water, new Vector3(x, y, z), Quaternion.identity);
        go.name = $"B_{x}_{y}_{z}_W";
        go.transform.SetParent(this.transform); // 하이어라키 정리
    }

    private void SetGold(int x, int y, int z)
    {
        CreateBlock(gold, x, y, z, ItemType.Gold, "Gold", 5);
    }

    // [코드 중복 제거] 블록 생성 헬퍼 함수
    private void CreateBlock(GameObject prefab, int x, int y, int z, ItemType type, string suffix, int hp)
    {
        var go = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
        go.name = $"B_{x}_{y}_{z}_{suffix}";
        go.transform.SetParent(this.transform); // 부모 설정으로 하이어라키 정리

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = type;
        b.maxHp = hp;
        b.dropCount = 1;
        b.minable = true;
    }
}