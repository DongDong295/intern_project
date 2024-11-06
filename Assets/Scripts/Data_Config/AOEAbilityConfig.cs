using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class AOEAbilityConfigItem
    {
        public float abilityDuration;
        public float abilityDamage;
    }
    public class AOEAbilityConfig : SerializedScriptableObject
    {
        public AOEAbilityConfigItem[] items;
    }
}

