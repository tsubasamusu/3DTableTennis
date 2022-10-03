using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using System;//Serializable属性を使用
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用
using UnityEngine;

namespace yamap
{
    /// <summary>
    /// UIを制御する
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// ロゴのデータを管理する 
        /// </summary>
        [Serializable]
        private class LogoData
        {
            public PerformType performType;//演出の種類
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

        [SerializeField]
        private Text txtMessage;//メッセージのテキスト

        /// <summary>
        /// ロゴのスプライトを取得する
        /// </summary>
        /// <param name="performType">演出の種類</param>
        /// <returns>ロゴのスプライト</returns>
        private Sprite GetLogoSprite(PerformType performType)
        {
            //ロゴのスプライトを返す
            return logoDatasList.Find(x => x.performType==performType).sprite;
        }

        /// <summary>
        /// ゲームの演出を行う
        /// </summary>
        /// <param name="performType">演出の種類</param>
        /// <returns>待ち時間</returns>
        public IEnumerator PlayGamePerform(PerformType performType)
        {
            //まだ演出が終了していない状態に切り替える
            bool isUIEffect = false;

            //メッセージのテキストを空にする
            txtMessage.text=String.Empty;

            //ロゴのスプライトを設定
            imgLogo.sprite = GetLogoSprite(performType);

            //ボタンに登録されている処理を削除
            button.onClick.RemoveAllListeners();

            //ボタンに処理を追加
            button.onClick.AddListener(() => ClickedButton());

            //ボタンが押された際の処理
            void ClickedButton()
            {
                //ボタンを非活性化する
                button.interactable = false;

                //演出の種類に応じて処理を変更
                switch (performType)
                {
                    case PerformType.GameStart://ゲームスタート演出なら
                        imgBackground.DOFade(0f, 1f);//背景を一定時間かけて非表示にする
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameStartSE);//効果音を再生
                        break;

                    case PerformType.GameOver://ゲームオーバー演出なら
                        imgBackground.DOColor(Color.white, 1f);//背景を一定時間かけて白色に変化させる
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);//効果音を再生
                        break;

                    case PerformType.GameClear://ゲームクリア演出なら
                        cgScore.DOFade(0f, 1f);//得点のキャンバスグループを一定時間かけて非表示にする
                        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);//効果音を再生
                        break;
                }

                //ロゴを一定時間かけて非表示にする
                imgLogo.DOFade(0f, 1f);

                //ボタンのキャンバスグループを一定時間かけて非表示にする
                cgButton.DOFade(0f, 1f)

                    //演出が終了した状態に切り替える
                    .OnComplete(() => isUIEffect = true);
            }

            //Sequenceを作成
            Sequence sequence = DOTween.Sequence();

            //演出の種類によって処理を変更
            switch (performType)
            {
                case PerformType.GameStart://ゲームスタート演出なら
                    imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);//背景の色を設定
                    cgScore.alpha = 0f;//得点のキャンバスグループを非表示に設定
                    imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);//ボタンの色を設定
                    txtButton.text = "Start";//ボタンのテキストを設定
                    break;

                case PerformType.GameOver://ゲームオーバー演出なら
                    imgBackground.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);//背景の色を設定
                    cgScore.alpha = 0f;//得点のキャンバスグループを非表示に設定
                    imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);//ボタンの色を設定
                    txtButton.text = "Restart";//ボタンのテキストを設定
                    break;

                case PerformType.GameClear://ゲームクリア演出なら
                    imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);//背景の色を設定
                    imgButton.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);//ボタンの色を設定
                    txtButton.text = "Restart";//ボタンのテキストを設定
                    break;
            }

            //演出を行う
            {
                if (performType == PerformType.GameClear) txtScore.DOColor(Color.blue, 2f);
                sequence.Append(imgLogo.DOFade(0f, 0f));
                if (performType != PerformType.GameStart) sequence.Append(imgBackground.DOFade(1f, 1f));
                sequence.Append(imgLogo.DOFade(1f, 1f));
                sequence.Append(imgButton.DOFade(1f, 1f));
                sequence.Join(cgButton.DOFade(1f, 1f))
                    .OnComplete(() => button.interactable = true).SetLink(gameObject);
            }

            //ボタンを非活性化する
            button.interactable = false;

            //演出が終了するまで待つ
            yield return new WaitUntil(() => isUIEffect == true);
        }

        /// <summary>
        /// 得点の表示を更新する準備を行う
        /// 従来の処理(UI 表示部分と、演出部分を分ける)
        /// </summary>
        public void PrepareUpdateTxtScore()
        {
            //得点のテキストを設定する
            txtScore.text = GameData.instance.score.playerScore.ToString() + ":" + GameData.instance.score.enemyScore.ToString();

            //得点の表示を更新する
            StartCoroutine(PlayScoreEffect());
        }

        /// <summary>
        /// 得点の表示を更新する（ReactivePropertyの購読により、値更新時にイベントとして自動的に実行される）
        /// </summary>
        /// <param name="playerScore">プレイヤーの得点</param>
        /// <param name="enemyScore">エネミーの得点</param>
        public void UpdateDisplayScoreObservable(int playerScore, int enemyScore)
        {
            //ReactivePropertyで購読している情報を受け取り、表示を更新する(View 側)
            txtScore.text = playerScore + " : " + enemyScore;

            //得点の表示を更新する
            StartCoroutine(PlayScoreEffect());
        }

        /// <summary>
        /// 得点更新時の演出を行う
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator PlayScoreEffect()
        {
            //得点のキャンバスグループを一定時間かけて表示する
            cgScore.DOFade(1f, 0.25f);

            //得点を一定時間、表示し続ける
            yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreAndMessageTime);

            //プレイヤーが勝利したら
            if (GameData.instance.score.playerScore == GameData.instance.MaxScore)
            {
                //以降の処理を行わない
                yield break;
            }

            //得点のキャンバスグループを一定時間かけて非表示にする
            cgScore.DOFade(0f, 0.25f);
        }

        /// <summary>
        /// メッセージを表示する準備を行う
        /// </summary>
        /// <param name="server">サーバー</param>
        public void PrepareDisplayMessage(OwnerType server)
        {
            //メッセージのテキストを設定
            txtMessage.text = server == OwnerType.Player ? "Your Serve" : "Enemy's Serve";

            //メッセージを表示する
            StartCoroutine(DisplayMessage());
        }

        /// <summary>
        /// メッセージを表示する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator DisplayMessage()
        {
            //メッセージのテキストを一定時間かけて表示する
            txtMessage.DOFade(1f, 0.25f);

            //メッセージを一定時間、表示し続ける
            yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreAndMessageTime);

            //メッセージのテキストを一定時間かけて非表示にする
            txtMessage.DOFade(0f, 0.25f);
        }
    }
}