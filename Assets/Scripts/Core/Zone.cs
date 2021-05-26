using System;
using Core.Sortables;
using UnityEngine;

namespace Core
{
    public class Zone : MonoBehaviour
    {
        [SerializeField] private SortableColor color;
        [SerializeField] private new Collider collider;

        public SortableColor Color => color;

        public static event Action OnCorrectSortableEntered, OnCorrectSortableExited;

        public bool CheckInside(Collider sortableCollider) =>
            collider.bounds.Intersects(sortableCollider.bounds);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Sortable sortable) && 
                sortable.Color == Color)
            {
                sortable.IsInsideCorrectZone = true;
                OnCorrectSortableEntered?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Sortable sortable) && 
                sortable.Color == Color)
            {
                sortable.IsInsideCorrectZone = false;
                OnCorrectSortableExited?.Invoke();
            }
        }
    }
}