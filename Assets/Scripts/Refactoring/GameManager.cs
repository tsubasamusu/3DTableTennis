using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用
using UnityEngine;

namespace yamap {

    public enum GameState {
        Wait,
        Play,
        GameUp
    }

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private List<ControllerBase> controllersList = new();//コントローラーのリスト

        [SerializeField]
        private BallController ballController;//BallController

        [SerializeField]
        private ScoreManager scoreManager;//ScoreManager

        [SerializeField]
        private UIManager uiManager;//UIManager

        private PlayerController playerController;//PlayerController

        //private EnemyController enemyController;//EnemyController

        [SerializeField] // Debug
        private GameState currentGameState;


        /// <summary>
        /// ゲーム開始直後に呼び出される
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator Start() {
            currentGameState = GameState.Wait;

            //各コントローラーの初期設定を行う
            SetUpControllers();

            //ScoreManagerの初期設定を行う
            scoreManager.SetUpScoreManager(ballController);

            //ゲームスタート演出が終わるまで待つ
            yield return uiManager.PlayGameStart();

            //PlayerControllerとEnemyControllerを活性化する
            //playerController.enabled = enemyController.enabled = true;

            // コントローラーの活性化
            ActivateControllers(true);

            //BGMを再生
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);

            currentGameState = GameState.Play;
            
