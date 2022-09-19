using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobBase : MonoBehaviour
{
    private int healthPoints = 100;
    [SerializeField]
    protected float speed = 5f;
    protected static bool isMoving = false;
    private Vector3Int mobPos;
    protected static MapManager mapManager;
    protected static Tilemap tileMap;
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log(mapManager);
        tileMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
    }
    private void Update()
    {
        Death();
    }
    protected virtual IEnumerator UnitMovement(List<Vector3> waypoints)
    {
        //while (transform.position != waypoints[waypoints.Count - 1])
        //{
        //    for (int i = 0; i < waypoints.Count;)
        //    {
        //        if (transform.position != waypoints[i]) transform.position = Vector2.MoveTowards(transform.position, waypoints[i], speed);
        //        else i++;
        //        yield return new WaitForSeconds(1);
        //    }
        //}
        foreach (Vector3 waypoint in waypoints)
        {
            if (transform.position != waypoints[waypoints.Count - 1])
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoint, speed);
                yield return new WaitForSeconds(0.1f);
            }
            else isMoving = false;
        }
    }
    void Death()
    {
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    protected List<Vector3> PathCalc(Vector3 targetPos)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3Int targetCell = tileMap.WorldToCell(targetPos);
        Vector3 cellPosInWorld = new Vector3();
        mobPos = tileMap.WorldToCell(transform.position);


        Vector3Int waypointProjection = mobPos;
        while (waypointProjection != targetCell)
        {
            if (mobPos.x > targetCell.x)
            {
                mobPos = new Vector3Int(mobPos.x - 1, mobPos.y, 0);
                cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
            }
            else if (mobPos.x < targetCell.x)
            {
                mobPos = new Vector3Int(mobPos.x + 1, mobPos.y, 0);
                cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
            }
            if (mobPos.y > targetCell.y)
            {
                mobPos = new Vector3Int(mobPos.x, mobPos.y - 1, 0);
                cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
            }
            else if (mobPos.y < targetCell.y)
            {
                mobPos = new Vector3Int(mobPos.x, mobPos.y + 1, 0);
                cellPosInWorld = tileMap.GetCellCenterWorld(mobPos);
                
            }
            path.Add(cellPosInWorld);
            waypointProjection = mobPos;
        }
        return path;
    }   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach(Vector3 point in PathCalc(Camera.main.ScreenToWorldPoint(Input.mousePosition))) Gizmos.DrawSphere(point, 0.2f);
    }
}
