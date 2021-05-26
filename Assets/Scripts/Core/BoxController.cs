using Core.Sortables;
using UnityEngine;

namespace Core
{
    public class BoxController : SortableController
    {
        protected override void Update()
        {
            base.Update();

            if (inputManager.CurrentSwipe != null && currentlySelected)
            {
                Box box = currentlySelected.GetComponent<Box>();
                Swipe current = inputManager.CurrentSwipe;
                Vector3 dir = new Vector3(current.directionNormalized.x, 0f, current.directionNormalized.y);
                box.Move(dir, inputManager.CurrentSwipe.strength);
            }
        }
    }
}