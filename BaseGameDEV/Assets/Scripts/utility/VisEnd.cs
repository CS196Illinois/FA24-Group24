using UnityEngine;
using PlayerEnd;
using TMPro;
using LogicEnd;
using UnityEditor;
using System.Data;
using Unity.VisualScripting;

namespace VisEnd {
    public class PlayerVis : MonoBehaviour
    {
        public TextMeshProUGUI playerstatus;
        public TextMeshProUGUI inventory;
        public TextMeshProUGUI updatetext;
        
        void OnEnable() 
        {
            //Subscribe to the Text update event;
            EventTrigger.OnUpdateText += UpdateText;
            EventTrigger.OnUpdateInv += UpdateInv;
            EventTrigger.OnUpdateupdate += Updateupdate;
        }
        void OnDisable() {
            EventTrigger.OnUpdateText -= UpdateText;
            EventTrigger.OnUpdateInv -= UpdateInv;
            EventTrigger.OnUpdateupdate -= Updateupdate;
        }

        //Method to update the text when the event is fired
        private void UpdateText(string newText) 
        {
            if (playerstatus != null) {
                playerstatus.text = newText;
            }
        }

        private void UpdateInv(string newText) {
            if (inventory != null) {
                inventory.text = newText;
            }
        }

        private void Updateupdate(string newText) {
            if (newText != null) {
                updatetext.text = newText;
            }
        }



    }

    public class MapVis : MonoBehaviour 
    {

    }
}