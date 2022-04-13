using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public SelectModePanel selectModePanel;  //定义选择模式面案
    public SetPanel setPanel;   //定义设置面板   后期显示时调用

    public AudioClip bgClip;

    private void Start()
    {
        AudioManager._instance.PlaybgMusic(bgClip);
    }



    //点击开始游戏
    public void OnStarGameClick()
    {
        //显示选择模式界面
        selectModePanel.Show();

    }
    //点击设置
    public void OnSetClick()
    {
        //显示设置的界面
        setPanel.Show();
    }
    //点击退出游戏
    public void OnExitClick()
    {
        //退出游戏
        Application.Quit(); //关闭程序
    }
    
}
