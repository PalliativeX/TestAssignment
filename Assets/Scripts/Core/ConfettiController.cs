using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ConfettiController : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> confettis;

        private LevelManager levelManager;

        private void Awake()
        {
            confettis.ForEach(cf => cf.gameObject.SetActive(false));
        }

        private void Start()
        {
            levelManager = FindObjectOfType<LevelManager>();
            levelManager.OnLevelPassed += TriggerConfetti;
        }

        private void TriggerConfetti()
        {
            foreach (ParticleSystem cf in confettis)
            {
                cf.gameObject.SetActive(true);
                cf.Play();
            }
        }
    }
}
