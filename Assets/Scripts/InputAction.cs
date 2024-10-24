using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public enum CharacterInputAction
{
    None,
    Primary,
    Dash
}

public struct InputActionMessage : IMessage
{
    CharacterInputAction action;
    public InputActionMessage(CharacterInputAction input)
    {
        action = input;
    }
}