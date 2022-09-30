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
    /// <param name="uIManager">UIManager</param>
    /// <param name="playerController">PlayerController</param>
    public void SetUpScoreManager(BallController ballController, UIManager uIManager,PlayerController playerController)
    {
        //���_�̍X�V�̊m�F���J�n����
        StartCoroutine(CheckScore());

        //���_�̍X�V���m�F����
        IEnumerator CheckScore()
        {
            //�����ɌJ��Ԃ�
            while (true)
            {
                //�{�[�������ɗ����Ă��Ȃ��Ȃ�
                if (ballController.transform.position.y > 0.25f)
                {
                    //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                    yield return null;

                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //���_�̋L�^���X�V
                UpdateScore(GetUpadateValue(ballController), uIManager);

                //�{�[���̓������~�߂�
                ballController.PrepareRestartGame(GetAppropriatServer(),playerController);

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }
    }

    /// <summary>
    /// �K�؂ȃT�[�o�[���擾����i�{�[�������������ۂɌĂяo�������j
    /// </summary>
    /// <returns>�K�؂ȃT�[�o�[</returns>
    private OwnerType GetAppropriatServer()
    {
        //�T�[�u�񐔂��L�^
        count++;

        //�܂�2�{�T�[�u��ł��Ă��Ȃ��Ȃ�
        if (count < 2)
        {
            //�T�[�o�[��ύX���Ȃ�
            return server;
        }

        //�T�[�u�񐔂�������
        count = 0;

        //�T�[�o�[��ς��āA�L�^����
        return server = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;
    }

    /// <summary>
    /// ���_�̋L�^���X�V����
    /// </summary>
    /// <param name="updateValue">�X�V��</param>
    /// <param name="uIManager">UIManager</param>
    private void UpdateScore((int playerUpdateValue, int enemyUpdateValue) updateValue, UIManager uIManager)
    {
        //���ʉ����Đ�
        SoundManager.instance.PlaySound(updateValue.playerUpdateValue > 0 ? SoundDataSO.SoundName.PlayerPointSE : SoundDataSO.SoundName.EnemyPointSE);

        //�v���C���[�̓��_���X�V
        GameData.instance.score.playerScore += updateValue.playerUpdateValue;

        //�G�l�~�[�̓��_���X�V
        GameData.instance.score.enemyScore += updateValue.enemyUpdateValue;

        //���_�̕\���̍X�V�����鏀�����s��
        uIManager.PrepareUpdateTxtScore();
    }

    /// <summary>
    /// ���_�̍X�V�ʂ��擾����
    /// </summary>
    /// <param name="ballController">BallController</param>
    /// <returns>���_�̍X�V��</returns>
    private (int playerUpdateValue, int enemyUpdateValue) GetUpadateValue(BallController ballController)
    {
        //�R�[�g�ɓ��������ǂ����ŏ�����ύX
        return ballController.InCourt ?

            //�{�[���̏��L�҂ɉ����Ė߂�l��ύX
            ballController.CurrentOwner == OwnerType.Player ? (1, 0) : (0, 1)

            //�{�[���̏��L�҂ɉ����Ė߂�l��ύX
            : ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);
    }
}
