using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.PubSub;

public enum CharacterInputAction
{
    None,
    Primary,
    AbilityQ,
    Dash
}

public struct InputActionMessage : IMessage
{
    public CharacterInputAction action;
    public InputActionMessage(CharacterInputAction input)
    {
        action = input;
    }
}