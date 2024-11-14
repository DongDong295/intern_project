using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private AbilityStrategy _owner;
    private List<string> _targetTags;
    private List<EntityHolder> _hit;
    public void InitHitbox(AbilityStrategy owner, List<string> targetTags)
    {
        _owner = owner;
        _targetTags = targetTags;
        _hit = new List<EntityHolder>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetTags.Contains(collision.gameObject.tag))
        {
            _owner.AddHitList(collision.gameObject.GetComponent<EntityHolder>());
            _hit.Add(collision.gameObject.GetComponent<EntityHolder>());

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_targetTags.Contains(collision.gameObject.tag))
        {
            _owner.RemoveHitList(collision.gameObject.GetComponent<EntityHolder>());
            _hit.Remove(collision.gameObject.GetComponent<EntityHolder>());
        }
    }
}
