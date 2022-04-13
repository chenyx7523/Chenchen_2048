//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Chen : Number
//{
//    private Image bg;
//    private Text number_text;
//    private MyGrid inGrid;  //数字所在的格子
//    public NumberStatus status;   //数字的可合并状态
//    private int number2;          //控制不连续生成4


//    private bool isPlayingSpawnAnim = false;             //是否在生成的播放动画
//    private float spawnSCaleTime = 1;         //生成格子的动画参数
//    private bool isPlayingMergeAnim = false;  //是否在播放合并动画
//    private float mergeSCaleTime = 1;       //合并格子的动画参数（放大）
//    private float mergeSCaleTimeBack = 1;       //合并格子的动画参数（缩小）

//    private float movePosTime = 1;
//    private bool isMoveing = false;            //判断是否在移动
//    private bool isDestroyOnMoveEnd = false;    //是移动结束
//    private Vector3 startMovePos, endMovePos;   //开始移动位置，目标位置s

//    public Color[] bg_colors;               //数字的颜色
//    public List<int> number_index;


//    private void Awake()
//    {
//        bg = transform.GetComponent<Image>(); //获取当前组件中的image属性
//        number_text = transform.Find("Text").GetComponent<Text>();   //在属性中找到text名称的组件并将其赋值给text
//    }

//    //初始化数字方法
//    public void Init(MyGrid myGrid)
//    {
//        myGrid.SetNumber(this);
//        this.SetGrid(myGrid);//设置所在的格子

//        //首次生成
//        number2 = 0;
//        this.SetNumber(2);   //给他一个初始化的数字 (2)
//        number2 = 1;


//        //int num = Random.Range(1, 3);    //在1，2中取值，不包括三。（min，max）

//        float num = Random.Range(0f, 1.0f);        //浮点类型
//        Debug.Log(num);
//        if (num <= 0.1 || number2 == 3)
//        {
//            this.SetNumber(2);   //给他一个初始化的数字 (2)
//            number2 = 1;
//            Debug.Log("生成2的num：" + number2);

//        }
//        else
//        {
//            this.SetNumber(4);
//            number2++;
//            Debug.Log("生成4的num：" + number2);
//        }




//        status = NumberStatus.Normal;

//        //动画方案
//        PlaySpawnAnim();



//    }
//    //设置格子
//    public void SetGrid(MyGrid myGrid)
//    {
//        this.inGrid = myGrid;
//    }

//    //获取格子
//    public MyGrid GetGrid()
//    {
//        return this.inGrid;
//    }

//    //生成数字
//    public void SetNumber(int number)
//    {
//        this.number_text.text = number.ToString();   //将数字设置为显示出的数字

//        this.bg.color = this.bg_colors[number_index.IndexOf(number)];
//    }
//    //获取数字
//    public int GetNumber()
//    {
//        return int.Parse(number_text.text);   //强制转换为int类型   number——text.text为string
//    }

//    //将数字移动到某一个格子的下面
//    public void MoveToGrid(MyGrid myGrid)
//    {
//        transform.SetParent(myGrid.transform);   //将其父物体设置为格子
//        //transform.localPosition = Vector3.zero;  //局部坐标为0，使其在格子中间(使其坐标相等)
//        startMovePos = transform.localPosition;
//        //endMovePos=myGrid.transform.position;
//        movePosTime = 0;
//        isMoveing = true;
//        this.GetGrid().SetNumber(null);    //获取格子数字，使其变为空
//        myGrid.SetNumber(this);            //目标格子设为当前数字
//        this.SetGrid(myGrid);              //当前数字的格子变成有数字的格子

//    }

//    //在移动结束后销毁
//    public void DestroyOnMoveEnd(MyGrid myGrid)
//    {
//        transform.SetParent(myGrid.transform);
//        startMovePos = transform.localPosition;

//        movePosTime = 0;
//        isMoveing = true;
//        isDestroyOnMoveEnd = true;
//    }


//    //合并
//    public void Merge()
//    {
//        int number = this.GetNumber() * 2;
//        this.SetNumber(number);

//        if (number == 2048)
//        {
//            //游戏胜利
//            //GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().GameWin();
//            GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().GameWin();

//        }

//        status = NumberStatus.NotMerge;
//        PlayMergeAnim();


//    }

//    //判断能否合并

//    public bool IsMerge(Number number)
//    {
//        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal)
//        {
//            return true;
//        }
//        return false;
//    }

//    //动画方法，播放方块刚创建的动画
//    public void PlaySpawnAnim()
//    {
//        spawnSCaleTime = 0;  //spawnSCaleTime<1则调用update中的缩放动画
//        isPlayingSpawnAnim = true;
//    }

//    //播放合并的动画
//    public void PlayMergeAnim()
//    {
//        mergeSCaleTime = 0;
//        mergeSCaleTimeBack = 0;
//        isPlayingMergeAnim = true;

//    }

//    //判断是否为第一个生成的计数器




//    private void Update()
//    {
//        //创建格子动画
//        if (isPlayingSpawnAnim)
//        {
//            if (spawnSCaleTime <= 1)
//            {
//                spawnSCaleTime += Time.deltaTime * 8;    //使其加上每一帧的时间，不断自增
//                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnSCaleTime);     //使其不断变大  0-1
//            }
//        }
//        else
//        {
//            isPlayingSpawnAnim = false;
//        }

//        //合并格子动画
//        if (isPlayingMergeAnim)
//        {
//            //合并的动画（放大）
//            if (mergeSCaleTime <= 1 && mergeSCaleTimeBack == 0)
//            {
//                mergeSCaleTime += Time.deltaTime * 9;
//                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergeSCaleTime);  //最后的mer是时间
//            }
//            //合并的动画（缩小）
//            if (mergeSCaleTime >= 1 && mergeSCaleTimeBack <= 1)
//            {
//                mergeSCaleTimeBack += Time.deltaTime * 9;
//                transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, mergeSCaleTimeBack);  //最后的mer是时间
//            }
//            if (mergeSCaleTime >= 1 && mergeSCaleTimeBack >= 1)
//            {
//                isPlayingMergeAnim = false;
//            }

//        }
//        //移动的动画
//        if (isMoveing)
//        {
//            movePosTime += Time.deltaTime * 8;
//            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, movePosTime);

//            //if (movePosTime <= 1)
//            //{

//            //}
//            if (movePosTime >= 1)
//            {
//                isMoveing = false;
//                if (isDestroyOnMoveEnd)
//                {
//                    GameObject.Destroy(gameObject);
//                }
//            }

//        }


//    }



















//}


