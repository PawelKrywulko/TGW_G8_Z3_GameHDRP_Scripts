using UnityEngine;
using UnityEngine.AI;

public class StateData
{
    public StateFactory StateFactory { get; set; }
    public GameObject Npc { get; set; }
    public Animator Animator { get; set; }
    public NavMeshAgent NavMeshAgent { get; set; }
    public EnemySight EnemySight { get; set; }
    public PatrolPath PatrolPath { get; set; }
    public float WalkSpeed { get; set; }
    public float RunSpeed { get; set; }
    public float AccelerationFactor { get; set; }
    public float DecelerationFactor { get; set; }
    public float DestinationDistanceAccuracy { get; set; }
    public bool IsInDebugMode { get; set; }
    public Sword Sword { get; set; }
    public Rifle Rifle { get; set; }
    public int LastWaypointIndex { get; set; }
    public bool IdleAtWaypoint { get; set; }
    public float MinIdleTime { get; set; }
    public float MaxIdleTime { get; set; }
    public float SafeDistance { get; set; }
    public bool CheckLastKnownPosition { get; set; }
    public int ScareChance { get; set; }
}