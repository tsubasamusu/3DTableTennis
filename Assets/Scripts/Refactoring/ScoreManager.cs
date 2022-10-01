using UniRx;//UniRx��ǉ�
using UnityEngine;

namespace yamap {

    /// <summary>
    /// ���_���Ǘ�����
    /// </summary>
    public class ScoreManager : MonoBehaviour 
    {
        //BallController
        private BallController ballController;

        //TODO:UniRx�m�F

        // ReactiveProperty�@(Model)
        // ���̇A�̎������Ɏg��
        public ReactiveProperty<int> PlayerScore = new();
        public ReactiveProperty<int> EnemyScore = new();

        void Reset() {
            PlayerScore = new(0);
            EnemyScore = new(0);
        }

        /// <summary>
        /// ScoreManager�̏����ݒ���s��
        /// </summary>
        /// <param name="ballController">BallController</param>
        public void SetUpScoreManager(BallController ballController) 
        {
            //BallController���擾
            this.ballController = ballController;

            //���Z�b�g����
            Reset();
        }


        /// <summary>
        /// ���_�̋L�^���X�V����
        /// </summary>
        /// <param name="updateValue">�X�V��</param>
        public void UpdateScore((int playerUpdateValue, int enemyUpdateValue) updateValue) 
        {
            //���ʉ����Đ�
            SoundManager.instance.PlaySound(updateValue.playerUpdateValue > 0 ? SoundDataSO.SoundName.PlayerPointSE : SoundDataSO.SoundName.EnemyPointSE);

            //�v���C���[�̓��_���X�V
            GameData.instance.score.playerScore += updateValue.playerUpdateValue;

            ///�G�l�~�[�̓��_���X�V
            GameData.instance.score.enemyScore += updateValue.enemyUpdateValue;

            //TODO:UniRx�m�F
            {
                // ReactiveProperty �� GameData �ŗ��p���Ă���ꍇ(���̇@�ł̎�����)
                GameData.instance.PlayerScore.Value += updateValue.playerUpdateValue;
                GameData.instance.EnemyScore.Value += updateValue.enemyUpdateValue;

                // ScoreManager �� ReactiveProperty �𗘗p���Ă���ꍇ(���̇A�ł̎�����)
                PlayerScore.Value += updateValue.playerUpdateValue;
                EnemyScore.Value += updateValue.enemyUpdateValue;
            }
        }

        /// <summary>
        /// ���_�̍X�V�ʂ��擾����
        /// </summary>
        /// <returns>���_�̍X�V��</returns>
        public (int playerUpdateValue, int enemyUpdateValue) GetUpadateValue() 
        {
            //�R�[�g�ɓ��������ǂ����ŏ�����ύX
            return ballController.InCourt ?

                //�{�[���̏��L�҂ɉ����Ė߂�l��ύX
                ballController.CurrentOwner == OwnerType.Player ? (1, 0) : (0, 1)

                //�{�[���̏��L�҂ɉ����Ė߂�l��ύX
                : ballController.CurrentOwner == OwnerType.Player ? (0, 1) : (1, 0);
        }
    }
}