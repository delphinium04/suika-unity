using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")] 
    public GameObject startPanel;
    public Button startButton;
    public GameObject retryPanel;
    public Button retryButton;
    public TMP_Text scoreText;
 
    // Events
    public Action StartButtonClicked;
    public Action RetryButtonClicked;
    
    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);      
        retryButton.onClick.AddListener(OnRetryButtonClicked);      
    }

    void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    void OnStartButtonClicked()
    {
        StartButtonClicked?.Invoke();
        
        startPanel.SetActive(false);;
    }
    
    void OnRetryButtonClicked()
    {
        RetryButtonClicked?.Invoke();
        
        retryPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void SetGameOver(int score)
    {
        retryPanel.SetActive(true);
        scoreText.text = $"Your Score: {score}";
    }
}
