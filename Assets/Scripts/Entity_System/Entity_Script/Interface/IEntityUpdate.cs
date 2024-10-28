using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityUpdate
{
    void OnUpdate(float deltaTime);
    void OnFixedUpdate(float deltaTime);
}
