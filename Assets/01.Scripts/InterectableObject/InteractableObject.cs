using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityEvent OnInteractEvent;
    public UnityEvent OnSelectedEvent;
    public UnityEvent OnCanceledEvent;
    private bool _isSelected = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_isSelected == false)
            {
                OnSelectedEvent?.Invoke();
            }
            _isSelected = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _isSelected)
        {
            OnInteractEvent?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isSelected == true)
        {
            OnCanceledEvent?.Invoke();
        }
        _isSelected = false;
    }
}
