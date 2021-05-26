using System;
using UnityEngine;
using Utils;

namespace Core
{
    public class Swipe
    {
        public float strength;
        public Vector3 directionNormalized;

        public Swipe()
        {
        }

        public Swipe(float strength, Vector3 directionNormalized)
        {
            this.strength = strength;
            this.directionNormalized = directionNormalized;
        }
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField] private float swipeStrength = 0.07f;

        public bool IsTouchInterface { get; set; }
        public bool DraggingLeft { get; private set; }
        public bool DraggingRight { get; private set; }

        public bool HasLeftClicked { get; private set; }
        public bool HasRightClicked { get; private set; }
        public Ray TouchRay { get; private set; }

        public Swipe CurrentSwipe { get; private set; }

        private Camera mainCamera;

        private Vector2 startTouch, swipeDelta;

        private Vector2 draggingStartTouch;

        private bool prevFrameRightClick;

        private void Awake()
        {
            mainCamera = Camera.main;
            IsTouchInterface = Util.IsTouchInterface;
        }

        private void Update()
        {
            ResetInput();

            if (IsTouchInterface)
                HandleMobileInput();
            else
                HandlePcInput();
        }

        private void HandleMobileInput()
        {
            if (Input.touchCount == 0) return;

            Touch firstTouch = Input.touches[0];
            TouchRay = mainCamera.ScreenPointToRay(firstTouch.position);

            // NOTE: Click
            HasLeftClicked = true;
            if (prevFrameRightClick)
                HasRightClicked = true;
            prevFrameRightClick = true;

            // NOTE: Camera dragging
            if (firstTouch.phase == TouchPhase.Began)
            {
                draggingStartTouch = firstTouch.position;
            }
            else if (firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary)
            {
                Vector2 draggingDelta = firstTouch.position - draggingStartTouch;
                if (draggingDelta.x > 0)
                {
                    DraggingLeft = true;
                }
                else if (draggingDelta.x < 0)
                {
                    DraggingRight = true;
                }
            }
            else if (firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled)
            {
                DraggingLeft = DraggingRight = false;
            }
            
            // NOTE: Swipes for box movement
            if (firstTouch.phase == TouchPhase.Began)
            {
                startTouch = firstTouch.position;
            }
            else if (firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled)
            {
                swipeDelta = firstTouch.position - startTouch;
                float strength = swipeDelta.sqrMagnitude;
                CurrentSwipe = new Swipe(strength * swipeStrength, swipeDelta.normalized);
            }
        }

        private void HandlePcInput()
        {
            // NOTE: Camera input using arrows
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                DraggingLeft = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                DraggingRight = true;
            }

            bool lClick = Input.GetMouseButtonDown(0);
            bool rClick = Input.GetMouseButtonDown(1);
            bool lUp = Input.GetMouseButtonUp(0);
            
            TouchRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (lClick)
            {
                HasLeftClicked = true;
                startTouch = Input.mousePosition;
            }
            else if (rClick)
            {
                HasRightClicked = true;
            }
            else if (lUp)
            {
                swipeDelta = Input.mousePosition - new Vector3(startTouch.x, startTouch.y);
                float strength = swipeDelta.sqrMagnitude;
                CurrentSwipe = new Swipe(strength * swipeStrength, swipeDelta.normalized);
            }
        }

        private void ResetInput()
        {
            DraggingLeft = DraggingRight = false;

            HasLeftClicked = false;
            HasRightClicked = false;

            CurrentSwipe = null;
        }
    }
}