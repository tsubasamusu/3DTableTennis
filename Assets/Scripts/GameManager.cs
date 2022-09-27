using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<ControllerBase> controllersList = new();//�R���g���[���[�̃��X�g

    [SerializeField]
    private BallController ballController;//BallController

    [SerializeField]
    private ScoreManager scoreManager;//ScoreManager

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�e�R���g���[���[�̏����ݒ���s��
        SetUpControllers();

        //ScoreManager�̏����ݒ���s��
        scoreManager.SetUpScoreManager(ballController);
    }

    /// <summary>
    /// �e�R���g���[���[�̏����ݒ���s��
    /// </summary>
    private void SetUpControllers()
    {
        //�o�^����Ă���R���g���[���[�̐������J��Ԃ�
        for(int i = 0; i < controllersList.Count; i++)
        {
            //ControllerBase�̏����ݒ���s��    
            controllersList[i].SetUpControllerBase();
            
            //�e�R���g���[���[�ŗL�̏����ݒ���s��
            {
                //PlayerController���擾�o������
                if (controllersList[i].TryGetComponent(out PlayerController playerController))
                {
                    //PlayerController�̏����ݒ���s��
                    playerController.SetUpPlayerController();

                    //BallController�̏����ݒ���s��
                    ballController.SetUpBallController(playerController);
                }
                //EnemyController���擾�ł�����
                else if (controllersList[i].TryGetComponent(out EnemyController enemyController))
                {
                    //EnemyController�̏����ݒ���s��
                    enemyController.SetUpEnemyController(ballController);
                }
            }
        }
    }
}
