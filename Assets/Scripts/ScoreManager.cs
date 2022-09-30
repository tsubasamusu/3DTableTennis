using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 得点を管理する
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private int count;//サーブの回数の記録用

    private OwnerType server;//サーバー保持用

    /// <summary>
    /// ScoreManagerの初期設定を行う
    /// </summary>
    /// <param name="ballController">BallController</param>
    /// <param name="uIManager">UIManager</param>
    /// <param name="playerController">PlayerController</param>
    public void SetUpScoreManager(BallController ballController, UIManager uIManager,PlayerController playerController)
    {
        //得点の更新の確認を開始する
        StartCoroutine(CheckScore());

        //得点の更新を確認する
        IEnumerator CheckScore()
        {
            //無限に繰り返す
            while (true)
            {
                //ボールが床に落ちていないなら
                if (ballController.transform.position.y > 0.25f)
                {
                    //次のフレームへ飛ばす（実質、Updateメソッド）
                    yield return null;

                    //次の繰り返し処理へ移る
                    continue;
                }

                //得点の記録を更新
                UpdateScore(GetUpadateValue(ballController), uIManager);

                //ボールの動きを止める
                ballController.PrepareRestartGame(GetAppropriatServer(),playerController);

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }
    }

    /// <summary>
    /// 適切なサーバーを取得する（ボールが落下した際に呼び出だされる）
    /// </summary>
    /// <returns>適切なサーバー</returns>
    private OwnerType GetAppropriatServer()
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

    /// <summary>
    /// 得点の記録を更新する
    /// </summary>
    /// <param name="updateValue">更新量</param>
    /// <param name="uIManager">UIManager</param>
    private void UpdateScore((int playerUpdateValue, int enemyUpdateValue) updateValue, UIManager uIManager)
    {
        //効果音を再生
        SoundManager.instance.PlaySound(updateValue.playerUpdateValue > 0 ? SoundDataSO.SoundName.PlayerPointSE : SoundDataSO.SoundName.EnemyPointSE);

        //プレイヤーの得点を更新
        GameData.instance.score.playerScore += updateValue.playerUpdateValue;

        //エネミーの得点を更新
        GameData.instance.score.enemyScore += updateValue.enemyUpdateValue;

        //得点の表示の更新をする準備を行う
        uIManager.PrepareUpdateTxtScore();
    }

    /// <summary>
    /// 得点の更新量を取得する
    /// </summary>
    /// <param name="ballController">BallController</param>
    /// <returns>得点の更新量</returns>
    private (int playerUpdateValue, int enemyUpdateValue) GetUpadateValue(BallController ballController)
    {
        //コートに入ったかどうかで処理を変更
        return ballController.InCourt ?

            //ボールの所有者に応じて戻り値を変更
            ballController.CurrentOwner == OwnerType.Player ? (1, 0) : (0, 1)

            //ボールの所有者に応じて戻り値を変更
            : ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);
    }
}
