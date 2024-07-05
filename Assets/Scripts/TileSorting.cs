using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class TileSorting
{
    [SerializeField] private List<SpriteRenderer> tileSprite = new List<SpriteRenderer>();
    public TileSorting() { }

    public void Clear()
    {
        tileSprite.Clear();
    }
    public void Add(SpriteRenderer renderer)
    {
        tileSprite.Add(renderer);
    }
    public void Remove(SpriteRenderer renderer)
    {
        tileSprite.Remove(renderer);
    }

    public void ResetSortingOrder()
    {
        int i = 0;
        foreach (var renderer in tileSprite)
        {
            i++;
            renderer.sortingOrder = i;
            SetZPosition(renderer, i);

        }
    }
    public void SetZPosition(SpriteRenderer renderer, int order)
    {
        Vector3 currPos = renderer.transform.position;
        float newZ = -order / 10f;
        renderer.transform.position = new Vector3(currPos.x, currPos.y, newZ);


    }
    public void BringToFront(SpriteRenderer renderer)
    {
        int highestSortingOrder = tileSprite[tileSprite.Count - 1].sortingOrder;
        if (highestSortingOrder != renderer.sortingOrder)
        {
            Debug.Log("not highest");

            Remove(renderer);
            //int highestSortingOrder = tileSprite[tileSprite.Count - 1].sortingOrder;
            Add(renderer);
            if (highestSortingOrder < 30)
            {
                renderer.sortingOrder = highestSortingOrder + 1;
                //SetZPosition(renderer, renderer.sortingOrder);
                SetZPosition(renderer, highestSortingOrder + 1);

            }
            else
            {

                ResetSortingOrder();
            }
        }

    }
}
