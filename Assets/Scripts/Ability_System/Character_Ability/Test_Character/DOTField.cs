using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DOTField : MonoBehaviour
{
    private AbilityStrategy _owner;
    private List<EntityHolder> _hit;

    public void InitField(AbilityStrategy owner)
    {
        _hit = new List<EntityHolder>();
        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _owner.AddHitList(collision.gameObject.GetComponent<EntityHolder>());
            _hit.Add(collision.gameObject.GetComponent<EntityHolder>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _owner.RemoveHitList(collision.gameObject.GetComponent<EntityHolder>());
            _hit.Remove(collision.gameObject.GetComponent<EntityHolder>());
        }
    }

    private void OnDestroy()
    {
        foreach (var item in _hit)
        {
            _owner.RemoveHitList(item);
            _hit.Remove(item);
        }
    }
}
