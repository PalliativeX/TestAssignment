using System;
using Core.Sortables;
using UnityEngine;

namespace Core
{
    public class CharacterController : SortableController
    {
        protected override void Update()
        {
            base.Update();
            
            if (inputManager.HasRightClicked && currentlySelected && !selectedThisFrame)
            {
                TryMoveCharacter();
            }    
        }
        
        private void TryMoveCharacter()
        {
            if (Physics.Raycast(inputManager.TouchRay, out RaycastHit hit, MainPlaneMask))
            {
                currentlySelected.GetComponent<Character>().MoveToDest(hit.point);
            }
        }

        
    }
}