using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// エネミーの行動を制御する
    /// </summary>
    public class EnemyController : ControllerBase 
    {
        private BoundPoint enemyBoundPoint;//エネミーの跳ねる位置

        /// <summary>
        /// リセットする
        /// </summary>
        private void Reset() 
        {
            //自分がエネミーなら
            if (TryGetComponent(out EnemyController _)) 
            {
                //所有者の種類をエネミーに設定
                ownerType = OwnerType.Enemy;
            }
        }

        /// <summary>
        /// ControllerBaseの初期設定を行う（親クラスで用意している設定の処理は残しつつ、このクラス独自の処理を追加する）
        /// </summary>
        public override void SetUpControllerBase(BallController ballController) 
        {
            //親クラスのSetUpControllerBaseメソッドを呼び出す
            base.SetUpControllerBase(ballController);

            //リセットする
            Reset();

            //エネミー用の跳ねる位置を取得
            enemyBoundPoint = ballController.GetBoundPoint(ownerType);
        }

        /// <summary>
        /// 移動方向を取得する
        /// </summary>
        /// <returns>移動方向</returns>
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
            if (!racketController.IsIdle) 
            {
                //以降の処理を行わない
                return;
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
}