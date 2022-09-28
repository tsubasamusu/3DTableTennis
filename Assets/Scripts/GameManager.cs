using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<ControllerBase> controllersList = new();//�R���g���[���[�̃��X�g

    [SerializeField]
    private BallController ballController;//BallController

    [SerializeField]
    private ScoreManager scoreManager;//ScoreManager

    [SerializeField]
    private UIManager uiManager;//UIManager

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�e�R���g���[���[�̏����ݒ���s��
        SetUpControllers();

        //ScoreManager�̏����ݒ���s��
        scoreManager.SetUpScoreManager(ballController,uiManager);

        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return uiManager.PlayGameStart();
    }

    /// <summary>
    /// �e�R���g���[���[�̏����ݒ���s��
    /// </summary>
    private void SetUpControllers()
    {
        //�o�^����Ă���R���g���[���[�̐������J��Ԃ�
        for (int i = 0; i < controllersList.Count; i++)
        {
            //ControllerBase�̏����ݒ���s��    
            controllersList[i].SetUpControllerBase();

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
