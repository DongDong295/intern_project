using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterMolly : MonoBehaviour
{
    List<EntityHolder> list;

    private void Start()
    {
        list = new List<EntityHolder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            list.Add(collision.gameObject.GetComponent<EntityHolder>());
        }
    }
}
