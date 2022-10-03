using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// PlayerControllerクラスとEnemyControllerクラスの親クラス
    /// </summary>
    public class ControllerBase : MonoBehaviour 
    {
        protected CharacterController charaController;//CharacterController

        protected RacketController racketController;//RacketController

        protected BallController ballController;//BallController

        protected Vector3 firstPos;//初期位置
       
        protected OwnerType ownerType;//所有者の種類

        /// <summary>
        /// ControllerBaseの初期設定を行う
        /// </summary>
        /// <param name="ballController">BallController</param>
        public virtual void SetUpControllerBase(BallController ballController) 
        { 
            //CharacterControllerを取得
            charaController = GetComponent<CharacterController>();

            //RacketControllerを取得
            racketController = transform.GetChild(1).GetComponent<RacketController>();

            //RacketControllerの初期設定を行う
            racketController.SetUpRacketController();

            //初期位置を取得
            firstPos = transform.position;

            //BallControllerを取得
            this.ballController = ballController;
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //キャラクターの向きを設定する
            SetCharaDirection();

            //ラケットを振っている最中なら
            if (!racketController.IsIdle) 
            {
                //以降の処理を行わない
                return;
            }

            //ラケットを制御する
            ControlRacket();

            //移動する
            Move();
        }

        /// <summary>
        /// 移動する
        /// </summary>
        private void Move() 
        {
            //移動を実行する
            charaController.Move(GetMoveDir() * Time.deltaTime * GameData.instance.MoveSpeed + (Vector3.down * GameData.instance.Gravity));
        }

        /// <summary>
        /// 移動方向を取得する
        /// </summary>
        /// <returns>移動方向</returns>
        protected virtual Vector3 GetMoveDir() 
        {
            //各子クラスで処理を記述

            //仮
            return Vector3.zero;
        }

        /// <summary>
        /// ラケットを制御する（Updateメソッドで呼び出され続ける）
        /// </summary>
        protected virtual void ControlRacket() 
        {
            //各子クラスで処理を記述
        }

        /// <summary>
        /// キャラクターの向きを設定する（Updateメソッドで呼び出され続ける）
        /// </summary>
        protected virtual void SetCharaDirection() 
        {
            //各子クラスで処理を記述
        }
    }
}