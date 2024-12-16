using System.Collections.Generic;
using UnityEngine;

namespace PHH
{

    public class TorsoModelChanger : MonoBehaviour
    {
        public List<GameObject> torsoModels;

        private void Awake()
        {
            GetAllTorsoModels();
        }

        private void GetAllTorsoModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                torsoModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllTorsoModels()
        {
            foreach (GameObject torsoModel in torsoModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipTorsoModelByName(string torsoName)
        {
            for (int i = 0; i < torsoModels.Count; i++)
            {
                if (torsoModels[i].name == torsoName)
                {
                    torsoModels[i].SetActive(true);
                }
            }
        }
    }
}
