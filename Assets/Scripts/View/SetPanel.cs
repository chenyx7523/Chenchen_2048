using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : View
{

    public Slider slider_sound; //Slider 滑块
    public Slider slider_music;    

    //关闭按钮
    public void OnBtnCloseClick()
    {
        //隐藏当前界面
        Hide();


    }
    //音效
    public void ObSoundValueChange(float f)
    {
        //修改音效的大小
        AudioManager._instance.OnSoundVolumChange(f);
        //保存当前的修改
        PlayerPrefs.SetFloat(Const.Sound,f);


    }
    //音乐
    public void OnMusicValueChange(float f)
    {
        AudioManager._instance.OnMusicVolumChange(f);
        PlayerPrefs.SetFloat(Const.Music, f);
    }
    public override void Show()
    {
        base.Show();
        //对界面进行初始化
        slider_sound.value = PlayerPrefs.GetFloat(Const.Sound, 0);  //0则代表若Const sound没有值则0
        slider_music.value = PlayerPrefs.GetFloat(Const.Music, 0);
    }

   

}