            // ボールの位置監視
            StartCoroutine(CheckBallPosition());
        }

        /// <summary>
        /// 各コントローラーの初期設定を行う
        /// </summary>
        private void SetUpControllers() {
            //登録されているコントローラーの数だけ繰り返す
            for (int i = 0; i < controllersList.Count; i++) {
                //ControllerBaseの初期設定を行う    
                controllersList[i].SetUpControllerBase(ballController);

                // ここで子クラスごとの分岐を作ってしまうと、【他に子クラスが増えたときに分岐を追加する】必要性が生まれてしまう。
                // それではクラスの継承の機能を活用できているとはいえないため、親クラスのメソッドを実行し、子クラスで「メソッドの振る舞い」を変更させるようにする

                ////PlayerControllerを取得出来たら
                //if (controllersList[i].TryGetComponent(out PlayerController playerController)) {
                //    //PlayerControllerを取得
                //    this.playerController = playerController;

                //    //PlayerControllerの初期設定を行う
                //    playerController.SetUpPlayerController();

                //    //BallControllerの初期設定を行う
                //    ballController.SetUpBallController();
                //}
                ////EnemyControllerを取得できたら
                //else if (controllersList[i].TryGetComponent(out EnemyController enemyController)) {
                //    //EnemyControllerを取得
                //    this.enemyController = enemyController;

                //    //EnemyControllerの初期設定を行う
                //    enemyController.SetUpEnemyController(ballController);
                //}

                if (playerController == null && controllersList[i].TryGetComponent(out playerController)) {
                    //PlayerControllerを取得
                    Debug.Log("PlayerController 取得");
                }
            }

            //PlayerControllerとEnemyControllerを非活性化する
            //playerController.enabled = enemyController.enabled = false;

            ActivateControllers(false);
        }

        /// <summary>
        /// コントローラーの活性化切り替え
        /// </summary>
        /// <param name="isSwitch"></param>
        private void ActivateControllers(bool isSwitch) {

            // ①
            //for (int i = 0; i < controllersList.Count; i++) {
            //    controllersList[i].enabled = isSwitch;
            //}

            // ②　どちらでも同じです
            controllersList[0].enabled = controllersList[1].enabled = isSwitch;

            //playerController.enabled = enemyController.enabled = isSwitch;
        }

        /// <summary>
        /// ゲーム終了演出を行う
        /// </summary>
        /// <param name="isGameOverPerformance">行うゲーム終了演出がゲームオーバー演出かどうか</param>
        /// <returns>待ち時間</returns>
        private IEnumerator PlayGameEndPerformance(bool isGameOverPerformance) {
            //全ての音が完全に止まるまで待つ
            yield return new WaitForSeconds(GameData.instance.FadeOutTime);

            //効果音を再生
            SoundManager.instance.PlaySound(isGameOverPerformance ? SoundDataSO.SoundName.GameOverSE : SoundDataSO.SoundName.PlayerPointSE);

            //Debug.Log("SE OK");

            //ゲーム終了演出を行う
            yield return isGameOverPerformance ? uiManager.PlayGameOver() : uiManager.PlayGameClear();

            //メインシーンを読み込む
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// ゲーム終了の準備を行う
        /// </summary>
        private void PrepareGameEnd() {
            //PlayerControllerとEnemyControllerを非活性化する
            //playerController.enabled = enemyController.enabled = false;

            ActivateControllers(false);

            //全ての音を停止する
            SoundManager.instance.StopSound();

            //Debug.Log("BGM Stop");
        }

        //TODO:リファクタリングの段階で、以降の監視処理をUniRxを使用して書き換える

        //private bool flag;//重複処理防止用

        ////マッチポイントを取得
        //int matchPoints = GameData.instance.MaxScore - 1;


        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        //private void Update() {
        //    //マッチポイントを取得
        //    //int matchPoints = GameData.instance.MaxScore - 1;

        //    //プレイヤーの得点も、エネミーの得点もマッチポイント以下なら
        //    if (GameData.instance.score.playerScore <= matchPoints && GameData.instance.score.enemyScore <= matchPoints) {
        //        //以降の処理を行わない
        //        return;
        //    }

        //    //flagがfalseなら
        //    if (!flag) {
        //        //ゲーム終了の準備を行う
        //        PrepareGameEnd();

        //        //ゲーム終了演出を行う
        //        StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));

        //        //flagにtrueを入れる
        //        flag = true;
        //    }
        //}


        // ScoreManager から移管

        private int count;//サーブの回数の記録用
        private OwnerType server;//サーバー保持用


        /// <summary>
        /// ボールの位置の監視
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckBallPosition() {　　　// ScoreManager の CheckScore メソッド
            // ゲームプレイ中の間だけ監視
            while (currentGameState == GameState.Play) {
                //ボールが床に落ちたなら
                if (ballController.transform.position.y <= 0.25f) {
                    // 1セット終了
                    currentGameState = GameState.Wait;
                }

                ////得点の記録を更新
                //scoreManager.UpdateScore(scoreManager.GetUpadateValue());

                ////得点の表示の更新をする準備を行う
                //uiManager.PrepareUpdateTxtScore();

                ////ボールの動きを止める
                //ballController.PrepareRestartGame(GetAppropriatServer());   //, playerController

                //// Player の初期化
                //playerController.ResetPlayerPos();

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }

            // ボールが下に落ちたので、このセットは終了
            EndOneSet();
        }

        /// <summary>
        /// 1セット終了時の処理
        /// </summary>
        private void EndOneSet() {           
            //得点の記録を更新
            scoreManager.UpdateScore(scoreManager.GetUpadateValue());

            //得点の表示の更新をする準備を行う
            uiManager.PrepareUpdateTxtScore();

            //サーバーを更新を確認してからサーバーを取得して、ボールの動きを止める
            ballController.PrepareRestartGame(GetAppropriatServer());   //, playerController

            // Player の初期化
            playerController.ResetPlayerPos();

            //プレイヤーの得点かエネミーの得点がマックススコアなら
            if (GameData.instance.score.playerScore >= GameData.instance.MaxScore || GameData.instance.score.enemyScore >= GameData.instance.MaxScore) {

                currentGameState = GameState.GameUp;

                //ゲーム終了の準備を行う
                PrepareGameEnd();

                //ゲーム終了演出を行う
                StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));
            } else {
                currentGameState = GameState.Play;

                // 次のセットを開始
                // ボールの位置の監視をスタート
                StartCoroutine(CheckBallPosition());
            }

            /// <summary>
            /// 適切なサーバーを取得する（ボールが落下した際に呼び出だされる）
            /// </summary>
            /// <returns>適切なサーバー</returns>
            OwnerType GetAppropriatServer() {
                //サーブ回数を記録
                count++;

                //まだ2本サーブを打っていないなら
                if (count < 2) {
                    //サーバーを変更しない
                    return server;
                }

                //サーブ回数を初期化
                count = 0;

                //サーバーを変えて、記録する
                return server = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;
            }
        }
    }
}