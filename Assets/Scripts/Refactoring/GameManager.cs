using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用
using UniRx;//UniRxを使用
using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState 
    {
        Wait,//待機状態
        Play,//試合中
        GameUp//ゲーム終了
    }

    public class GameManager : MonoBehaviour 
    {
        [SerializeField]
        private List<ControllerBase> controllersList = new();//コントローラーのリスト

        [SerializeField]
        private BallController ballController;//BallController

        [SerializeField]
        private ScoreManager scoreManager;//ScoreManager

        [SerializeField]
        private UIManager uiManager;//UIManager

        private PlayerController playerController;//PlayerController

        private GameState currentGameState;//現在のゲームの状態


        /// <summary>
        /// ゲーム開始直後に呼び出される
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator Start() 
        {
            //現在のゲームの状態を待機状態に設定
            currentGameState = GameState.Wait;

            //各コントローラーの初期設定を行う
            SetUpControllers();

            //ScoreManagerの初期設定を行う
            scoreManager.SetUpScoreManager(ballController);

            //ゲームスタート演出が終わるまで待つ
            yield return uiManager.PlayGamePerform(PerformType.GameStart);

            // コントローラーの活性化
            ActivateControllers(true);

            //BGMを再生
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);

            //TODO:UniRx確認
            {
                // ReactiveProperty の利用しやすい状況は、MVP パターンと呼ばれる、UI の表示更新にかかわる処理
                // 値(得点)を購読(監視)することで、自動的に UI の表示更新を行うようなイベント処理を作り出すことができる

                // GameManager は Presenter という役割をもつ
                // Presenter とは、Model である ScoreManager と、View である UIManager の双方を知っていて、それをつなぐ役目をになう

                // ScoreManager に用意してある ReactiveProperty(Player と Enemy の得点を管理する値) をここで購読(監視)する命令を出す
                // いずれかの値が更新された場合、Subscribe メソッド内にある処理を自動的に実行する
                // 今回は値が更新されるたび、UIManager のメソッドを実行し、双方の値の情報を引数を使って提供する

                // こうすることで、Presenter 役である GameManager のみが、ScoreManager と UIManager を知っている状況(疎結合)でありながら
                // 値の更新に合わせて、画面の表示更新も一緒に連動して処理を行うことができる
                // よって、現在のように、値の更新に合わせてその都度、画面表示更新の命令を行う必要がなくなる


                // ReactiveProperty を購読　その①(始めに覚える方法)
                // Model として ScoreManager の代わりに GameData を利用しているが、これは、元の UpdateTxtScore メソッドの処理を活かしているため(メソッドに引数がないため)
                GameData.instance.PlayerScore.Subscribe(_ => uiManager.PrepareUpdateTxtScore()).AddTo(this);
                GameData.instance.EnemyScore.Subscribe(_ => uiManager.PrepareUpdateTxtScore()).AddTo(this);


                // ReactiveProperty を購読　その②(難しいが、覚えたい方法。①の処理をコメントアウトすれば、同じように正常に動きます)
                //Observable.CombineLatest(scoreManager.PlayerScore, scoreManager.EnemyScore, (playerScore, enemyScore) => (playerScore, enemyScore))
                //    .Subscribe(scores => uiManager.UpdateDisplayScoreObservable(scores.playerScore, scores.enemyScore))
                //   .AddTo(this);


                // まずは、ReactiveProperty を利用した MVP パターンによる、値と UI 表示更新の連動をしっかりと覚えること
                // ただし、MVP パターンは UI にのみ使うようにすること
                // Subscribe や AddTo といったメソッドの機能をしっかりと理解すること

                // プログラムには絶対な書き方はないので、UniRx においても、あくまでも、スキルの引き出しの幅を広げるものであると考えること
                // 頭でっかちにならないようにする。柔軟な思考を忘れない
                // ReactiveProperty 自体は色々な処理に応用可能だが、なんでもかんでも利用する、ということではない
                // 便利な機能であり、処理を書ける幅が広がるが、先ほども言っているように、すべてにおいて有効というわけではない(読み解けない人もいる)

                // 上記の実装例は、まずは、①の方で色々な処理を書いてみて、どういった処理が動くのかを試して、スラスラと書けるレベルにすることを目標にする

                // その後、②の処理の内容を理解していくようにする
                // ネットなどにも実装例はあるものの、自分のプロジェクトに落とし込んだものは絶対に見つからないので、処理の動きを覚えること
            }

            //現在のゲームの状態を試合中に設定
            currentGameState = GameState.Play;
            
            //ボールの位置の監視を開始する
            StartCoroutine(CheckBallPosition());
        }

        /// <summary>
        /// 各コントローラーの初期設定を行う
        /// </summary>
        private void SetUpControllers() 
        {
            //登録されているコントローラーの数だけ繰り返す
            for (int i = 0; i < controllersList.Count; i++) 
            {
                //ControllerBaseの初期設定を行う    
                controllersList[i].SetUpControllerBase(ballController);

                //まだPlayerControllerを取得していないかつ、PlayerControllerの取得に成功したら
                if (playerController == null && controllersList[i].TryGetComponent(out playerController)) 
                {
                    //結果を報告
                    Debug.Log("PlayerControllerの取得に成功");
                }
            }

            //各コントローラーを非活性化する
            ActivateControllers(false);
        }

        /// <summary>
        /// コントローラーの活性化・非活性化を切り替える
        /// </summary>
        /// <param name="isSwitch">活性化or非活性化</param>
        private void ActivateControllers(bool isSwitch) 
        {
            //コントローラーのリストの要素数だけ繰り返す
            for (int i = 0; i < controllersList.Count; i++)
            {
                //取得した要素の活性化・非活性化を切り替える
                controllersList[i].enabled = isSwitch;
            }
        }

        /// <summary>
        /// ゲーム終了演出を行う
        /// </summary>
        /// <param name="isGameOverPerformance">行うゲーム終了演出がゲームオーバー演出かどうか</param>
        /// <returns>待ち時間</returns>
        private IEnumerator PlayGameEndPerformance(bool isGameOverPerformance) 
        {
            //全ての音が完全に止まるまで待つ
            yield return new WaitForSeconds(GameData.instance.FadeOutTime);

            //効果音を再生
            SoundManager.instance.PlaySound(isGameOverPerformance ? SoundDataSO.SoundName.GameOverSE : SoundDataSO.SoundName.PlayerPointSE);

            //ゲーム終了演出を行う
            yield return uiManager.PlayGamePerform(isGameOverPerformance ? PerformType.GameOver : PerformType.GameClear);

            //メインシーンを読み込む
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// ゲーム終了の準備を行う
        /// </summary>
        private void PrepareGameEnd() 
        {
            //各コントローラーを非活性化する
            ActivateControllers(false);

            //全ての音を停止する
            SoundManager.instance.StopSound();
        }

        private int count;//サーブの回数の記録用

        private OwnerType server;//サーバー保持用

        /// <summary>
        /// ボールの位置を監視する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator CheckBallPosition() 
        {　　　
            //ゲームの状態が試合中なら繰り返す（ゲームプレイ中の間だけ監視する）
            while (currentGameState == GameState.Play) 
            {
                //ボールが床に落ちたなら
                if (ballController.transform.position.y <= 0.25f) 
                {
                    //ゲームの状態を待機状態に設定
                    currentGameState = GameState.Wait;
                }

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }

            //1セット終了時の処理を行う
            EndOneSet();
        }

        /// <summary>
        /// 1セット終了時の処理を行う
        /// </summary>
        private void EndOneSet() 
        {           
            //得点の記録を更新
            scoreManager.UpdateScore(scoreManager.GetUpadateValue());

            //サーバーを取得して、ボールの動きを止める
            ballController.PrepareRestartGame(GetAppropriatServer());

            //プレーヤーを初期位置に移動させる
            playerController.ResetPlayerPos();

            //プレイヤーの得点か、エネミーの得点が、最高得点なら
            if (GameData.instance.score.playerScore >= GameData.instance.MaxScore || GameData.instance.score.enemyScore >= GameData.instance.MaxScore)
            {
                //現在のゲームの状態をゲーム終了状態に設定
                currentGameState = GameState.GameUp;

                //ゲーム終了の準備を行う
                PrepareGameEnd();

                //ゲーム終了演出を行う
                StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));
            } 
            //プレーヤーの得点も、エネミーの得点も、最高得点より小さいなら
            else 
            {
                //現在のゲームの状態を試合中に設定
                currentGameState = GameState.Play;

                //ボールの位置の監視を開始
                StartCoroutine(CheckBallPosition());
            }

            /// <summary>
            /// 適切なサーバーを取得する（ボールが落下した際に呼び出だされる）
            /// </summary>
            /// <returns>適切なサーバー</returns>
            OwnerType GetAppropriatServer() 
            {
                //サーブ回数を記録
                count++;

                //まだ2本サーブを打っていないなら
                if (count < 2) 
                {
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