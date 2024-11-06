using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MollyDamageField : MonoBehaviour
{
    public TestCharacterAbilityQ1 owner;
    public List<EntityHolder> hit;

    public void Start()
    {
        hit = new List<EntityHolder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            owner.AddHitList(collision.gameObject.GetComponent<EntityHolder>());
            hit.Add(collision.gameObject.GetComponent<EntityHolder>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            owner.RemoveHitList(collision.gameObject.GetComponent<EntityHolder>());
            hit.Remove(collision.gameObject.GetComponent<EntityHolder>());
        }
    }

    private void OnDestroy()
    {
        foreach (var item in hit)
        {
            owner.RemoveHitList(item);
            hit.Remove(item);
        }
    }
}
