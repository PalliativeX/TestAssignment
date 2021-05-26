using System;
using System.Collections;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentLevelIndexText;
        [SerializeField] private TMP_Text nextLevelIndexText;
        [SerializeField] private Image currentProgressFill;
        [SerializeField] private float updateProgressFillTime = 0.3f;

        private LevelManager levelManager;

        private void Awake()
        {
            levelManager = FindObjectOfType<LevelManager>();
            levelManager.OnLevelProgressChanged += UpdateProgressFill;
            currentProgressFill.fillAmount = 0f;
        }

        private void Start()
        {
            UpdateLevelIndicesText();
        }

        private void UpdateProgressFill(float newProgress)
        {
            if (Math.Abs(newProgress - 1f) < 0.01f)
                currentProgressFill.color = Color.green;
            StartCoroutine(LerpProgressFill(newProgress));
        }

        private IEnumerator LerpProgressFill(float newProgress)
        {
            float initialFill = currentProgressFill.fillAmount;

            float elapsed = 0f;

            while (elapsed <= updateProgressFillTime)
            {
                elapsed += Time.deltaTime;
                currentProgressFill.fillAmount = Mathf.Lerp(initialFill, newProgress, elapsed / updateProgressFillTime);
                yield return null;
            }

            currentProgressFill.fillAmount = newProgress;
        }

        private void UpdateLevelIndicesText()
        {
            int currentLevel = GameManager.Instance.CurrentLevel;
            currentLevelIndexText.text = currentLevel.ToString();
            nextLevelIndexText.text = (currentLevel + 1).ToString();
        }

        private void OnDestroy()
        {
            levelManager.OnLevelProgressChanged -= UpdateProgressFill;
        }
    }
}