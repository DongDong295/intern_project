using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityUpdate
{
    public void OnUpdate(float deltaTime);
    public void OnFixedUpdate(float deltaTime);
}
