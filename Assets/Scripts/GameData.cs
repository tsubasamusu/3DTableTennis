using UnityEngine;

/// <summary>
/// �Q�[���S�̂̃f�[�^���Ǘ�����
/// </summary>
public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȉ��A�V���O���g���ɕK�{�̋L�q
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField,Header("���P�b�g���\����̂ɗv���鎞��")]
    private float prepareRacketTime;//���P�b�g���\����̂ɗv���鎞��

    [SerializeField,Header("���P�b�g��U�鎞��")]
    private float swingTime;//���P�b�g��U�鎞��

    [SerializeField,Header("�ړ����x")]
    private float moveSpeed;//�ړ����x

    [SerializeField, Header("�d��")]
    private float gravity;//�d��

    [SerializeField,Header("�{�[���̑���")]
    private float ballSpeed;//�{�[���̑���

    [SerializeField,Header("�G�l�~�[�̍U������")]
    private float enemyShotRange;//�G�l�~�[�̍U������

    [SerializeField,Header("�G�l�~�[���T�[�u��ł܂ł̎���")]
    private float enemyServeTime;//�G�l�~�[���T�[�u��ł܂ł̎���

    [SerializeField,Header("���_��\�����鎞��")]
    private float displayScoreTime;

    [SerializeField,Header("���̃t�F�[�h�A�E�g����")]
    private float fadeOutTime;//���̃t�F�[�h�A�E�g����

    [SerializeField, Header("�ō����_")]
    private int maxScore;//�ō����_

    [HideInInspector]
    public (int playerScore,int enemyScore) score;//���_

    /// <summary>
    /// �u���P�b�g���\����̂ɗv���鎞�ԁv�̎擾�p
    /// </summary>
    public float PrepareRacketTime { get => prepareRacketTime; }

    /// <summary>
    /// �u���P�b�g��U�鎞�ԁv�̎擾�p
    /// </summary>
    public float SwingTime { get => swingTime; }

    /// <summary>
    /// �ړ����x�̎擾�p
    /// </summary>
    public float MoveSpeed { get => moveSpeed; }

    /// <summary>
    /// �d�͎擾�p
    /// </summary>
    public float Gravity { get => gravity; }

    /// <summary>
    /// �u�{�[���̑����v�̎擾�p
    /// </summary>
    public float BallSpeed { get => ballSpeed; }

    /// <summary>
    /// �u�G�l�~�[�̍U�������v�̎擾�p
    /// </summary>
    public float EnemyShotRange { get => enemyShotRange; }

    /// <summary>
    /// �u�G�l�~�[���T�[�u��ł܂ł̎��ԁv�̎擾�p
    /// </summary>
    public float EnemyServeTime { get => enemyServeTime; }

    /// <summary>
    /// �u���_��\�����鎞�ԁv�̎擾�p
    /// </summary>
    public float DisplayScoreTime { get => displayScoreTime; }

    /// <summary>
    /// �u���̃t�F�[�h�A�E�g���ԁv�̎擾�p
    /// </summary>
    public float FadeOutTime { get => fadeOutTime; }

    /// <summary>
    /// �ō����_�擾�p
    /// </summary>
    public int MaxScore { get => maxScore; }
}
