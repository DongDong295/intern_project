using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class ProjectileAbilityConfigItem
    {
        public float projectileSpeed;
        public float abilityDamage;
        public float projectileRange;
        public bool canPierce;
        public bool canSearchEnemy;
        public float searchRange;
    }

    public class ProjectileAbilityConfig : SerializedScriptableObject
    {
        public ProjectileAbilityConfigItem[] items;
    }
}
