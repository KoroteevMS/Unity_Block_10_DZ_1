using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;

    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;

    private bool _isContact = false;
    private Quaternion _currentRotation;
    private WaitForSeconds _wait;

    public event Action<Cube> LifeExpired;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _wait = new WaitForSeconds(UnityEngine.Random.Range(_minDelay, _maxDelay));
    }

    public void HandleCollision()
    {
        if (_isContact)
            return;

        _isContact = true;
        SetColor(Color.red);
        StartCoroutine(LifeCountdown());
    }

    public void SetColor(Color color)
    {
        _meshRenderer.material.color = color;
    }

    public void ResetState()
    {
        _isContact = false;
        _currentRotation.eulerAngles = Vector3.zero;
        transform.rotation = _currentRotation;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private IEnumerator LifeCountdown()
    {
        yield return _wait;
        LifeExpired?.Invoke(this);
    }
}