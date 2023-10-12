using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] List<Sprite> WallTiles;
    [SerializeField] List<Sprite> HoleTiles;
    [SerializeField] bool isHole;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToWallTile()
    {
        int index = Random.Range(0, WallTiles.Count);
        spriteRenderer.sprite = WallTiles[index];
    }

    public int ChangeToHoleTile(int holeIndex)
    {
        int index = Random.Range(0, HoleTiles.Count);
        spriteRenderer.sprite = HoleTiles[index];
        isHole = true;
        return index;
    }

    public void ClearTile()
    {
        isHole = false;
        spriteRenderer.sprite = null;
    }

    public void RepairTile()
    {
        isHole = false;
        ChangeToWallTile();
        //send info to level script
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHole)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerRepair>(out PlayerRepair playerRepair);
            {
                playerRepair.SetCanRepairTile(this, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isHole)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerRepair>(out PlayerRepair playerRepair);
            {
                playerRepair.SetCanRepairTile(this, false);
            }
        }
    }
}
