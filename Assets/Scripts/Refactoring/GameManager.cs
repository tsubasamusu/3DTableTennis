using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p
using UnityEngine;

namespace yamap {

    public enum GameState {
        Wait,
        Play,
        GameUp
    }

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private List<ControllerBase> controllersList = new();//�R���g���[���[�̃��X�g

        [SerializeField]
        private BallController ballController;//BallController

        [SerializeField]
        private ScoreManager scoreManager;//ScoreManager

        [SerializeField]
        private UIManager uiManager;//UIManager

        private PlayerController playerController;//PlayerController

        //private EnemyController enemyController;//EnemyController

        [SerializeField] // Debug
        private GameState currentGameState;


        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator Start() {
            currentGameState = GameState.Wait;

            //�e�R���g���[���[�̏����ݒ���s��
            SetUpControllers();

            //ScoreManager�̏����ݒ���s��
            scoreManager.SetUpScoreManager(ballController);

            //�Q�[���X�^�[�g���o���I���܂ő҂�
            yield return uiManager.PlayGameStart();

            //PlayerController��EnemyController������������
            //playerController.enabled = enemyController.enabled = true;

            // �R���g���[���[�̊�����
            ActivateControllers(true);

            //BGM���Đ�
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);

            currentGameState = GameState.Play;
            
