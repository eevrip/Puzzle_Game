using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragDrop : MonoBehaviour
{
   
    private Camera cam;
    private Vector3 offset;
    private Tile tile;
    private void OnMouseDown()
    {
      
        if (EventSystem.current.IsPointerOverGameObject())
        {
            
            return;
        }
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        offset = transform.position - mousePos;
    }
    private void OnMouseDrag()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 currPos = cam.ScreenToWorldPoint(mousePos) + offset; 
       // transform.position = currPos;
        tile.UpdatePosition(currPos);
    }
    private void OnMouseUp()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        tile.IsCorrectPositionNeighbours();
        //if (tile.IsCorrectPosition())
       // {
       //     Debug.Log("Correct " + tile.TileID);
       // }
    }
    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(Input.GetMouseButtonDown(1)) {
            //transform.Rotate(Vector3.back, 90f);
            tile.UpdateRotation();

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        tile = GetComponent<Tile>();

    }

    
}
