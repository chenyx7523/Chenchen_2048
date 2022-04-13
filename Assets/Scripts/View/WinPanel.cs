using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : View
{
    //重新开始的按钮点击事件
    public void OnRestartClick()
    {
        //调用gamepanel 中的重新开始
        //GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
        GamePanel gamepanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();        
        gamepanel.RestartGame();
        //Debug.Log("按下");
        Hide();
    }

    //退出的按钮点击事件
    public void OnexitClick()
    {
        //Debug.Log("按下");
        //退出到菜单场景
        SceneManager.LoadSceneAsync(0);
    }















}
