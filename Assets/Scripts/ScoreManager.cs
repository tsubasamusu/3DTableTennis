using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 得点を管理する
/// </summary>
public class ScoreManager : MonoBehaviour
{
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

                //得点の更新量を取得
                (int, int) updateValue = ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);

                //得点を更新
                UpdateScore(updateValue.Item1, updateValue.Item2);

                //ボールの動きを止める
                ballController.StopBall();

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }
    }

    /// <summary>
    /// 得点を更新する
    /// </summary>
    /// <param name="playerValue">プレイヤーの得点の更新量</param>
    /// <param name="enemyValue">エネミーの得点の更新量</param>
    private void UpdateScore(int playerValue,int enemyValue)
    {
        //プレイヤーの得点を更新
        GameData.instance.score.playerScore+=playerValue;

        //エネミーの得点を更新
        GameData.instance.score.enemyScore+=enemyValue;
    }
}
