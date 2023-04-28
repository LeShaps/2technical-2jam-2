using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    public static GameManager Instance { get; private set; }
    void Awake() => Instance = this;

    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState) {
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

    private void HandleStarting()
    {
    }

    private void HandleBossPatternFocus()
    {
        // Boss.Instance.PatternFocus();
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
public enum GameState {
    Starting = 0,
    BossPatternFocus = 1,
    BossPatternCircle = 2,
    Win = 3,
}