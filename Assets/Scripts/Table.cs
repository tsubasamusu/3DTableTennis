using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卓球台に関する処理を行う
/// </summary>
public class Table : MonoBehaviour
{
    /// <summary>
    /// 他のコライダーに接触した際に呼び出される
    /// </summary>
    /// <param name="other">接触相手</param>
    private void OnTriggerEnter(Collider other)
    {
        //接触相手がボールなら
        if (other.TryGetComponent(out BallController ballController))
        {
            //ボールが跳ねた状態に切り替える
            ballController.IsBounded = true;
        }
    }
}
