using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class MeleeAbilityConfigItem
    {
        public float abilityDamage;
        public float knockbackForce;
    }

    public class MeleeAbilityConfig : SerializedScriptableObject
    {
        public MeleeAbilityConfigItem[] dummy;
    }
}

