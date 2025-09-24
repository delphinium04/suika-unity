using System;
using System.Collections;
using UnityEngine;

public class SuikaManager : MonoBehaviour
{
    // Components
    InputManager _inputManager;

    // Preset
    [Header("References")] [SerializeField]
    Camera mainCamera;

    [SerializeField] GameObject suikaPrefab;
    [SerializeField] Transform suikaParent;

    // settings
    float _defaultY = 7f;
    float _defaultXMin = -3.2f;
    float _defaultXMax = 3.2f;
    float _touchCooldown = 2f;
    float _waitTimeThreshold = 12f;

    // variables
    Suika _currentSuika;
    bool _isSettled = true;
    bool _isTouchBegan; // Track 끝 이후 KeepTouching에 들어가는 상태 방지

    void Start()
    {
        if (!mainCamera) mainCamera = Camera.main;
        _inputManager = GetComponent<InputManager>();

        _inputManager.Touched += Input_OnTouched;
    }

    void OnDestroy()
    {
        // _inputManager.Touched -= Input_OnTouched;
    }

    public void ResetSuika()
    {
        foreach (Transform child in suikaParent)
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

    void StartTouching(Touch touch)
    {
        _isTouchBegan = true;
        Vector2 touchWorldPosition = ConvertTouchToValidPosition(touch);
        _currentSuika = InstantiateSuika(touchWorldPosition);
        _currentSuika.GetComponent<Rigidbody2D>().simulated = false;
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
        StartCoroutine(nameof(TrackSuika));
    }

    Suika InstantiateSuika(Vector2 spawnPosition)
    {
        GameObject go = Instantiate(
            suikaPrefab,
            spawnPosition,
            Quaternion.identity,
            suikaParent);
        return go.GetComponent<Suika>();
    }

    IEnumerator TrackSuika()
    {
        _isSettled = false;

        // waitTime 대기
        float startTime = Time.time;
        yield return new WaitForSeconds(_touchCooldown);

        // suika IsMoving || 흐른 시간 확인
        do
        {
            yield return null;
            if (Time.time - startTime > _waitTimeThreshold) break;

            var isBreakable = true;
            for (var i = 0; i < suikaParent.childCount && isBreakable; i++)
            {
                var suika = suikaParent.GetChild(i).GetComponent<Suika>();
                isBreakable &= !suika.IsMoving;
            }

            if (isBreakable) break;
        } while (true);

        _isSettled = true;
    }

    Vector2 ConvertTouchToValidPosition(Touch touch)
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(touch.position);
        position.x = Mathf.Clamp(position.x, _defaultXMin, _defaultXMax);
        position.y = _defaultY;

        return position;
    }
}