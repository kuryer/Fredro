using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] List<Sprite> WallTiles;
    [SerializeField] List<Sprite> HoleTiles;
    [SerializeField] bool isHole;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToWallTile()
    {

    }

    public int ChangeToHoleTile(int holeIndex)
    {
        //must return index of the used hole
        return 0;
    }

    public void ClearTile()
    {
        //set ishole na false
        //set sprite to empty
    }

    public void RepairTile()
    {
        //change sprite to randomFixedWall
        //send info to level script
    }
}
