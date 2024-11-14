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

    private List<string> _targetTag;
    

    public void InitField(AbilityStrategy owner, List<string> targetTag)
    {
        _hit = new List<EntityHolder>();
        _owner = owner;
        _targetTag = targetTag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetTag.Contains(collision.gameObject.tag))
        {
            _owner.AddHitList(collision.gameObject.GetComponent<EntityHolder>());
            _hit.Add(collision.gameObject.GetComponent<EntityHolder>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_targetTag.Contains(collision.gameObject.tag))
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
