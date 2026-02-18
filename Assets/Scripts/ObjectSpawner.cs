using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T _objectPrefab;

    [SerializeField] private int _capacityPool = 10;
    [SerializeField] private int _maxSizePool = 20;

    private int _totalSpawnedObjects = 0;

    protected float _spawnInterval;
    protected string _textUI;
    protected ObjectPool<T> _objectPool;

    public int ActiveCount => _objectPool != null ? _objectPool.CountActive : 0;
    public int CreatedCount => _objectPool != null ? _objectPool.CountAll : 0;
    public int TotalSpawnedCount => _totalSpawnedObjects;
    public string TextUI => _textUI;

    public event Action OnPoolDataChanged;

    protected virtual void Awake()
    {
        _objectPool = new ObjectPool<T>
            (
            createFunc: () => Instantiate(_objectPrefab),
            actionOnGet: GetingFromPool,
            actionOnRelease: ReleasingCleanUp,
            actionOnDestroy: (spawnObject) => Destroy(spawnObject.gameObject),
            collectionCheck: true,
            defaultCapacity: _capacityPool,
            maxSize: _maxSizePool
            );

        OnPoolDataChanged?.Invoke();
    }

    protected virtual IEnumerator StartingSpawning()
    {
        WaitForSeconds timer = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return timer;
            _objectPool.Get();
        }
    }

    protected virtual void GetingFromPool(T poolObject)
    {
        poolObject.gameObject.SetActive(true);
        _totalSpawnedObjects++;
        OnPoolDataChanged?.Invoke();
    }

    protected virtual void ReleasingCleanUp(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
        OnPoolDataChanged?.Invoke();
    }

    protected virtual void ReturningToPool(T poolObject)
    {
        _objectPool.Release(poolObject);
    }
}
