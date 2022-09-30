using DG.Tweening;//DOTweenを使用
using UnityEngine;

/// <summary>
/// 音源を管理する
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;//インスタンス

    [SerializeField]
    private SoundDataSO soundDataSO;//SoundDataSO

    private AudioSource[] audioSources;//音再生用のAudioSourceの配列

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    void Awake()
    {
        //音再生用のAudioSourceの配列の要素数を設定
        audioSources = new AudioSource[soundDataSO.soundDataList.Count];

        //音再生用のAudioSourceの配列の要素数だけ繰り返す
        for (int i = 0; i < audioSources.Length; i++)
        {
            //AudioSorceコンポーネントを作成し、配列に格納する
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }

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

    /// <summary>
    /// 音を再生する
    /// </summary>
    /// <param name="name">音の名前</param>
    /// <param name="volume">音のボリューム</param>
    /// <param name="loop">繰り返すかどうか</param>
    public void PlaySound(SoundDataSO.SoundName name, float volume = 1f, bool loop = false)
    {
        //音再生用のAudioSourceの配列の要素を1つずつ取り出す
        foreach (AudioSource source in audioSources)
        {
            //取り出したAudioSourceが再生中ではない（使用されていない）なら
            if (source.isPlaying == false)
            {
                //音のクリップを取得して、登録する
                source.clip = soundDataSO.soundDataList.Find(x => x.name == name).clip;

                //音のボリュームを設定
                source.volume = volume;

                //繰り返すなら
                if (loop)
                {
                    //繰り返す
                    source.loop = true;
                }

                //音を再生する
                source.Play();

                //以降の処理を行わない
                return;
            }
        }
    }

    /// <summary>
    /// 全ての音を停止する
    /// </summary>
    public void StopSound()
    {
        //音再生用のAudioSourceの配列の要素を1つずつ取り出す
        foreach (AudioSource source in audioSources)
        {
            //音をフェードアウトさせる
            source.DOFade(0f, GameData.instance.FadeOutTime)

                //音のフェードアウトが終ったら
                .OnComplete(() =>
                {
                    //音を完全に止める
                    { source.Stop(); }

                    //取り出したAudioSourceのクリップを空にする
                    { source.clip = null; }
                });
        }
    }
}
