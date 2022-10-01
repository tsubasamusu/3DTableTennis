using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p
using UniRx;//UniRx���g�p
using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// �Q�[���̏��
    /// </summary>
    public enum GameState 
    {
        Wait,//�ҋ@���
        Play,//������
        GameUp//�Q�[���I��
    }

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

        private GameState currentGameState;//���݂̃Q�[���̏��


        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator Start() 
        {
            //���݂̃Q�[���̏�Ԃ�ҋ@��Ԃɐݒ�
            currentGameState = GameState.Wait;

            //�e�R���g���[���[�̏����ݒ���s��
            SetUpControllers();

            //ScoreManager�̏����ݒ���s��
            scoreManager.SetUpScoreManager(ballController);

            //�Q�[���X�^�[�g���o���I���܂ő҂�
            yield return uiManager.PlayGamePerform(PerformType.GameStart);

            // �R���g���[���[�̊�����
            ActivateControllers(true);

            //BGM���Đ�
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.MainBGM, 0.3f, true);

            //TODO:UniRx�m�F
            {
                // ReactiveProperty �̗��p���₷���󋵂́AMVP �p�^�[���ƌĂ΂��AUI �̕\���X�V�ɂ�����鏈��
                // �l(���_)���w��(�Ď�)���邱�ƂŁA�����I�� UI �̕\���X�V���s���悤�ȃC�x���g���������o�����Ƃ��ł���

                // GameManager �� Presenter �Ƃ�������������
                // Presenter �Ƃ́AModel �ł��� ScoreManager �ƁAView �ł��� UIManager �̑o����m���Ă��āA������Ȃ���ڂ��ɂȂ�

                // ScoreManager �ɗp�ӂ��Ă��� ReactiveProperty(Player �� Enemy �̓��_���Ǘ�����l) �������ōw��(�Ď�)���閽�߂��o��
                // �����ꂩ�̒l���X�V���ꂽ�ꍇ�ASubscribe ���\�b�h���ɂ��鏈���������I�Ɏ��s����
                // ����͒l���X�V����邽�сAUIManager �̃��\�b�h�����s���A�o���̒l�̏����������g���Ē񋟂���

                // �������邱�ƂŁAPresenter ���ł��� GameManager �݂̂��AScoreManager �� UIManager ��m���Ă����(�a����)�ł���Ȃ���
                // �l�̍X�V�ɍ��킹�āA��ʂ̕\���X�V���ꏏ�ɘA�����ď������s�����Ƃ��ł���
                // ����āA���݂̂悤�ɁA�l�̍X�V�ɍ��킹�Ă��̓s�x�A��ʕ\���X�V�̖��߂��s���K�v���Ȃ��Ȃ�


                // ReactiveProperty ���w�ǁ@���̇@(�n�߂Ɋo������@)
                // Model �Ƃ��� ScoreManager �̑���� GameData �𗘗p���Ă��邪�A����́A���� UpdateTxtScore ���\�b�h�̏������������Ă��邽��(���\�b�h�Ɉ������Ȃ�����)
                GameData.instance.PlayerScore.Subscribe(_ => uiManager.PrepareUpdateTxtScore()).AddTo(this);
                GameData.instance.EnemyScore.Subscribe(_ => uiManager.PrepareUpdateTxtScore()).AddTo(this);


                // ReactiveProperty ���w�ǁ@���̇A(������A�o���������@�B�@�̏������R�����g�A�E�g����΁A�����悤�ɐ���ɓ����܂�)
                //Observable.CombineLatest(scoreManager.PlayerScore, scoreManager.EnemyScore, (playerScore, enemyScore) => (playerScore, enemyScore))
                //    .Subscribe(scores => uiManager.UpdateDisplayScoreObservable(scores.playerScore, scores.enemyScore))
                //   .AddTo(this);


                // �܂��́AReactiveProperty �𗘗p���� MVP �p�^�[���ɂ��A�l�� UI �\���X�V�̘A������������Ɗo���邱��
                // �������AMVP �p�^�[���� UI �ɂ̂ݎg���悤�ɂ��邱��
                // Subscribe �� AddTo �Ƃ��������\�b�h�̋@�\����������Ɨ������邱��

                // �v���O�����ɂ͐�΂ȏ������͂Ȃ��̂ŁAUniRx �ɂ����Ă��A�����܂ł��A�X�L���̈����o���̕����L������̂ł���ƍl���邱��
                // ���ł������ɂȂ�Ȃ��悤�ɂ���B�_��Ȏv�l��Y��Ȃ�
                // ReactiveProperty ���̂͐F�X�ȏ����ɉ��p�\�����A�Ȃ�ł�����ł����p����A�Ƃ������Ƃł͂Ȃ�
                // �֗��ȋ@�\�ł���A�����������镝���L���邪�A��قǂ������Ă���悤�ɁA���ׂĂɂ����ėL���Ƃ����킯�ł͂Ȃ�(�ǂ݉����Ȃ��l������)

                // ��L�̎�����́A�܂��́A�@�̕��ŐF�X�ȏ����������Ă݂āA�ǂ������������������̂��������āA�X���X���Ə����郌�x���ɂ��邱�Ƃ�ڕW�ɂ���

                // ���̌�A�A�̏����̓��e�𗝉����Ă����悤�ɂ���
                // �l�b�g�Ȃǂɂ�������͂�����̂́A�����̃v���W�F�N�g�ɗ��Ƃ����񂾂��̂͐�΂Ɍ�����Ȃ��̂ŁA�����̓������o���邱��
            }

            //���݂̃Q�[���̏�Ԃ��������ɐݒ�
            currentGameState = GameState.Play;
            
            //�{�[���̈ʒu�̊Ď����J�n����
            StartCoroutine(CheckBallPosition());
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
                controllersList[i].SetUpControllerBase(ballController);

                //�܂�PlayerController���擾���Ă��Ȃ����APlayerController�̎擾�ɐ���������
                if (playerController == null && controllersList[i].TryGetComponent(out playerController)) 
                {
                    //���ʂ��
                    Debug.Log("PlayerController�̎擾�ɐ���");
                }
            }

            //�e�R���g���[���[��񊈐�������
            ActivateControllers(false);
        }

        /// <summary>
        /// �R���g���[���[�̊������E�񊈐�����؂�ւ���
        /// </summary>
        /// <param name="isSwitch">������or�񊈐���</param>
        private void ActivateControllers(bool isSwitch) 
        {
            //�R���g���[���[�̃��X�g�̗v�f�������J��Ԃ�
            for (int i = 0; i < controllersList.Count; i++)
            {
                //�擾�����v�f�̊������E�񊈐�����؂�ւ���
                controllersList[i].enabled = isSwitch;
            }
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
            yield return uiManager.PlayGamePerform(isGameOverPerformance ? PerformType.GameOver : PerformType.GameClear);

            //���C���V�[����ǂݍ���
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// �Q�[���I���̏������s��
        /// </summary>
        private void PrepareGameEnd() 
        {
            //�e�R���g���[���[��񊈐�������
            ActivateControllers(false);

            //�S�Ẳ����~����
            SoundManager.instance.StopSound();
        }

        private int count;//�T�[�u�̉񐔂̋L�^�p

        private OwnerType server;//�T�[�o�[�ێ��p

        /// <summary>
        /// �{�[���̈ʒu���Ď�����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator CheckBallPosition() 
        {�@�@�@
            //�Q�[���̏�Ԃ��������Ȃ�J��Ԃ��i�Q�[���v���C���̊Ԃ����Ď�����j
            while (currentGameState == GameState.Play) 
            {
                //�{�[�������ɗ������Ȃ�
                if (ballController.transform.position.y <= 0.25f) 
                {
                    //�Q�[���̏�Ԃ�ҋ@��Ԃɐݒ�
                    currentGameState = GameState.Wait;
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }

            //1�Z�b�g�I�����̏������s��
            EndOneSet();
        }

        /// <summary>
        /// 1�Z�b�g�I�����̏������s��
        /// </summary>
        private void EndOneSet() 
        {           
            //���_�̋L�^���X�V
            scoreManager.UpdateScore(scoreManager.GetUpadateValue());

            //�T�[�o�[���擾���āA�{�[���̓������~�߂�
            ballController.PrepareRestartGame(GetAppropriatServer());

            //�v���[���[�������ʒu�Ɉړ�������
            playerController.ResetPlayerPos();

            //�v���C���[�̓��_���A�G�l�~�[�̓��_���A�ō����_�Ȃ�
            if (GameData.instance.score.playerScore >= GameData.instance.MaxScore || GameData.instance.score.enemyScore >= GameData.instance.MaxScore)
            {
                //���݂̃Q�[���̏�Ԃ��Q�[���I����Ԃɐݒ�
                currentGameState = GameState.GameUp;

                //�Q�[���I���̏������s��
                PrepareGameEnd();

                //�Q�[���I�����o���s��
                StartCoroutine(PlayGameEndPerformance(GameData.instance.score.enemyScore == GameData.instance.MaxScore));
            } 
            //�v���[���[�̓��_���A�G�l�~�[�̓��_���A�ō����_��菬�����Ȃ�
            else 
            {
                //���݂̃Q�[���̏�Ԃ��������ɐݒ�
                currentGameState = GameState.Play;

                //�{�[���̈ʒu�̊Ď����J�n
                StartCoroutine(CheckBallPosition());
            }

            /// <summary>
            /// �K�؂ȃT�[�o�[���擾����i�{�[�������������ۂɌĂяo�������j
            /// </summary>
            /// <returns>�K�؂ȃT�[�o�[</returns>
            OwnerType GetAppropriatServer() 
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
        }
    }
}