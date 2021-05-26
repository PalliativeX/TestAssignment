using System;
using Core.Sortables;
using UnityEngine;

namespace Core
{
    public abstract class SortableController : MonoBehaviour
    {
        [SerializeField] private LayerMask selectableMask;
        [SerializeField] private LayerMask mainPlaneMask;

        public LayerMask SelectableMask => selectableMask;
        public LayerMask MainPlaneMask => mainPlaneMask;

        public bool HasSelectedSortable => currentlySelected != null;

        protected InputManager inputManager; 
        
        protected Sortable currentlySelected;

        protected bool selectedThisFrame;
        
        protected virtual void Awake()
        {
            currentlySelected = null;
            inputManager = FindObjectOfType<InputManager>();
        }

        protected virtual void Update()
        {
            if (inputManager.HasLeftClicked)
            {
                TrySelectSortable();
            }
        }

        private void LateUpdate()
        {
            selectedThisFrame = false;
        }

        private void TrySelectSortable()
        {
            if (Physics.Raycast(inputManager.TouchRay, out RaycastHit hit, selectableMask))
            {
                if (hit.collider.TryGetComponent(out Sortable sortable))
                {
                    CancelSelection();

                    if (!sortable.Selectable) return;

                    currentlySelected = sortable;
                    currentlySelected.GetSelected(true);
                    selectedThisFrame = true;
                }
            }
            else
            {
                CancelSelection();
            }
        }

        private void CancelSelection()
        {
            if (currentlySelected)
            {
                currentlySelected.GetSelected(false);
                currentlySelected = null;
            }
        }
    }
}