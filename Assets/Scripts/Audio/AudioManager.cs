using System;
using UnityEngine;
using System.Collections;
using Utils;

namespace Audio
{
   public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] [Range(0f, 1f)] private float volumePercent;

        public static AudioManager Instance { get; private set; }
        
        public bool MusicActive { get; set; }
        public bool SfxActive { get; set; }
        
        private AudioSource sfx2DSource;
        private AudioSource[] musicSources;
        private int activeMusicSourceIndex;
        
        private SoundLibrary library;

        protected void Awake()
        {
            if (Instance) Destroy(Instance.gameObject);
            Instance = this;
            
            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                musicSources[i].volume = 0.35f;
                newMusicSource.transform.parent = transform;
            }

            GameObject newSfx2Dsource = new GameObject("2D Sfx Source");
            sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
            newSfx2Dsource.transform.parent = transform;
            
            MusicActive = true;
            SfxActive = true;

            if (PlayerPrefs.HasKey("MusicOn"))
            {
                MusicActive = PlayerPrefs.GetInt("MusicOn") == 1;
            }
            if (PlayerPrefs.HasKey("SfxOn"))
            {
                SfxActive = PlayerPrefs.GetInt("SfxOn") == 1;
            }

            TurnMusicActive(MusicActive);
            TurnMusicActive(SfxActive);
        }

        public void PlayMusic(AudioClip clip, float fadeDuration = 1)
        {
            activeMusicSourceIndex = 1 - activeMusicSourceIndex;
            musicSources[activeMusicSourceIndex].clip = clip;
            musicSources[activeMusicSourceIndex].Play();

            StartCoroutine(AnimateMusicCrossfade(fadeDuration));
        }

        public void PlaySound(string soundName, Vector3 pos)
        {
            if (SfxActive)
                PlaySound(library.GetClipFromName(soundName), pos);
        }
        
        // NOTE(vladimir): Volume is a multiplier
        public void PlaySound(string soundName, Vector3 pos, float volume)
        {
            if (SfxActive)
                PlaySound(library.GetClipFromName(soundName), pos, volume);
        }
        
        private void PlaySound(AudioClip clip, Vector3 pos)
        {
            if (clip != null && SfxActive)
            {
                AudioSource.PlayClipAtPoint(clip, pos, volumePercent * 5);
            }
        }
        
        private void PlaySound(AudioClip clip, Vector3 pos, float volume)
        {
            if (clip != null && SfxActive)
            {
                AudioSource.PlayClipAtPoint(clip, pos, volumePercent * 5 * volume);
            }
        }

        public void PlaySound2D(string soundName)
        {
            if (SfxActive)
                sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), volumePercent);
        }
        
        public void PlaySound2D(string soundName, float volume)
        {
            if (SfxActive)
                sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), volumePercent * volume);
        }
        
        private IEnumerator AnimateMusicCrossfade(float duration)
        {
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * 1 / duration;
                musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, 0.35f, percent);
                musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(0.35f, 0, percent);
                yield return null;
            }
        }

        public void ChangeVolumePercent(float newVolume)
        {
            volumePercent = newVolume;
        }
        
        public void TurnActive(bool active)
        {
            sfx2DSource.mute = !active;
            musicSources[0].mute = !active;
            musicSources[1].mute = !active;
            volumePercent = active ? 1f: 0f;
        }
        
        public void TurnMusicActive(bool active)
        {
            foreach (AudioSource source in musicSources)
            {
                source.mute = !active;
            }
            MusicActive = active;
            PlayerPrefs.SetInt("MusicOn", MusicActive ? 1 : 0);
        }
        
        public void TurnSfxActive(bool active)
        {
            SfxActive = active;
            PlayerPrefs.SetInt("SfxOn", SfxActive ? 1 : 0);
        }
    }
}