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

    [SerializeField,Header("ラケットを振る距離")]
    private float swingLength;//ラケットを振る距離

    [SerializeField,Header("ラケットを振る時間")]
    private float swingTime;//ラケットを振る時間

    /// <summary>
    /// 「ラケットを構えるのに要する時間」の取得用
    /// </summary>
    public float PrepareRacketTime { get => prepareRacketTime; }

    /// <summary>
    /// 「ラケットを振る距離」の取得用
    /// </summary>
    public float SwingLength { get => swingLength; }

    /// <summary>
    /// 「ラケットを振る時間」の取得用
    /// </summary>
    public float SwingTime { get => swingTime; }
}
