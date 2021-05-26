using System;
using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform centerT;
        [SerializeField] private float rotationSpeed;

        private SortableController sortableController;
        private InputManager inputManager;

        private void Awake()
        {
            sortableController = FindObjectOfType<SortableController>();
            inputManager = FindObjectOfType<InputManager>();
        }

        private void Update()
        {
            if (sortableController.HasSelectedSortable)
                return;

            if (inputManager.DraggingLeft)
            {
                RotateAroundCenter(RotationDirection.Left);
            }
            else if (inputManager.DraggingRight)
            {
                RotateAroundCenter(RotationDirection.Right);
            }
        }

        private void RotateAroundCenter(RotationDirection dir, float strength = 1f)
        {
            float yValue = dir == RotationDirection.Left ? 1f : -1f;
            transform.RotateAround(centerT.position, new Vector3(0f, yValue, 0f),
                rotationSpeed * strength * Time.deltaTime);
        }
    }

    public enum RotationDirection
    {
        Left,
        Right
    }
}