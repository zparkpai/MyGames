using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MouseMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 拖动脚本
    ///     挂载到具体卡牌上
    ///     负责卡牌的拖动并判断抬手位置是否能创建
    ///     以及是否能创建英雄单位
    ///     以及不能创建的返回
    ///     地板赋创建参数
    /// </summary>
    public GameObject Themodel;//预制体
    Transform MyTransform;//储存自身坐标
    Vector3 selfScencePosition;//保存自身坐标转换世界坐标
    GameObject Parents, OldParent;//父对象选择(用于拖拽图片)
    GameObject StartCube;//预制体创建位置坐标
    public bool IsSmModel;//是否为小兵预制体
    public GameObject Xb;//小兵模型
    public GameObject Hero;//英雄模型
    public int Cost;//花费能量
    CubeMark HitCube;//点击的地板脚本
    Music Mic;//声音

    private void Start()
    {
        MyTransform = transform;//调用自身坐标
        selfScencePosition = Camera.main.WorldToScreenPoint(MyTransform.position);//转换为世界坐标
        Parents = GameObject.Find("Cards");//查找画布为父对象
        OldParent = GameObject.Find("MiniCards");//查找滑动控件为父对象
        StartCube = GameObject.Find("Cube (18)");//查找预制体创建位置
        Mic = GameObject.Find("BulledMic").GetComponent<Music>();//撞击声
    }
    private void Update()
    {
    }
    void MouseDown()
    {

    }
    private void OnMouseDown()
    {
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.position.y > 7)
        {//判断是否在卡牌外
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool ss = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("bg"));//射线创建及得到碰撞地板
            CubeMark TheStartCube = StartCube.GetComponent<CubeMark>();//获得标记脚本
            if (ss)
            {
                HitCube = hit.collider.gameObject.GetComponent<CubeMark>();
            }
            if ((TheStartCube.Isbulled == false || TheStartCube.IsbiMO == false || TheStartCube.IssmMo == false) && GamesChoose.Powers > 0)
            {//判断是否可以创建
                if (IsSmModel == true && TheStartCube.IssmMo != true && GamesChoose.Powers > this.Cost)
                {//小兵创建
                    Mic.kaishi();
                    Destroy(this.gameObject);//卡片销毁(创建完成后)
                    GamesChoose.Powers -= Cost;
                    Xb = GameObject.Instantiate(Themodel, new Vector3(StartCube.transform.position.x, StartCube.transform.position.y + 1, StartCube.transform.position.z), Quaternion.identity);
                    TheStartCube.Issm = Xb;
                    TheStartCube.IssmMo = true;
                    TheStartCube.Isbulled = true;
                    TheStartCube.IsMyplayers = 1;
                    TheStartCube.Thelifes += 4000;
                    TheStartCube.Theattacks += 500;//地板属性添加
                    this.transform.DOScale(new Vector3(-1f, -1f, 1f), 0.1f).SetEase(Ease.Linear).SetRelative().OnComplete(() =>
                    {
                    });
                }
                else if (HitCube.IsMyplayers == 1 && IsSmModel != true && HitCube.IsbiMO == false && HitCube.IssmMo == true && GamesChoose.Powers > this.Cost)
                {//英雄单位创建
                    Mic.kaishi();
                    Destroy(this.gameObject);//卡片销毁(创建完成后)
                    GamesChoose.Powers -= Cost;
                    Hero = GameObject.Instantiate(Themodel, new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 1, hit.collider.gameObject.transform.position.z), Quaternion.identity);
                    HitCube.IsbigMO = Hero;
                    HitCube.IsbiMO = true;
                    HitCube.Thelifes += 2000;
                    HitCube.Theattacks += 1000;//攻击数值结算
                    HitCube.IsMyplayers = 1;/////////////////////////////////////暂时标记/////////////////
                    this.transform.DOScale(new Vector3(-1f, -1f, 1f), 0.2f).SetEase(Ease.Linear).SetRelative().OnComplete(() =>
                    {
                    });
                }
                else if (EventSystem.current.IsPointerOverGameObject())

                {
                    transform.SetParent(OldParent.transform);//卡牌返回
                }
            }
            else
            {
                transform.SetParent(OldParent.transform);//卡牌返回
            }

        }
        else
        {
            transform.SetParent(OldParent.transform);//未创建则返回
        }

    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.SetParent(Parents.transform);//转换父对象为画布
        Vector3 currentScenePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, selfScencePosition.z);
        //鼠标位置传入
        Vector3 currentWorldPosition = Camera.main.ScreenToWorldPoint(currentScenePosition);
        //鼠标位置转世界坐标
        MyTransform.position = currentWorldPosition;
        //卡牌位置变化
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
}
