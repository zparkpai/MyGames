using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesChoose : MonoBehaviour
{
    /// <summary>
    /// 弹窗显示脚本
    ///     控制各弹窗的显示
    /// </summary>
    public GameObject Gv1, Gv2;//弹窗对象
    GameObject Jd3;
    Text Powtxt;
    public static int Powers = 10;
    private void Start()
    {
        Powtxt = GameObject.Find("Powertxt").GetComponent<Text>();//能量显示
        Jd3 = GameObject.Find("JobBtnDown");
    }
    private void Update()
    {
        Powtxt.text = Powers.ToString();
        ExitGet();
    }
    public void ResetGames()
    {
        SceneManager.LoadScene("StartChess");//重新加载游戏场景
    }
    public void BackItems()
    {
        SceneManager.LoadScene("ChessStartScene");//返回主菜单
    }
    public void QuitGames()
    {
        Application.Quit();//退出游戏
    }
    void ExitGet()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Gv1.SetActive(true);//按下退出键弹出弹窗GV1
        }
    }
    public void BackGame()
    {
        Gv1.SetActive(false);
        Gv2.SetActive(false);//返回则关闭弹窗
    }
    public void Set()
    {
        Gv2.SetActive(true);//设置按钮弹窗
    }
    public void JobBtnDo()
    {
        Jd3.SetActive(false);//回合结束时关闭按钮
    }
}
