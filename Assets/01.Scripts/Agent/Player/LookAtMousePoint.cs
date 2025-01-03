using Crogen.PowerfulInput;
using DG.Tweening;
using UnityEngine;

public class LookAtMousePoint : MonoBehaviour
{
    [field:SerializeField] public InputReader InputReader { get; private set; }

    private void LateUpdate()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(InputReader.MousePosition) - transform.position;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.DORotateQuaternion(Quaternion.Euler(0, 0, angle-90), 0.01f);
    }
}
