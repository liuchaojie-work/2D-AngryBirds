using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    //标志是否在按下状态
    private bool isClick = false;
    //限定最大距离
    private float maxDis = 1.2f;
    //限定最大距离的基准点
    private Transform rightPos;
    //弹簧关节组件
    private SpringJoint2D sj2D;
    //刚体组件
    private Rigidbody2D rg2D;
    private void Awake()
    {
        rightPos = GameObject.Find("Slingshot/Slingshot_right/RightPos").transform;
        sj2D = transform.GetComponent<SpringJoint2D>();
        rg2D = transform.GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isClick)
        {
            //若是在按下状态，则小鸟跟着鼠标移动
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //小鸟的z轴不是相机的z轴
            transform.position += new Vector3(0, 0,- Camera.main.transform.position.z);

            if(Vector3.Distance(transform.position,rightPos.position)>maxDis)
            {
                //若小鸟与基准点的距离大于最大距离，则进行距离限定
                //基准点指向小鸟的向量，并单位化
                Vector3 pos = (transform.position - rightPos.position).normalized;
                //最大长度的向量
                pos *= maxDis;
                transform.position = pos + rightPos.position;
            }

        }
    }

    /// <summary>
    /// 鼠标按下
    /// </summary>
    private void OnMouseDown()
    {
        isClick = true;
        sj2D.enabled = true;
        //鼠标按下。开启运动学，忽略物理引擎
        rg2D.isKinematic = true;
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    private void OnMouseUp()
    {
        isClick = false;
        //鼠标抬起，关闭运动学
        rg2D.isKinematic = false;  
        //延迟禁用弹簧关节
        Invoke("BirdFly", 0.1f);   
    }

    /// <summary>
    /// 小鸟飞出
    /// </summary>
    private void BirdFly()
    {
        //鼠标抬起，禁用弹簧关节
        sj2D.enabled = false;
    }
}
