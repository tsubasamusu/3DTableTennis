using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerControllerクラスとEnemyControllerクラスの親クラス
/// </summary>
public class ControllerBase : MonoBehaviour
{
    protected CharacterController charaController;//CharacterController

    /// <summary>
    /// ControllerBaseの初期設定を行う
    /// </summary>
    public void SetUpControllerBase()
    {
        //CharacterControllerを取得
        charaController = GetComponent<CharacterController>();

        //各Controllerの初期設定を行う
        SetUpController();
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //移動する
        Move();
    }

    /// <summary>
    /// 移動する
    /// </summary>
    private void Move()
    {
        charaController.Move(GetMoveDir() * Time.deltaTime * GameData.instance.MoveSpeed + (Vector3.down * GameData.instance.Gravity));
    }

    /// <summary>
    /// 移動方向を取得する
    /// </summary>
    /// <returns></returns>
    protected virtual Vector3 GetMoveDir()
    {
        //TODO:各子クラスで処理を記述

        //仮
        return Vector3.zero;
    }

    /// <summary>
    /// 各Controllerの初期設定を行う
    /// </summary>
    protected virtual void SetUpController()
    {
        //TODO:各子クラスで処理を記述
    }
}
