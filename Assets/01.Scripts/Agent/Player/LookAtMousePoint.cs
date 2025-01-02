using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.PowerfulInput;
using UnityEngine;

public class LookAtMousePoint : MonoBehaviour
{
    [field:SerializeField] public InputReader InputReader { get; private set; }

    private void LateUpdate()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(InputReader.MousePosition) - transform.position;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
    }
}
