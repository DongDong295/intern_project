using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyModel : IEntityHealthData
{
    private float _health;

    private bool _isAlive;
    public float Health => _health;
    public bool IsAlive => _isAlive;

    public void InitHealthData(float health)
    {
        _isAlive = true;
        _health = health;
    }
}
