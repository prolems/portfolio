using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] public AudioSource bgSound;
    [SerializeField] private AudioClip[] bgList;
   // public GameObject optionPanel;
    Queue<GameObject> goQ = new Queue<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("씬전환로드");
        
        //if(GameObject.FindGameObjectWithTag("SfxSlider") != null&& GameObject.FindGameObjectWithTag("BgmSlider") != null)
        //{
        //    optionPanel = GameObject.FindGameObjectWithTag("Option");
        //    sfxVolSlider = GameObject.FindGameObjectWithTag("SfxSlider").GetComponent<Slider>();
        //    bgmVolSlider = GameObject.FindGameObjectWithTag("BgmSlider").GetComponent<Slider>();
        //    optionPanel.SetActive(false);
        //}
        //else
        //{
        //    // sfxslider등 없는 씬의 경우 음악 종료
        //    Debug.Log("음악이 안나와야함");
        //    bgSound.Stop();
        //    return;
        //}
        for (int i = 0; i < bgList.Length; i++)
        {

            if (arg0.name == bgList[i].name )
            {
                BgsoundPlay(bgList[i]);
            }
            else
            {
                bgSound.Stop();
            }
        }
    }
   
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        go.transform.SetParent(transform);
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void BgsoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.3f;
        bgSound.Play();
    }
   public  void BgmSoundVolume(float volume, TMP_Text bgmVolTxt)
    {
       // volume = bgmVolSlider.value;
        mixer.SetFloat("bgm", Mathf.Log10(volume)*20);
        bgmVolTxt.text = string.Format("{0:0.0}", volume);
    }
    public void SFXVolume(float volume, TMP_Text sfxVolTxt)    
    {
       //volume = sfxVolSlider.value;
        mixer.SetFloat("sfx",Mathf.Log10(volume)*20);
        sfxVolTxt.text = string.Format("{0:0.0}", volume);
    }
   
}
