using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMark : MonoBehaviour {
    /// <summary>
    /// 标记脚本
    ///     主要负责地板数值的保存和向其他脚本传值
    ///     判断当前地板生命是否归零,并调用归零后的方法
    /// </summary>

    public bool Isbulled=false;//是否创建
    public bool Ismove=false;//是否移动
    public bool Isatrack=false;//是否攻击
    public bool IssmMo = false;//小兵
    public bool IsbiMO = false;//英雄
    public int IsMyplayers = 0;//判断敌我方,1为我方,2为敌方
    public GameObject Issm;//小兵模型
    public GameObject IsbigMO;//英雄模型
    public float Thelifes=0;//生命值
    public float Theattacks=0;//攻击力
    public List<GameObject> Ontrigglesobj = new List<GameObject>();
   
    private void Start()
    {
        
    }
    private void Update()
    {
        Islifeszero();
        show();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="bg")
        {
            Ontrigglesobj.Add(other.gameObject);//保存该地板周围地板对象
        }
    }
    void show()
    {            
        //创建有东西时显示攻击移动提示动画
        if (Isbulled==true)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "HP" + Thelifes+"\n"+"ATK" +Theattacks ;
        }
        if (Isatrack==true)
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else if (Isatrack==false)
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);//攻击提示显示与否
        }
        if (Ismove==true)
        {
            transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
        else if (Ismove==false)
        {
            transform.GetChild(0).GetChild(2).gameObject.SetActive(true);//移动提示显示与否
        }
    }
    private void Islifeszero()
    {
        if (Thelifes<=0)
        {
            Destroy(Issm);
            Destroy(IsbigMO);
            Isbulled = false;
            Ismove = false;
            IssmMo = false;
            IsbiMO = false;
            Isatrack = false;
            Theattacks = 0;
            IsMyplayers = 0;
            Thelifes = 0;//生命归零则重置该位置
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
