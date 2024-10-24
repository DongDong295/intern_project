using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public EntityHolder entityHolder;
    void Start()
    {
        var model = new EntityModel();
        model.InitMovementData(5, 10);
        entityHolder.InitializeBehaviour(model);
    }

    void Update()
    {
        
    }
}
