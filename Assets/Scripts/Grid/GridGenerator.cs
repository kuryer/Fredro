using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    [SerializeField] List<Tile> InvalidTiles;
    [SerializeField] List<Tile> ValidTiles;

    [SerializeField] Vector2 possibleHolesAmount;
    [SerializeField] int actualHolesAmount;


    [SerializeField] int levelsAmountToAddHole;

    [SerializeField] List<GameObject> LadderPlaces_1;
    [SerializeField] List<GameObject> LadderPlaces_2;
    [SerializeField] List<GameObject> LadderPlaces_3;

    GameObject Ladder_1;
    GameObject Ladder_2;
    GameObject Ladder_3;

    [SerializeField] GameObject LadderPrefab;
    void Start()
    {
        GenerateGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DeleteGrid();
            GenerateGrid();
        }
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
                    if(holesLeft <= 0)
                        return holesIndexAndTile;
                }
            }
        }
        Debug.Log(holesIndexAndTile.Keys);
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
        for(int i = 0; ValidTiles.Count > i; i++)
        {
            if (!holes.ContainsKey(i))
                ValidTiles[i].ChangeToWallTile();
        }
    }
    void GenerateLadders()
    {
        int ladder1 = GenerateLadders_1();
        int ladder2 = GenerateLadders_2(ladder1);
        GenerateLadders_3(ladder2);
    }

    int GenerateLadders_1()
    {
        int place = Random.Range(0, LadderPlaces_1.Count);
        Vector3 placing = LadderPlaces_1[place].transform.position;
        Ladder_1 = Instantiate(LadderPrefab, placing, Quaternion.identity);
        return place;
    }
    int GenerateLadders_2(int previousPlace)
    {
        int place = -1;
        while (place < 0 || place == previousPlace)
        {
            place = Random.Range(0, LadderPlaces_2.Count);
        }
        Vector3 placing = LadderPlaces_2[place].transform.position;
        Ladder_2 = Instantiate(LadderPrefab, placing, Quaternion.identity);
        return place;
    }
    int GenerateLadders_3(int previousPlace)
    {
        int place = -1;
        while (place < 0 || place == previousPlace)
        {
            place = Random.Range(0, LadderPlaces_3.Count);
        }
        Vector3 placing = LadderPlaces_3[place].transform.position;
        Ladder_3 = Instantiate(LadderPrefab, placing, Quaternion.identity);
        return place;
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
