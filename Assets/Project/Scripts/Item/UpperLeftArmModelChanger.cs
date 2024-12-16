using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class UpperLeftArmModelChanger : MonoBehaviour
    {
        public List<GameObject> upperArmModels;

        private void Awake()
        {
            GetAllUpperArmModels();
        }

        private void GetAllUpperArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                upperArmModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllUpperArmModels()
        {
            foreach (GameObject upperArmModel in upperArmModels)
            {
                upperArmModel.SetActive(false);
            }
        }

        public void EquipUpperArmModelByName(string upperArmName)
        {
            for (int i = 0; i < upperArmModels.Count; i++)
            {
                if (upperArmModels[i].name == upperArmName)
                {
                    upperArmModels[i].SetActive(true);
                }
            }
        }
    }
}
