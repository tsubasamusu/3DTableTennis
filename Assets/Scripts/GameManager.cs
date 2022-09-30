using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用
using UnityEngine;

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

    private EnemyController enemyController;//EnemyController

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Start()
    {
        //各コントローラーの初期設定を行う
        SetUpControllers();

        //ScoreManagerの初期設定を行う
        scoreManager.SetUpScoreManager(ballController, uiManager);

        //ゲームスタート演出が終わるまで待つ
        yield return uiManager.PlayGameStart();

        //PlayerControllerとEnemyControllerを活性化する
        playerController.enabled = enemyController.enabled = true;

        //BGMを再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);
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
            controllersList[i].SetUpControllerBase();

            //PlayerControllerを取得出来たら
            if (controllersList[i].TryGetComponent(out PlayerController playerController))
            {
                //PlayerControllerを取得
                this.playerController = playerController;

                //PlayerControllerの初期設定を行う
                playerController.SetUpPlayerController();

                //BallControllerの初期設定を行う
                ballController.SetUpBallController(playerController);
            }
            //EnemyControllerを取得できたら
            else if (controllersList[i].TryGetComponent(out EnemyController enemyController))
            {
                //EnemyControllerを取得
                this.enemyController = enemyController;

                //EnemyControllerの初期設定を行う
                enemyController.SetUpEnemyController(ballController);
            }
        }

        //PlayerControllerとEnemyControllerを非活性化する
        playerController.enabled = enemyController.enabled = false;
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
        yield return isGameOverPerformance ? uiManager.PlayGameOver() : uiManager.PlayGameClear();

        //メインシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ゲーム終了の準備を行う
    /// </summary>
    private void PrepareGameEnd()
    {
        //PlayerControllerとEnemyControllerを非活性化する
        playerController.enabled = enemyController.enabled = false;

        //全ての音を停止する
        SoundManager.instance.StopSound();
    }

    //TODO:リファクタリングの段階で、以降の処理をUniRxを使用して書き換える

    private bool flag;//重複処理防止用

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //マッチポイントを取得
        int matchPoints = GameData.instance.MaxScore - 1;

        //プレイヤーの得点も、エネミーの得点もマッチポイント以下なら
        if (GameData.instance.score.playerScore <= matchPoints && GameData.instance.score.enemyScore <= matchPoints)
        {
            //以降の処理を行わない
            return;
        }

        //flagがfalseなら
        if (!flag)
        {
            //ゲーム終了の準備を行う
            PrepareGameEnd();

            //ゲーム終了演出を行う
            StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));

            //flagにtrueを入れる
            flag = true;
        }
    }
}

