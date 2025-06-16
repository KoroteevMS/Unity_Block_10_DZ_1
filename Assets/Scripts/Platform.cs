using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Changer _changer;

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Cube cube);
        _changer.OnTriggerZone(cube);
    }
}