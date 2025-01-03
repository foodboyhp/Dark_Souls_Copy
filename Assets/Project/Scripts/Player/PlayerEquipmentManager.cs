using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{

    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;

        [Header("Equipment Model Changers")]
        //Head
        HelmetModelChanger helmetModelChanger;
        //Torso
        TorsoModelChanger torsoModelChanger;
        UpperLeftArmModelChanger upperLeftArmModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;
        //Leg
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;

        //Hand
        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;
        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorso;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedHipModel;
        public string nakedLeftLeg;
        public string nakedRightLeg;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public string nakedLeftHand;
        public string nakedRightHand;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
            upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
            lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModelOnStart();
        }

        private void EquipAllEquipmentModelOnStart()
        {
            //Head
            helmetModelChanger.UnequipAllHelmetModels();
            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                playerStatsManager.physicalDamageAbsorbtionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                nakedHeadModel.SetActive(true);
                playerStatsManager.physicalDamageAbsorbtionHead = 0;
            }
            //Torso
            torsoModelChanger.UnequipAllTorsoModels();
            upperLeftArmModelChanger.UnequipAllUpperArmModels();
            upperRightArmModelChanger.UnequipAllUpperArmModels();
            if (playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
                upperLeftArmModelChanger.EquipUpperArmModelByName(playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquipUpperArmModelByName(playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
                playerStatsManager.physicalDamageAbsorbtionTorso = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorso);
                upperLeftArmModelChanger.EquipUpperArmModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquipUpperArmModelByName(nakedUpperRightArm);
                playerStatsManager.physicalDamageAbsorbtionTorso = 0;
            }
            //Leg
            hipModelChanger.UnequipAllHipModels();
            leftLegModelChanger.UnequipAllLegModels();
            rightLegModelChanger.UnequipAllLegModels();
            if (playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.rightLegName);
                playerStatsManager.physicalDamageAbsorbtionLeg = playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                playerStatsManager.physicalDamageAbsorbtionLeg = 0;
            }
            //Hand
            lowerLeftArmModelChanger.UnequipAllModels();
            lowerRightArmModelChanger.UnequipAllModels();
            leftHandModelChanger.UnequipAllModels();
            rightHandModelChanger.UnequipAllModels();
            if (playerInventoryManager.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
                playerStatsManager.physicalDamageAbsorbtionHand = playerInventoryManager.currentHandEquipment.physicalDefense;
            }
            else
            {
                lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipModelByName(nakedLeftHand);
                rightHandModelChanger.EquipModelByName(nakedRightHand);
                playerStatsManager.physicalDamageAbsorbtionHand = 0;
            }
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventoryManager.leftWeapon);

            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
