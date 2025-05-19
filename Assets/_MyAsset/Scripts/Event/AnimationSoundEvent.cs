using System;
using UnityEngine;
public class AnimationSoundEvent : MonoBehaviour
{
    [Serializable]
    public struct SoundEvent
    {
        public string Name;
        public AudioClip sound;
    }

    public SoundEvent[] SoundEvents;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        foreach (var sound in SoundEvents)
        {
            if (sound.Name == name)
            {
                AudioManager.Instance.PlayEffect(sound.sound);
                break;
            }
        }
    }
}
    