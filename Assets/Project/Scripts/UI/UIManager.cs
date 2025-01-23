using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace PHH
{
    public class UIManager : MonoBehaviour
    {
        public PlayerManager playerManager;
        public EquipmentWindowUI equipmentWindowUI;
        public QuickSlotsUI quickSlotsUI;

        [Header("HUD")]
        public GameObject crossHair;
        public TMP_Text soulCount;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;
        public GameObject levelUpWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Weapons Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Awake()
        {
            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerManager.playerInventoryManager);
            quickSlotsUI.UpdateCurrentSpellIcon(playerManager.playerInventoryManager.currentSpell);
            quickSlotsUI.UpdateCurrentConsumableIcon(playerManager.playerInventoryManager.currentConsumable);
            soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerManager.playerInventoryManager.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerManager.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerManager.playerInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        public void ToggleSelectWindow(bool toggle)
        {
            selectWindow.SetActive(toggle);
        }

        public void CloseAllInventoryWindow()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }
        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }
}