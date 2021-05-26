using System.Collections;
using UnityEngine;
using Utils;

namespace Audio
{
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private bool musicOn;
        [SerializeField] private AudioClip soundtrack;
        
        private void Start()
        {
            AudioManager.Instance.PlayMusic(soundtrack);
            
            if (musicOn)
            {
                StartCoroutine(PlayMainSoundtrack(soundtrack));
            }
        }

        private IEnumerator PlayMainSoundtrack(AudioClip clip)
        {
            while (true)
            {
                AudioManager.Instance.PlayMusic(clip);
                yield return new WaitForSeconds(clip.length - 4f);
            }
        }

    }
}
