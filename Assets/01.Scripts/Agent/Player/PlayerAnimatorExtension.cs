using UnityEngine;

public class PlayerAnimatorExtension : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetValueFalse(string valueName)
    {
        _animator.SetBool(valueName, false);
    }

    public void SetValueTrue(string valueName)
    {
        _animator.SetBool(valueName, true);
    }
}
