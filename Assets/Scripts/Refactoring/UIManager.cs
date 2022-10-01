using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using System;//Serializable�������g�p
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p
using UnityEngine;

namespace yamap
{
    /// <summary>
    /// UI�𐧌䂷��
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// ���S�̃f�[�^���Ǘ����� 
        /// </summary>
        [Serializable]
        private class LogoData
        {
            public PerformType performType;//���o�̎��
            public Sprite sprite;//�X�v���C�g
        }

        [SerializeField]
        private List<LogoData> logoDatasList = new();//���S�̃f�[�^�̃��X�g

        [SerializeField]
        private Image imgLogo;//���S�̃C���[�W

        [SerializeField]
        private Image imgBackground;//�w�i�̃C���[�W

        [SerializeField]
        private CanvasGroup cgScore;//���_�̃L�����o�X�O���[�v

        [SerializeField]
        private Text txtScore;//���_�̃e�L�X�g

        [SerializeField]
        private CanvasGroup cgButton;//�{�^���̃L�����o�X�O���[�v

        [SerializeField]
        private Button button;//�{�^��

        [SerializeField]
        private Image imgButton;//�{�^���̃C���[�W

        [SerializeField]
        private Text txtButton;//�{�^���̃e�L�X�g

        private bool isUIEffect;//���o�I������p

        /// <summary>
        /// ���S�̃X�v���C�g���擾����
        /// </summary>
        /// <param name="performType">���o�̎��</param>
        /// <returns>���S�̃X�v���C�g</returns>
        private Sprite GetLogoSprite(PerformType performType)
        {
            //���S�̃X�v���C�g��Ԃ�
            return logoDatasList.Find(x => x.performType==performType).sprite;
        }

        /// <summary>
        /// �Q�[���̉��o���s��
        /// </summary>
        /// <param name="performType">���o�̎��</param>
        /// <returns>�҂�����</returns>
        public IEnumerator PlayGamePerform(PerformType performType)
        {
            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;

            //���_�Ɋւ��鏈��
            {
                //�Q�[���N���A���o�Ȃ�
                if (performType == PerformType.GameClear)
                {
                    //���_�̃e�L�X�g����莞�Ԃ����ĐF�ɕς���
                    txtScore.DOColor(Color.blue, 2f);
                }
                //�Q�[���N���A���o�ł͂Ȃ��Ȃ�
                else
                {
                    //���_�̃L�����o�X�O���[�v���\���ɂ���
                    cgScore.alpha = 0f;
                }
            }

            //�w�i�Ɋւ��鏈��
            {
                //�Q�[���I�[�o�[���o�Ȃ�
                if (performType == PerformType.GameOver)
                {
                    //�w�i�����F�ɐݒ�
                    imgBackground.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
                }
                //�Q�[���I�[�o�[���o�ł͂Ȃ��Ȃ�
                else
                {
                    //�w�i�𔒐F�ɐݒ�
                    imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);
                }
            }

            //���S�̃X�v���C�g��ݒ�
            imgLogo.sprite = GetLogoSprite(performType);

            //�{�^���ɓo�^����Ă��鏈�����폜
            button.onClick.RemoveAllListeners();

            //�{�^���ɏ�����ǉ�
            button.onClick.AddListener(() => ClickedButton(performType));

            //Sequence���쐬
            Sequence sequence = DOTween.Sequence();

            //���o�̎�ނɂ���ď�����ύX
            switch (performType)
            {
                case PerformType.GameStart://�Q�[���X�^�[�g���o�Ȃ�
                    imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);//�{�^���̐F��ݒ�
                    txtButton.text = "Start";//�{�^���̃e�L�X�g��ݒ�
                    {
                        sequence.Append(imgLogo.DOFade(0f, 0f));
                        sequence.Append(imgLogo.DOFade(1f, 1f));
                        sequence.Append(imgButton.DOFade(1f, 1f));
                        sequence.Join(cgButton.DOFade(1f, 1f))
                            .OnComplete(() => button.interactable = true).SetLink(gameObject);
                    }
                    break;

