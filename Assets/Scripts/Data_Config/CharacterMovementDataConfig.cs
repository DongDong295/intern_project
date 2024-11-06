using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runtime.DataConfig
{
    public class CharacterMovementDataConfigItem
    {
        public float characterSpeed;
        public float characterDashRange;
        public float characterDashSpeed;
        public float characterDashDuration;
        public int characterDashCounter;
        public float characterDashCooldown;
    }

    public class CharacterMovementDataConfig : SerializedScriptableObject
    {
        public CharacterMovementDataConfigItem[] items;
    }
}

