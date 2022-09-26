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
    public void SetUpScoreManager(BallController ballController)
    {
        //得点の更新の確認を開始する
        StartCoroutine(CheckScore());

        //得点の更新を確認する
        IEnumerator CheckScore()
        {
            //無限に繰り返す
            while(true)
            {
                //ボールが床に落ちていないなら
                if (ballController.transform.position.y > 0.25f)
                {
                    //次のフレームへ飛ばす（実質、Updateメソッド）
                    yield return null;

                    //次の繰り返し処理へ移る
                    continue;
                }

                //得点を更新
                UpdateScore(ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0));

                //ボールの動きを止める
                ballController.StopBall(GetAppropriatServer(ballController));

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }
    }

    /// <summary>
    /// 適切なサーバーを取得する（ボールが落下した際に呼び出だされる）
    /// </summary>
    /// <param name="ballController">BallController</param>
    /// <returns>適切なサーバー</returns>
    private OwnerType GetAppropriatServer(BallController ballController)
    { 
        //サーブ回数を記録
        count++;

        //まだ2本サーブを打っていないなら
        if (count<2)
        {
            //サーバーを変更しない
            return server;
        }

        //サーブ回数を初期化
        count = 0;

        //サーバーを変えて、記録する
        return server= server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;
    }

    /// <summary>
    /// 得点を更新する
    /// </summary>
    /// <param name="value">更新量</param>
    private void UpdateScore((int playerValue,int enemyValue) value)
    {
        //得点を更新
        GameData.instance.score = value;
    }
}
