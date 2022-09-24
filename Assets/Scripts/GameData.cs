using System.Collections;
using System.Collections.Generic;
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

    [SerializeField,Header("���P�b�g��U�鋗��")]
    private float swingLength;//���P�b�g��U�鋗��

    [SerializeField,Header("���P�b�g��U�鎞��")]
    private float swingTime;//���P�b�g��U�鎞��

    /// <summary>
    /// �u���P�b�g���\����̂ɗv���鎞�ԁv�̎擾�p
    /// </summary>
    public float PrepareRacketTime { get => prepareRacketTime; }

    /// <summary>
    /// �u���P�b�g��U�鋗���v�̎擾�p
    /// </summary>
    public float SwingLength { get => swingLength; }

    /// <summary>
    /// �u���P�b�g��U�鎞�ԁv�̎擾�p
    /// </summary>
    public float SwingTime { get => swingTime; }
}
