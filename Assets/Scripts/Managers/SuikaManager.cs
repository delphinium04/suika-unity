using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class SuikaManager : MonoBehaviour
{
    // Preset
    [Header("References")] [SerializeField]
    GameObject suikaPrefab;

    // settings
    const float DefaultY = 4f;
    const float DefaultXMin = -6f;
    const float DefaultXMax = 6f;
    const float TouchCooldown = 2f;
    const float WaitTimeThreshold = 12f;

    // variables
    Camera _mainCamera;
    Suika _currentSuika;
    Transform _suikaParent;

    bool _isSettled = true;
    bool _isTouchBegan; // Track 끝 이후 KeepTouching에 들어가는 상태 방지

    void Awake()
    {
        enabled = false;
    }

    void Start()
    {
        _mainCamera = Camera.main;
        _suikaParent = new GameObject("SuikaParent").transform;

        Managers.Input.Touched += Input_OnTouched;
    }

    void OnEnable()
    {
        ShowSuikaPreview();
    }

    void OnDisable()
    {
        DestroyAllSuika();
    }

    void DestroyAllSuika()
    {
        _currentSuika = null;
        if (!_suikaParent) return;
        foreach (Transform child in _suikaParent)
        {
            Destroy(child.gameObject);
        }
    }

    void Input_OnTouched(Touch touch)
    {
        if (!_isSettled) return;

        switch (touch.phase)
        {
            // Instantiate
            case TouchPhase.Began:
                StartTouching(touch);
                break;
            // Moving
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (!_isTouchBegan) return;
                KeepTouching(touch);
                break;
            // Drop
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (!_isTouchBegan) return;
                EndTouching(touch);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void ShowSuikaPreview()
    {
        _currentSuika = InstantiateSuika(Vector2.up * DefaultY);
        _currentSuika.GetComponent<Rigidbody2D>().simulated = false;
    }

    void StartTouching(Touch touch)
    {
        _isTouchBegan = true;
        Vector2 touchWorldPosition = ConvertTouchToValidPosition(touch);
        _currentSuika.transform.position = touchWorldPosition;
    }

    void KeepTouching(Touch touch)
    {
        Vector2 touchWorldPosition = ConvertTouchToValidPosition(touch);
        _currentSuika.transform.position = touchWorldPosition;
    }

    void EndTouching(Touch touch)
    {
        _isTouchBegan = false;
        _currentSuika.GetComponent<Rigidbody2D>().simulated = true;
        StartCoroutine(nameof(WaitForSuikaToSettle));
    }

    Suika InstantiateSuika(Vector2 spawnPosition)
    {
        GameObject go = Instantiate(
            suikaPrefab,
            spawnPosition,
            Quaternion.identity,
            _suikaParent);
        return go.GetComponent<Suika>();
    }

    IEnumerator WaitForSuikaToSettle()
    {
        _isSettled = false;

        // waitTime 대기
        float startTime = Time.time;
        yield return new WaitForSeconds(TouchCooldown);

        // suika IsMoving || 흐른 시간 확인
        do
        {
            yield return null;
            if (Time.time - startTime > WaitTimeThreshold) break;

            var isBreakable = true;
            for (var i = 0; i < _suikaParent.childCount && isBreakable; i++)
            {
                var suika = _suikaParent.GetChild(i).GetComponent<Suika>();
                isBreakable &= !suika.IsMoving;
            }

            if (isBreakable) break;
        } while (true);

        _isSettled = true;
        ShowSuikaPreview();
    }

    Vector2 ConvertTouchToValidPosition(Touch touch)
    {
        Vector3 position = _mainCamera.ScreenToWorldPoint(touch.position);
        position.x = Mathf.Clamp(position.x, DefaultXMin, DefaultXMax);
        position.y = DefaultY;

        return position;
    }
}