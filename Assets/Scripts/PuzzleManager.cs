using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Tile[] tiles;
    int[,] puzzleGraph;
    int[,] connectedTiles ;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
   
    void Start()
    {
        tiles = GetComponentsInChildren<Tile>();
        sprite = GetComponent<SpriteRenderer>();
    }
    public void isComplete()
    {
        foreach (Tile tile in tiles)
        {
            if (!tile.IsComplete)
            {
                return;
            }
        }
        foreach (Tile tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
        sprite.enabled = true;
    }
   
   
}
