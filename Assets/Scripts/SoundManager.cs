using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }
    public enum Sound
    {
        BuildingPlaced,
        BuildingDestroyed,
        BuildingDamaged,
        EnemyDie,
        EnemyHit,
        GameOver,
        EnemyWaveStarting,
    }
    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipsDictionary;
    private float volume = .5f;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("soundVolume", .5f);

        soundAudioClipsDictionary = new Dictionary<Sound, AudioClip>();
        foreach(Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipsDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }
    public void PlaySound(Sound sound)
    {
       audioSource.PlayOneShot(soundAudioClipsDictionary[sound], volume);
    }
    public void IncreaseVolume()
    {
        volume += .1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public void  DecreaseVolume()
    {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}
