using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���_���Ǘ�����
/// </summary>
public class ScoreManager : MonoBehaviour
{
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

                //���_�̍X�V�ʂ��擾
                (int, int) updateValue = ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);

                //���_���X�V
                UpdateScore(updateValue.Item1, updateValue.Item2);

                //�{�[���̓������~�߂�
                ballController.StopBall();

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }
    }

    /// <summary>
    /// ���_���X�V����
    /// </summary>
    /// <param name="playerValue">�v���C���[�̓��_�̍X�V��</param>
    /// <param name="enemyValue">�G�l�~�[�̓��_�̍X�V��</param>
    private void UpdateScore(int playerValue,int enemyValue)
    {
        //�v���C���[�̓��_���X�V
        GameData.instance.score.playerScore+=playerValue;

        //�G�l�~�[�̓��_���X�V
        GameData.instance.score.enemyScore+=enemyValue;
    }
}
