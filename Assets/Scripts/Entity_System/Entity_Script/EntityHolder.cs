using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EntityHolder : MonoBehaviour
{
    private bool _built = false;
    private List<IEntityUpdate> _updateBehaviour;

    public async UniTask InitializeBehaviour(IEntityData data)
    {
        _built = true;
        _updateBehaviour = new List<IEntityUpdate>();
        var behaviours = GetComponents<IEntityBehaviour>();
        foreach (var behaviour in behaviours)
        {
            try
            {
                await behaviour.Initialize(data);
            }
            catch(Exception e)
            {
                Debug.LogError("GO HERE");
            }

            if (behaviour is IEntityUpdate)
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
