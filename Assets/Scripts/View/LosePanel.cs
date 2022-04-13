﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : View
{
    //重新开始的按钮点击事件
    public void OnRestartClick()
    {
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
        Hide();  //隐藏当前界面
    }

    //退出的按钮点击事件
    public void OnexitClick()
    {
        //退出到菜单场景
        SceneManager.LoadSceneAsync(0);
    }
















}


