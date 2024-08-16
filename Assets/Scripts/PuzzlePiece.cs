using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class PuzzlePiece : MonoBehaviour
{
    [SerializeField]private static int totNumPieces;
     public static int TotNumPieces { get { return totNumPieces; } set { totNumPieces = value; } }
    [SerializeField] private PuzzlePiece[] neighbours;
    private int tileID;
    public int TileID => tileID;
    private Vector2 center;
    public Vector2 TileCenter { get { return center; } set { center = value; } }
    [SerializeField] private Transform waypointTransform;
    private Vector2 waypoint;
    public Transform TileWaypoint { get { return waypointTransform; } set { waypointTransform = value; } }

    
    private bool isComplete;
    public bool IsComplete { get { return isComplete; } set { isComplete = value; } }
   
    private float[] CorrectAngle;
    private float[] CorrectDistanceCenter;
    private float[] CorrectDistanceWaypoint;
    private Vector3[] CorrectVectorCenter;
    [SerializeField][Range(0f, 1f)] private float thresholdAngles;
    [SerializeField][Range(0f, 1f)] private float thresholdDistancePercentage;
    private PuzzlePiece[] connectedTiles;
    [SerializeField] private PuzzleManager puzzleManager;
    private SpriteRenderer spRenderer;
    public SpriteRenderer SpRenderer => spRenderer;
    public bool ToPrint;
    void Awake()
    {
        
        tileID = transform.GetSiblingIndex();
        waypointTransform = transform.GetChild(0);
        thresholdAngles = 0.2f;
        thresholdDistancePercentage = 0.1f;
        center = transform.position;
        waypoint = waypointTransform.position;
        CorrectVectorCenter = new Vector3[neighbours.Length];
        CorrectDistanceCenter = new float[neighbours.Length];
        CorrectDistanceWaypoint = new float[neighbours.Length];
        CorrectAngle = new float[neighbours.Length];

        connectedTiles = new PuzzlePiece[neighbours.Length];
        spRenderer = GetComponent<SpriteRenderer>();
    }
    string[] data;
    private void Start()
    {

        puzzleManager = PuzzleManager.instance;
        totNumPieces = puzzleManager.totNumPieces;
       
        string fullNameDoc = "/PuzzleData/DataTile_" + tileID + ".csv";
        string path = Application.dataPath + fullNameDoc;
        StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
        while (line != "")
        {

           

            data = line.Split(';');

            int i = int.Parse(data[0]);
            float cctx = float.Parse(data[1]);
            float ccty = float.Parse(data[2]);

            float ccD = float.Parse(data[3]);
            float wwD = float.Parse(data[4]);
            float angle = float.Parse(data[5]);

            int currTileID = FindPosition(i); 
            if (currTileID != -1)
            {
              
                CorrectVectorCenter[currTileID] = new Vector2(cctx, ccty);
                CorrectDistanceCenter[currTileID] = ccD;
                CorrectDistanceWaypoint[currTileID] = wwD;

                CorrectAngle[currTileID] = angle;
            }
            line = sr.ReadLine();
        }

        sr.Close();
        // PrintData();
        RotateCorrectVectorCenter(transform.localEulerAngles.z);
    }
    public int FindPosition(int tileID)
    {
       for(int i = 0; i < neighbours.Length; i++) 
        {
            if (neighbours[i].TileID == tileID)
                return i;
        }
       return -1;
    }
    public void UpdatePosition(Vector3 currPos)
    {

        int[] alreadyMoved = new int[totNumPieces];

        Vector3 dx = currPos - transform.position;
        transform.position = currPos;
        ToMove(tileID, alreadyMoved, currPos, dx);
    }
    public void UpdateRotation()
    {


        int[] alreadyRotated = new int[totNumPieces];

        transform.Rotate(Vector3.back, 90f);
        //transform.RotateAround(transform.position, Vector3.back, 90f);
        RotateCorrectVectorCenter();
        ToRotate(tileID, alreadyRotated, transform.position);

    }

    public void ToRotate(int tileID, int[] alreadyRotated, Vector3 pos)
    {

        alreadyRotated[tileID] = 1;
        for (int i = 0; i < neighbours.Length; i++)
        {

            if (connectedTiles[i])
            {
                int currTileID = neighbours[i].TileID;
                if (alreadyRotated[currTileID] == 0)
                {
                    connectedTiles[i].transform.RotateAround(pos, Vector3.back, 90f);

                    connectedTiles[i].RotateCorrectVectorCenter();

                    connectedTiles[i].ToRotate(currTileID, alreadyRotated, pos);

                }
            }
        }
    }
    public void RotateCorrectVectorCenter()
    {
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (connectedTiles[i] == null)
            {
                CorrectVectorCenter[i] = VectorRotation(CorrectVectorCenter[i], -Mathf.PI / 2f);

            }
        }
    }
    public void RotateCorrectVectorCenter(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
       // Debug.Log(tileID + " " + angle + " " + angleRad);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (connectedTiles[i] == null)
            {
                CorrectVectorCenter[i] = VectorRotation(CorrectVectorCenter[i], angleRad);
                
            }
        }

    }
   
    public Vector3 VectorRotation(Vector3 v, float degrees)
    {
        Vector3 u = Vector3.zero;
        u.x = v.x * Mathf.Cos(degrees) - v.y * Mathf.Sin(degrees);
        u.y = v.x * Mathf.Sin(degrees) + v.y * Mathf.Cos(degrees);

        return u;
    }

    public void ToMove(int tilePos, int[] alreadyMoved, Vector3 pos, Vector3 dx)
    {

        alreadyMoved[tilePos] = 1;
        for (int i = 0; i < neighbours.Length; i++)
        {

            if (connectedTiles[i])
            {
                int currTileID = neighbours[i].TileID;
                
                    if (alreadyMoved[currTileID] == 0)
                {

                    connectedTiles[i].transform.position = connectedTiles[i].transform.position + dx;// pos + CorrectVectorCenter[i];

                    connectedTiles[i].ToMove(currTileID, alreadyMoved, connectedTiles[i].transform.position, dx);

                }
            }
        }
    }
    public void IsCorrectPositionNeighbours()
    {
        int[] alreadyChecked = new int[totNumPieces];
        IsCorrectPosition(alreadyChecked);
    }
    public void IsCorrectPosition(int[] alreadyChecked)
    {
        alreadyChecked[tileID] = 1;
        int k = 0;

        for (int i = 0; i < neighbours.Length; i++)
        {

            if (connectedTiles[i] == null)
            {
               
                Vector2 cc = neighbours[i].transform.position - transform.position;
                Vector2 ww = neighbours[i].TileWaypoint.position - waypointTransform.position;
                float distCenter = cc.sqrMagnitude;
                float distWaypoint = ww.sqrMagnitude;
                float currAngle = Vector2.Dot(cc.normalized, ww.normalized);
                float offSetCenter = Mathf.Abs(distCenter - CorrectDistanceCenter[i]);
                float offSetWaypoint = Mathf.Abs(distWaypoint - CorrectDistanceWaypoint[i]);
                float offSetAngle = Mathf.Abs(currAngle - CorrectAngle[i]);

                if (offSetCenter <= thresholdDistancePercentage * CorrectDistanceCenter[i] &&
                    offSetWaypoint <= thresholdDistancePercentage * CorrectDistanceWaypoint[i] &&
                    offSetAngle <= thresholdAngles)
                { Debug.Log(i);
                    //neighbours[i].transform.position = transform.position + CorrectVectorCenter[i];
                    // transform.position = neighbours[i].transform.position - CorrectVectorCenter[i];
                    UpdatePosition(neighbours[i].transform.position - CorrectVectorCenter[i]);
                    connectedTiles[i] = neighbours[i];
                    neighbours[i].RemoveNeighbour(tileID);
                    k++;

                }



            }
            else
            {
                k++;
            }
            int currTileID = neighbours[i].TileID;
            if (alreadyChecked[currTileID] != 1 && connectedTiles[i] != null)
            {
                connectedTiles[i].IsCorrectPosition(alreadyChecked);
            }
        }

        if (k == neighbours.Length && !isComplete)
        {
            NeighboursComplete();
        }


    }
    public void NeighboursComplete()
    {
        isComplete = true;
        puzzleManager.isComplete();
    }
    public void RemoveNeighbour(int tilePos)
    {
        int i = FindPosition(tilePos);

        connectedTiles[i] = neighbours[i];
        

    }
    public PuzzlePiece GetNeighbour(int position)
    {
        return neighbours[position];
    }

    public void PrintData()
    {
        string fullNameDoc = "/PuzzleData/DataTile_" + tileID + ".csv";
        string path = Application.dataPath + fullNameDoc;
        StreamWriter sr = new StreamWriter(path);
        for (int i = 0; i < neighbours.Length; i++)
        {
            

            int currTileID = neighbours[i].TileID;
                Vector2 cct = neighbours[i].transform.position - transform.position;

                Vector2 wwt = neighbours[i].TileWaypoint.position - waypointTransform.position;

                sr.WriteLine(currTileID + ";" + cct.x + ";" + cct.y + ";" + cct.sqrMagnitude + ";" + wwt.sqrMagnitude + ";" + Vector2.Dot(cct.normalized, wwt.normalized));


            
        }


        sr.WriteLine();
        sr.Close();
    }
}
