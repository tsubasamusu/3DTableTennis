using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

/// <summary>
/// ラケットの動きを制御する
/// </summary>
public class RacketController : MonoBehaviour
{
    private Vector3 normalLocalPos;//通常時のラケットの座標

    private Vector3 normalLocalRot;//通常時のラケットの角度

    private bool isIdle;//ラケットを振っていないかどうか

    /// <summary>
    /// 「ラケットを振っていないかどうか」の判定の取得用
    /// </summary>
    public bool IsIdle { get => isIdle; }

    /// <summary>
    /// RacketControllerの初期設定を行う
    /// </summary>
    public void SetUpRacketController()
    {
        //通常時のラケットの座標を取得
        normalLocalPos = transform.localPosition;

        //通常時のラケットの角度を取得
        normalLocalRot = transform.localEulerAngles;

        //ラケットを振っていない状態に切り替える
        isIdle = true;
    }

    /// <summary>
    /// ラケットを基本状態に戻す
    /// </summary>
    public void SetNormalCondition()
    {
        //ラケットを基本位置に移動させる
        transform.DOLocalMove(normalLocalPos, GameData.instance.PrepareRacketTime);

        //ラケットを基本角度に直す
        transform.DOLocalRotate(normalLocalRot, GameData.instance.PrepareRacketTime)

            //ラケットを振っていない状態に切り替える
            .OnComplete(() => isIdle = true);
    }

    /// <summary>
    /// ドライブする
    /// </summary>
    /// <param name="isForehandDrive">フォアハンドドライブかどうか</param>
    public void Drive(bool isForehandDrive)
    {
        //ラケットを振っている状態に切り替える
        isIdle = false;

        //準備位置を取得
        Vector3 prepareLocalPos = isForehandDrive ? new Vector3(1f, 0f, 0f) : new Vector3(0.8f, 0f, 1f);

        //準備角度を取得
        Vector3 prepareLocalRot = isForehandDrive ? new Vector3(30f, 0f, 270f) : new Vector3(330f, 180f, 270f);

        //ラケットを準備位置に移動させる
        transform.DOLocalMove(prepareLocalPos, GameData.instance.PrepareRacketTime);

        //ラケットを準備角度に直す
        transform.DOLocalRotate(prepareLocalRot, GameData.instance.PrepareRacketTime)

            //ラケットを振る
            .OnComplete(() => transform.DOMove(transform.GetChild(0).transform.position, GameData.instance.SwingTime)

            //ラケットを基本状態に戻す
            .OnComplete(() => SetNormalCondition()));
    }
}
