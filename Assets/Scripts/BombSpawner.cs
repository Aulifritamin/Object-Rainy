using UnityEngine;

public class BombSpawner : ObjectSpawner<Bomb>
{
    [SerializeField] private UtilitiesRandom _utilities;

    private string _nameObject = "Bomb";

    protected override void Awake()
    {
        base.Awake();
        _textUI = _nameObject;
    }

    public void SpawnBomb(Vector3 position)
    {
        Bomb newBomb = _objectPool.Get();

        newBomb.transform.position = position;
    }

    protected override void GetingFromPool(Bomb poolObject)
    {
        base.GetingFromPool(poolObject);
        poolObject.SetTimeToLive(_utilities.GetingRandomTimeToLive());
        poolObject.Activate();
        poolObject.OnDespawn += ReturningToPool;
    }

    protected override void ReleasingCleanUp(Bomb poolObject)
    {
        poolObject.OnDespawn -= ReturningToPool;
        poolObject.ResetState();
        base.ReleasingCleanUp(poolObject);
    }
}
