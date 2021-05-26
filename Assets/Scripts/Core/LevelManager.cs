using System;
using System.Collections.Generic;
using System.Linq;
using Core.Sortables;
using UnityEngine;

namespace Core
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Zone> zones;

        public int TotalSortableCount { get; set; }

        public event Action OnLevelPassed;
        public event Action<float> OnLevelProgressChanged;

        public int SortedCount
        {
            get => sortedCount;
            set
            {
                sortedCount = value;
                OnLevelProgressChanged?.Invoke(LevelProgress);
                CheckLevelCompletion();
            }
        }

        public float LevelProgress => (float) SortedCount / TotalSortableCount;

        public List<Sortable> Sortables { get; set; }

        private int sortedCount;

        private SortableGenerator sortableGenerator;

        private void Start()
        {
            sortableGenerator = FindObjectOfType<SortableGenerator>();

            Zone.OnCorrectSortableEntered += IncreaseSortedCount;
            Zone.OnCorrectSortableExited += DecreaseSortedCount;
            
            Sortables = sortableGenerator.Generate();

            TotalSortableCount = Sortables.Count;
            
            OnLevelPassed += OnLevelCompletion;
        }

        public Zone GetZoneByColor(SortableColor sortableColor) =>
            zones.FirstOrDefault(z => z.Color == sortableColor);

        private void CheckLevelCompletion()
        {
            if (sortedCount >= TotalSortableCount)
            {
                OnLevelPassed?.Invoke();
            }
        }

        private void OnLevelCompletion()
        {
            Invoke(nameof(Next), 3f);
        }
        
        // NOTE: For easier testing
        public void Next()
        {
            GameManager.Instance.NextLevel();
        }

        private void IncreaseSortedCount() => SortedCount++;
        private void DecreaseSortedCount() => SortedCount--;

        private void OnDestroy()
        {
            Zone.OnCorrectSortableEntered -= IncreaseSortedCount;
            Zone.OnCorrectSortableExited -= DecreaseSortedCount;
            OnLevelPassed -= OnLevelCompletion;

        }
    }
}