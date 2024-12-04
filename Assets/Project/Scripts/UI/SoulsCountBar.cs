using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PHH
{
    public class SoulsCountBar : MonoBehaviour
    {
        public TMP_Text soulsCountText;

        public void SetSoulsCountText(int soulsCount)
        {
            soulsCountText.text = soulsCount.ToString();
        }
    }
}
