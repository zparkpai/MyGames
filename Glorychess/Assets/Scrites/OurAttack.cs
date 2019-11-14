using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OurAttack : MonoBehaviour
{
    /// <summary>
    /// 攻击脚本
    ///     主要通过判断点击的两地板之间的关系
    ///     从而达到地板数值变化
    ///     完成攻击
    /// </summary>
    public GameObject[] Cubess = new GameObject[2];//储存点击地板
    int i = 0;
    CubeMark cubeMark;//声明标记脚本
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {//点击不为UI
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
                    if (cubeMark.IsMyplayers == 1 && cubeMark.Isbulled == true && cubeMark.Isatrack == false && cubeMark.IsMyplayers == 1)
                    {//判断是否建造了东西
                        Cubess[0] = hit.collider.gameObject;
                        Cubess[1] = null;

                    }
                    else if (Cubess[0] && !Cubess[1] && cubeMark.IsMyplayers == 2)
                    {
                        Cubess[1] = hit.collider.gameObject;//点击到第一个有物体的地板后储存第二次点击的地板
                        float hs = Vector3.Distance(Cubess[0].transform.position, Cubess[1].transform.position);
                        if (hs <= 18)
                        {

                            CubeMark Fir = Cubess[0].GetComponent<CubeMark>();//地板1标记脚本
                            CubeMark Sec = Cubess[1].GetComponent<CubeMark>();//地板2标记脚本
                            Sec.Thelifes -= Fir.Theattacks;
                            Fir.Thelifes -= (Sec.Theattacks * 0.8f);//攻击(数值变化)
                            Fir.Isatrack = true;
                        }

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
