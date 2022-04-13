using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //调用SceneManager

public class SelectModePanel : View
{
    //点击模式切换
    public void OnSelectModeClick(int count)
    {
        //选择模式
        PlayerPrefs.SetInt(Const.GameModel,count);  //保存数据，退出后仍存在。
        //跳转场景
        SceneManager.LoadSceneAsync(1);
        //SceneManager.LoadScene("OtherSceneName", LoadSceneMode.Additive);
    }

    









}
