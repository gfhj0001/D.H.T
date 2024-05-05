using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Setting : MonoBehaviour
{

    public Slider bgmAudioSlider;
    public Slider sfxAudioSlider;
    public Slider allAudioSlider;
    public AudioMixer masterMixer;

    RectTransform settingPage;




    void Awake()
    {
        settingPage = GameObject.Find("Setting").GetComponent<RectTransform>();
        bgmAudioSlider = GameObject.Find("BGM_Slider").GetComponent<Slider>();
        sfxAudioSlider = GameObject.Find("SFX_Slider").GetComponent<Slider>();
        allAudioSlider = GameObject.Find("ALL_Slider").GetComponent<Slider>();
        
    }


    public void BGMAudioSlider()
    {   
        AudioManager.instance.BGMAudioChange(bgmAudioSlider.value);

    }

    public void SFXAudioSlider()
    {   
        AudioManager.instance.SFXAudioChange(sfxAudioSlider.value);
    }

    public void AudioSlider()
    {
        AudioManager.instance.AudioChange(allAudioSlider.value);
    }


    public void OnClick_OpenSettingPage()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        GameManager.instance.Stop();
        settingPage.localScale = Vector3.one;
        
    }

    public void OnClick_CloseSettingPage()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        GameManager.instance.Resume();
        settingPage.localScale = Vector3.zero;
    }

    public void OnClick_ReStartGame()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        GameManager.instance.GameRetry();
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }



}
