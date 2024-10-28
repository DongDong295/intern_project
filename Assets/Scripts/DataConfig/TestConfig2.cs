using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig{
    public class TestConfig2Item
    {
        public int id;
        public string name;
        public string codeName;
    }
    public class TestConfig2 : SerializedScriptableObject
    {
        public TestConfig2Item[] item;
    }
}
