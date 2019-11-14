using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greedy : MonoBehaviour
{
    /// <summary>
    /// 敌方移动位置判断脚本
    ///     敌方为电脑
    ///     我方为玩家
    /// </summary>
    EnamyControl cubes;
    GameObject Could;
    List<GameObject> CouldMove = new List<GameObject>();//当前地板可移动位置数组
    GameObject Starts, EnamyStart;//双方关键位置
    bool Empty = false;//判空
    private void Start()
    {
        Starts = GameObject.Find("Cube (18)");//我方关键点
        EnamyStart = GameObject.Find("Cube (5)");//敌方关键点
    }
    public GameObject Greedys(CubeMark ss)
    {
        if (ss == EnamyStart.GetComponent<CubeMark>())
        {
            for (int i = 0; i < ss.Ontrigglesobj.Count; i++)
            {
                if (ss.Ontrigglesobj[i].GetComponent<CubeMark>().IsMyplayers == 0)
                {
                    return ss.Ontrigglesobj[i];//若该对象在敌方关键位置,则移动出来
                }
            }

        }
        for (int q = 0; q < EnamyStart.GetComponent<CubeMark>().Ontrigglesobj.Count; q++)
        {
            for (int p = 0; p < EnamyStart.GetComponent<CubeMark>().Ontrigglesobj[q].GetComponent<CubeMark>().Ontrigglesobj.Count; p++)
            {
                if (EnamyStart.GetComponent<CubeMark>().Ontrigglesobj[q].GetComponent<CubeMark>().Ontrigglesobj[p].GetComponent<CubeMark>().IsMyplayers == 1)
                {
                    return BackMove(ss);//若我方进入敌方危险范围则敌方回防
                }
            }
        }
        Could = null;
        CouldMove.Clear();
        for (int i = 0; i < ss.Ontrigglesobj.Count; i++)
        {
            CouldMove.Add(ss.Ontrigglesobj[i]);
        }
        for (int j = 0; j < CouldMove.Count; j++)
        {
            if (CouldMove[j].GetComponent<CubeMark>().Isbulled == false)
            {

                for (int k = 0; k < CouldMove[j].GetComponent<CubeMark>().Ontrigglesobj.Count; k++)
                {
                    if (CouldMove[j].GetComponent<CubeMark>().Ontrigglesobj[k].GetComponent<CubeMark>().IsMyplayers == 1)
                    {
                        Could = CouldMove[j].GetComponent<CubeMark>().Ontrigglesobj[k];
                        if (ss.Thelifes > Could.GetComponent<CubeMark>().Thelifes && Could.GetComponent<CubeMark>().Thelifes != 0)
                        {
                            return CouldMove[j];//能移动位置攻击范围生命大于我方则返回该位置
                        }
                    }
                }
                if (Could != null)
                {
                    if (Vector3.Distance(CouldMove[j].transform.position, Starts.transform.position) <= 24)
                    {
                        return EndMove(ss);//若离我方关键点小于三个地板则向我方关键点移动
                    }
                    for (int i = 0; i < CouldMove[j].GetComponent<CubeMark>().Ontrigglesobj.Count; i++)
                    {
                        if (!CouldMove[j].GetComponent<CubeMark>().Ontrigglesobj[i].GetComponent<CubeMark>().Isbulled)
                        {
                            Empty = true;
                        }
                        else
                        {
                            Empty = false;
                        }
                    }
                    if (Empty)
                    {
                        return CouldMove[j];//判断可移动位置的攻击范围是否有我方单位,没有则移动到该位置
                    }
                }
            }
            else
            {
                return EndMove(ss);
            }
        }
        return EndMove(ss);
    }
    GameObject EndMove(CubeMark ss)
    {
        GameObject tt = CouldMove[0];
        for (int i = 0; i < CouldMove.Count - 1; i++)
        {
            if (CouldMove[i].GetComponent<CubeMark>().IsMyplayers == 0)
            {
                if (CouldMove[i].transform.position == Starts.transform.position)
                {
                    return CouldMove[i];//若我方关键点在该单位可移动位置上则返回该可移动位置
                }
                if (Vector3.Distance(CouldMove[i].transform.position, Starts.transform.position) > Vector3.Distance(CouldMove[i + 1].transform.position, Starts.transform.position))
                {
                    tt = CouldMove[i + 1];//距离我方关键点最近的一个可移动位置
                }
            }
        }
        return tt;//返回距离我方关键点最近的一个可移动位置
    }
    GameObject BackMove(CubeMark ss)
    {
        GameObject tt = CouldMove[0];
        for (int i = 0; i < CouldMove.Count - 1; i++)
        {
            if (CouldMove[i].GetComponent<CubeMark>().IsMyplayers != 2)
            {
                if (CouldMove[i].transform.position == EnamyStart.transform.position)
                {
                    return CouldMove[i];//若回防时可移动位置为敌方关键点则返回该可移动位置
                }
                if (Vector3.Distance(CouldMove[i].transform.position, EnamyStart.transform.position) > Vector3.Distance(CouldMove[i + 1].transform.position, EnamyStart.transform.position))
                {
                    tt = CouldMove[i + 1];//离敌方关键点最近的可移动位置
                }
            }
        }
        return tt; //返回离敌方关键点最近的可移动位置
    }
}
