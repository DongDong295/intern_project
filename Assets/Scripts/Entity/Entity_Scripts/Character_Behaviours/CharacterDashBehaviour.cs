using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashBehaviour : EntityBehavior<IEntityMovementData>, IEntityUpdate
{
    private float _dashRange;

    IEntityMovementData _data;

    public override void InitializeData(IEntityMovementData data)
    {
        _data = data;
        _dashRange = _data.DashRange;
    }

    public void OnUpdate(float deltaTime)
    {

    }

    public void OnFixedUpdate(float deltaTime)
    {

    }

    void Dash()
    {
        if (_data.CanDash)
            Debug.Log("Dash");
    }
}
