using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runtime.DataConfig
{
    public class MovementDataConfigItem
    {
        public float speed;
        public float dashRange;
        public float dashSpeed;
        public float dashDuration;
        public int dashCounter;
        public float dashCooldown;
    }

    public class MovementDataConfig : SerializedScriptableObject
    {
        public MovementDataConfigItem[] items;
    }
}

