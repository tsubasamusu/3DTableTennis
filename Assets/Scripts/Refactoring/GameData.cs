using UniRx;//UniRxを使用
using UnityEngine;

namespace yamap 
{
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

        [SerializeField, Header("ラケットを構えるのに要する時間")]
        private float prepareRacketTime = 0.1f;//ラケットを構えるのに要する時間

        [SerializeField, Header("ラケットを振る時間")]
        private float swingTime = 0.2f;//ラケットを振る時間

        [SerializeField, Header("移動速度")]
        private float moveSpeed = 5.0f;//移動速度

        [SerializeField, Header("重力")]
        private float gravity = 10.0f;//重力

        [SerializeField, Header("ボールの速さ")]
        private float ballSpeed = 6.0f;//ボールの速さ

        [SerializeField, Header("エネミーの攻撃圏内")]
        private float enemyShotRange = 1.5f;//エネミーの攻撃圏内

        [SerializeField, Header("エネミーがサーブを打つまでの時間")]
        private float enemyServeTime = 2.0f;//エネミーがサーブを打つまでの時間

        [SerializeField, Header("得点とメッセージを表示する時間")]
        private float displayScoreAndMessageTime = 2.0f;

        [SerializeField, Header("音のフェードアウト時間")]
        private float fadeOutTime = 0.5f;//音のフェードアウト時間

        [SerializeField, Header("最高得点")]
        private int maxScore = 11;//最高得点

        [HideInInspector]
        public (int playerScore, int enemyScore) score;//得点

        //TODO:UniRx確認

        // ReactiveProperty　(Model)
        // その①の実装時に使う
        public ReactiveProperty<int> PlayerScore = new();
        public ReactiveProperty<int> EnemyScore = new();

        void Reset() {
            PlayerScore = new(0);
            EnemyScore = new(0);

            Debug.Log("Reset : GameData");
        }

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

        /// <summary>
        /// 「エネミーの攻撃圏内」の取得用
        /// </summary>
        public float EnemyShotRange { get => enemyShotRange; }

        /// <summary>
        /// 「エネミーがサーブを打つまでの時間」の取得用
        /// </summary>
        public float EnemyServeTime { get => enemyServeTime; }

        /// <summary>
        /// 「得点とメッセージを表示する時間」の取得用
        /// </summary>
        public float DisplayScoreAndMessageTime { get => displayScoreAndMessageTime; }

        /// <summary>
        /// 「音のフェードアウト時間」の取得用
        /// </summary>
        public float FadeOutTime { get => fadeOutTime; }

        /// <summary>
        /// 最高得点取得用
        /// </summary>
        public int MaxScore { get => maxScore; }
    }
}