using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用
using UnityEngine.UI;//UIを使用

/// <summary>
/// UIを制御する
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// ロゴの種類
    /// </summary>
    private enum LogoType
    {
        Title,GameOver,GameClear//列挙子
    }

    /// <summary>
    /// ロゴのデータを管理する 
    /// </summary>
    [Serializable]
    private class LogoData
    {
        public LogoType LogoType;//ロゴの種類
        public Sprite sprite;//スプライト
    }

    [SerializeField]
    private List<LogoData> logoDatasList = new();//ロゴのデータのリスト

    [SerializeField]
    private Image imgLogo;//ロゴのイメージ

    [SerializeField]
    private Image imgBackground;//背景のイメージ

    [SerializeField]
    private CanvasGroup cvScore;//得点のキャンバスグループ

    [SerializeField]
    private Text txtScore;//得点のテキスト

    [SerializeField]
    private Button button;//ボタン

    [SerializeField]
    private Image imgButton;//ボタンのイメージ

    [SerializeField]
    private Text txtButton;//ボタンのテキスト

    /// <summary>
    /// ロゴのスプライトを取得する
    /// </summary>
    /// <param name="logoType">ロゴの種類</param>
    /// <returns>ロゴのスプライト</returns>
    private Sprite GetLogoSprite(LogoType logoType)
    {
        //ロゴのスプライトを返す
        return logoDatasList.Find(x => x.LogoType == logoType).sprite;
    }
}
