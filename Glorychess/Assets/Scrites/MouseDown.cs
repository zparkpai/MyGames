using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MouseDown : MonoBehaviour
{
    /// <summary>
    /// 物体移动脚本
    ///     主要负责小兵以及英雄单位的移动和判断移动条件
    ///     
    /// </summary>
    public GameObject[] Cubess = new GameObject[2];//储存点击地板
    int i = 0;
    GameObject Themovesbig, Themovessml;//分别表示英雄物体和小兵物体
    CubeMark cubeMark;//声明标记脚本
    OurAttack isattacking;//攻击脚本
    Music Mic;//音乐
    private void Start()
    {
        Mic = GameObject.Find("WalkMic").GetComponent<Music>();//脚步声声明
        isattacking = GetComponent<OurAttack>();
    }
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {//点击的不为UI
            if (Input.GetMouseButtonDown(0))
            {//按下按钮

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //射线的创建
                bool ss = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("bg"));
                //是否碰撞bg层
                if (ss)
                {
                    cubeMark = hit.collider.gameObject.GetComponent<CubeMark>();//获得点击地板的标记脚本
                    if (cubeMark.Isbulled == true && cubeMark.Ismove == false && cubeMark.IsMyplayers != 2)
                    {//判断是否建造了东西
                        Cubess[0] = hit.collider.gameObject;
                        Cubess[1] = null;
                        //若点击地板上有物体则数组Cubess[1]置空
                        Themovesbig = Cubess[0].gameObject.GetComponent<CubeMark>().IsbigMO;
                        //判断是否为英雄物体

                        Themovessml = Cubess[0].gameObject.GetComponent<CubeMark>().Issm;
                        //判断是否为小兵物体
                    }
                    else if (!isattacking.Cubess[1] && cubeMark.IsMyplayers == 0 && Cubess[0] && !Cubess[1] && cubeMark.Ismove == false)
                    {
                        Cubess[1] = hit.collider.gameObject;//点击到第一个有物体的地板后储存第二次点击的地板
                        float hs = Vector3.Distance(Cubess[0].transform.position, Cubess[1].transform.position);
                        if (hs <= 18)
                        {
                            Mic.kaishi();//播放声音
                            if (Themovesbig && Cubess[0].gameObject.GetComponent<CubeMark>().IsbiMO == true)
                            {
                                Themovesbig.transform.DOMove((Cubess[1].transform.position - Cubess[0].transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
                            }
                            Themovessml.transform.DOMove((Cubess[1].transform.position - Cubess[0].transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
                            CubeMark Fir = Cubess[0].GetComponent<CubeMark>();//地板1标记脚本
                            CubeMark Sec = Cubess[1].GetComponent<CubeMark>();//地板2标记脚本
                            Sec.Isbulled = true;
                            Sec.Isatrack = Fir.Isatrack;
                            Sec.Theattacks = Fir.Theattacks;
                            Sec.Thelifes = Fir.Thelifes;
                            if (Themovesbig)
                            {
                                Sec.IsbiMO = true;
                                Sec.IsbigMO = Themovesbig;
                            }//地板2添加数值

                            Sec.Issm = Themovessml;

                            Sec.IssmMo = true;
                            Sec.Ismove = true;
                            Sec.IsMyplayers = 1;//标记地板2
                            Fir.Isbulled = false;
                            Fir.Isatrack = false;
                            Fir.Ismove = false;
                            Fir.IsbiMO = false;
                            Fir.IssmMo = false;
                            Fir.IsbigMO = null;
                            Fir.Issm = null;
                            Fir.Theattacks = 0;
                            Fir.Thelifes = 0;
                            Fir.IsMyplayers = 0;//将地板1的标记置空

                        }
                        //判断地板1上的元素并向地板2移动            
                    }
                    else if (Cubess[0] && Cubess[1])
                    {
                        Cubess[1] = null;
                        Cubess[0] = null;//点击其他位置则所有数组置空
                    }
                    if (i > 1)
                    {
                        i = 0;
                    }
                }
            }
        }
    }
}
