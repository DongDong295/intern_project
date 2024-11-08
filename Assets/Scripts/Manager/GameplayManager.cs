using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using UnityEditor;

public class GameplayManager : MonoSingleton<GameplayManager>
{
    public GameState CurrentGameState;
    public List<GameState> PauseState;


    protected override void Awake()
    {
        CurrentGameState = GameState.Prepare;
    }
    void Start()
    {
        
    }

    void Update()
    {
    }

    public void ChangeState(GameState gameState)
    {
        CurrentGameState = gameState;
    }

}

public enum GameState
{
    None,
    Prepare,
    Start,
    Playing,
    Pause,
    End
}