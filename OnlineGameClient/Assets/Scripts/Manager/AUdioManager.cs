using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUdioManager : BaseManager {
    public AUdioManager(GameFacade facade) : base(facade) { }

    private const string Sound_Path_Prefix = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShot = "ArrowShoot";
    public const string Sound_Bg_Fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;
    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();

        PlaySound(bgAudioSource, LoadSound(Sound_Bg_Moderate),0.5f,true);

    }

    public void PlayBgSound(string soundName)
    {
        PlaySound(bgAudioSource, LoadSound(soundName),0.5f,true);
    }
    public void PlayNormalSound(string soundName)
    {
        PlaySound(normalAudioSource,LoadSound(soundName),1f);
    }
    private void PlaySound(AudioSource audioSource, AudioClip clip,float volume, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.volume = volume;
        audioSource.loop = loop;
    }
    private AudioClip LoadSound(string soundsName)
    {
        return Resources.Load<AudioClip>(Sound_Path_Prefix+soundsName);
    }
}
