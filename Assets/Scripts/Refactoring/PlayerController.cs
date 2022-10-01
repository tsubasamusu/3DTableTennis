using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// �v���[���[�̍s���𐧌䂷��
    /// </summary>
    public class PlayerController : ControllerBase 
    {
        /// <summary>
        /// ���Z�b�g����
        /// </summary>
        private void Reset() 
        {
            //�������v���C���[�Ȃ�
            if (TryGetComponent(out PlayerController _)) 
            {
                //���L�҂��v���C���[�ɐݒ�
                ownerType = OwnerType.Player;
            }
        }

        /// <summary>
        /// ControllerBase�̏����ݒ���s���i�e�N���X�ŗp�ӂ��Ă���ݒ�̏���(base ����)�͎c���A���̃N���X�Ǝ��̏�����ǉ�����j
        /// </summary>
        /// <param name="ballController">BallController</param>
        public override void SetUpControllerBase(BallController ballController) 
        {
            //TODO:����͉��H
            base.SetUpControllerBase(ballController);

            //���Z�b�g����
            Reset();

            //BallController�̏����ݒ���s��
            ballController.SetUpBallController();
        }

        /// <summary>
        /// �ړ��������擾����
        /// </summary>
        protected override Vector3 GetMoveDir() 
        {
            //����������Ă��Ȃ��Ȃ�
            if (!Input.anyKey) 
            {
                //�ȍ~�̏������s��Ȃ��i���ʂȏ�����h�~�E������ł̈ړ��h�~�j
                return Vector3.zero;
            }

            //���E�ړ��̓��͂��擾
            float moveH = Input.GetAxis("Horizontal");

            //�O��ړ��̓��͂��擾
            float moveV = Input.GetAxis("Vertical");

            //���̈ړ�������ݒ�
            Vector3 movement = new Vector3(moveH, 0, moveV);

            //�ړ��������擾���A�Ԃ�
            return Camera.main.transform.forward * movement.z + Camera.main.transform.right * movement.x;
        }

        /// <summary>
        /// ���P�b�g�𐧌䂷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
        /// </summary>
        protected override void ControlRacket() 
        {
            //���N���b�N���ꂽ��
            if (Input.GetMouseButtonDown(0)) 
            {
                //�o�b�N�n���h�h���C�u������
                racketController.Drive(false);
            }
            //�E�N���b�N���ꂽ��
            else if (Input.GetMouseButtonDown(1)) 
            {
                //�t�H�A�n���h�h���C�u������
                racketController.Drive(true);
            }
        }

        /// <summary>
        /// �L�����N�^�[�̌�����ݒ肷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
        /// </summary>
        protected override void SetCharaDirection() 
        {
            //�L�����N�^�[�̌�����ݒ肷��
            transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);
        }

        /// <summary>
        /// �v���C���[�������ʒu�Ɉړ�������
        /// </summary>
        public void ResetPlayerPos() 
        {
            //CharacterController��񊈐�������
            charaController.enabled = false;

            //�v���[���[�������ʒu�Ɉړ�������
            transform.position = firstPos;

            //CharacterController������������
            charaController.enabled = true;
        }
    }
}