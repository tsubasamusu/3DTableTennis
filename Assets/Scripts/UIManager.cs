using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用

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
        Title, GameOver, GameClear//列挙子
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
    private CanvasGroup cgScore;//得点のキャンバスグループ

    [SerializeField]
    private Text txtScore;//得点のテキスト

    [SerializeField]
    private CanvasGroup cgButton;//ボタンのキャンバスグループ

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

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameStart()
    {
        //ゲームスタート演出終了判定用
        bool end = false;

        //背景を白色に設定
        imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);

        //ロゴをタイトルに設定
        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        //ボタンを青色に設定
        imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);

        //ボタンのテキストを「Start」に設定
        txtButton.text = "Start";

        //ボタンが押された際の処理を設定
        button.onClick.AddListener(() => ClickedButton());

        //ボタンを非活性化する
        button.interactable = false;

        //ロゴを非表示にする
        imgLogo.DOFade(0f, 0f)

            //ロゴを一定時間かけて表示する
            .OnComplete(() => imgLogo.DOFade(1f, 1f)

            .OnComplete(() =>
            {
                //ボタンのイメージを一定時間かけて表示する
                { imgButton.DOFade(1f, 1f); }

                {
                    //ボタンのキャンバスグループを一定時間かけて表示する
                    cgButton.DOFade(1f, 1f)

                    //ボタンを活性化する
                    .OnComplete(() => button.interactable = true);
                }

            }));

        //ボタンが押された際の処理
        void ClickedButton()
        {
            //背景を一定時間かけて非表示にする
            imgBackground.DOFade(0f, 1f);

            //ロゴを一定時間かけて非表示にする
            imgLogo.DOFade(0f, 1f);

            //ボタンのキャンバスグループを一定時間かけて非表示にする
            cgButton.DOFade(0f, 1f)

                //ゲームスタート演出が終了した状態に切り替える
                .OnComplete(()=>end=true);

            //ボタンを非活性化する
            button.interactable = false;
        }

        //ゲームスタート演出が終わるまで待つ
        yield return new WaitUntil(() => end == true);
    }

    /// <summary>
    /// ゲームオーバー演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameOver()
    {
        //ゲームオーバー演出終了判定用
        bool end = false;

        //背景を黒色に設定
        imgBackground.color = new Color(Color.black.r,Color.black.g,Color.black.b,0f);

        //ロゴをゲームオーバーに設定
        imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

        //ボタンを赤色に設定
        imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);

        //ボタンのテキストを「Restart」に設定
        txtButton.text = "Restart";

        //ボタンが押された際の処理を設定
        button.onClick.AddListener(() => ClickedButton());

        //ボタンを非活性化する
        button.interactable = false;

        //背景を一定時間かけて表示する
        imgBackground.DOFade(1f,1f)

        //ロゴを非表示にする
        .OnComplete(()=> imgLogo.DOFade(0f, 0f)

            //ロゴを一定時間かけて表示する
            .OnComplete(() => imgLogo.DOFade(1f, 1f)

            .OnComplete(() =>
            {
                //ボタンのイメージを一定時間かけて表示する
                { imgButton.DOFade(1f, 1f); }

                {
                    //ボタンのキャンバスグループを一定時間かけて表示する
                    cgButton.DOFade(1f, 1f)

                    //ボタンを活性化する
                    .OnComplete(() => button.interactable = true);
                }

            })));

        //ボタンが押された際の処理
        void ClickedButton()
        {
            //背景を一定時間かけて白色にする
            imgBackground.DOColor(Color.white, 1f);

            //ロゴを一定時間かけて非表示にする
            imgLogo.DOFade(0f, 1f);

            //ボタンのキャンバスグループを一定時間かけて非表示にする
            cgButton.DOFade(0f, 1f)

                //ゲームスタート演出が終了した状態に切り替える
                .OnComplete(() => end = true);

            //ボタンを非活性化する
            button.interactable = false;
        }

        //ゲームオーバー演出が終わるまで待つ
        yield return new WaitUntil(() => end == true);
    }
}
