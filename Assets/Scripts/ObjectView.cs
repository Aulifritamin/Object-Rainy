using TMPro;
using UnityEngine;

public abstract class ObjectView<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected ObjectSpawner<T> _spawner;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        RefreshView();
    }

    private void OnEnable()
    {
        if (_spawner != null)
        {
            _spawner.OnPoolDataChanged += RefreshView;
            RefreshView();
        }
    }

    private void OnDisable()
    {
        if (_spawner != null)
        {
            _spawner.OnPoolDataChanged -= RefreshView;
        }
    }

    private void RefreshView()
    {
        _text.text = $"{_spawner.TextUI}\n" +
                     $"Активно: {_spawner.ActiveCount} | " +
                     $"Создано: {_spawner.CreatedCount} | " +
                     $"Всего: {_spawner.TotalSpawnedCount}";
    }
}
