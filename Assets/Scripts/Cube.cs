using UnityEngine;

public class Cube : MonoBehaviour
{
    private bool _isContact = false;
    private MeshRenderer _meshRenderer;

    public bool IsContact => _isContact;

    public void Initialize(bool isContact)
    {
        _isContact = isContact;
    }

    public void ConfirmContact()
    {
        _isContact = !_isContact;
    }

    public void SetColor(Color color)
    {
        _meshRenderer.material.color = color;
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}