using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzlePiece[] tiles;
    int[,] puzzleGraph;
    int[,] connectedTiles ;
    private SpriteRenderer sprite;
    private static TileSorting sorting = new TileSorting();
    public static TileSorting Sorting => sorting;
    // Start is called before the first frame update


    public static PuzzleManager instance;
    public int totNumPieces;
    void Awake()
    {
        instance = this; 
        tiles = GetComponentsInChildren<PuzzlePiece>();
        totNumPieces = tiles.Length;
    }
    void Start()
    {
       
        sprite = GetComponent<SpriteRenderer>();
        sorting.Clear();
        foreach (var tile in tiles)
        {
            sorting.Add(tile.SpRenderer);
        }
        sorting.ResetSortingOrder();
    }
    public void isComplete()
    {
        foreach (PuzzlePiece tile in tiles)
        {
            if (!tile.IsComplete)
            {
                return;
            }
        }
        foreach (PuzzlePiece tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
        sprite.enabled = true;
    }
   
   
}
