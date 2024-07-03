using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField] private Tile parentTile;
    private int connectorID;
    private int parentTileID;
    public int ConnectorID => connectorID;
    public int ParentTileID => parentTileID;
    // Start is called before the first frame update
    void Start()
    {
        parentTileID = parentTile.TileID;
      //  connectorID = parentTile.GetNeighboursID(transform.GetSiblingIndex());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger");
        Connector conn = col.gameObject.GetComponent<Connector>();
        if (conn != null)
        {
            Debug.Log("Found connector");
           // if (conn.ConnectorID == parentTileID && connectorID == conn.ParentTileID)
           // {
           //     Debug.Log("Connected" + parentTileID + " " + connectorID);
           // }
        }
    }
    
}
