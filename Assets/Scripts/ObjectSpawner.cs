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
            actionOnGet: GetingFromPool,
            actionOnRelease: ReleasingCleanUp,
            actionOnDestroy: (spawnObject) => Destroy(spawnObject.gameObject),
            collectionCheck: true,
            defaultCapacity: _capacityPool,
            maxSize: _maxSizePool
            );
    }

    private void Start()
    {
        StartCoroutine(StartingSpawning());
    }

    private IEnumerator StartingSpawning()
    {
        WaitForSeconds timer = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return timer;
            _objectPool.Get();
        }
    }

    private void GetingFromPool(FallingObject poolObject)
    {
        poolObject.OnDespawn += ReturningToPool;
        poolObject.transform.position = _utilities.GetingRandomPositionOnTerrain();
        poolObject.gameObject.SetActive(true);
    }

    private void ReleasingCleanUp(FallingObject poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolObject.OnDespawn -= ReturningToPool;
        poolObject.ResetingState();
    }

    private void ReturningToPool(FallingObject poolObject)
    {
        _objectPool.Release(poolObject);
    }
}
