using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    //实例化
    public GameObject gridPrefab;     //实例化预制体格子队列
    public GameObject numberPrefab;   //实例化可创建数字



    //声明变量
    public Text text_score;        //分数
    public Text text_best_score;   //最高分
    public int currentScore;
    //声音变量
    public AudioClip bgClip;
    public AudioClip soundClip;
    public AudioClip sound2Clip;



    //public Button btn_last;          //上一步
    public Button btn_restart;     //重新开始
    public Button btn_exit;        //退出
    public Transform gridParent;   //数字格子的父物体    
    private int row;  //行
    private int col;  //列
    private Vector3 pointerDownPos, pointerUpPos;   //鼠标按下和抬起的坐标    vector:矢量
    private bool isNeedCreateNumber = false;         //是否需要创建数字
    public WinPanel winPanel;            //赢的界面
    public LosePanel LosePanel;            //输掉的界面
    public Daqipanel Daqipanel;
    public static bool fast;             //判断是否为第一次开始



    //功能性参数
    //创建一个新的字典,保存grid对应行列的缩放信息{4，85}行高为4，缩放为85    config  配置
    public Dictionary<int, int> grid_config = new Dictionary<int, int>() { { 4, 85 }, { 5, 70 }, { 6, 59 } };
    public List<MyGrid> canCreateNumberGrid = new List<MyGrid>();   //可以创建数字的格子的队列 
    public MyGrid[][] grids = null;   //所有的格子组成的数组

    public StepModel lastStepModel;        //上一步的数据





    //上一步(打气)
    public void OnLastClick()
    {
        //BackToLastStep();
        //btn_last.interactable = false;
        Daqipanel.Show();
    }

    //重新开始
    public void OnReStartClick()
    {

        // SceneManager.LoadScene(1);            //重新载入界面
        RestartGame();
    }

    //退出
    public void OnExitClick()
    {
        //退出
        SceneManager.LoadSceneAsync(0);
    }
    //初始化格子
    public void InitGrid()
    {
        //获取格子数量
        int gridNum = PlayerPrefs.GetInt(Const.GameModel, 4);
        //初始化并获取其父节点的组件
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridNum;
        //将上面字典的长宽高赋值给gridLayoutGroup
        gridLayoutGroup.cellSize = new Vector2(grid_config[gridNum], grid_config[gridNum]);

        fast = true;            //第一次开始为true
        //Debug.Log("初始化啦,且fast状态为：" + fast);

        grids = new MyGrid[gridNum][];
        //创建格子
        row = gridNum;
        col = gridNum;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {//创建  i  j  格子
                if (grids[i] == null)
                {
                    grids[i] = new MyGrid[gridNum];
                }
                grids[i][j] = CreateGride();

            }
        }
    }

    //创建格子
    public MyGrid CreateGride()
    {//实例化格子预制体           Instantiate  举例
        GameObject gameObject = GameObject.Instantiate(gridPrefab, gridParent);
        return gameObject.GetComponent<MyGrid>();  //将当前创建的格子返回去

    }

    //创建数字 
    public void CreateNumber()
    {
        //数字所在的格子
        canCreateNumberGrid.Clear();    //初始化一下
        //遍历格子并判断有无数字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {//没有数字则继续执行
                    //将其加入集合中
                    canCreateNumberGrid.Add(grids[i][j]);

                }
            }
        }
        if (canCreateNumberGrid.Count == 0)
        {
            return;
        }
        //随机一个格子
        int index = Random.Range(0, canCreateNumberGrid.Count);  //随机数范围为0，到可创建格子的数的上限

        //创建数字，把他放入格子                       transform:使转换
        GameObject gameObj = GameObject.Instantiate(numberPrefab, canCreateNumberGrid[index].transform);//canCreateNumberGrid为父物体
        gameObj.GetComponent<Number>().Init(canCreateNumberGrid[index]);  //对数字进行初始化

    }

    public void CreatNumber(MyGrid mygrid,int number)
    {
        // 创建数字，把他放入格子 transform:使转换
        GameObject gameObj = GameObject.Instantiate(numberPrefab, mygrid.transform);//canCreateNumberGrid为父物体
        gameObj.GetComponent<Number>().Init(mygrid);  //对数字进行初始化
        gameObj.GetComponent<Number>().SetNumber(number);
    }

    private void Awake()    //awake  唤醒
    {
        //初始化界面
        InitPanelMessage();
        InitGrid();   //初始化格子方法
        //创建第一个数字
        CreateNumber();

    }


    public void OnPointerDown()
    {

        //Debug.Log("按下：" + Input.mousePosition);
        pointerDownPos = Input.mousePosition;
        //DLog();
    }

    public void OnPointerup()
    {
        //Debug.Log("抬起：" + Input.mousePosition);
        pointerUpPos = Input.mousePosition;
        if (Vector3.Distance(pointerDownPos, pointerUpPos) < 50)
        {
            //Debug.Log("操作无效");
            return;
        }
        lastStepModel.UpdateDate(this.currentScore, PlayerPrefs.GetInt(Const.BestScore, 0), grids);
        //btn_last.interactable = true;
        MoveType moveType = CaculateMoveType();    //获取CaculateMoveType方法，得到其最终运动方向，即move类型                                                   
        MoveNumber(moveType);

        //产生数字
        if (isNeedCreateNumber)
        {
            CreateNumber();
        }

        //恢复所有数字的状态
        RestNumberStatus();
        isNeedCreateNumber = false;
        //判断游戏是否结束
        if (IsGameLose())   //true说明游戏结束
        {
            GameLose();
        }

    }


    //判断滑动屏幕的方向
    public MoveType CaculateMoveType()
    {
        fast = false;              //一旦滑动，第一次失效 
        //Debug.Log("滑动啦,且fast状态为：" + fast);
        if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerDownPos.y - pointerUpPos.y))
        {
            //则说明是左右移动，x轴位移绝对值大于y轴绝对值
            if (pointerUpPos.x - pointerDownPos.x > 0)
            {
                //向右
                //Debug.Log("右边");
                return MoveType.RIGHT;
            }
            else
            {//向左
                //Debug.Log("左边");
                return MoveType.LEFT;
            }
        }
        else
        //(Mathf.Abs(pointerUpPos.y - pointerDownPos.y) > Mathf.Abs(pointerDownPos.x - pointerUpPos.x))
        {
            //则说明是上下移动，y轴位移绝对值大于x轴绝对值
            if (pointerUpPos.y - pointerDownPos.y > 0)
            {
                //向上
                //Debug.Log("上边");
                return MoveType.TOP;
            }
            else
            {//向下
                //Debug.Log("下边");
                return MoveType.DOWN;
            }
        }
    }

    //定义移动的数字（获取即将进行的移动类型）
    public void MoveNumber(MoveType moveType)
    {
        switch (moveType)
        {


            case MoveType.TOP:
                for (int j = 0; j < col; j++)            //（i，j） （1，0）
                {
                    for (int i = 1; i < row; i++)   //遍历整个grids数组
                    {

                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();   //获取当前遍历格子的数字，赋值给number
                                                                       //Debug.Log("坐标" + i + "," + j);
                            for (int m = i - 1; m >= 0; m--)  //遍历方块的上一个方块
                            {
                                //if (grids[m][j].IsHaveNumber())    //当前遍历的方块是否有方块  .ISHaveNumber==true
                                //{
                                //    //有数字  判断能否合并
                                //    if (number.GetNumber() == grids[m][j].GetNumber().GetNumber())
                                //    {
                                //        //相等则合并
                                //        grids[m][j].GetNumber().Merge();
                                //        //销毁当前数字
                                //        number.GetGrid().SetNumber(null);            //将要销毁的方块中的数字设置为空
                                //        GameObject.Destroy(number.gameObject);
                                //    }
                                //    break;
                                //}
                                //else
                                //{
                                //    //没数字  移动上去
                                //    Debug.Log("准备移动");
                                //    //number.MoveToGrid(grids[m][j]);   //获取目标格子坐标返回给移动方法
                                //    number.MoveToGrid(grids[m][j]);
                                //}
                                //调用方法
                                Number targerNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targerNumber = grids[m][j].GetNumber();

                                }
                                HandleNumber(number, targerNumber, grids[m][j]);
                                if (targerNumber != null)    //如果有数字则跳过
                                {
                                    break;
                                }

                            }
                        }

                    }
                }

                break;
            case MoveType.DOWN:


                for (int j = 0; j < col; j++)            //（i，j） （1，0）
                {
                    for (int i = row - 2; i >= 0; i--)   //遍历整个grids数组
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();   //获取当前遍历格子的数字，赋值给number
                                                                       //Debug.Log("坐标" + i + "," + j);
                            for (int m = i + 1; m < row; m++)  //遍历方块的上一个方块
                            {
                                //调用方法
                                Number targerNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targerNumber = grids[m][j].GetNumber();

                                }
                                HandleNumber(number, targerNumber, grids[m][j]);
                                if (targerNumber != null)    //如果有数字则跳过
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.LEFT:
                for (int i = 0; i < row; i++)            //（i，j） （1，0）
                {
                    for (int j = 1; j < col; j++)   //遍历整个grids数组
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();   //获取当前遍历格子的数字，赋值给number
                                                                       //Debug.Log("坐标" + i + "," + j);
                            for (int m = j - 1; m >= 0; m--)  //遍历方块的上一个方块
                            {
                                //调用方法
                                Number targerNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targerNumber = grids[i][m].GetNumber();

                                }
                                HandleNumber(number, targerNumber, grids[i][m]);
                                if (targerNumber != null)    //如果有数字则跳过
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.RIGHT:
                for (int i = 0; i < row; i++)            //（i，j） （1，0）
                {
                    for (int j = col - 2; j >= 0; j--)   //遍历整个grids数组
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();   //获取当前遍历格子的数字，赋值给number
                                                                       //Debug.Log("坐标" + i + "," + j);
                            for (int m = j + 1; m < col; m++)  //遍历方块的上一个方块
                            {
                                //调用方法
                                Number targerNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targerNumber = grids[i][m].GetNumber();

                                }
                                HandleNumber(number, targerNumber, grids[i][m]);
                                if (targerNumber != null)    //如果有数字则跳过
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;

        }
    }

    //处理数字合并的方法
    public void HandleNumber(Number current, Number target, MyGrid targetGrid)   //当前数字  目标数字   格子
    {
        if (target != null)  //要比较的格子是否为空
        {
            //判断能否合并
            if (current.IsMerge(target))
            {
                target.Merge();
                //销毁当前数字
                current.GetGrid().SetNumber(null);            //将要销毁的方块中的数字设置为空
                //GameObject.Destroy(current.gameObject);
                current.DestroyOnMoveEnd(target.GetGrid());
                isNeedCreateNumber = true;
            }
        }
        else
        {   //没有数字
            current.MoveToGrid(targetGrid);
            isNeedCreateNumber = true;
        }
    }


    //遍历数字，然后恢复初始状态
    public void RestNumberStatus()
    {
        for (int j = 0; j < col; j++)            //（i，j） （1，0）
        {
            for (int i = 0; i < row; i++)   //遍历整个grids数组
            {
                if (grids[i][j].IsHaveNumber())
                {
                    grids[i][j].GetNumber().status = NumberStatus.Normal;
                }
            }
        }
    }

    //判断游戏是否失败
    public bool IsGameLose()
    {
        //判断格子是否为满
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {
                    return false;   //未满
                }
            }
        }
        //遍历格子进行判断是否有左右相等的格子

        for (int i = 0; i < row; i += 2)
        {
            for (int j = 0; j < col; j++)
            {
                MyGrid up = IsHaveGrid(i - 1, j) ? (MyGrid)grids[i - 1][j] : null; //使用ishave方法看有没有格子，有则再看格子在的数字，没有
                MyGrid down = IsHaveGrid(i + 1, j) ? (MyGrid)grids[i + 1][j] : null;
                MyGrid left = IsHaveGrid(i, j - 1) ? (MyGrid)grids[i][j - 1] : null;
                MyGrid right = IsHaveGrid(i, j + 1) ? (MyGrid)grids[i][j + 1] : null;

                if (up != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(up.GetNumber()))
                    {
                        return false;
                    }
                }
                if (down != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(down.GetNumber()))
                    {
                        return false;
                    }
                }
                if (left != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(left.GetNumber()))
                    {
                        return false;
                    }
                }
                if (right != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(right.GetNumber()))
                    {
                        return false;
                    }
                }
            }
        }



        return true;  //失败
    }

    public bool IsHaveGrid(int i, int j)
    {
        if (i >= 0 && i < row && j >= 0 && j < col)
        {
            return true;
        }
        return false;
    }



    #region 游戏流程(开始与退出)




    public void ExitGame()
    {

    }
    //重新开始
    public void RestartGame()
    {
        //数据清空
        //btn_last.interactable = false;
        //清空分数
        RestScore();
        //清空数字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {                
                Destroy(grids[i][j].GetNumber());
                //Debug.Log("产生了销毁");                
                grids[i][j].SetNumber(null);
            }
        }
        //创建数字
        //CreateNumber();
        SceneManager.LoadScene(1);
    }

    //初始化界面
    public void InitPanelMessage()
    {
        this.text_best_score.text = PlayerPrefs.GetInt(Const.BestScore, 0).ToString();
        lastStepModel = new StepModel();
        //btn_last.interactable = false;
        //播放bg音乐
        AudioManager._instance.PlaybgMusic(bgClip);

    }

    public void GameWin()
    {
        winPanel.Show();

    }

    public void GameLose()
    {
        //Debug.Log("游戏结束");
        LosePanel.Show();
    }

    //上一步
    public void BackToLastStep()
    {
        //分数
        currentScore = lastStepModel.score;
        UpdateScore(lastStepModel.score);

        PlayerPrefs.SetInt(Const.BestScore, lastStepModel.bestScore);  //保存分数和最高分
        UpdateBastScore(lastStepModel.bestScore);


        // 数字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (lastStepModel.numbers[i][j] == 0)
                {
                    if (grids[i][j].IsHaveNumber())
                    {

                        Destroy(grids[i][j].GetNumber());
                    }
                }
                else if (lastStepModel.numbers[i][j] != 0)
                {
                    if (grids[i][j].IsHaveNumber())
                    {
                        // 修改数字
                        grids[i][j].GetNumber().SetNumber(lastStepModel.numbers[i][j]);
                    }
                    else
                    {
                        // 创建数字                        
                        CreatNumber(grids[i][j], lastStepModel.numbers[i][j]);  
                    }
                }
            }
        }
    }


    





    //分数

    //添加分数
    public void AddScore(int score)
    {
        currentScore += score;       //当前分数+=新增分数
        UpdateScore(currentScore);
        //判断是否为最高分
        if (currentScore >= PlayerPrefs.GetInt(Const.BestScore, 0))
        {
            PlayerPrefs.SetInt(Const.BestScore, currentScore);
            UpdateBastScore(currentScore);
        }
    }

    //更新分数
    public void UpdateScore(int score)
    {
        this.text_score.text = score.ToString();     //令UI中的text=（score的强制转化string）
        

    }
    //重置分数
    public void RestScore()
    {
        currentScore = 0;
        UpdateScore(currentScore);
    }
    //更新最高分
    public void UpdateBastScore(int bestScore)
    {
        this.text_best_score.text = bestScore.ToString();
    }

    #endregion

    //public void DLog()
    //{
    //    float bg = PlayerPrefs.GetFloat(Const.Music, 0.5f);
    //    float sound = PlayerPrefs.GetFloat(Const.Sound, 0.5f);
    //    float sound2 = PlayerPrefs.GetFloat(Const.Sound, 0.5f);

    //    Debug.Log("bg：" + bg + "移动" + sound + "合成" + sound2);
    //}


    //测试git——1
    //测试2
    //3.51的测试版本

}
