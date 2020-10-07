using System;
using System.Collections.Generic;
using UnityEngine;

public class FireballPool : MonoBehaviour
{
    [SerializeField] private int fireballCount = 20;
    [SerializeField] private GameObject fireballPrefab = default;

    private readonly Queue<GameObject> _fireballGameObjectsQueue = new Queue<GameObject>();
    [HideInInspector] public GameObject voiceOverObj;

    private void Start()
    {
        for (int i = 0; i < fireballCount; i++)
        {
            var fireBall = Instantiate(fireballPrefab, transform, true);
            fireBall.SetActive(false);
            _fireballGameObjectsQueue.Enqueue(fireBall);
        }
    }

    private void Update()
    {
        if (!voiceOverObj)
        {
            voiceOverObj = GameObject.Find("/Witch VO Triggers"); //TODO refactor this!
        }
    }

    public GameObject GetFireBall()
    {
        var fireball = _fireballGameObjectsQueue.Dequeue();
        fireball.SetActive(true);
        return fireball;
    }

    public void ReturnFireBall(GameObject fireBall)
    {
        fireBall.SetActive(false);
        _fireballGameObjectsQueue.Enqueue(fireBall);
    }
}