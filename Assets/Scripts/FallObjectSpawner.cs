using System;
using System.Collections;
using UnityEngine;

public class FallObjectSpawner : ObjectSpawner<FallingObject>
{
    [SerializeField] private UtilitiesRandom _utilities;
    private float _FallingObjectSpawnInterval = 1f;
    private string _nameObject = "Falling Object";

    public event Action<Vector3> ObjectReturnedToPool;

    public void ActivateSpawner()
    {
        _spawnInterval = _FallingObjectSpawnInterval;
        _textUI = _nameObject;
        StartCoroutine(StartingSpawning());
    }

    protected override void GetingFromPool(FallingObject poolObject)
    {
        base.GetingFromPool(poolObject);
        poolObject.OnDespawn += ReturningToPool;
        poolObject.transform.position = _utilities.GetingRandomPositionOnTerrain();
        poolObject.gameObject.SetActive(true);
    }

    protected override void ReleasingCleanUp(FallingObject poolObject)
    {
        base.ReleasingCleanUp(poolObject);
        ObjectReturnedToPool?.Invoke(poolObject.transform.position);
        poolObject.OnDespawn -= ReturningToPool;
        poolObject.ResetingState();
    }
}
