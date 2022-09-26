using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの行動を制御する
/// </summary>
public class EnemyController : ControllerBase
{
    [SerializeField]
    private BoundPoint enemyBoundPoint;//エネミーの跳ねる位置

    [SerializeField]
    private BallController ballController;//BallController

    /// <summary>
    /// 移動方向を取得する
    /// </summary>
    /// <returns></returns>
    protected override Vector3 GetMoveDir()
    {
        //各子クラスで処理を記述

        //仮
        return Vector3.zero;
    }

    /// <summary>
    /// EnemyControllerの初期設定を行う
    /// </summary>
    public void SetUpEnemyController(BallController ballController)
    {
        //BallControllerを取得
        this.ballController = ballController;
    }

    /// <summary>
    /// ラケットを制御する（Updateメソッドで呼び出され続ける）
    /// </summary>
    protected override void ControlRacket()
    {
        //各子クラスで処理を記述
    }

    /// <summary>
    /// キャラクターの向きを設定する（Updateメソッドで呼び出され続ける）
    /// </summary>
    protected override void SetCharaDirection()
    {
        //常にエネミーのコートの方向に向く
        transform.LookAt(enemyBoundPoint.transform.position);
    }
}
