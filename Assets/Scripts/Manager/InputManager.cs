using Runtime.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoSingleton<InputManager>
{
    public Vector2 MoveDirection;
    void Start()
    {
        
    }
    void Update()
    {
        var moveVector = new Vector2();
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
        MoveDirection = moveVector;
    }
}
