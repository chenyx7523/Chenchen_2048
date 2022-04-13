using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public AudioSource bg,sound,sound2; 
    public static bool isPlaySound = false;    



    private void Awake()
    {
        _instance = this;       //变成单例模式

        bg = transform.Find("bg").GetComponent<AudioSource>();   //初始化
        sound = transform.Find("sound").GetComponent<AudioSource>();
        sound2 = transform.Find("sound2").GetComponent<AudioSource>();

        //获取保存的声音
        bg.volume = PlayerPrefs.GetFloat(Const.Music, 0.5f);
        sound.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);
        sound2.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);



    }
    //bg播放
    public void PlaybgMusic(AudioClip audioClip)
    {
        bg.clip = audioClip;         //
        bg.loop = true;             //循环播放
        bg.Play();
        Debug.Log("bg");
    }
    //移动播放
    public void PlaySound(AudioClip audioClip)
    {
        sound2.PlayOneShot(audioClip);
    }
    //合成播放
    public void PlaySound2(AudioClip audioClip)
    {
        sound2.PlayOneShot(audioClip);
        isPlaySound = true;

    }




    //bg音量的更改
    public void OnMusicVolumChange(float value)
    {
        bg.volume = value;
    }

    //音量的更改
    public void OnSoundVolumChange(float value)
    {
        sound.volume = value;
        sound2.volume = value;
    }






    
}
