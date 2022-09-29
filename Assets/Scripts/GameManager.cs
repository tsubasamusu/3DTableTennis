using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
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

        //PlayerControllerを活性化する（プレイヤーの操作を許可する）
        playerController.enabled = true;
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

                //PlayerControllerを非活性化する
                playerController.enabled = false;

                //BallControllerの初期設定を行う
                ballController.SetUpBallController(playerController);
            }
            //EnemyControllerを取得できたら
            else if (controllersList[i].TryGetComponent(out EnemyController enemyController))
            {
                //EnemyControllerの初期設定を行う
                enemyController.SetUpEnemyController(ballController);
            }
        }
    }
}
