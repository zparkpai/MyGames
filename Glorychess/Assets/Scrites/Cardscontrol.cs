using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cardscontrol : MonoBehaviour
{
    /// <summary>
    /// 发牌脚本
    ///     主要负责卡牌的创建
    /// </summary>

    public Image[] Cards;//我方卡牌
    public Image[] EnamyCard;//敌方卡牌
    public GameObject Startpos;//我方卡牌放置位置
    public GameObject EnamyStartpos;//敌方卡牌放置位置
    public GameObject Gv;//结束界面判断
    GameObject s1, s2;//获取胜负关键地板
    // Use this for initialization
    void Start()
    {
        StartGetCard();
        StartGetEnamyCard();
        s1 = GameObject.Find("Cube (18)");//我方关键地板
        s2 = GameObject.Find("Cube (5)");//敌方关键地板
        //Gv = GameObject.Find("GameOvers");
        //Gv.SetActive(false);
    }
    void Jugement()
    {
        if (s1.GetComponent<CubeMark>().IsMyplayers == 2)
        {
            GameOver();
        }
        else if (s2.GetComponent<CubeMark>().IsMyplayers == 1)
        {
            GameOver();
        }
        else { }
    }
    void GameOver()
    {
        Gv.SetActive(true);//界面显示
    }
    // Update is called once per frame
    void Update()
    {
        Jugement();
    }
    void StartGetCard()
    {
        Image s1 = Instantiate(Cards[0], transform);
        s1.transform.SetParent(Startpos.transform);
        Image s2 = Instantiate(Cards[1], transform);
        s2.transform.SetParent(Startpos.transform);
        for (int i = 0; i < 3; i++)
        {
            Image ss = Instantiate(Cards[Random.Range(0, 6)], transform);//随机发放卡牌
            ss.transform.SetParent(Startpos.transform);//设置父对象
        }
    }
    void StartGetEnamyCard()
    {
        Image s1 = Instantiate(EnamyCard[0], transform);
        s1.transform.SetParent(EnamyStartpos.transform);
        Image s2 = Instantiate(EnamyCard[1], transform);
        s2.transform.SetParent(EnamyStartpos.transform);
        for (int i = 0; i < 3; i++)
        {
            Image ss = Instantiate(EnamyCard[Random.Range(0, 6)], transform);//随机发放卡牌
            ss.transform.SetParent(EnamyStartpos.transform);//设置敌方父对象
        }
    }
    public void GetCards()
    {
        if (Startpos.transform.childCount <= 6)
        {
            for (int i = 0; i < 2; i++)
            {
                Image ss = Instantiate(Cards[Random.Range(0, 6)], transform);//随机发放卡牌
                ss.transform.SetParent(Startpos.transform);//设置父对象
            }
        }
    }
    public void GetEnamyCards()
    {
        if (EnamyStartpos.transform.childCount <= 6)
        {
            for (int i = 0; i < 2; i++)
            {
                Image ss = Instantiate(EnamyCard[Random.Range(0, 6)], transform);//随机发放卡牌
                ss.transform.SetParent(EnamyStartpos.transform);//设置敌方父对象
            }
        }
    }
}
