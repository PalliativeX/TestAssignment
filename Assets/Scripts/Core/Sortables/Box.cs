using System;
using UnityEngine;

namespace Core.Sortables
{
    public class Box : Sortable
    {
        private Rigidbody rb;
        
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }

        public override void GetSelected(bool selected)
        {
            if (!Selectable) return;
            
            base.GetSelected(selected);
        }
        
        protected override void SetCorrectMaterial()
        {
            PrefabStorage ps = PrefabStorage.Instance;

            foreach (Renderer r in MainRenderers)
            {
                r.material = ps.BoxMatByColor(Color);
            }
            
            mainColor = MainRenderers[0].material.GetColor(PulsationColor);
        }

        public void Move(Vector3 forceDirection, float strength)
        {
            rb.AddForce(forceDirection * strength);
        }
        
    }
}