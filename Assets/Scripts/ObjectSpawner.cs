using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private UtilitiesRandom _utilities;
    [SerializeField] private FallingObject _objectPrefab;

    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private int _capacityPool = 10;
    [SerializeField] private int _maxSizePool = 20;

    private ObjectPool<FallingObject> _objectPool;

    private void Awake()
    {
        _objectPool = new ObjectPool<FallingObject>
            (
            createFunc: () => Instantiate(_objectPrefab),
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseCleanUp,
            actionOnDestroy: (spawnObject) => Destroy(spawnObject.gameObject),
            collectionCheck: true,
            defaultCapacity: _capacityPool,
            maxSize: _maxSizePool
            );

        for (int i = 0; i < _capacityPool; i++)
        {
            var obj = _objectPool.Get();
            _objectPool.Release(obj);
        }
    }

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            _objectPool.Get();
        }
    }

    private void OnGetFromPool(FallingObject poolObject)
    {
        poolObject.OnDespawn += ReturnToPool;
        poolObject.CollidedGround += HandleCollidedGround;
        poolObject.transform.position = _utilities.GetRandomPositionOnTerrain();
        poolObject.gameObject.SetActive(true);
    }

    private void OnReleaseCleanUp(FallingObject poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolObject.OnDespawn -= ReturnToPool;
        poolObject.CollidedGround -= HandleCollidedGround;
        poolObject.ResetState();
    }

    private void ReturnToPool(FallingObject poolObject)
    {
        _objectPool.Release(poolObject);
    }
    
    private void HandleCollidedGround(FallingObject obj)
    {
        obj.SetColor(_utilities.GetRandomColor());
        obj.SetTimeToLive(_utilities.GetRandomTimeToLive());
    }
}
