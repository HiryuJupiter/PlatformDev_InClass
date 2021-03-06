﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesPoolManager : MonoBehaviour
{
    [SerializeField] private Tile[] tilePfs;

    private List<TileTypes> allTileTypes = new List<TileTypes>();
    private Dictionary<TileTypes, Pool> poolLookUp = new Dictionary<TileTypes, Pool>();

    public float GetTileSize() => tilePfs[0].GetComponent<BoxCollider2D>().size.x;

    public void Initialize()
    {
        //Initialize reference collections
        foreach (var tile in tilePfs)
        {
            //If we haven't stored this key in the look up dictionary, then store it.
            if (!poolLookUp.ContainsKey(tile.TileType))
            {
                allTileTypes.Add(tile.TileType);
                poolLookUp.Add(tile.TileType, new Pool(tile.gameObject, transform));
            }
        }

        foreach (var item in poolLookUp)
        {
            item.Value.InitializeInactives(5);
        }
    }

    #region Spawn tile
    public Tile Spawn(TileTypes type)
    {
        GameObject go = poolLookUp[type].Spawn();
        return go.GetComponent<Tile>();
    }

    public Tile SpawnRandom() => 
        Spawn(allTileTypes[Random.Range(0, allTileTypes.Count)]);

    public Tile SpawnRandomExcept(List<TileTypes> excepts)
    {
        List<TileTypes> validTypes = new List<TileTypes>(allTileTypes);
        foreach (var e in excepts)
        {
            if (validTypes.Contains(e)) 
                validTypes.Remove(e);
        }

        return Spawn(allTileTypes[Random.Range(0, allTileTypes.Count)]);
    }
    #endregion
}


//public Tile GetRandomPrefabExcept(List<TileTypes> excepts)
//{
//    List<TileTypes> validTypes = new List<TileTypes>(allTileTypes);
//    foreach (var t in excepts)
//    {
//        validTypes.Remove(t);
//    }
//    return prefabLookUp[validTypes[Random.Range(0, validTypes.Count)]];
//}