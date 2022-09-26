using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<ControllerBase> controllersList = new();//コントローラーのリスト

    [SerializeField]
    private BallController ballController;//BallController

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //各コントローラーの初期設定を行う
        SetUpControllers();
    }

    /// <summary>
    /// 各コントローラーの初期設定を行う
    /// </summary>
    private void SetUpControllers()
    {
        //登録されているコントローラーの数だけ繰り返す
        for(int i = 0; i < controllersList.Count; i++)
        {
            //ControllerBaseの初期設定を行う    
            controllersList[i].SetUpControllerBase();

            //PlayerControllerを取得出来たら
            if (controllersList[i].TryGetComponent(out PlayerController playerController))
            {
                //PlayerControllerの初期設定を行う
                playerController.SetUpPlayerController();
            }
            //EnemyControllerを取得できたら
            else if(controllersList[i].TryGetComponent(out EnemyController enemyController))
            {
                //EnemyControllerの初期設定を行う
                enemyController.SetUpEnemyController(ballController);
            }
        }
    }
}
