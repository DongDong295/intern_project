using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class AbilityConfigItem
    {
        public float cooldown;
        public CastType castType;
        public AbilityType abilityType;
        public Target target;
        public float attackRange; //Enemy Attack Range
    }

    public class AbilityConfig : SerializedScriptableObject
    {
        public AbilityConfigItem[] testCharacter;
        public AbilityConfigItem[] dummy;
    }
}
