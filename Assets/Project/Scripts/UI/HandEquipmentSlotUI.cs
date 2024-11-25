using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        WeaponItem item;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool leftHandSlot01;
        public bool leftHandSlot02;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();  
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            if (item.itemIcon != null)
            {
                icon.sprite = item.itemIcon;
                icon.enabled = true;
            }
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if(rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected=true;
            }
            else if(leftHandSlot02)
            {
                uiManager.leftHandSlot02Selected=true;
            }
        }
    }
}
