using UnityEngine;

public class Patrol : State
{
    private int _currentWaypoint = 0;
    private readonly PatrolPath _patrolPath;
    private GameObject _waypoint;

    public Patrol(StateData stateData): base(stateData)
    {
        _patrolPath = StateData.PatrolPath;
    }

    protected override void Enter()
    {
        if (!_patrolPath) return;
        if (!_patrolPath.HasClosestWaypoint)
        {
            SetClosestWaypoint();
        }
        else
        {
            _currentWaypoint = StateData.LastWaypointIndex + 1;
            _waypoint = _patrolPath.GetWaypointGameObject(_currentWaypoint);
            SetWaypointDestination();
        }

        base.Enter();
    }

    protected override void Update()
    {
        SetNavMeshAgentSpeed();
        
        if (StateData.EnemySight.CanSeeTarget() || StateData.EnemySight.IsAttacked)
        {
            _patrolPath.HasClosestWaypoint = false;
            NextState = StateData.StateFactory.GetPursueState(StateData);
            Stage = StateEvent.Exit;
        }
        
        if (CalculateDistanceToWaypoint() <= StateData.DestinationDistanceAccuracy)
        {
            if (_patrolPath.Length == 1)
            {
                NextState = StateData.StateFactory.GetGuardState(StateData);
                Stage = StateEvent.Exit;
                return;
            }

            if (StateData.IdleAtWaypoint)
            {
                StateData.LastWaypointIndex = _currentWaypoint;
                NextState = StateData.StateFactory.GetIdleState(StateData);
                Stage = StateEvent.Exit;
            }
            else
            {
                _currentWaypoint++;
                _currentWaypoint %= _patrolPath.Length;
                
                _waypoint = _patrolPath.GetWaypointGameObject(_currentWaypoint);
                SetWaypointDestination();
            }
        }
    }

    private float CalculateDistanceToWaypoint()
    {
        return !_waypoint ? Mathf.Infinity : Vector3.Distance(StateData.Npc.transform.position, _waypoint.transform.position);
    }
    
    private void SetClosestWaypoint()
    {
        var waypoints = StateData.PatrolPath.GetAllWaypoints();
        float lastDistance = Mathf.Infinity;
        for (int i = 0; i < waypoints.Count; i++)
        {
            GameObject currentWaypoint = waypoints[i];
            float distance = Vector3.Distance(StateData.Npc.transform.position, currentWaypoint.transform.position);
            if (distance < lastDistance)
            {
                lastDistance = distance;
                _currentWaypoint = i - 1;
            }
        }

        _currentWaypoint = Mathf.Max(0, _currentWaypoint);
        _waypoint = _patrolPath.GetWaypointGameObject(_currentWaypoint);
        SetWaypointDestination();
        _patrolPath.HasClosestWaypoint = true;
    }

    private void SetWaypointDestination()
    {
        var newDestination = _waypoint.transform.position;
        newDestination.y = StateData.Npc.transform.position.y;
        StateData.NavMeshAgent.SetDestination(newDestination);
    }
}