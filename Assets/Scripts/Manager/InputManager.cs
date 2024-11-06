using Runtime.Core.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using ZBase.Foundation.PubSub;

public class InputManager : MonoSingleton<InputManager>
{
    public Vector2 MoveDirection;
    void Update()
    {
        InputMovement();
        CursorPosition();
        InputAction();
        PlayerInput();
    }

    public Vector3 InputMovement()
    {
        var moveVector = new Vector3();
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
        return moveVector;
    }

    public Vector3 CursorPosition()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        return cursorPosition;
    }

    public void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SimpleMessenger.Publish(new PlayerInputMessage(PlayerInputAction.Pause));
        }
    }

    public void InputAction()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SimpleMessenger.Publish(new InputActionMessage(CharacterInputAction.Dash));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SimpleMessenger.Publish(new InputActionMessage(CharacterInputAction.AbilityQ));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SimpleMessenger.Publish(new InputActionMessage(CharacterInputAction.AbilityE));
        }
        if (Input.GetMouseButton(0))
        {
            SimpleMessenger.Publish(new InputActionMessage(CharacterInputAction.Primary));
        }
    }
}