            // �{�[���̈ʒu�Ď�
            StartCoroutine(CheckBallPosition());
        }

        /// <summary>
        /// �e�R���g���[���[�̏����ݒ���s��
        /// </summary>
        private void SetUpControllers() {
            //�o�^����Ă���R���g���[���[�̐������J��Ԃ�
            for (int i = 0; i < controllersList.Count; i++) {
                //ControllerBase�̏����ݒ���s��    
                controllersList[i].SetUpControllerBase(ballController);

                // �����Ŏq�N���X���Ƃ̕��������Ă��܂��ƁA�y���Ɏq�N���X���������Ƃ��ɕ����ǉ�����z�K�v�������܂�Ă��܂��B
                // ����ł̓N���X�̌p���̋@�\�����p�ł��Ă���Ƃ͂����Ȃ����߁A�e�N���X�̃��\�b�h�����s���A�q�N���X�Łu���\�b�h�̐U�镑���v��ύX������悤�ɂ���

                ////PlayerController���擾�o������
                //if (controllersList[i].TryGetComponent(out PlayerController playerController)) {
                //    //PlayerController���擾
                //    this.playerController = playerController;

                //    //PlayerController�̏����ݒ���s��
                //    playerController.SetUpPlayerController();

                //    //BallController�̏����ݒ���s��
                //    ballController.SetUpBallController();
                //}
                ////EnemyController���擾�ł�����
                //else if (controllersList[i].TryGetComponent(out EnemyController enemyController)) {
                //    //EnemyController���擾
                //    this.enemyController = enemyController;

                //    //EnemyController�̏����ݒ���s��
                //    enemyController.SetUpEnemyController(ballController);
                //}

                if (playerController == null && controllersList[i].TryGetComponent(out playerController)) {
                    //PlayerController���擾
                    Debug.Log("PlayerController �擾");
                }
            }

            //PlayerController��EnemyController��񊈐�������
            //playerController.enabled = enemyController.enabled = false;

            ActivateControllers(false);
        }

        /// <summary>
        /// �R���g���[���[�̊������؂�ւ�
        /// </summary>
        /// <param name="isSwitch"></param>
        private void ActivateControllers(bool isSwitch) {

            // �@
            //for (int i = 0; i < controllersList.Count; i++) {
            //    controllersList[i].enabled = isSwitch;
            //}

            // �A�@�ǂ���ł������ł�
            controllersList[0].enabled = controllersList[1].enabled = isSwitch;

            //playerController.enabled = enemyController.enabled = isSwitch;
        }

        /// <summary>
        /// �Q�[���I�����o���s��
        /// </summary>
        /// <param name="isGameOverPerformance">�s���Q�[���I�����o���Q�[���I�[�o�[���o���ǂ���</param>
        /// <returns>�҂�����</returns>
        private IEnumerator PlayGameEndPerformance(bool isGameOverPerformance) {
            //�S�Ẳ������S�Ɏ~�܂�܂ő҂�
            yield return new WaitForSeconds(GameData.instance.FadeOutTime);

            //���ʉ����Đ�
            SoundManager.instance.PlaySound(isGameOverPerformance ? SoundDataSO.SoundName.GameOverSE : SoundDataSO.SoundName.PlayerPointSE);

            //Debug.Log("SE OK");

            //�Q�[���I�����o���s��
            yield return isGameOverPerformance ? uiManager.PlayGameOver() : uiManager.PlayGameClear();

            //���C���V�[����ǂݍ���
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// �Q�[���I���̏������s��
        /// </summary>
        private void PrepareGameEnd() {
            //PlayerController��EnemyController��񊈐�������
            //playerController.enabled = enemyController.enabled = false;

            ActivateControllers(false);

            //�S�Ẳ����~����
            SoundManager.instance.StopSound();

            //Debug.Log("BGM Stop");
        }

        //TODO:���t�@�N�^�����O�̒i�K�ŁA�ȍ~�̊Ď�������UniRx���g�p���ď���������

        //private bool flag;//�d�������h�~�p

        ////�}�b�`�|�C���g���擾
        //int matchPoints = GameData.instance.MaxScore - 1;


        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        //private void Update() {
        //    //�}�b�`�|�C���g���擾
        //    //int matchPoints = GameData.instance.MaxScore - 1;

        //    //�v���C���[�̓��_���A�G�l�~�[�̓��_���}�b�`�|�C���g�ȉ��Ȃ�
        //    if (GameData.instance.score.playerScore <= matchPoints && GameData.instance.score.enemyScore <= matchPoints) {
        //        //�ȍ~�̏������s��Ȃ�
        //        return;
        //    }

        //    //flag��false�Ȃ�
        //    if (!flag) {
        //        //�Q�[���I���̏������s��
        //        PrepareGameEnd();

        //        //�Q�[���I�����o���s��
        //        StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));

        //        //flag��true������
        //        flag = true;
        //    }
        //}


        // ScoreManager ����ڊ�

        private int count;//�T�[�u�̉񐔂̋L�^�p
        private OwnerType server;//�T�[�o�[�ێ��p


        /// <summary>
        /// �{�[���̈ʒu�̊Ď�
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckBallPosition() {�@�@�@// ScoreManager �� CheckScore ���\�b�h
            // �Q�[���v���C���̊Ԃ����Ď�
            while (currentGameState == GameState.Play) {
                //�{�[�������ɗ������Ȃ�
                if (ballController.transform.position.y <= 0.25f) {
                    // 1�Z�b�g�I��
                    currentGameState = GameState.Wait;
                }

                ////���_�̋L�^���X�V
                //scoreManager.UpdateScore(scoreManager.GetUpadateValue());

                ////���_�̕\���̍X�V�����鏀�����s��
                //uiManager.PrepareUpdateTxtScore();

                ////�{�[���̓������~�߂�
                //ballController.PrepareRestartGame(GetAppropriatServer());   //, playerController

                //// Player �̏�����
                //playerController.ResetPlayerPos();

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }

            // �{�[�������ɗ������̂ŁA���̃Z�b�g�͏I��
            EndOneSet();
        }

        /// <summary>
        /// 1�Z�b�g�I�����̏���
        /// </summary>
        private void EndOneSet() {           
            //���_�̋L�^���X�V
            scoreManager.UpdateScore(scoreManager.GetUpadateValue());

            //���_�̕\���̍X�V�����鏀�����s��
            uiManager.PrepareUpdateTxtScore();

            //�T�[�o�[���X�V���m�F���Ă���T�[�o�[���擾���āA�{�[���̓������~�߂�
            ballController.PrepareRestartGame(GetAppropriatServer());   //, playerController

            // Player �̏�����
            playerController.ResetPlayerPos();

            //�v���C���[�̓��_���G�l�~�[�̓��_���}�b�N�X�X�R�A�Ȃ�
            if (GameData.instance.score.playerScore >= GameData.instance.MaxScore || GameData.instance.score.enemyScore >= GameData.instance.MaxScore) {

                currentGameState = GameState.GameUp;

                //�Q�[���I���̏������s��
                PrepareGameEnd();

                //�Q�[���I�����o���s��
                StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));
            } else {
                currentGameState = GameState.Play;

                // ���̃Z�b�g���J�n
                // �{�[���̈ʒu�̊Ď����X�^�[�g
                StartCoroutine(CheckBallPosition());
            }

            /// <summary>
            /// �K�؂ȃT�[�o�[���擾����i�{�[�������������ۂɌĂяo�������j
            /// </summary>
            /// <returns>�K�؂ȃT�[�o�[</returns>
            OwnerType GetAppropriatServer() {
                //�T�[�u�񐔂��L�^
                count++;

                //�܂�2�{�T�[�u��ł��Ă��Ȃ��Ȃ�
                if (count < 2) {
                    //�T�[�o�[��ύX���Ȃ�
                    return server;
                }

                //�T�[�u�񐔂�������
                count = 0;

                //�T�[�o�[��ς��āA�L�^����
                return server = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;
            }
        }
    }
}