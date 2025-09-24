using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // Components
    UIManager _uiManager;
    SuikaManager _suikaManager;
    
    // Game Variables
    int _score;
    bool _isGameOver = false;

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

        _uiManager.StartButtonClicked += UI_OnStartButtonClicked;

        _suikaManager.enabled = false;
    }

    void OnDestroy()
    {
        // _uiManager.StartButtonClicked -= UI_OnStartButtonClicked;
    }

    void UI_OnStartButtonClicked()
    {
        StartGame();
    }

    public void StartGame()
    {
        _isGameOver = false;
        _suikaManager.enabled = true;
        _suikaManager.ResetSuika();

        _score = 0;
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public void EndGame()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        _uiManager.SetGameOver(_score);
    }
}