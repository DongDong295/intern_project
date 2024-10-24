using Runtime.Core.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.EventSystems;
using ZBase.Foundation.PubSub;

public class InputManager : MonoSingleton<InputManager>
{
    public Vector2 MoveDirection;
    void Start()
    {
        
    }
    void Update()
    {
        InputMovement();
        CursorPosition();
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
        return cursorPosition;
    }

    public void InputAction()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SimpleMessenger.Publish(new InputActionMessage(CharacterInputAction.Dash));
        }
    }
}
