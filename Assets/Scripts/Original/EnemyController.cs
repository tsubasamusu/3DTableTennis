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

    private Vector3 firstPos;//初期位置

    /// <summary>
    /// 移動方向を取得する
    /// </summary>
    /// <returns></returns>
    protected override Vector3 GetMoveDir()
    {
        //現在のボールの所有者がエネミーなら
        if (ballController.CurrentOwner == OwnerType.Enemy)
        {
            //初期位置への方向を返す（初期位置に向かって移動する）
            return firstPos - transform.position;
        }

        //相手（プレイヤー）のボールがコートに入るなら
        if (ballController.InCourt)
        {
            //目的地への方向を返す（ボールに向かって移動する）
            return ballController.transform.position - transform.position;
        }

        //移動しない
        return Vector3.zero;
    }

    /// <summary>
    /// EnemyControllerの初期設定を行う
    /// </summary>
    public void SetUpEnemyController(BallController ballController)
    {
        //BallControllerを取得
        this.ballController = ballController;

        //初期位置を取得
        firstPos = transform.position;
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

        //フォアハンドドライブにするかバックハンドドライブにするかを決めて、ドライブをする
        racketController.Drive(transform.position.x >= ballController.transform.position.x);
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
