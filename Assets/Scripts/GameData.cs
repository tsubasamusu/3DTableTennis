using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体のデータを管理する
/// </summary>
public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    private void Awake()
    {
        //以下、シングルトンに必須の記述
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField,Header("ラケットを構えるのに要する時間")]
    private float prepareRacketTime;//ラケットを構えるのに要する時間

    [SerializeField,Header("ラケットを振る時間")]
    private float swingTime;//ラケットを振る時間

    [SerializeField,Header("移動速度")]
    private float moveSpeed;//移動速度

    [SerializeField, Header("重力")]
    private float gravity;//重力

    [SerializeField,Header("ボールの速さ")]
    private float ballSpeed;//ボールの速さ

    /// <summary>
    /// 「ラケットを構えるのに要する時間」の取得用
    /// </summary>
    public float PrepareRacketTime { get => prepareRacketTime; }

    /// <summary>
    /// 「ラケットを振る時間」の取得用
    /// </summary>
    public float SwingTime { get => swingTime; }

    /// <summary>
    /// 移動速度の取得用
    /// </summary>
    public float MoveSpeed { get => moveSpeed; }

    /// <summary>
    /// 重力取得用
    /// </summary>
    public float Gravity { get => gravity; }

    /// <summary>
    /// 「ボールの速さ」の取得用
    /// </summary>
    public float BallSpeed { get => ballSpeed; }
}
