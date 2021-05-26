using System;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Sortables
{
    public class Character : Sortable
    {
        private CharacterAnimator characterAnimator;
        private NavMeshAgent agent;

        protected override void Awake()
        {
            base.Awake();
            
            characterAnimator = GetComponent<CharacterAnimator>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (characterAnimator.CurrentAnimationName == CharacterAnimator.WALKING && IsNotMoving())
            {
                characterAnimator.SwitchAnimation(CharacterAnimator.IDLE);
            }
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
                r.material = ps.CharacterMatByColor(Color);
            }

            mainColor = MainRenderers[0].material.GetColor(PulsationColor);
        }

        public void MoveToDest(Vector3 dest)
        {
            agent.SetDestination(dest);
            characterAnimator.SwitchAnimation(CharacterAnimator.WALKING);
        }
        
        private bool IsNotMoving()
        {
            Vector3 agentPos = agent.transform.position;
            agentPos.y = agent.destination.y;

            return !agent.pathPending &&
                   Vector3.Distance(agent.destination, agentPos) <= agent.stoppingDistance &&
                   !agent.hasPath && agent.velocity.sqrMagnitude == 0f;
        }
    }
}