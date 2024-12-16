using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{

    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;

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
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
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
            if (playerInventory.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            }
            else
            {
                nakedHeadModel.SetActive(true);
            }
            //Torso
            torsoModelChanger.UnequipAllTorsoModels();
            upperLeftArmModelChanger.UnequipAllUpperArmModels();
            upperRightArmModelChanger.UnequipAllUpperArmModels();
            if (playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                upperLeftArmModelChanger.EquipUpperArmModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquipUpperArmModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorso);
                upperLeftArmModelChanger.EquipUpperArmModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquipUpperArmModelByName(nakedUpperRightArm);
            }
            //Leg
            hipModelChanger.UnequipAllHipModels();
            leftLegModelChanger.UnequipAllLegModels();
            rightLegModelChanger.UnequipAllLegModels();
            if (playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
            }
            //Hand
            lowerLeftArmModelChanger.UnequipAllModels();
            lowerRightArmModelChanger.UnequipAllModels();
            leftHandModelChanger.UnequipAllModels();
            rightHandModelChanger.UnequipAllModels();
            if (playerInventory.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.rightHandModelName);
            }
            else
            {
                lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipModelByName(nakedLeftHand);
                rightHandModelChanger.EquipModelByName(nakedRightHand);
            }
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventory.leftWeapon);

            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