                case PerformType.GameOver://�Q�[���I�[�o�[���o�Ȃ�
                    imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);//�{�^���̐F��ݒ�
                    txtButton.text = "Restart";//�{�^���̃e�L�X�g��ݒ�
                    {
                        sequence.Append(imgLogo.DOFade(0f, 0f));
                        sequence.Append(imgBackground.DOFade(1f, 1f));
                        sequence.Append(imgLogo.DOFade(1f, 1f));
                        sequence.Append(imgButton.DOFade(1f, 1f));
                        sequence.Join(cgButton.DOFade(1f, 1f))
                            .OnComplete(() => button.interactable = true);
                    }
                    break;

                case PerformType.GameClear://�Q�[���N���A���o�Ȃ�
                    imgButton.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);//�{�^���̐F��ݒ�
                    txtButton.text = "Restart";//�{�^���̃e�L�X�g��ݒ�
                    {
                        sequence.Append(imgLogo.DOFade(0f, 0f));
                        sequence.Append(imgBackground.DOFade(1f, 1f));
                        sequence.Append(imgLogo.DOFade(1f, 1f));
                        sequence.Append(imgButton.DOFade(1f, 1f));
                        sequence.Join(cgButton.DOFade(1f, 1f))
                            .OnComplete(() => button.interactable = true);
                    }
                    break;
            }

            //�{�^����񊈐�������
            button.interactable = false;

            //���o���I������܂ő҂�
            yield return new WaitUntil(() => isUIEffect == true);

            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;
        }

        /// <summary>
        /// �{�^���������ꂽ�ۂ̏���
        /// </summary>
        /// <param name="performType">���o�̎��</param>
        private void ClickedButton(PerformType performType)
        {
            //�{�^����񊈐�������
            button.interactable = false;

            //�Q�[���X�^�[�g���o�Ȃ�
            if (performType == PerformType.GameStart)
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameStartSE);
            }
            //�Q�[���X�^�[�g���o�ł͂Ȃ��Ȃ�
            else
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);
            }

            //���o�̎�ނɉ����ď�����ύX
            switch (performType)
            {
                //�Q�[���X�^�[�g���o�Ȃ�A�w�i�̃C���[�W������莞�Ԃ����Ĕ�\���ɂ���
                case PerformType.GameStart: imgBackground.DOFade(0f, 1f); break;

                //�Q�[���I�[�o�[���o�Ȃ�A�w�i�̃C���[�W����莞�Ԃ����Ĕ��F�ɕω�������
                case PerformType.GameOver: imgBackground.DOColor(Color.white, 1f); break;

                //�Q�[���N���A���o�Ȃ�
                case PerformType.GameClear: cgScore.DOFade(0f, 1f); break;
            }

            //���S����莞�Ԃ����Ĕ�\���ɂ���
            imgLogo.DOFade(0f, 1f);

            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgButton.DOFade(0f, 1f)

                //���o���I��������Ԃɐ؂�ւ���
                .OnComplete(() => isUIEffect = true);
        }

        /// <summary>
        /// ���_�̕\�����X�V���鏀�����s��
        /// �]���̏���(UI �\�������ƁA���o�����𕪂���)
        /// </summary>
        public void PrepareUpdateTxtScore()
        {
            //���_�̃e�L�X�g��ݒ肷��
            txtScore.text = GameData.instance.score.playerScore.ToString() + ":" + GameData.instance.score.enemyScore.ToString();

            //���_�̕\�����X�V����
            StartCoroutine(PlayScoreEffect());
        }

        /// <summary>
        /// ���_�̕\�����X�V����iReactiveProperty�̍w�ǂɂ��A�l�X�V���ɃC�x���g�Ƃ��Ď����I�Ɏ��s�����j
        /// </summary>
        /// <param name="playerScore">�v���C���[�̓��_</param>
        /// <param name="enemyScore">�G�l�~�[�̓��_</param>
        public void UpdateDisplayScoreObservable(int playerScore, int enemyScore)
        {
            //ReactiveProperty�ōw�ǂ��Ă�������󂯎��A�\�����X�V����(View ��)
            txtScore.text = playerScore + " : " + enemyScore;

            //���_�̕\�����X�V����
            StartCoroutine(PlayScoreEffect());
        }

        /// <summary>
        /// ���_�X�V���̉��o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator PlayScoreEffect()
        {
            //���_�̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
            cgScore.DOFade(1f, 0.25f);

            //���_����莞�ԁA�\����������
            yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreTime);

            //�v���C���[������������
            if (GameData.instance.score.playerScore == GameData.instance.MaxScore)
            {
                //�ȍ~�̏������s��Ȃ�
                yield break;
            }

            //���_�̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgScore.DOFade(0f, 0.25f);
        }
    }
}