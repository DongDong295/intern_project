using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class SelfBuffAbilityConfigItem
    {
        public float healthBuffAmount;
        public float healthDuration;
        public float speedBuffAmount;
        public float speedDuration;
        public float primaryBuffAmount;
        public float primaryDuration;
        public float qBuffAmount;
        public float qDuration;
        public float ebuffAmount;
        public float ebuffDuration;
        public float ultBuffAmount;
        public float ultDuration;
    }

    public class SelfBuffAbilityConfig : SerializedScriptableObject
    {
        public SelfBuffAbilityConfigItem[] items;
    }
}

