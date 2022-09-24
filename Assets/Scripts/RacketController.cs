using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

/// <summary>
/// ���P�b�g�̓����𐧌䂷��
/// </summary>
public class RacketController : MonoBehaviour
{
    private Vector3 normalLocalPos;//�ʏ펞�̃��P�b�g�̍��W

    private Vector3 normalLocalRot;//�ʏ펞�̃��P�b�g�̊p�x

    private bool isIdle;//���P�b�g��U���Ă��Ȃ����ǂ���

    /// <summary>
    /// �u���P�b�g��U���Ă��Ȃ����ǂ����v�̔���̎擾�p
    /// </summary>
    public bool IsIdle { get => isIdle; }

    /// <summary>
    /// RacketController�̏����ݒ���s��
    /// </summary>
    public void SetUpRacketController()
    {
        //�ʏ펞�̃��P�b�g�̍��W���擾
        normalLocalPos = transform.localPosition;

        //�ʏ펞�̃��P�b�g�̊p�x���擾
        normalLocalRot = transform.localEulerAngles;

        //���P�b�g��U���Ă��Ȃ���Ԃɐ؂�ւ���
        isIdle = true;
    }

    /// <summary>
    /// ���P�b�g����{��Ԃɖ߂�
    /// </summary>
    public void SetNormalCondition()
    {
        //���P�b�g����{�ʒu�Ɉړ�������
        transform.DOLocalMove(normalLocalPos, GameData.instance.PrepareRacketTime);

        //���P�b�g����{�p�x�ɒ���
        transform.DOLocalRotate(normalLocalRot, GameData.instance.PrepareRacketTime)

            //���P�b�g��U���Ă��Ȃ���Ԃɐ؂�ւ���
            .OnComplete(() => isIdle = true);
    }

    /// <summary>
    /// �h���C�u����
    /// </summary>
    /// <param name="isForehandDrive">�t�H�A�n���h�h���C�u���ǂ���</param>
    public void Drive(bool isForehandDrive)
    {
        //���P�b�g��U���Ă����Ԃɐ؂�ւ���
        isIdle = false;

        //�����ʒu���擾
        Vector3 prepareLocalPos = isForehandDrive ? new Vector3(1f, 0f, 0f) : new Vector3(0.8f, 0f, 1f);

        //�����p�x���擾
        Vector3 prepareLocalRot = isForehandDrive ? new Vector3(30f, 0f, 270f) : new Vector3(330f, 180f, 270f);

        //���P�b�g�������ʒu�Ɉړ�������
        transform.DOLocalMove(prepareLocalPos, GameData.instance.PrepareRacketTime);

        //���P�b�g�������p�x�ɒ���
        transform.DOLocalRotate(prepareLocalRot, GameData.instance.PrepareRacketTime)

            //���P�b�g��U��
            .OnComplete(() => transform.DOLocalMoveX(GameData.instance.SwingLength, GameData.instance.SwingTime))

            //���P�b�g��U���Ă��Ȃ���Ԃɐ؂�ւ���
            .OnComplete(() => isIdle = true);
    }
}