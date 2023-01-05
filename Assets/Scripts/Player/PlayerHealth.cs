using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public bool IsInvinsible => _isInvinsible;

    [SerializeField]
    private Color _invisibilityColor;

    [SerializeField]
    private float _invisibilityDuration;

    [SerializeField]
    private List<Renderer> _coloredComponents;

    private IEnumerable<Material> _materials;

    [SyncVar(hook = nameof(OnInvinsibilityChanged))]
    private bool _isInvinsible = false;

    private Color _defaultColor;

    public void Hit()
    {
        if (_isInvinsible)
            return;

        ApplyInvincibility();
    }

    private void Awake()
    {
        _materials = _coloredComponents.Select(renderer => renderer.material);

        _defaultColor = _materials.First().color;
    }

    private void OnInvinsibilityChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            ChangeColorTo(_invisibilityColor);
        }
        else
        {
            ChangeColorTo(_defaultColor);
        }
    }

    private void ChangeColorTo(Color color)
    {
        foreach (Material material in _materials)
        {
            material.color = color;
        }
    }

    private void ApplyInvincibility()
    {
        _isInvinsible = true;

        StartCoroutine(ResetInvinsibility());
    }

    private IEnumerator ResetInvinsibility()
    {
        yield return new WaitForSeconds(_invisibilityDuration);

        _isInvinsible = false;
    }
}
