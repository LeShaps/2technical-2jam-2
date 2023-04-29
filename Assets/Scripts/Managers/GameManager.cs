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
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.BossPatternFocus:
                HandleBossPatternFocus();
                break;
            case GameState.BossPatternCircle:
                HandleBossPatternCircle();
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
        ChangeState(GameState.BossPatternFocus);
    }

    private void HandleBossPatternFocus()
    {
        Boss.Instance.StartPatterns();
    }

    private void HandleBossPatternCircle()
    {
        // Boss.Instance.PatternCircle();
    }

    private void HandleWin()
    {
        Debug.Log("======== GameManager::Win ========");
    }
}

[Serializable]
public enum GameState
{
    Starting = 0,
    BossPatternFocus = 1,
    BossPatternCircle = 2,
    Win = 3,
}

[Serializable]
public enum Player
{
    Yin = 0,
    Yang = 1,
}