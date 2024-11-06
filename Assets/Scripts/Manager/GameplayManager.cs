using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;
using UnityEditor;

public class GameplayManager : MonoSingleton<GameplayManager>
{
    public GameState GameState;
    public List<GameState> PauseState;


    protected override void Awake()
    {
        GameState = GameState.Prepare;
        PauseState = new List<GameState>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangeState(GameState gameState)
    {
        GameState = gameState;
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