using System;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour
{
    // References
    public TriggerEventHelper suikaTrigger;
    
    // Components
    UIManager _uiManager;
    SuikaManager _suikaManager;
    
    public Action<int> ScoreChanged;
    public Action GameStarted;
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

    void Start()
    {
        _uiManager = Managers.UI;
        _suikaManager = Managers.Suika;

        suikaTrigger.Enter += GameOver;
        _uiManager.StartButtonClicked += UI_OnStartButtonClicked;
        _suikaManager.enabled = false;
    }

    void UI_OnStartButtonClicked()
    {
        StartGame();
        GameStarted?.Invoke();
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