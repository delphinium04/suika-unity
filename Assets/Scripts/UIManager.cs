using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")] 
    public Button startButton;
    public Button retryButton;
    public TMP_Text scoreText;
 
    // Components
    GameManager _gameManager;
    
    // Events
    public Action StartButtonClicked;
    public Action RetryButtonClicked;
    
    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _gameManager.ScoreChanged += SetScoreUI;
        _gameManager.GameEnded += ShowRetryButton;
        
        startButton.onClick.AddListener(OnStartButtonClicked);      
        retryButton.onClick.AddListener(OnRetryButtonClicked);      
        
        startButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    void ShowRetryButton()
    {
        retryButton.gameObject.SetActive(true);
    }

    void OnStartButtonClicked()
    {
        StartButtonClicked?.Invoke();
        startButton.gameObject.SetActive(false);
    }
    
    void OnRetryButtonClicked()
    {
        RetryButtonClicked?.Invoke();
        retryButton.gameObject.SetActive(false);
    }

    void SetScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }
}
