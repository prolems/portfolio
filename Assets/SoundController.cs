using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TMP_Text bgmVolTxt;
    [SerializeField] TMP_Text sfxVolTxt;
  public void BgmVolume()
    {
        SoundManager.instance.BgmSoundVolume(bgmSlider.value,bgmVolTxt);
    }
    public void SfxVolume()
    {
        SoundManager.instance.SFXVolume(sfxSlider.value,sfxVolTxt);
    }
    public void SaveVolumeBttn()
    {
        float[] volumeValues = new float[2];
        volumeValues[0] = bgmSlider.value;
        volumeValues[1] = sfxSlider.value; ;
        for (int i = 0; i < volumeValues.Length; i++)
        {
            PlayerPrefs.SetFloat($"volumeValue{i}", volumeValues[i]);
        }
    }
    void LoadValues()
    {
        float[] volumeValues = new float[2];
        for (int i = 0; i < volumeValues.Length; i++)
        {
            volumeValues[i] = PlayerPrefs.GetFloat($"volumeValue{i}");
        }
            bgmSlider.value = volumeValues[0];
            sfxSlider.value = volumeValues[1];
    }
    private void Start()
    {
        LoadValues();
    }
}
