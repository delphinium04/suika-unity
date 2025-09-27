using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        if (!IsTouching) return;
        if (IsTouchOverInteractableUI(Input.GetTouch(0).position)) return;
        Touched?.Invoke(Input.GetTouch(0));
    }

    bool IsTouchOverInteractableUI(Vector2 touchPosition)
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var r in results)
        {
            // 필터링하고 싶은 interact 가능한 UI 나열
            if (r.gameObject.TryGetComponent(out Button btn))
            {
                if (btn.interactable)
                    return true;
            }
        }
        return false;
    }
}