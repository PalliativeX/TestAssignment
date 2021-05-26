using System;
using UnityEngine;
using System.Collections.Generic;
using Utils;
using Random = UnityEngine.Random;

namespace Audio
{
    public class SoundLibrary : Singleton<SoundLibrary>
    {
        [SerializeField] private SoundGroup[] soundGroups;

        private Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

        protected override void Awake()
        {
            foreach (SoundGroup soundGroup in soundGroups)
            {
                groupDictionary.Add(soundGroup.groupID, soundGroup.group);
            }
        }

        public AudioClip GetClipFromName(string clipName)
        {
            if (groupDictionary.ContainsKey(clipName))
            {
                AudioClip[] sounds = groupDictionary[clipName];
                return sounds[Random.Range(0, sounds.Length)];
            }

            return null;
        }

        [Serializable]
        public class SoundGroup
        {
            public string groupID;
            public AudioClip[] group;
        }
    }
}