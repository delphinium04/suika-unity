using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public event Action<Touch> Touched;

    bool IsTouching => Input.touchCount > 0;

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!IsTouching) return;
        Touch touch = Input.GetTouch(0);
        Touched?.Invoke(touch);
    }
}