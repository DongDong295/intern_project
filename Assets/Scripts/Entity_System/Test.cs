using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public EntityHolder entityHolder;
    void Start()
    {
        var model = new CharacterModel();
        model.InitMovementData(5, 5, 5, 0.2f);
        model.InitEventData();
        entityHolder.InitializeBehaviour(model);
    }
}
