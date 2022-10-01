using UniRx;//UniRx���g�p
using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// �Q�[���S�̂̃f�[�^���Ǘ�����
    /// </summary>
    public class GameData : MonoBehaviour 
    {
        public static GameData instance;//�C���X�^���X

        /// <summary>
        /// Start���\�b�h���O�ɌĂяo�����
        /// </summary>
        private void Awake()
        {
            //�ȉ��A�V���O���g���ɕK�{�̋L�q
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [SerializeField, Header("���P�b�g���\����̂ɗv���鎞��")]
        private float prepareRacketTime = 0.1f;//���P�b�g���\����̂ɗv���鎞��

        [SerializeField, Header("���P�b�g��U�鎞��")]
        private float swingTime = 0.2f;//���P�b�g��U�鎞��

        [SerializeField, Header("�ړ����x")]
        private float moveSpeed = 5.0f;//�ړ����x

        [SerializeField, Header("�d��")]
        private float gravity = 10.0f;//�d��

        [SerializeField, Header("�{�[���̑���")]
        private float ballSpeed = 6.0f;//�{�[���̑���

        [SerializeField, Header("�G�l�~�[�̍U������")]
        private float enemyShotRange = 1.5f;//�G�l�~�[�̍U������

        [SerializeField, Header("�G�l�~�[���T�[�u��ł܂ł̎���")]
        private float enemyServeTime = 2.0f;//�G�l�~�[���T�[�u��ł܂ł̎���

        [SerializeField, Header("���_�ƃ��b�Z�[�W��\�����鎞��")]
        private float displayScoreAndMessageTime = 2.0f;

        [SerializeField, Header("���̃t�F�[�h�A�E�g����")]
        private float fadeOutTime = 0.5f;//���̃t�F�[�h�A�E�g����

        [SerializeField, Header("�ō����_")]
        private int maxScore = 11;//�ō����_

        [HideInInspector]
        public (int playerScore, int enemyScore) score;//���_

        //TODO:UniRx�m�F

        // ReactiveProperty�@(Model)
        // ���̇@�̎������Ɏg��
        public ReactiveProperty<int> PlayerScore = new();
        public ReactiveProperty<int> EnemyScore = new();

        void Reset() {
            PlayerScore = new(0);
            EnemyScore = new(0);

            Debug.Log("Reset : GameData");
        }

        /// <summary>
        /// �u���P�b�g���\����̂ɗv���鎞�ԁv�̎擾�p
        /// </summary>
        public float PrepareRacketTime { get => prepareRacketTime; }

        /// <summary>
        /// �u���P�b�g��U�鎞�ԁv�̎擾�p
        /// </summary>
        public float SwingTime { get => swingTime; }

        /// <summary>
        /// �ړ����x�̎擾�p
        /// </summary>
        public float MoveSpeed { get => moveSpeed; }

        /// <summary>
        /// �d�͎擾�p
        /// </summary>
        public float Gravity { get => gravity; }

        /// <summary>
        /// �u�{�[���̑����v�̎擾�p
        /// </summary>
        public float BallSpeed { get => ballSpeed; }

        /// <summary>
        /// �u�G�l�~�[�̍U�������v�̎擾�p
        /// </summary>
        public float EnemyShotRange { get => enemyShotRange; }

        /// <summary>
        /// �u�G�l�~�[���T�[�u��ł܂ł̎��ԁv�̎擾�p
        /// </summary>
        public float EnemyServeTime { get => enemyServeTime; }

        /// <summary>
        /// �u���_�ƃ��b�Z�[�W��\�����鎞�ԁv�̎擾�p
        /// </summary>
        public float DisplayScoreAndMessageTime { get => displayScoreAndMessageTime; }

        /// <summary>
        /// �u���̃t�F�[�h�A�E�g���ԁv�̎擾�p
        /// </summary>
        public float FadeOutTime { get => fadeOutTime; }

        /// <summary>
        /// �ō����_�擾�p
        /// </summary>
        public int MaxScore { get => maxScore; }
    }
}