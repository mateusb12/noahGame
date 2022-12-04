using System;
using UnityEngine;

namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        [SerializeField] private GameObject thisGuyHips;
        [SerializeField] private Rigidbody handRig;
        
        private Animator _animatorComponent;
        private CapsuleCollider _mainColliderComponent;
        private Transform _characterTransform;
        private Rigidbody _rigidBodyComponent;


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
            _animatorComponent.SetBool(Run, true);
            _animatorComponent.SetBool(Run2, false);
        }

        private void DisableRunningAnimation()
        {
            _animatorComponent.SetBool(Run, false);
            _animatorComponent.SetBool(Run2, true);
        }

        private void EnableWalkingAnimation()
        {
            _animatorComponent.SetBool(Bool1, true);
            _animatorComponent.SetBool(Bool2, true);
        }

        private void DisableWalkingAnimation()
        {
            _animatorComponent.SetBool(Bool1, false);
            _animatorComponent.SetBool(Bool2, false);
        }

        private void WalkingMechanics()
        {
            if (Input.GetKeyDown(upKey))
            {
                EnableWalkingAnimation();
                var moveForce = 5f;
                _rigidBodyComponent.AddForce(transform.forward * moveForce);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
