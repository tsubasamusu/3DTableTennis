using UnityEngine;
using UniRx;　　　//　宣言追加

namespace yamap {

    /// <summary>
    /// 得点を管理する
    /// </summary>
    public class ScoreManager : MonoBehaviour {

        private BallController ballController;

        // ReactiveProperty　(Model)
        // その②の実装時に使う
        public ReactiveProperty<int> PlayerScore = new();
        public ReactiveProperty<int> EnemyScore = new();


        void Reset() {
            PlayerScore = new(0);
            EnemyScore = new(0);
        }


        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="ballController"></param>
        public void SetUpScoreManager(BallController ballController) {  // , UIManager uIManager, PlayerController playerController

            //得点の更新の確認を開始する
            //StartCoroutine(CheckScore());

            this.ballController = ballController;

            Reset();
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

            ///エネミーの得点を更新
            GameData.instance.score.enemyScore += updateValue.enemyUpdateValue;



            // ReactiveProperty を GameData で利用している場合(その①での実装時)
            GameData.instance.PlayerScore.Value += updateValue.playerUpdateValue;
            
            GameData.instance.EnemyScore.Value += updateValue.enemyUpdateValue;



            // ScoreManager で ReactiveProperty を利用している場合(その②での実装時)
            PlayerScore.Value += updateValue.playerUpdateValue;

            EnemyScore.Value += updateValue.enemyUpdateValue;
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