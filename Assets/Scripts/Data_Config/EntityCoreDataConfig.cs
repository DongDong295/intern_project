using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class EntityCoreDataConfigItem
    {
        public int id;
        public string name;
        public EntityType type;
    }

    public class EntityCoreDataConfig : SerializedScriptableObject
    {
        public EntityCoreDataConfigItem[] items;
    }
}


