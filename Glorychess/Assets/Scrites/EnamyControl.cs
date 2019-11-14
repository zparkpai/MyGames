using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnamyControl : MonoBehaviour
{
    /// <summary>
    /// 左移当前减一,右移加一
    /// 上移减六,下移加六
    /// </summary>
    public List<GameObject> Enamycard = new List<GameObject>();//卡牌链表,记录卡牌数据
    GameObject EnamyStart;//敌人大本营开始位置
    public bool ss = false, x = true;
    GameObject s;
    public GameObject[] AllCube;//地板数组
    public CubeMark[] AllCubeMark;//地板脚本数组
    public List<CubeMark> Needscontrol = new List<CubeMark>();//对象脚本,储存已建立的对象,用于下一次操作
    Greedy Greed;//敌方判断移动位置脚本
    Cardscontrol Card;//得到卡牌(用于敌方创建)
    GameObject Jd3;//回合结束按钮对象
    private void Start()
    {
        Jd3 = GameObject.Find("JobBtnDown");//得到回合结束按钮
        AllCubeMark = new CubeMark[24];//获取地板
        EnamyStart = GameObject.Find("Cube (5)");//敌方大本营
        for (int i = 0; i < AllCube.Length; i++)
        {
            AllCubeMark[i] = AllCube[i].GetComponent<CubeMark>();//获取地板脚本数组
        }
        Greed = transform.GetComponent<Greedy>();//找到判断脚本
        Card = GameObject.Find("Cards").GetComponent<Cardscontrol>();//得到卡牌脚本
    }
    private void Update()
    {
        //if (ss)
        //{
        //    Cards.GetEnamyCards();
        //}


        if (ss)
        {
            IsDestory();//判断地板元素的存在与否

            TanXin();//创建移动方法
            if (Needscontrol != null)
            {
                StartCoroutine(NeedMoves());
                //已建对象的操作判断
                ss = false;
                Card.GetCards();//敌方回合结束,我方回合开始并得到两张牌
            }
            else
            {
                ss = false;
                JobDone.mar.GetZero();//敌方结束所有地板移动攻击重置
                Card.GetCards();//敌方回合结束,我方回合开始并得到两张牌
            }
            //TanXin();
        }

    }
    public void Jobover()
    {
        ss = true;//按钮控制(暂用)
    }
    public void TanXin()
    {
        Enamycard.Clear();//清空原卡牌
        Transform Ec = GameObject.Find("EnamysCards").transform; //获取卡牌父对象       
        int Count = Ec.childCount;//卡牌数量
        Transform[] zz = new Transform[Count];//新建数组储存卡牌
        for (int k = 0; k < Count; k++)
        {
            zz[k] = Ec.GetChild(k);//子对象卡牌传入
        }
        for (int i = 0; i < Count; i++)
        {
            Transform max = zz[i];
            for (int j = i; j < Count; j++)
            {
                if (max.GetComponent<EnamyCardAttribute>().EnamyLife < zz[j].GetComponent<EnamyCardAttribute>().EnamyLife)
                {
                    max = zz[j];
                    zz[j] = zz[i];
                    zz[i] = max;            //卡牌排序(倒序)
                }

            }
            Enamycard.Add(max.gameObject);//添加入对象脚本
        }
        CubeMark TheStartCube = EnamyStart.GetComponent<CubeMark>();
        for (int i = 0; i < Enamycard.Count; i++)
        {//在开始位置新建对象
            if (Enamycard[i].GetComponent<EnamyCardAttribute>().Ishero == false && TheStartCube.Isbulled == false)
            {
                GameObject Ena = GameObject.Instantiate(Enamycard[i].GetComponent<EnamyCardAttribute>().EnamyXb, new Vector3(EnamyStart.transform.position.x, EnamyStart.transform.position.y + 1, EnamyStart.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)));
                TheStartCube.Issm = Ena;       //为地板作标记
                TheStartCube.IssmMo = true;
                TheStartCube.Isbulled = true;
                TheStartCube.IsMyplayers = 2;
                TheStartCube.Thelifes += Enamycard[i].GetComponent<EnamyCardAttribute>().EnamyLife;
                TheStartCube.Theattacks += Enamycard[i].GetComponent<EnamyCardAttribute>().EnamyAttack;
                Destroy(Enamycard[i].gameObject);
                Enamycard.Remove(Enamycard[i]);
                MoveJudge();//移动判断

            }
            //else if (TheStartCube.IssmMo == true && item.GetComponent<EnamyCardAttribute>().Ishero == true&&TheStartCube.IsbigMO==false)
            //{
            //    GameObject Enah = GameObject.Instantiate(item.GetComponent<EnamyCardAttribute>().EnamyHero, new Vector3(EnamyStart.transform.position.x, EnamyStart.transform.position.y + 1, EnamyStart.transform.position.z), Quaternion.Euler(new Vector3(0,180,0)));
            //    TheStartCube.IsbigMO = Enah;
            //    TheStartCube.IsbiMO = true;
            //    TheStartCube.Thelifes += item.GetComponent<EnamyCardAttribute>().EnamyLife;
            //    TheStartCube.Theattacks += item.GetComponent<EnamyCardAttribute>().EnamyAttack;
            //}
            else
            {
            }
        }
    }
    void MoveJudge()
    {//新创建完判断能否移动到周围地板
        //创建完成后移动出创建点
        if (!(AllCubeMark[4].Isbulled) && AllCubeMark[5].Isbulled == true && AllCubeMark[5].Ismove == false)
        {
            AllCube[5].GetComponent<CubeMark>().Issm.transform.DOMove((AllCube[4].transform.position - AllCube[5].transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
            if (AllCube[5].GetComponent<CubeMark>().IsbigMO)
            {
                AllCube[5].GetComponent<CubeMark>().IsbigMO.transform.Translate((AllCube[4].transform.position - AllCube[5].transform.position), Space.World);
            }
            SetCube(AllCubeMark[4], AllCubeMark[5]);//交换移动前后地板数据
            GetNull(AllCubeMark[5]);//原地板置空
            Needscontrol.Add(AllCubeMark[4]);//添加到对象脚本
            AttackJudge(AllCubeMark[4]);

        }
        else if (AllCubeMark[10].Isbulled == false && AllCubeMark[5].Isbulled == true && AllCubeMark[5].Ismove == false)
        {
            AllCube[5].GetComponent<CubeMark>().Issm.transform.DOMove((AllCube[10].transform.position - AllCube[5].transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
            if (AllCube[5].GetComponent<CubeMark>().IsbigMO)
            {
                AllCube[5].GetComponent<CubeMark>().IsbigMO.transform.Translate((AllCube[10].transform.position - AllCube[5].transform.position), Space.World);
            }
            SetCube(AllCubeMark[10], AllCubeMark[5]);
            GetNull(AllCubeMark[5]);
            Needscontrol.Add(AllCubeMark[10]);
            AttackJudge(AllCubeMark[10]);
        }
        else if (AllCubeMark[11].Isbulled == false && AllCubeMark[5].Isbulled == true && AllCubeMark[5].Ismove == false)
        {
            AllCube[5].GetComponent<CubeMark>().Issm.transform.DOMove((AllCube[11].transform.position - AllCube[5].transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
            if (AllCube[5].GetComponent<CubeMark>().IsbigMO)
            {
                AllCube[5].GetComponent<CubeMark>().IsbigMO.transform.Translate((AllCube[11].transform.position - AllCube[5].transform.position), Space.World);
            }
            SetCube(AllCubeMark[11], AllCubeMark[5]);
            GetNull(AllCubeMark[5]);
            Needscontrol.Add(AllCubeMark[11]);
            AttackJudge(AllCubeMark[11]);

        }
        else
        {
            Needscontrol.Add(AllCubeMark[5]);
        }
    }
    IEnumerator NeedMoves()
    {//已存在元素的操作
        int i = 0;
        for (; i < Needscontrol.Count; i++)
        {
            move(Needscontrol[i]);
            yield return new WaitForSeconds(0.3f);//逐个判断
        }
        if (i == Needscontrol.Count)
        {
            JobDone.mar.GetZero();//最后地板重置
            Jd3.SetActive(true);//回合结束按钮显示
            GamesChoose.Powers += 1;//我方能量加三
        }
    }
    void move(CubeMark NeedNum)
    {//移动脚本
        //实现移动
        if (NeedNum.Ismove == false)
        {
            GameObject t = Greed.Greedys(NeedNum);
            GameObject Moveto = t;//通过移动位置判断脚本得到移动位置
            if (Moveto.GetComponent<CubeMark>().Isbulled == false && Moveto && NeedNum.Issm)
            {
                NeedNum.Issm.transform.DOMove((Moveto.transform.position - NeedNum.gameObject.transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
                if (NeedNum.IsbigMO)
                {
                    NeedNum.IsbigMO.transform.DOMove((Moveto.transform.position - NeedNum.gameObject.transform.position), 0.2f).SetEase(Ease.Linear).SetRelative();
                }//小兵以及英雄单位的移动实现
                SetCube(Moveto.GetComponent<CubeMark>(), NeedNum);//移动后各自地板属性变换
                GetNull(NeedNum);//原地板归零
            }
            else
            {
                Moveto = NeedNum.gameObject;//不移动
            }
            Needscontrol.Remove(NeedNum);//移除原已存在对象
            Needscontrol.Add(Moveto.GetComponent<CubeMark>());//添加移动后的对象
            AttackJudge(Moveto.GetComponent<CubeMark>());//攻击选择
        }
    }
    void AttackJudge(CubeMark ss)
    {
        CubeMark Theena = null;
        for (int i = 0; i < ss.Ontrigglesobj.Count; i++)
        {
            if (ss.Ontrigglesobj[i].GetComponent<CubeMark>().IsMyplayers == 1)
            {
                if (Theena == null)
                {
                    Theena = ss.Ontrigglesobj[i].GetComponent<CubeMark>();
                }
                if (Theena.Thelifes > ss.Ontrigglesobj[i].GetComponent<CubeMark>().Thelifes)
                {
                    Theena = ss.Ontrigglesobj[i].GetComponent<CubeMark>();//找到对方生命值最低的单位
                }
            }
        }
        for (int i = 0; i < Enamycard.Count; i++)
        {
            if (ss.IsMyplayers==2&&ss.IssmMo == true && Enamycard[i].GetComponent<EnamyCardAttribute>().Ishero == true && ss.IsbigMO == false)
            {
                GameObject Enah = GameObject.Instantiate(Enamycard[i].GetComponent<EnamyCardAttribute>().EnamyHero, new Vector3(ss.transform.position.x, ss.transform.position.y + 2, ss.transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)));
                ss.IsbigMO = Enah;//英雄对象的创建并于对应地板赋值
                ss.IsbiMO = true;
                ss.Thelifes += Enamycard[i].GetComponent<EnamyCardAttribute>().EnamyLife;
                ss.Theattacks += Enamycard[i].GetComponent<EnamyCardAttribute>().EnamyAttack;
                Destroy(Enamycard[i].gameObject);
                Enamycard.Remove(Enamycard[i]);
            }

        }

        if (Theena && ss.Isatrack == false)
        {
            Attack(Theena, ss);//攻击方法
        }
        else
        {
            return;
        }
    }
    void Attack(CubeMark Ena, CubeMark Mys)
    {
        Ena.Thelifes -= Mys.Theattacks;
        Mys.Thelifes -= (Ena.Theattacks * 0.8f);//攻击(数值变化)
        Mys.Isatrack = true;
    }
    void IsDestory()
    {
        if (Needscontrol != null)
        {
            for (int i = 0; i < Needscontrol.Count; i++)
            {
                if (Needscontrol[i].Thelifes <= 0)
                {
                    Needscontrol.Remove(Needscontrol[i]);//生命值是否为零
                }
            }
        }
    }
    void GetNull(CubeMark ss)
    {//地板属性归零
        ss.Issm = null;
        ss.IsbigMO = null;
        ss.Isbulled = false;
        ss.Ismove = false;
        ss.IssmMo = false;
        ss.IsbiMO = false;
        ss.Isatrack = false;
        ss.Theattacks = 0;
        ss.IsMyplayers = 0;
        ss.Thelifes = 0;
    }
    void SetCube(CubeMark s1, CubeMark s2)
    {//地板属性交换
        s1.IsbigMO = s2.IsbigMO;
        s1.Issm = s2.Issm;
        s1.Thelifes = s2.Thelifes;
        s1.Theattacks = s2.Theattacks;
        s1.Isbulled = s2.Isbulled;
        s1.Ismove = true;
        s1.Isatrack = s2.Isatrack;
        s1.IsMyplayers = s2.IsMyplayers;
        s1.IsbiMO = s2.IsbiMO;
        s1.IssmMo = s2.IssmMo;
    }
}
