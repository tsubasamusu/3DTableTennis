using System.Collections.Generic;//リストを使用 
using System;//Serializable属性を使用
using UnityEngine; 

//アセットメニューで「Create SoundDataSO」を選択すると「SoundDataSO」を作成できる 
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>  
    /// 音の名前  
    /// </summary>  
    public enum SoundName
    {
        MainBGM,//メインBGM
        GameStartSE,//ゲームスタート音
        GameOverSE,//ゲームオーバー音
        GameClearSE,//ゲームクリア音
        PlayerPointSE,//プレイヤーが得点した時の音
        EnemyPointSE,//エネミーが得点した時の音
        RacketSE,//ボールがラケットに当たった時の音
        BoundSE,//ボールが卓球台でバウンドした時の音
    }

    /// <summary>  
    /// 音のデータを管理する
    /// </summary>  
    [Serializable]
    public class SoundData
    {
        public SoundName name;//名前 
        public AudioClip clip;//クリップ
    }

    public List<SoundData> soundDataList = new();//音のデータのリスト 
}