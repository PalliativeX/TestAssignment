using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class Util
    {
        public static string SceneNameFromIndex(int buildIndex)
        {
            if (buildIndex > SceneManager.sceneCountInBuildSettings - 1)
            {
                Debug.LogErrorFormat("Incorrect buildIndex {0}!", buildIndex);
                return null;
            }

            return Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(buildIndex));
        }
        
        public static bool RandomPointOnNavmesh(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }

            result = Vector3.zero;
            return false;
        }
        
        public static void Invoke(this MonoBehaviour mb, Action f, float delay)
        {
            mb.StartCoroutine(InvokeRoutine(f, delay));
        }

        private static IEnumerator InvokeRoutine(Action f, float delay)
        {
            yield return new WaitForSeconds(delay);
            f();
        }

        public static bool IsTouchInterface
        {
            get
            {
                #if UNITY_EDITOR
                    return false;
                #endif

                return Platform == RuntimePlatform.Android || Platform == RuntimePlatform.IPhonePlayer;
            }
        }

        public static RuntimePlatform Platform
        {
            get
            {
                #if UNITY_ANDROID
                    return RuntimePlatform.Android;
                #elif UNITY_IOS
                    return RuntimePlatform.IPhonePlayer;
                #elif UNITY_STANDALONE_OSX
                    return RuntimePlatform.OSXPlayer;
                #elif UNITY_STANDALONE_WIN
                    return RuntimePlatform.WindowsPlayer;
                #endif
            }
        }
        
    }
}