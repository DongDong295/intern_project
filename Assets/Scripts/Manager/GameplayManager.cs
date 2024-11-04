using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;

public class GameplayManager : MonoSingleton<GameplayManager>
{
    public GameState GameState;

    protected override void Awake()
    {
        GameState = GameState.Prepare;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameState GetGameState()
    {
        return GameState;
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