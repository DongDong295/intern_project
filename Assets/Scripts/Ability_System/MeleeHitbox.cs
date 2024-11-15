using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private AbilityStrategy _owner;
    public void InitHitbox(AbilityStrategy owner)
    {
        _owner = owner;
        Debug.Log(_owner);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            _owner.AddHitList(collision.gameObject.GetComponent<EntityHolder>());
        }
        else
            return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            _owner.RemoveHitList(collision.gameObject.GetComponent<EntityHolder>());
        }
        else
            return;
    }
}
