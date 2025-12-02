using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BlockType { Dirt, Grass, Water, Gold }

public class Block : MonoBehaviour
{
    [Header("Block Stat")]
    public BlockType type = BlockType.Dirt;
    public int maxHp = 3;

    [Header("Block inven")]
    public Image icon;
    [HideInInspector] public int hp;

    public int dropCount = 1;
    public bool minable = true;

    Inventory inventory;


    bool isItem;
    void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
        hp = maxHp;
        if (GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
            gameObject.tag = "Block";
        isItem = false;
    }
    void Update()
    {
        // 'isItem'이 true일 때만 OverlapSphere를 활성화합니다.
        if (isItem)
        {
            int playerLayer = LayerMask.GetMask("Player");
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, playerLayer);

            bool hasPlayer = colliders.Length > 0;

            // 감지된 플레이어가 있으면
            if (hasPlayer)
            {

                // 플레이어의 인벤토리가 있고, 드랍 수량이 0보다 크면
                if (inventory != null && dropCount > 0)
                {
                    inventory.Add(type, dropCount); // 인벤토리에 아이템 추가
                    Destroy(gameObject); // 아이템이 줍혔으므로 오브젝트 파괴
                }
            }
        }
    }
    public void Hit(int damage, Inventory inven)
    {
        // 채굴 불가능하거나, 이미 아이템 상태면 리턴
        if (!minable || isItem) return;

        hp -= damage;

        if (hp <= 0)
        {
            // 블록이 파괴됨 -> 아이템 상태로 변경
            isItem = true;
            transform.localScale = gameObject.transform.localScale * 0.1f; // 크기 줄이기

            // 태그를 변경하고 콜라이더를 트리거로 만들어 플레이어가 통과할 수 있게 함 (선택 사항)
            gameObject.tag = "Item";
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.isTrigger = true;
        }
        // hp가 0보다 크면 isItem은 false 유지
    }

    private void OnDrawGizmos()
    {
        if (isItem)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }


}
