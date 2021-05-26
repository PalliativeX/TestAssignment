using System;
using UnityEngine;
using Utils;

namespace Core.Sortables
{
    public abstract class Sortable : MonoBehaviour
    {
        [SerializeField] private new Collider collider;

        [SerializeField] private Renderer[] mainRenderers;

        public Collider Collider => collider;

        public bool IsInsideCorrectZone
        {
            get => isInsideCorrectZone;
            set
            {
                if (isInsideCorrectZone == value) 
                    return;
                
                isInsideCorrectZone = value;
                if (isInsideCorrectZone) 
                    OnEnteredCorrectZone();
                else
                    OnLeftCorrectZone();
            }
        }

        public SortableColor Color { get; set; }
        public bool Selected { get; set; }

        // NOTE: If an object is in the correct zone, we just restrict selection
        public bool Selectable { get; set; }

        protected Outline Outline { get; private set; }
        protected Renderer[] MainRenderers => mainRenderers;
        protected Color mainColor;

        private bool isInsideCorrectZone;
        
        protected static readonly int EnablePulsation = Shader.PropertyToID("EnablePulsation");
        protected static readonly int PulsationColor = Shader.PropertyToID("PulsationColor");


        protected virtual void Awake()
        {
            Selectable = true;
            Outline = GetComponent<Outline>();
        }

        private void Start()
        {
            SetCorrectMaterial();
        }

        protected abstract void SetCorrectMaterial();

        public virtual void GetSelected(bool selected)
        {
            Selected = selected;
            
            Outline.enabled = selected;
        }

        private void OnEnteredCorrectZone()
        {
            foreach (Renderer r in MainRenderers)
            {
                r.material.SetColor(PulsationColor, UnityEngine.Color.white);
                this.Invoke(() => { r.material.SetInt(EnablePulsation, 0);}, 1f);
            }
        }

        private void OnLeftCorrectZone()
        {
            foreach (Renderer r in MainRenderers)
            {
                r.material.SetInt(EnablePulsation, 1);
                r.material.SetColor(PulsationColor, mainColor);
            }
        }

    }
    
}