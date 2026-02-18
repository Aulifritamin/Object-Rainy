using UnityEngine;

public class Spawners : MonoBehaviour
{
    [SerializeField] private FallObjectSpawner _fallObjectSpawner;
    [SerializeField] private BombSpawner _bombSpawner;

    private void Start()
    {
        _fallObjectSpawner.ActivateSpawner();
        _fallObjectSpawner.ObjectReturnedToPool += _bombSpawner.SpawnBomb;
    }

    private void OnDisable()
    {
        _fallObjectSpawner.ObjectReturnedToPool -= _bombSpawner.SpawnBomb;
    }
}
