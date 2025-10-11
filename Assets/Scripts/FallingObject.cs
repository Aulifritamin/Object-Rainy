using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class FallingObject : MonoBehaviour
{
    [SerializeField] private UtilitiesRandom _utilities;

    private float _initialTimeToLive = 0f;
    private bool _initialIsContacted = false;
    private Vector3 _initialPosition = Vector3.zero;
    private Color _initialColor = Color.gray;

    private Coroutine _despawnCoroutine;

    public bool IsContacted { get; private set; } = false;
    public float TimeToLive { get; private set; } = 0f;

    public Renderer Renderer { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    public event Action <FallingObject> OnDespawn;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void ResetingState()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }

        IsContacted = _initialIsContacted;
        TimeToLive = _initialTimeToLive;

        Rigidbody.linearVelocity = _initialPosition;
        Rigidbody.angularVelocity = _initialPosition;
        Renderer.material.color = _initialColor;
    }

    public void SetingColor(Color color)
    {
        if (Renderer != null)
        {
            Renderer.material.color = color;
        }
    }

    public void SetingTimeToLive(float time)
    {
        TimeToLive = time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsContacted == false && collision.gameObject.TryGetComponent<Ground>(out _))
        {
            IsContacted = true;
            SetingTimeToLive(_utilities.GetingRandomTimeToLive());
            SetingColor(_utilities.GetingRandomColor());
            _despawnCoroutine = StartCoroutine(DespawningTimer());
        }
    }

    private IEnumerator DespawningTimer()
    {
        yield return new WaitForSeconds(TimeToLive);
        OnDespawn?.Invoke(this);
    }
}