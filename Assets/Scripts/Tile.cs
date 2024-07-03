using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class Tile : MonoBehaviour
{

    private int tileID;
    public int TileID => tileID;
    [SerializeField] private Tile[] neighbours;
    private Vector2 center;
    public Vector2 TileCenter { get { return center; } set { center = value; } }
    [SerializeField] private Transform waypointTransform;
    private Vector2 waypoint;
    public Transform TileWaypoint { get { return waypointTransform; } set { waypointTransform = value; } }

    private int totNumNeighbours;
    private bool isComplete;
    public bool IsComplete { get { return isComplete; } set { isComplete = value; } }
    public int TotNumNeighbours { get { return totNumNeighbours; } set { totNumNeighbours = value; } }
    private float[] CorrectAngle;
    private float[] CorrectDistanceCenter;
    private float[] CorrectDistanceWaypoint;
    private Vector3[] CorrectVectorCenter;
    [SerializeField][Range(0f, 1f)] private float thresholdAngles;
    [SerializeField][Range(0f, 1f)] private float thresholdDistancePercentage;
    [SerializeField] private Tile[] connectedTiles;
    [SerializeField] private PuzzleManager puzzleManager;
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

        connectedTiles = new Tile[neighbours.Length];
    }
    string[] data;
    private void Start()
    {

        puzzleManager = GetComponentInParent<PuzzleManager>();

        /*   for (int i = 0; i < neighbours.Length; i++)
        // {
        //     if (neighbours[i] != null)
        //   { Vector2 cct = neighbours[i].transform.position - transform.position;
             CorrectVectorCenter[i] = cct;
             Vector2 wwt = neighbours[i].TileWaypoint.position - waypointTransform.position;


             CorrectDistanceCenter[i] = cct.sqrMagnitude;
             CorrectDistanceWaypoint[i] = wwt.sqrMagnitude;

             CorrectAngle[i] = Vector2.Dot(cct.normalized, wwt.normalized);
           }
       }*/
        string fullNameDoc = "/PuzzleData/DataTile_" + tileID + ".csv";
        string path = Application.dataPath + fullNameDoc;
        StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
        while (line != "")
        {

            totNumNeighbours++;

            data = line.Split(';');

            int i = int.Parse(data[0]);
            float cctx = float.Parse(data[1]);
            float ccty = float.Parse(data[2]);

            float ccD = float.Parse(data[3]);
            float wwD = float.Parse(data[4]);
            float angle = float.Parse(data[5]);

            CorrectVectorCenter[i] = new Vector2(cctx, ccty);
            CorrectDistanceCenter[i] = ccD;
            CorrectDistanceWaypoint[i] = wwD;

            CorrectAngle[i] = angle;
            line = sr.ReadLine();
        }

        sr.Close();
        // PrintData();
    }

    public void UpdatePosition(Vector3 currPos)
    {

        int[] alreadyMoved = new int[neighbours.Length];

        Vector3 dx = currPos - transform.position;
        transform.position = currPos;
        ToMove(tileID, alreadyMoved, currPos, dx);
    }
    public void UpdateRotation()
    {


        int[] alreadyRotated = new int[neighbours.Length];

        // transform.Rotate(Vector3.back, 90f);
        transform.RotateAround(transform.position, Vector3.back, 90f);
        RotateCorrectVectorCenter();
        ToRotate(tileID, alreadyRotated, transform.position);

    }

    public void ToRotate(int tilePos, int[] alreadyRotated, Vector3 pos)
    {

        alreadyRotated[tilePos] = 1;
        for (int i = 0; i < neighbours.Length; i++)
        {

            if (connectedTiles[i])
            {

                if (alreadyRotated[i] == 0)
                {
                    connectedTiles[i].transform.RotateAround(pos, Vector3.back, 90f);

                    connectedTiles[i].RotateCorrectVectorCenter();

                    connectedTiles[i].ToRotate(i, alreadyRotated, pos);

                }
            }
        }
    }
    public void RotateCorrectVectorCenter()
    {
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null && connectedTiles[i] == null)
            {
                CorrectVectorCenter[i] = VectorRotation(CorrectVectorCenter[i], -Mathf.PI / 2f);

            }
        }
    }
    public void RotateCorrectVectorCenter(int tileID)
    {

        if (neighbours[tileID] != null && connectedTiles[tileID] == null)
        {
            CorrectVectorCenter[tileID] = VectorRotation(CorrectVectorCenter[tileID], -Mathf.PI / 2f);


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

                if (alreadyMoved[i] == 0)
                {

                    connectedTiles[i].transform.position = connectedTiles[i].transform.position + dx;// pos + CorrectVectorCenter[i];

                    connectedTiles[i].ToMove(i, alreadyMoved, connectedTiles[i].transform.position, dx);

                }
            }
        }
    }
    public void IsCorrectPositionNeighbours()
    {
        int[] alreadyChecked = new int[connectedTiles.Length];
        IsCorrectPosition(alreadyChecked);
    }
    public void IsCorrectPosition(int[] alreadyChecked)
    {
        alreadyChecked[tileID] = 1;
        int k = 0;

        for (int i = 0; i < neighbours.Length; i++)
        {

            if (neighbours[i] != null && connectedTiles[i] == null)
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
                {
                    //neighbours[i].transform.position = transform.position + CorrectVectorCenter[i];
                    // transform.position = neighbours[i].transform.position - CorrectVectorCenter[i];
                    UpdatePosition(neighbours[i].transform.position - CorrectVectorCenter[i]);
                    connectedTiles[i] = neighbours[i];
                    neighbours[i].RemoveNeighbour(tileID);
                    k++;

                }



            }
            else if (neighbours[i] != null && connectedTiles[i] != null)
            {
                k++;
            }
            if (alreadyChecked[i] != 1 && connectedTiles[i] != null)
            {
                connectedTiles[i].IsCorrectPosition(alreadyChecked);
            }
        }

        if (k == totNumNeighbours && !isComplete)
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

        connectedTiles[tilePos] = neighbours[tilePos];
        //neighbours[tilePos] = null;

    }
    public Tile GetNeighbour(int position)
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
            if (neighbours[i] != null)
            {


                Vector2 cct = neighbours[i].transform.position - transform.position;
                
                Vector2 wwt = neighbours[i].TileWaypoint.position - waypointTransform.position;

                sr.WriteLine(i + ";" + cct.x + ";" + cct.y + ";" + cct.sqrMagnitude + ";" + wwt.sqrMagnitude + ";" + Vector2.Dot(cct.normalized, wwt.normalized));
               

            }
        }


        sr.WriteLine();
        sr.Close();
    }
}
