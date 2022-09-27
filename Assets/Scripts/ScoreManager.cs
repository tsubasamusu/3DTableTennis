using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���_���Ǘ�����
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private int count;//�T�[�u�̉񐔂̋L�^�p

    private OwnerType server;//�T�[�o�[�ێ��p

    /// <summary>
    /// ScoreManager�̏����ݒ���s��
    /// </summary>
    /// <param name="ballController">BallController</param>
    public void SetUpScoreManager(BallController ballController)
    {
        //���_�̍X�V�̊m�F���J�n����
        StartCoroutine(CheckScore());

        //���_�̍X�V���m�F����
        IEnumerator CheckScore()
        {
            //�����ɌJ��Ԃ�
            while(true)
            {
                //�{�[�������ɗ����Ă��Ȃ��Ȃ�
                if (ballController.transform.position.y > 0.25f)
                {
                    //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                    yield return null;

                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //���_���X�V
                UpdateScore(ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0));

                //�{�[���̓������~�߂�
                ballController.PrepareRestartGame(GetAppropriatServer(ballController));

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }
    }

    /// <summary>
    /// �K�؂ȃT�[�o�[���擾����i�{�[�������������ۂɌĂяo�������j
    /// </summary>
    /// <param name="ballController">BallController</param>
    /// <returns>�K�؂ȃT�[�o�[</returns>
    private OwnerType GetAppropriatServer(BallController ballController)
    { 
        //�T�[�u�񐔂��L�^
        count++;

        //�܂�2�{�T�[�u��ł��Ă��Ȃ��Ȃ�
        if (count<2)
        {
            //�T�[�o�[��ύX���Ȃ�
            return server;
        }

        //�T�[�u�񐔂�������
        count = 0;

        //�T�[�o�[��ς��āA�L�^����
        return server= server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;
    }

    /// <summary>
    /// ���_���X�V����
    /// </summary>
    /// <param name="value">�X�V��</param>
    private void UpdateScore((int playerValue,int enemyValue) value)
    {
        //���_���X�V
        GameData.instance.score = value;
    }
}
