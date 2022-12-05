using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode runKey = KeyCode.LeftShift;
        public float turnCap = 3f;
        public float runningSpeed = 1.5f;
        [SerializeField] private GameObject thisGuyHips;
        [SerializeField] private Rigidbody handRig;
        
        private float _moveForce = 5f;
        private const float MoveFriction = 0.1f;

        private Animator _animatorComponent;
        private CapsuleCollider _mainColliderComponent;
        private Transform _characterTransform;
        private Rigidbody _rigidBodyComponent;

        private float _runningTime = 0f;
        
        private float _inX;
        private float _inZ;
        private Vector3 _verticalMovement;
        private Vector3 _verticalVelocity;
        
        private Rigidbody[] _ragdollRigidBodyList;
        private Collider[] _ragdollColliderList;
        private static readonly int Kick = Animator.StringToHash("kick");
        private static readonly int Kick2 = Animator.StringToHash("kick2");
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int Run2 = Animator.StringToHash("run2");
        private static readonly int Bool1 = Animator.StringToHash("bool1");
        private static readonly int Bool2 = Animator.StringToHash("bool2");
        private static readonly int Lbool = Animator.StringToHash("lbool");
        private static readonly int Lbool2 = Animator.StringToHash("lbool2");
        private static readonly int Rbool = Animator.StringToHash("rbool");
        private static readonly int Rbool2 = Animator.StringToHash("rbool2");
        private static readonly int RunningTransition = Animator.StringToHash("runningTransition");
        private static readonly int Running = Animator.StringToHash("running");
        private static readonly int Agacha = Animator.StringToHash("Agacha");

        private void Awake()
        {
            _animatorComponent = GetComponent<Animator>();
            _characterTransform = GetComponent<Transform>();
            _rigidBodyComponent = GetComponent<Rigidbody>();
            _mainColliderComponent = GetComponent<CapsuleCollider>();
        }

        private void GetRagdollBits()
        {
            _ragdollRigidBodyList = thisGuyHips.GetComponentsInChildren<Rigidbody>();
            _ragdollColliderList = thisGuyHips.GetComponentsInChildren<Collider>();
        }

        private void DisableRagdoll()
        {
            foreach (var col in _ragdollColliderList)
            {
                col.enabled = false;
            }
            foreach (var rag in _ragdollRigidBodyList)
            {
                rag.isKinematic = true;
            }
            _animatorComponent.enabled = true;
            _mainColliderComponent.enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            handRig.isKinematic = false;
        }

        private void EnableRagdoll()
        {
            _animatorComponent.enabled = false;
            foreach (var col in _ragdollColliderList)
            {
                col.enabled = true;
            }
            foreach (var rag in _ragdollRigidBodyList)
            {
                rag.isKinematic = false;
            }
            _mainColliderComponent.enabled = false;
            _rigidBodyComponent.isKinematic = true;
        }

        // Start is called before the first frame update
        private void Start()
        {
            GetRagdollBits();
            DisableRagdoll();
            _animatorComponent.SetBool(Kick, false);
            _animatorComponent.SetBool(Kick2, true);
        }

        private void EnableRunningAnimation()
        {
            _animatorComponent.SetBool(Running, true);
            // _animatorComponent.SetBool(Run, true);
            // _animatorComponent.SetBool(Run2, false);
        }

        private void DisableRunningAnimation()
        {
            _animatorComponent.SetBool(Running, false);
            // _animatorComponent.SetBool(Run, false);
            // _animatorComponent.SetBool(Run2, true);
        }

        private void EnableForwardWalkingAnimation()
        {
            _animatorComponent.SetBool(Bool1, true);
            _animatorComponent.SetBool(Bool2, true);
        }

        private void DisableForwardWalkingAnimation()
        {
            _animatorComponent.SetBool(Bool1, false);
            _animatorComponent.SetBool(Bool2, false);
        }

        private void EnableCrouchingAnimation()
        {
            _animatorComponent.SetBool(Agacha, true);
        }

        private void DisableCrouchingAnimation()
        {
            _animatorComponent.SetBool(Agacha, false);
        }

        private void WalkingMechanics()
        {
            if (Input.GetKey(upKey))
            {
                EnableForwardWalkingAnimation();
                _rigidBodyComponent.AddForce(transform.forward * _moveForce);
            }
            else
            {
                DisableForwardWalkingAnimation();
                _moveForce -= MoveFriction;
                _rigidBodyComponent.AddForce(transform.forward * _moveForce);
            }
            if (Input.GetKey(leftKey))
            {
                _rigidBodyComponent.freezeRotation = false;
                transform.Rotate(Vector3.up, -turnCap);
            }
            if (Input.GetKey(rightKey))
            {
                _rigidBodyComponent.freezeRotation = false;
                transform.Rotate(Vector3.up, turnCap);
            }
            if (Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey))
            {
                _rigidBodyComponent.freezeRotation = true;
            }
        }

        private void RunningMechanics()
        {
            if(Input.GetKeyDown(runKey))
            {
                EnableRunningAnimation();
                _moveForce = runningSpeed*200f;
                _rigidBodyComponent.AddForce(transform.forward * _moveForce);
            }
            if (Input.GetKeyUp(runKey))
            {
                DisableRunningAnimation();
                _moveForce = 5f;
                _rigidBodyComponent.AddForce(transform.forward * _moveForce);
            }
        }

        private void CrouchMechanics()
        {
            if (Input.GetKeyDown(downKey))
            {
                EnableCrouchingAnimation();
            }
            if(Input.GetKeyUp(downKey))
            {
                DisableCrouchingAnimation();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            _runningTime += Time.deltaTime;
            WalkingMechanics();
            RunningMechanics();
            CrouchMechanics();
        }
    }
}
