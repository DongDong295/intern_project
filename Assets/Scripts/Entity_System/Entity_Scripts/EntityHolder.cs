using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityHolder : MonoBehaviour
{
    private bool _built = false;
    private List<IEntityUpdate> _updateBehaviour;
    public void InitializeBehaviour(EntityModel model)
    {
        _built = true;
        _updateBehaviour = new List<IEntityUpdate>();
        var behaviours = GetComponents<IEntityBehaviour>();
        foreach (var behaviour in behaviours)
        {
            behaviour.Initialize(model);
            if(behaviour is IEntityUpdate)
                _updateBehaviour.Add((IEntityUpdate)behaviour);
        }
    }

    private void Update()
    {
        if(!_built)
            return;
        else
            foreach(var behaviour in _updateBehaviour)
            {
                behaviour.OnUpdate(Time.deltaTime);
            }
    }

    private void FixedUpdate()
    {
        if (!_built)
            return;
        else
            foreach (var behaviour in _updateBehaviour)
            {
                behaviour.OnFixedUpdate(Time.fixedDeltaTime);
            }
    }
}
