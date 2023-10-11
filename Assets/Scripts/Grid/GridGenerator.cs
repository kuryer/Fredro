using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    List<Tile> InvalidTiles;
    List<Tile> ValidTiles;

    [SerializeField] Vector2 possibleHolesAmount;
    int actualHolesAmount;


    [SerializeField] int levelsAmountToAddHole;

    [SerializeField] List<GameObject> LadderPlaces_1;
    [SerializeField] List<GameObject> LadderPlaces_2;
    [SerializeField] List<GameObject> LadderPlaces_3;

    [SerializeField] GameObject Ladder_1;
    [SerializeField] GameObject Ladder_2;
    [SerializeField] GameObject Ladder_3;

    [SerializeField] GameObject LadderPrefab;
    void Start()
    {
        
    }

    public void GenerateGrid()
    {
        Dictionary<int, int> holes = FindHoles();
        Generate(holes);
        GenerateLadders();
    }
    Dictionary<int, int> FindHoles()
    {
        Dictionary<int, int> holesIndexAndTile = new Dictionary<int, int>();
        int holesLeft = actualHolesAmount;
        int lastTileIndex = -1;
        
        while(holesLeft > 0)
        {
            for(int i = 0; ValidTiles.Count - 1 > i; i++)
            {
                if (holesIndexAndTile.ContainsKey(i))
                    continue;
                if (IsHole())
                {
                    int newTileIndex = ValidTiles[i].ChangeToHoleTile(lastTileIndex);
                    holesIndexAndTile.Add(i, newTileIndex);
                    lastTileIndex = newTileIndex;
                    holesLeft--;
                }
            }
        }
        return holesIndexAndTile;
    }

    bool IsHole()
    {
        int value = Random.Range(0, 2);
        if (value == 0)
            return true;
        return false;
    }

    void Generate(Dictionary<int, int> holes)
    {
        foreach(Tile tile in InvalidTiles)
        {
            tile.ChangeToWallTile();
        }
        for(int i = 0; ValidTiles.Count - 1 > i; i++)
        {
            if (!holes.ContainsKey(i))
                ValidTiles[i].ChangeToWallTile();
        }
    }
    void GenerateLadders()
    {
        GenerateLadders_1();
        GenerateLadders_2();
        GenerateLadders_3();
    }

    void GenerateLadders_1()
    {
        int place = Random.Range(0, LadderPlaces_1.Count);
        Vector3 placing = LadderPlaces_1[place].transform.position;
        Ladder_1 = Instantiate(LadderPrefab, placing, Quaternion.identity);
    }
    void GenerateLadders_2()
    {
        int place = Random.Range(0, LadderPlaces_2.Count);
        Vector3 placing = LadderPlaces_2[place].transform.position;
        Ladder_2 = Instantiate(LadderPrefab, placing, Quaternion.identity);
    }
    void GenerateLadders_3()
    {
        int place = Random.Range(0, LadderPlaces_3.Count);
        Vector3 placing = LadderPlaces_3[place].transform.position;
        Ladder_3 = Instantiate(LadderPrefab, placing, Quaternion.identity);
    }

    public void DeleteGrid()
    {
        DeleteLadders();
        DeleteTiles();
    }

    void DeleteLadders()
    {
        Destroy(Ladder_1);
        Destroy(Ladder_2);
        Destroy(Ladder_3);
    }

    void DeleteTiles()
    {
        foreach(Tile tile in ValidTiles)
        {
            tile.ClearTile();
        }
        foreach(Tile tile in InvalidTiles)
        {
            tile.ClearTile();
        }
    }
}
