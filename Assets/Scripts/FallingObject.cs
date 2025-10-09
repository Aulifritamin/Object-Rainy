using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class FallingObject : MonoBehaviour
{
    private float _initialTimeToLive = 0f;
    private bool _initialIsContacted = false;
    private Vector3 _initialPosition = Vector3.zero;
    private Color _initialColor = Color.gray;

    private Coroutine _despawnCoroutine;

    public bool IsContacted { get; private set; } = false;
    public float TimeToLive { get; private set; } = 0f;

    public Renderer ObjectRenderer { get; private set; }
    public Rigidbody ObjectRigidbody { get; private set; }

    public event Action <FallingObject> OnDespawn;
    public event Action <FallingObject> CollidedGround;

    private void Awake()
    {
        ObjectRenderer = GetComponent<Renderer>();
        ObjectRigidbody = GetComponent<Rigidbody>();
    }

    public void ResetState()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }

        IsContacted = _initialIsContacted;
        TimeToLive = _initialTimeToLive;

        ObjectRigidbody.linearVelocity = _initialPosition;
        ObjectRigidbody.angularVelocity = _initialPosition;
        ObjectRenderer.material.color = _initialColor;
    }

    public void SetColor(Color color)
    {
        if (ObjectRenderer != null)
        {
            ObjectRenderer.material.color = color;
        }
    }

    public void SetTimeToLive(float time)
    {
        TimeToLive = time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && IsContacted == false)
        {
            CollidedGround?.Invoke(this);
            IsContacted = true;
            _despawnCoroutine = StartCoroutine(DespawnTimer());
        }
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(TimeToLive);
        OnDespawn?.Invoke(this);
    }
}