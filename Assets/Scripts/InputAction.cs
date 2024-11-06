using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public enum CharacterInputAction
{
    None,
    Primary,
    AbilityQ,
    AbilityE,
    Dash
}

public enum PlayerInputAction{
    None,
    Pause
}


public struct InputActionMessage : IMessage
{
    public CharacterInputAction action;
    public InputActionMessage(CharacterInputAction input)
    {
        action = input;
    }
}

public struct PlayerInputMessage : IMessage
{
    public PlayerInputAction action;
    public PlayerInputMessage(PlayerInputAction input)
    {
        action = input;
    }
}