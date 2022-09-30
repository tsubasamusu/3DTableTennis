using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p
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

    private PlayerController playerController;//PlayerController

    private EnemyController enemyController;//EnemyController

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�e�R���g���[���[�̏����ݒ���s��
        SetUpControllers();

        //ScoreManager�̏����ݒ���s��
        scoreManager.SetUpScoreManager(ballController, uiManager);

        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return uiManager.PlayGameStart();

        //PlayerController��EnemyController������������
        playerController.enabled = enemyController.enabled = true;

        //BGM���Đ�
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);
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
                //PlayerController���擾
                this.playerController = playerController;

                //PlayerController�̏����ݒ���s��
                playerController.SetUpPlayerController();

                //BallController�̏����ݒ���s��
                ballController.SetUpBallController(playerController);
            }
            //EnemyController���擾�ł�����
            else if (controllersList[i].TryGetComponent(out EnemyController enemyController))
            {
                //EnemyController���擾
                this.enemyController = enemyController;

                //EnemyController�̏����ݒ���s��
                enemyController.SetUpEnemyController(ballController);
            }
        }

        //PlayerController��EnemyController��񊈐�������
        playerController.enabled = enemyController.enabled = false;
    }

    /// <summary>
    /// �Q�[���I�����o���s��
    /// </summary>
    /// <param name="isGameOverPerformance">�s���Q�[���I�����o���Q�[���I�[�o�[���o���ǂ���</param>
    /// <returns>�҂�����</returns>
    private IEnumerator PlayGameEndPerformance(bool isGameOverPerformance)
    {
        //�S�Ẳ������S�Ɏ~�܂�܂ő҂�
        yield return new WaitForSeconds(GameData.instance.FadeOutTime);

        //���ʉ����Đ�
        SoundManager.instance.PlaySound(isGameOverPerformance ? SoundDataSO.SoundName.GameOverSE : SoundDataSO.SoundName.PlayerPointSE);

        //�Q�[���I�����o���s��
        yield return isGameOverPerformance ? uiManager.PlayGameOver() : uiManager.PlayGameClear();

        //���C���V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �Q�[���I���̏������s��
    /// </summary>
    private void PrepareGameEnd()
    {
        //PlayerController��EnemyController��񊈐�������
        playerController.enabled = enemyController.enabled = false;

        //�S�Ẳ����~����
        SoundManager.instance.StopSound();
    }

    //TODO:���t�@�N�^�����O�̒i�K�ŁA�ȍ~�̏�����UniRx���g�p���ď���������

    private bool flag;//�d�������h�~�p

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�}�b�`�|�C���g���擾
        int matchPoints = GameData.instance.MaxScore - 1;

        //�v���C���[�̓��_���A�G�l�~�[�̓��_���}�b�`�|�C���g�ȉ��Ȃ�
        if (GameData.instance.score.playerScore <= matchPoints && GameData.instance.score.enemyScore <= matchPoints)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //flag��false�Ȃ�
        if (!flag)
        {
            //�Q�[���I���̏������s��
            PrepareGameEnd();

            //�Q�[���I�����o���s��
            StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));

            //flag��true������
            flag = true;
        }
    }
}

