using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAni : MonoBehaviour
{
    /// <summary>
    /// 开始界面脚本
    /// </summary>
    float Scend = 0;
    int K = 0;
    public GameObject[] Lines;//储存线条动画
                              // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LinesStart();//动画播放
    }
    void LinesStart()
    {
        Scend += Time.deltaTime;
        if (Scend > 0.5f && K <= 3)
        {
            Scend = 0;
            Lines[K++].SetActive(true);//判断动画播放先后
        }
    }
    public void StartGames()
    {
        SceneManager.LoadScene("StartChess");//转到游戏场景
    }
    public void ExitGames()
    {
        Application.Quit();//退出游戏
    }
}
