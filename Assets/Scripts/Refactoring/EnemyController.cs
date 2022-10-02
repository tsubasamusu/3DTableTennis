using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// �G�l�~�[�̍s���𐧌䂷��
    /// </summary>
    public class EnemyController : ControllerBase 
    {
        private BoundPoint enemyBoundPoint;//�G�l�~�[�̒��˂�ʒu

        /// <summary>
        /// ���Z�b�g����
        /// </summary>
        private void Reset() 
        {
            //�������G�l�~�[�Ȃ�
            if (TryGetComponent(out EnemyController _)) 
            {
                //���L�҂̎�ނ��G�l�~�[�ɐݒ�
                ownerType = OwnerType.Enemy;
            }
        }

        /// <summary>
        /// ControllerBase�̏����ݒ���s���i�e�N���X�ŗp�ӂ��Ă���ݒ�̏����͎c���A���̃N���X�Ǝ��̏�����ǉ�����j
        /// </summary>
        public override void SetUpControllerBase(BallController ballController) 
        {
            //�e�N���X��SetUpControllerBase���\�b�h���Ăяo��
            base.SetUpControllerBase(ballController);

            //���Z�b�g����
            Reset();

            //�G�l�~�[�p�̒��˂�ʒu���擾
            enemyBoundPoint = ballController.GetBoundPoint(ownerType);
        }

        /// <summary>
        /// �ړ��������擾����
        /// </summary>
        /// <returns>�ړ�����</returns>
        protected override Vector3 GetMoveDir() 
        {
            //���݂̃{�[���̏��L�҂��G�l�~�[�Ȃ�
            if (ballController.CurrentOwner == OwnerType.Enemy) 
            {
                //�����ʒu�ւ̕�����Ԃ��i�����ʒu�Ɍ������Ĉړ�����j
                return firstPos - transform.position;
            }

            //����i�v���C���[�j�̃{�[�����R�[�g�ɓ���Ȃ�
            if (ballController.InCourt) 
            {
                //�ړI�n�ւ̕�����Ԃ��i�{�[���Ɍ������Ĉړ�����j
                return ballController.transform.position - transform.position;
            }

            //�ړ����Ȃ�
            return Vector3.zero;
        }

        /// <summary>
        /// ���P�b�g�𐧌䂷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
        /// </summary>
        protected override void ControlRacket() 
        {
            //�{�[�����U�������ɓ����Ă��Ȃ�������
            if (Mathf.Abs((transform.position - ballController.transform.position).magnitude) > GameData.instance.EnemyShotRange) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //���P�b�g��U���Ă���Œ��Ȃ�
            if (!racketController.IsIdle) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�t�H�A�n���h�h���C�u�ɂ��邩�o�b�N�n���h�h���C�u�ɂ��邩�����߂āA�h���C�u������
            racketController.Drive(transform.position.x >= ballController.transform.position.x);
        }

        /// <summary>
        /// �L�����N�^�[�̌�����ݒ肷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
        /// </summary>
        protected override void SetCharaDirection() 
        {
            //��ɃG�l�~�[�̃R�[�g�̕����Ɍ���
            transform.LookAt(enemyBoundPoint.transform.position);
        }
    }
}