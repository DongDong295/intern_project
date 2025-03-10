using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class NewTestScript : MonoBehaviour {

        public TextMeshProUGUI text;
        public GameObject popup;
        private int index;
        
        public void SetText(int newIndex) {
            Debug.Log(newIndex + "DSDS");
            index = newIndex;
            text.text = $"{index}";
        }
    }

