using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Sortables
{
    public class CharacterAnimator : MonoBehaviour
    {
        public const string IDLE = "Idle";
        public const string WALKING = "Walking";

        public string CurrentAnimationName { get; set; }
        
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            CurrentAnimationName = IDLE;
            animator.Play(IDLE);
        }

        public void SwitchAnimation(string animationName)
        {
            animator.SetBool(CurrentAnimationName, false);
            animator.SetBool(animationName, true);
            CurrentAnimationName = animationName;
        }
    }
}