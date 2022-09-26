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
        //ボールが攻撃圏内に入っていなかったら
        if (Mathf.Abs((transform.position - ballController.transform.position).magnitude) > GameData.instance.EnemyShotRange)
        {
            //以降の処理を行わない
            return;
        }

        //ラケットを振っている最中なら
        if(!racketController.IsIdle)
        {
            //以降の処理を行わない
            return ;
        }

        //フォアハンドドライブにするかバックハンドドライブにするかをランダムに決めて、ドライブをする
        racketController.Drive(Random.Range(0, 2) == 0 ? true : false);
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
