using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Core.Singleton;

public class GameplayManager : MonoSingleton<GameplayManager>
{
    public GameState GameState;

    protected override void Awake()
    {
        GameState = GameState.Start;
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
    Start,
    Playing,
    End
}