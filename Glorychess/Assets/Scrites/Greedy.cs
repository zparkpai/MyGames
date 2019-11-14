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
        CouldMove.Clear();
        for (int i = 0; i < ss.Ontrigglesobj.Count; i++)
        {
            if (ss.Ontrigglesobj[i].GetComponent<CubeMark>().Isbulled==false)
            {
                CouldMove.Add(ss.Ontrigglesobj[i]);

            }
        }
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
        else
        {
            for (int q = 0; q < EnamyStart.GetComponent<CubeMark>().Ontrigglesobj.Count; q++)
            {
                for (int p = 0; p < EnamyStart.GetComponent<CubeMark>().Ontrigglesobj[q].GetComponent<CubeMark>().Ontrigglesobj.Count; p++)
                {
                    if (EnamyStart.GetComponent<CubeMark>().Ontrigglesobj[q].GetComponent<CubeMark>().IsMyplayers == 1)
                    {
                        return BackMove(ss);//若我方进入敌方危险范围则敌方回防
                    }
                    else if (EnamyStart.GetComponent<CubeMark>().Ontrigglesobj[q].GetComponent<CubeMark>().Ontrigglesobj[p].GetComponent<CubeMark>().IsMyplayers == 1)
                    {
                        return BackMove(ss);//若我方进入敌方危险范围则敌方回防
                    }
                }
            }
            Could = null;
            for (int j = 0; j < CouldMove.Count; j++)
            {
                if (Vector3.Distance(ss.transform.position, Starts.transform.position) <= 30)
                {
                    return EndMove(ss);//若离我方关键点小于三个地板则向我方关键点移动
                }
                if (ss.Ontrigglesobj[j].GetComponent<CubeMark>().Isbulled == false)
                {

                    for (int k = 0; k < ss.Ontrigglesobj[j].GetComponent<CubeMark>().Ontrigglesobj.Count; k++)
                    {
                        if (ss.Ontrigglesobj[j].GetComponent<CubeMark>().Ontrigglesobj[k].GetComponent<CubeMark>().IsMyplayers == 1)
                        {
                            Could = ss.Ontrigglesobj[j].GetComponent<CubeMark>().Ontrigglesobj[k];
                            if (ss.Thelifes > Could.GetComponent<CubeMark>().Thelifes && Could.GetComponent<CubeMark>().Thelifes != 0)
                            {
                                return ss.Ontrigglesobj[j];//能移动位置攻击范围生命大于我方则返回该位置
                            }
                        }
                        else
                        {
                            if (Could != null)
                            {


                                for (int i = 0; i < ss.Ontrigglesobj[j].GetComponent<CubeMark>().Ontrigglesobj.Count; i++)
                                {
                                    if (!ss.Ontrigglesobj[j].GetComponent<CubeMark>().Ontrigglesobj[i].GetComponent<CubeMark>().Isbulled)
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
                                    return ss.Ontrigglesobj[j];//判断可移动位置的攻击范围是否有我方单位,没有则移动到该位置
                                }
                            }
                            else
                            {
                                return EndMove(ss);
                            }
                        }
                    }


                }
                else
                {
                    return EndMove(ss);
                }
            }
        }
        return EndMove(ss);
    }

    GameObject EndMove(CubeMark ss)
    {
        if (CouldMove.Count>=1)
        {
            GameObject tt = CouldMove[0];

            for (int i = 1; i < CouldMove.Count - 1; i++)
            {

                if (CouldMove[i].transform.position == Starts.transform.position)
                {
                    return CouldMove[i];//若我方关键点在该单位可移动位置上则返回该可移动位置
                }
                else if (CouldMove[i + 1] && Vector3.Distance(CouldMove[i].transform.position, Starts.transform.position) > Vector3.Distance(CouldMove[i + 1].transform.position, Starts.transform.position))
                {
                    tt = CouldMove[i + 1];//距离我方关键点最近的一个可移动位置
                }

            }
            return tt;//返回距离我方关键点最近的一个可移动位置
        }
        else
        {
            return ss.gameObject;
        }
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
