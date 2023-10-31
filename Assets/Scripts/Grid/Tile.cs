using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] List<Sprite> WallTiles;
    [SerializeField] List<Sprite> HoleTiles;
    [SerializeField] List<Sprite> HoleHighlightTiles;
    [SerializeField] bool isHole;
    int index;
    GridGenerator grid;
    [SerializeField] SpriteRenderer spriteRenderer;


    public void ChangeToWallTile(GridGenerator gridGenerator)
    {
        int index = Random.Range(0, WallTiles.Count);
        spriteRenderer.sprite = WallTiles[index];
        grid = gridGenerator;
    }
    void ChangeToWallTile()
    {
        int index = Random.Range(0, 2);
        spriteRenderer.sprite = WallTiles[index];
    }

    public int ChangeToHoleTile(int holeIndex, GridGenerator gridGenerator)
    {
        index = Random.Range(0, 3);
        Sprite sp = HoleTiles[index];
        spriteRenderer.sprite = sp;
        isHole = true;
        grid = gridGenerator;
        return index;
    }

    public void SetHighlight(bool isActive)
    {
        if (isActive)
            spriteRenderer.sprite = HoleHighlightTiles[index];
        else
            spriteRenderer.sprite = HoleTiles[index];
    }

    public void ClearTile()
    {
        isHole = false;
    }

    public void RepairTile()
    {
        isHole = false;
        ChangeToWallTile();
        grid.TileFixed();
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
                SetHighlight(true);
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
                SetHighlight(false);
            }
        }
    }
}
