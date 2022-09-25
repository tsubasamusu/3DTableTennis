using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレーヤーの行動を制御する
/// </summary>
public class PlayerController : ControllerBase
{
    private Transform mainCameraTran;//メインカメラの位置情報

    /// <summary>
    /// PlayerControllerの初期設定を行う
    /// </summary>
    protected override void SetUpController()
    {
        //メインカメラの位置情報を取得
        mainCameraTran = Camera.main.transform;
    }

    /// <summary>
    /// 移動方向を取得する
    /// </summary>
    protected override Vector3 GetMoveDir()
    {
        //左右移動の入力を取得
        float moveH = Input.GetAxis("Horizontal");

        //前後移動の入力を取得
        float moveV = Input.GetAxis("Vertical");

        //仮の移動方向を設定
        Vector3 movement = new Vector3(moveH, 0, moveV);

        //移動方向を取得し、返す
        return mainCameraTran.forward * movement.z + mainCameraTran.right * movement.x;
    }
}
