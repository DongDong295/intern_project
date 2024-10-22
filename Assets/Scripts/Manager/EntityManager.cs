using Runtime.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoSingleton<EntityManager>
{
    [SerializeField] EntityHolder holder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
        }
    }
}
