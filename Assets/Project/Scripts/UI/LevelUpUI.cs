using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PHH
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Button confirmLevelUpButton;
        [Header("Player Level")]
        public int currentPlayerLevel;
        public int projectedPlayerLevel;
        public TMP_Text currentPlayerLevelText;
        public TMP_Text projectedPlayerLevelText;

        [Header("Souls")]
        public TMP_Text currentSoulsText;
        public TMP_Text soulRequiredToLevelUpText;
        public int soulsRequiredToLevelUp;
        public int baseLevelUpCost = 5;
        [Header("Health")]
        public Slider healthSlider;
        public TMP_Text currentHealthLevelText;
        public TMP_Text projectedHealthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public TMP_Text currentStaminaLevelText;
        public TMP_Text projectedStaminaLevelText;

        [Header("Focus")]
        public Slider focusSlider;
        public TMP_Text currentFocusLevelText;
        public TMP_Text projectedFocusLevelText;

        [Header("Poise")]
        public Slider poiseSlider;
        public TMP_Text currentPoiseLevelText;
        public TMP_Text projectedPoiseLevelText;

        [Header("Strength")]
        public Slider strengthSlider;
        public TMP_Text currentStrengthLevelText;
        public TMP_Text projectedStrengthLevelText;

        [Header("Dexterity")]
        public Slider dexteritySlider;
        public TMP_Text currentDexterityLevelText;
        public TMP_Text projectedDexterityLevelText;

        [Header("Faith")]
        public Slider faithSlider;
        public TMP_Text currentFaithLevelText;
        public TMP_Text projectedFaithLevelText;

        [Header("Intelligence")]
        public Slider intelligenceSlider;
        public TMP_Text currentIntelligenceLevelText;
        public TMP_Text projectedIntelligenceLevelText;

        private void OnEnable()
        {
            currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStatsManager.healthLevel;
            healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();

            staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();

            focusSlider.value = playerManager.playerStatsManager.focusLevel;
            focusSlider.minValue = playerManager.playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;
            currentFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();

            poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;
            currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();

            strengthSlider.value = playerManager.playerStatsManager.strengthLevel;
            strengthSlider.minValue = playerManager.playerStatsManager.strengthLevel;
            strengthSlider.maxValue = 99;
            currentStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();
            projectedStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();

            dexteritySlider.value = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.minValue = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;
            currentDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();

            intelligenceSlider.value = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.minValue = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;
            currentIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();

            currentSoulsText.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            UpdateProjectedPlayerLevel();
        }

        private void UpdateProjectedPlayerLevel()
        {
            soulsRequiredToLevelUp = 0;
            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerManager.playerStatsManager.focusLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStatsManager.strengthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStatsManager.dexterityLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStatsManager.intelligenceLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            soulRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

            if (playerManager.playerStatsManager.currentSoulCount < soulsRequiredToLevelUp)
            {
                confirmLevelUpButton.interactable = false;
            }
            else
            {
                confirmLevelUpButton.interactable = true;

            }
        }

        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
            playerManager.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
            playerManager.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
            playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
            playerManager.playerStatsManager.strengthLevel = Mathf.RoundToInt(strengthSlider.value);
            playerManager.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            playerManager.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
            playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);

            playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
            playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStatsManager.maxFocusPoint = playerManager.playerStatsManager.SetMaxFocusPointFromFocusPointLevel();

            playerManager.playerStatsManager.currentSoulCount = playerManager.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
            playerManager.uiManager.soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for (int i = 0; i < projectedPlayerLevel; i++)
            {
                soulsRequiredToLevelUp = soulsRequiredToLevelUp + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = focusSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = strengthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = dexteritySlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }
}
