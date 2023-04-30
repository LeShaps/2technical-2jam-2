using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _yangReticleCanvas;
    [SerializeField] private GameObject _athCanvas;
    [SerializeField] private TextMeshProUGUI _timeScoreText;
    [SerializeField] private GameObject _quitButton;

    [SerializeField] private PlayerController _yinController;
    [SerializeField] private PlayerController _yangController;
    public Player ActivePlayer { get; set; }
    public PlayerController ActivePlayerController { get; set; }

    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    public static GameManager Instance { get; private set; }
    void Awake() => Instance = this;

    void Start()
    {
        _yinController.Player = Player.Yin;
        _yangController.Player = Player.Yang;
        ActivePlayer = Player.Yin;
        ActivePlayerController = _yinController;
        
        ChangeState(GameState.Starting);
        SoundManager.GetInstance().StartLoop("Music1", "Yin", 1);
        SoundManager.GetInstance().StartLoop("Music2", "Yang", 0);
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.BossPatterns:
                HandleBossPatterns();
                break;
            case GameState.UltimateReady:
                HandleUltimateReady();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.End:
                HandleEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }

    public void SwitchActivePlayer()
    {
        if (ActivePlayer == Player.Yang) {
            ActivePlayer = Player.Yin;
            ActivePlayerController = _yinController;
        } else {
            ActivePlayer = Player.Yang;
            ActivePlayerController = _yangController;
        }
        _yinController.Move = Vector2.zero;
        _yangController.Move = Vector2.zero;
    }

    private void HandleStarting()
    {
        ChangeState(GameState.BossPatterns);
    }

    private void HandleBossPatterns()
    {
        Boss.Instance.StartPatterns();
    }

    private void HandleUltimateReady()
    {
        State = GameState.UltimateReady;
    }

    private void HandleWin()
    {
        _yangReticleCanvas.SetActive(false);
        _athCanvas.SetActive(false);
        Boss.Instance.StopPatterns();
    }

    private void HandleEnd()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _quitButton.SetActive(true);
        double timeScore = (Mathf.Round(Time.timeSinceLevelLoad * 100)) / 100.0;
        _timeScoreText.text = $"Time : {timeScore}s";
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame()");
    }
}

[Serializable]
public enum GameState
{
    Starting = 0,
    BossPatterns = 1,
    UltimateReady = 2,
    Win = 3,
    End = 4,
}

[Serializable]
public enum Player
{
    Yin = 0,
    Yang = 1,
}