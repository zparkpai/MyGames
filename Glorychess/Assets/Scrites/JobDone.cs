using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobDone : MonoBehaviour
{
    public static JobDone mar;//单例
    public GameObject[] CubeFloods;
    private void Awake()
    {
        mar = this;
    }
    public void GetZero()
    {
        for (int i = 0; i < CubeFloods.Length; i++)
        {
            CubeMark ss = CubeFloods[i].GetComponent<CubeMark>();
            ss.Ismove = false;
            ss.Isatrack = false;//所有地板重置
        }
    }

}
