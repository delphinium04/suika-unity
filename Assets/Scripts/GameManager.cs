using System;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour
{
    // Components
    UIManager _uiManager;
    SuikaManager _suikaManager;
    
    // References
    public TriggerEventHelper suikaTrigger;
    
    public static GameManager Instance { get; private set; }
    public Action<int> ScoreChanged;
    public Action GameEnded;
    
    int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreChanged?.Invoke(_score);
        }
    }
    int _score;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _uiManager = GetComponent<UIManager>();
        _suikaManager = GetComponent<SuikaManager>();

        suikaTrigger.Enter += GameOver;
        _uiManager.StartButtonClicked += UI_OnStartButtonClicked;
        _suikaManager.enabled = false;
    }

    void UI_OnStartButtonClicked()
    {
        StartGame();
    }

    void StartGame()
    {
        Score = 0;
        _suikaManager.enabled = true;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    void GameOver(Collider2D _)
    {
        _suikaManager.enabled = false;
        GameEnded?.Invoke();
    }
}