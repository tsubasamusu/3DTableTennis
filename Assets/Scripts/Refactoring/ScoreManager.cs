//using System.Collections;//IEnumeratorを使用
using UnityEngine;

namespace yamap {

    /// <summary>
    /// 得点を管理する
    /// </summary>
    public class ScoreManager : MonoBehaviour {

        private BallController ballController;



        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="ballController"></param>
        public void SetUpScoreManager(BallController ballController) {  // , UIManager uIManager, PlayerController playerController

            //得点の更新の確認を開始する
            //StartCoroutine(CheckScore());


            this.ballController = ballController;
        }


        /// <summary>
        /// 得点の記録を更新する
        /// </summary>
        /// <param name="updateValue">更新量</param>
        public void UpdateScore((int playerUpdateValue, int enemyUpdateValue) updateValue) {
            //効果音を再生
            SoundManager.instance.PlaySound(updateValue.playerUpdateValue > 0 ? SoundDataSO.SoundName.PlayerPointSE : SoundDataSO.SoundName.EnemyPointSE);

            //プレイヤーの得点を更新
            GameData.instance.score.playerScore += updateValue.playerUpdateValue;

            //エネミーの得点を更新
            GameData.instance.score.enemyScore += updateValue.enemyUpdateValue;
        }

        /// <summary>
        /// 得点の更新量を取得する
        /// </summary>
        /// <param name="ballController">BallController</param>
        /// <returns>得点の更新量</returns>
        public (int playerUpdateValue, int enemyUpdateValue) GetUpadateValue() {
            //コートに入ったかどうかで処理を変更
            return ballController.InCourt ?

                //ボールの所有者に応じて戻り値を変更
                ballController.CurrentOwner == OwnerType.Player ? (1, 0) : (0, 1)

                //ボールの所有者に応じて戻り値を変更
                : ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);
        }
    }
}