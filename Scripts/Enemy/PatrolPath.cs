using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private float waypointGizmoRadius = 0.3f;
    
    public bool HasClosestWaypoint { get; set; }
    public int Length { get; private set; }

    private void Awake()
    {
        Length = transform.childCount;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
        }
    }

    private static int GetNextIndex(int i)
    {
        return i + 1;
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i % transform.childCount).position;
    }

    public GameObject GetWaypointGameObject(int i)
    {
        return transform.GetChild(i % transform.childCount).gameObject;
    }

    public List<GameObject> GetAllWaypoints()
    {
        return transform.GetComponentsInChildren<Transform>().Select(obj => obj.gameObject).ToList();
    }
}