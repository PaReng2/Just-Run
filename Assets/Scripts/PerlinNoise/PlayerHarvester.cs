using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask hitMask = ~0;
    public int toolDamage = 2;
    public float hitCooldown = 0.15f;

    private float _nextHitTime;
    private Camera _cam;
    public Inventory inventory;

    [SerializeField]
    private InventoryUi inventoryUi;

    public GameObject selectedBlock;
    // Start is called before the first frame update
    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null)
        {
            inventory = gameObject.AddComponent<Inventory>();
            inventoryUi = FindObjectOfType<InventoryUi>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryUi.selectedIndex < 0)
        {
            selectedBlock.transform.localScale = Vector3.zero;
            if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
            {
                _nextHitTime = Time.time + hitCooldown;

                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    var block = hit.collider.GetComponent<Block>();
                    if (block != null)
                    {
                        block.Hit(toolDamage, inventory);
                    }
                }
            }
        }
        else
        {
            Ray rayDebug = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(rayDebug, out var hitDebug, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3Int placePos = AdjacentCellOnHtFace(hitDebug);
                    selectedBlock.transform.localScale = Vector3.one;
                    selectedBlock.transform.position = placePos;
                    selectedBlock.transform.rotation = Quaternion.identity;
                    
                }
                else
                {
                    selectedBlock.transform.localScale = Vector3.zero;
                }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3Int placePos = AdjacentCellOnHtFace(hit);

                    BlockType selected = inventoryUi.GetInventorySlot();
                    if (inventory.Consume(selected, 1))
                    {
                        FindAnyObjectByType<NoiseVoxelMap>().PlaceTile(placePos, selected);
                    }
                }
            }
        }
    }

    static Vector3Int AdjacentCellOnHtFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal;
        return Vector3Int.RoundToInt(adjCenter);
    }
}
