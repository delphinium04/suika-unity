using System;
using UnityEngine;

public class TriggerEventHelper : MonoBehaviour
{
    public Action<Collider2D> Enter;

    void OnTriggerEnter2D(Collider2D other)
    {
        Enter?.Invoke(other);
    }
}