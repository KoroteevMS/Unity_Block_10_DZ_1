using System;
using System.Collections;
using UnityEngine;

public class Changer : MonoBehaviour
{
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;

    private WaitForSeconds _wait;
    private Color _red;

    public event Action <Cube> CubeChange;

    private void Awake()
    {
        _wait = new WaitForSeconds(UnityEngine.Random.Range(_minDelay, _maxDelay));
        _red = new Color(1f, 0f, 0f);
    }

    public void OnTriggerZone(Cube cube)
    {
        if (cube.IsContact)
            return;

        cube.ConfirmContact();
        cube.SetColor(_red);
        StartCoroutine(LifeCountdown(cube));
    }

    private IEnumerator LifeCountdown(Cube cube)
    {
        yield return _wait;
        CubeChange?.Invoke(cube);
    }
}