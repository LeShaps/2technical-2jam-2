using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        Debug.Log("============================== WIN");
        Boss.Instance.StopPatterns();
    }
}

[Serializable]
public enum GameState
{
    Starting = 0,
    BossPatterns = 1,
    UltimateReady = 2,
    Win = 3,
}

[Serializable]
public enum Player
{
    Yin = 0,
    Yang = 1,
}