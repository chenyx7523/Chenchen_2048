using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public Number number;   //当前格子的数字
    //判断是否有数字
    public bool IsHaveNumber()
    {
        return number!=null;    //有数字为true
    }
    //获取数字
    public Number GetNumber()
    {
        return number;
    }
    //改变数字
    public void SetNumber(Number number)  
    {
        this.number = number;   //重新赋值
    }
    //public Number  GetNumberText()
    //{
    //    //return number_text;
    //}










}
