using System;
using System.Collections.Generic;
using System.Linq;
using Core.Sortables;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace Core
{
    public class SortableGenerator : MonoBehaviour
    {
        [SerializeField] private int minCount, maxCount;
        [SerializeField] private Sortable sortablePrefab;

        [SerializeField] private Collider mainZoneCollider;

        [Header("Different Params")]
        [SerializeField] private float ySpawnOffset;
        [SerializeField] private bool randomRotation;
        [SerializeField] private float offsetFromEdge;
        [SerializeField] private bool hasNavmesh;

        private static Collider[] results;
        
        private void Awake()
        {
            results = new Collider[maxCount];
        }

        public List<Sortable> Generate()
        {
            int sortablesToGenerate = Random.Range(minCount, maxCount);

            List<Sortable> sortables = new List<Sortable>(sortablesToGenerate);
            
            for (int i = 0; i < sortablesToGenerate; i++)
            {
                Sortable sortable = Instantiate(sortablePrefab, GetRandomSpawnPosition(), randomRotation ? GetRandomSpawnRotation() : Quaternion.identity);
                while (CheckBounds(sortable.transform.position, sortable.Collider.bounds.size,
                    LayerMask.NameToLayer("Selectable")))
                {
                    sortable.transform.position = GetRandomSpawnPosition();
                }

                sortable.Color = (SortableColor)Random.Range(0, 3);
                
                sortables.Add(sortable);
            }

            return sortables;
        }

        private Vector3 GetRandomSpawnPosition()
        {
            var bounds = mainZoneCollider.bounds;

            float yPosition = 0f + ySpawnOffset;

            float x = Mathf.Lerp(bounds.min.x + offsetFromEdge/2f, bounds.max.x - offsetFromEdge/2f, Random.value);
            float z = Mathf.Lerp(bounds.min.z + offsetFromEdge/2f, bounds.max.z - offsetFromEdge/2f, Random.value);
            
            Vector3 sourcePos = new Vector3(x, yPosition, z);

            if (hasNavmesh)
            {
                Util.RandomPointOnNavmesh(sourcePos, 6f, out Vector3 randomSpawnPos);
                return randomSpawnPos;
            }

            return sourcePos;
        }

        private Quaternion GetRandomSpawnRotation() => 
            Quaternion.Euler(Random.Range(0f, 180f), 0f, Random.Range(0f, 180f));
        
        public static bool CheckBounds(Vector3 position, Vector3 boundsSize, int layerMask)
        {
            Array.Clear(results, 0, results.Length);
            
            Bounds boxBounds = new Bounds(position, boundsSize);
 
            float sqrHalfBoxSize = boxBounds.extents.sqrMagnitude;
            float overlappingSphereRadius = Mathf.Sqrt(sqrHalfBoxSize + sqrHalfBoxSize);
 
            Physics.OverlapSphereNonAlloc(position, overlappingSphereRadius, results, layerMask);
            return results.Any(otherCollider => otherCollider && otherCollider.bounds.Intersects(boxBounds));
        }

    }
}