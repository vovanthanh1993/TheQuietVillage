using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("SFX Clips")]
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip noAmmo;
    [SerializeField] private AudioClip healthPickup;
    [SerializeField] private AudioClip flashLightOn;
    [SerializeField] private AudioClip flashLightOff;
    [SerializeField] private AudioClip shootPistol;
    [SerializeField] private AudioClip openNote;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip jumpScareSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Footstep & Hurt Sounds")]
    [SerializeField] private AudioClip[] hurtSound;
    [SerializeField] private AudioClip[] footSteps;

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gamePlayLv1Music;
    [SerializeField] private AudioClip gamePlayLv2Music;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundfxSource;

    private Dictionary<string, AudioClip> sceneMusicMap;
    private int footStepIndex = 0;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Khởi tạo map scene → nhạc
            sceneMusicMap = new Dictionary<string, AudioClip>
            {
                { "MainMenu", menuMusic },
                { "GamePlayLv1", gamePlayLv1Music },
                { "GamePlayLv2", gamePlayLv2Music },
                // thêm tên scene và nhạc tương ứng ở đây
            };

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneMusicMap.TryGetValue(scene.name, out AudioClip musicClip))
        {
            PlayBackGroundMusic(musicClip);
        }
        else
        {
            musicSource.Stop();
        }
       
    }

    public void PlayFootSteep()
    {
        soundfxSource.clip = footSteps[footStepIndex];
        soundfxSource.Play();
        footStepIndex = 1 - footStepIndex;
    }

    public void Reset()
    {
        musicSource.Stop();
        soundfxSource.Stop();
    }

    public void Pause()
    {
        musicSource.Pause();
        soundfxSource.Pause();
    }

    public void Resume()
    {
        musicSource.UnPause();
        soundfxSource.UnPause();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSoundFxVolume(float volume)
    {
        soundfxSource.volume = volume;
    }

    public void PlayBackGroundMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.Stop();
            musicSource.clip = clip;
        }

        musicSource.loop = true;
        musicSource.Play();
    }

    public void PickUpSound() => soundfxSource.PlayOneShot(pickup);
    public void ClickSound() => soundfxSource.PlayOneShot(clickSound);
    public void ReloadSound() => soundfxSource.PlayOneShot(reload);
    public void NoAmmoSound() => soundfxSource.PlayOneShot(noAmmo);
    public void HealthPickupSound() => soundfxSource.PlayOneShot(healthPickup);
    public void FlashLightOnSound() => soundfxSource.PlayOneShot(flashLightOn);
    public void FlashLightOffSound() => soundfxSource.PlayOneShot(flashLightOff);

    public void ShootPistolSound(Vector3 position)
    {
        soundfxSource.PlayOneShot(shootPistol);
        MakeNoise(position);
    }

    public void OpenNote() => soundfxSource.PlayOneShot(openNote);

    public void StepSound()
    {
        soundfxSource.clip = stepSound;
        soundfxSource.Play();
    }

    public void HurtSound()
    {
        soundfxSource.clip = hurtSound[Random.Range(0, hurtSound.Length)];
        soundfxSource.Play();
    }

    public void JumpScareSound()
    {
        soundfxSource.clip = jumpScareSound;
        soundfxSource.Play();
    }

    public void PlayEffect(AudioClip clip)
    {
        soundfxSource.PlayOneShot(clip);
    }

    public void PlayEffect(AudioClip clip, Transform transform)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, soundfxSource.volume);
    }

    private void MakeNoise(Vector3 soundPosition)
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.HearSound(soundPosition);
        }
    }


}
