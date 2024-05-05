using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mixer;
    public AudioMixerGroup[] mixers;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;



    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Ranege=7, Select, Win }

    void Awake()
    {
        instance = this;
        mixers = mixer.FindMatchingGroups("Master");
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmPlayer.outputAudioMixerGroup = mixers[1];
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index=0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].outputAudioMixerGroup = mixers[2];
        }

        //오디오 믹서 값 초기화
        mixer.SetFloat("Master", 0);
        mixer.SetFloat("BGM", 0);
        mixer.SetFloat("SFX", 0);
    }



    public void PlayBgm(bool isPlay)
    {
        if (isPlay) {
            bgmPlayer.Play();
        } else {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }    

    public void PlaySfx(Sfx sfx)
    {
        for (int index=0; index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;
            
            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee) { // Hit가 2개고 Melee이 3개면 Switch 문으로 나눠서 할것.
                ranIndex = Random.Range(0, 2);
            }
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void BGMAudioChange(float sound)
    {
        if (sound == -40f) {
            mixer.SetFloat("BGM", -80);
        } else {
            mixer.SetFloat ("BGM", sound);
        }
    }

    public void SFXAudioChange(float sound)
    {
        if (sound == -40f) {
            mixer.SetFloat("SFX", -80);
        } else {
            mixer.SetFloat ("SFX", sound);
        }
    }

    public void AudioChange(float sound)
    {
        if (sound == -40f) {
            mixer.SetFloat("Master", -80);
        } else {
            mixer.SetFloat ("Master", sound);
        }
    }

}
