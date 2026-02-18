using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Rigidbody _rigidbody;

    private float _initialTimeToLive = 0f;
    private Vector3 _initialPosition = Vector3.zero;
    private float TimeToLive = 0f;

    private float _explosionRadius = 10f;
    private float _explosionForce = 700f;

    private float _startingAlpha = 1f;
    private float _targetAlpha = 0f;

    private Color _color;
    private Material _material;

    private Coroutine _despawnCoroutine;
    private Coroutine _lifeCycleCoroutine;

    public event Action <Bomb> OnDespawn;

    public void SetTimeToLive(float time)
    {
        TimeToLive = time;
    }

    public void ResetState()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }

        if (_lifeCycleCoroutine != null)
        {
            StopCoroutine(_lifeCycleCoroutine);
            _lifeCycleCoroutine = null;
        }

        TimeToLive = _initialTimeToLive;
        transform.position = _initialPosition;
        _color.a = _startingAlpha;
    }

    public void Activate()
    {
        _despawnCoroutine = StartCoroutine(DespawningTimer());
        _lifeCycleCoroutine = StartCoroutine(LifeCycleRoutine());
    }

    private void Awake()
    {
        _material = _renderer.material;
        _color = _material.color;
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }

    private IEnumerator DespawningTimer()
    {
        yield return new WaitForSeconds(TimeToLive);
        Explode();
        OnDespawn?.Invoke(this);
    }

    private IEnumerator LifeCycleRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < TimeToLive)
        {
            elapsedTime += Time.deltaTime;

            float time = elapsedTime / TimeToLive;

            float alpha = Mathf.Lerp(_startingAlpha, _targetAlpha, time);

            _color.a = alpha;

            _material.color = _color;

            yield return null;
        }
    }
}
