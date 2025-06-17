using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _repaetRate;

    private ObjectPool<Cube> _pool;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolCapacity);

        _wait = new WaitForSeconds(_repaetRate);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubesRoutine());
    }

    private void ActionOnGet(Cube cube)
    {
        cube.ResetState();

        Vector3 spawnOffset = Random.insideUnitSphere * _spawnRadius;
        spawnOffset.y = Mathf.Abs(spawnOffset.y);
        cube.transform.position = _spawnPoint.transform.position + spawnOffset;

        cube.LifeExpired += ReleaseCube;
        cube.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.LifeExpired -= ReleaseCube;
        cube.gameObject.SetActive(false);
    }

    private IEnumerator SpawnCubesRoutine()
    {
        bool isWorking = true;

        while (isWorking)
        {
            yield return _wait;
            GetCube();
        }
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }
}