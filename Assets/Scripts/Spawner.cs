using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Changer _changer;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repaetRate;

    private ObjectPool<Cube> _pool;
    Color _prefabColor; 

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolCapacity);

        _changer.CubeChange += ReleaseCube;

        _prefabColor = _prefab.GetComponent<MeshRenderer>().sharedMaterial.color;
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repaetRate);
    }

    private void ActionOnGet(Cube cube)
    {
        Vector3 spawnOffset = Random.insideUnitSphere * _spawnRadius;
        spawnOffset.y = Mathf.Abs(spawnOffset.y);
        cube.transform.position = _spawnPoint.transform.position + spawnOffset;
        cube.transform.rotation = _prefab.transform.rotation;

        cube.Initialize(_prefab.IsContact);
        cube.SetColor(_prefabColor);

        cube.gameObject.SetActive(true);
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
