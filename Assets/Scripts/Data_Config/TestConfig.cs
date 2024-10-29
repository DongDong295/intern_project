using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataConfig
{
    public class  TestConfigItem
    {
        public int id;
        public int test;
    }

    public class TestConfig : SerializedScriptableObject
    {
        public TestConfigItem[] items;
    }
}