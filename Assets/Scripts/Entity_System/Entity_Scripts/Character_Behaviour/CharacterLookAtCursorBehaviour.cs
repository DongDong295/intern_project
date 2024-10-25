using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterLookAtCursorBehaviour : EntityBehaviour, IEntityUpdate
{
    [SerializeField] GameObject weaponHolder;
    public override void Initialize(IEntityData data)
    {
        base.Initialize(data);
    }

    public void OnUpdate(float deltaTime)
    {
        RotateWeapon();
    }

    public void OnFixedUpdate(float deltaTime)
    {

    }

    void RotateWeapon()
    {
        var direction = InputManager.Instance.CursorPosition() - transform.position;
        float rotation_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponHolder.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        weaponHolder.transform.localScale = scale;
    }
}
