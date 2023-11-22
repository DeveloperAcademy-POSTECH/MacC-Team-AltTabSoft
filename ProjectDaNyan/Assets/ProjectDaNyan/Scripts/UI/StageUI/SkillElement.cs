using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class SkillElement : MonoBehaviour
    {
        [SerializeField] private List<Image> _levels;
        // [SerializeField] private TextMeshProUGUI _skillElementText;
        
        public void SetLevel(int level)
        {
            int setLevel = level;
            if (level >= 4)
            {
                setLevel = 4;
            }
            for (int i = 0; i < setLevel; i++)
            {
                _levels[i].color = Color.red;
            }
        }

        // public void SetImage(String skillName)
        // {
        //     _skillElementText.text = skillName;
        // }
    }
}