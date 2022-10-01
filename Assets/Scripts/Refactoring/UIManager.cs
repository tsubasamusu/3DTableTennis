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

        private bool isUIEffect;//演出終了判定用

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
            isUIEffect = false;

            //得点に関する処理
            {
                //ゲームクリア演出なら
                if (performType == PerformType.GameClear)
                {
                    //得点のテキストを一定時間かけて青色に変える
                    txtScore.DOColor(Color.blue, 2f);
                }
                //ゲームクリア演出ではないなら
                else
                {
                    //得点のキャンバスグループを非表示にする
                    cgScore.alpha = 0f;
                }
            }

            //背景に関する処理
            {
                //ゲームオーバー演出なら
                if (performType == PerformType.GameOver)
                {
                    //背景を黒色に設定
                    imgBackground.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
                }
                //ゲームオーバー演出ではないなら
                else
                {
                    //背景を白色に設定
                    imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);
                }
            }

            //ロゴのスプライトを設定
            imgLogo.sprite = GetLogoSprite(performType);

            //ボタンに登録されている処理を削除
            button.onClick.RemoveAllListeners();

            //ボタンに処理を追加
            button.onClick.AddListener(() => ClickedButton(performType));

            //Sequenceを作成
            Sequence sequence = DOTween.Sequence();

            //演出の種類によって処理を変更
            switch (performType)
            {
                case PerformType.GameStart://ゲームスタート演出なら
                    imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);//ボタンの色を設定
                    txtButton.text = "Start";//ボタンのテキストを設定
                    {
                        sequence.Append(imgLogo.DOFade(0f, 0f));
                        sequence.Append(imgLogo.DOFade(1f, 1f));
                        sequence.Append(imgButton.DOFade(1f, 1f));
                        sequence.Join(cgButton.DOFade(1f, 1f))
                            .OnComplete(() => button.interactable = true).SetLink(gameObject);
                    }
                    break;

                case PerformType.GameOver://ゲームオーバー演出なら
                    imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);//ボタンの色を設定
                    txtButton.text = "Restart";//ボタンのテキストを設定
                    {
                        sequence.Append(imgLogo.DOFade(0f, 0f));
                        sequence.Append(imgBackground.DOFade(1f, 1f));
                        sequence.Append(imgLogo.DOFade(1f, 1f));
                        sequence.Append(imgButton.DOFade(1f, 1f));
                        sequence.Join(cgButton.DOFade(1f, 1f))
                            .OnComplete(() => button.interactable = true);
                    }
                    break;

                case PerformType.GameClear://ゲームクリア演出なら
                    imgButton.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);//ボタンの色を設定
                    txtButton.text = "Restart";//ボタンのテキストを設定
                    {
                        sequence.Append(imgLogo.DOFade(0f, 0f));
                        sequence.Append(imgBackground.DOFade(1f, 1f));
                        sequence.Append(imgLogo.DOFade(1f, 1f));
                        sequence.Append(imgButton.DOFade(1f, 1f));
                        sequence.Join(cgButton.DOFade(1f, 1f))
                            .OnComplete(() => button.interactable = true);
                    }
                    break;
            }

            //ボタンを非活性化する
            button.interactable = false;

            //演出が終了するまで待つ
            yield return new WaitUntil(() => isUIEffect == true);

            //まだ演出が終了していない状態に切り替える
            isUIEffect = false;
        }

        /// <summary>
        /// ボタンが押された際の処理
        /// </summary>
        /// <param name="performType">演出の種類</param>
        private void ClickedButton(PerformType performType)
        {
            //ボタンを非活性化する
            button.interactable = false;

            //ゲームスタート演出なら
            if (performType == PerformType.GameStart)
            {
                //効果音を再生
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameStartSE);
            }
            //ゲームスタート演出ではないなら
            else
            {
                //効果音を再生
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);
            }

            //演出の種類に応じて処理を変更
            switch (performType)
            {
                //ゲームスタート演出なら、背景のイメージをを一定時間かけて非表示にする
                case PerformType.GameStart: imgBackground.DOFade(0f, 1f); break;

                //ゲームオーバー演出なら、背景のイメージを一定時間かけて白色に変化させる
                case PerformType.GameOver: imgBackground.DOColor(Color.white, 1f); break;

                //ゲームクリア演出なら
                case PerformType.GameClear: cgScore.DOFade(0f, 1f); break;
            }

            //ロゴを一定時間かけて非表示にする
            imgLogo.DOFade(0f, 1f);

            //ボタンのキャンバスグループを一定時間かけて非表示にする
            cgButton.DOFade(0f, 1f)

                //演出が終了した状態に切り替える
                .OnComplete(() => isUIEffect = true);
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
            yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreTime);

            //プレイヤーが勝利したら
            if (GameData.instance.score.playerScore == GameData.instance.MaxScore)
            {
                //以降の処理を行わない
                yield break;
            }

            //得点のキャンバスグループを一定時間かけて非表示にする
            cgScore.DOFade(0f, 0.25f);
        }
    }
}