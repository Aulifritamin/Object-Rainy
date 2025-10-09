using UnityEngine;

public class UtilitiesRandom : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;
    [SerializeField] private float _spawnHeightOffset = 20f;
    [SerializeField] private float _edgePadding = 0.7f;

    private float _minValue = 2f;
    private float _maxValue = 5f;

    public Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    public float GetRandomTimeToLive()
    {
        return Random.Range(_minValue, _maxValue);
    }

    public Vector3 GetRandomPositionOnTerrain()
    {
        var terrain = _terrain != null ? _terrain : Terrain.activeTerrain;
        var data = terrain.terrainData;

        float x = Random.Range(_edgePadding, data.size.x - _edgePadding);
        float z = Random.Range(_edgePadding, data.size.z - _edgePadding);
        float y = terrain.SampleHeight(new Vector3(x, 0f, z));
        return new Vector3(x, y + _spawnHeightOffset, z);
    }
}
