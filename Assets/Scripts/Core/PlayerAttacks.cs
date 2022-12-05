using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class PlayerAttacks : MonoBehaviour
    {
        public KeyCode punchKey = KeyCode.P;
        public KeyCode kickKey = KeyCode.K;
        public KeyCode shootKey = KeyCode.J;
        [SerializeField] private Transform bulletPoint;
        [SerializeField] private GameObject bulletGameObject;
        private Animator _animatorComponent;
        private Quaternion _initialRotation;
        private static readonly int Punch = Animator.StringToHash("punch");
        private static readonly int Kick = Animator.StringToHash("kick");
        private static readonly int KickTransition = Animator.StringToHash("kickTransition");
        private static readonly int ShootTransition = Animator.StringToHash("shootTransition");
        private static readonly int Shoot = Animator.StringToHash("shoot");

        private void Awake()
        {
            _animatorComponent = GetComponent<Animator>();
        }

        private void EnablePunchAnimation()
        {
            _animatorComponent.SetBool(Punch, true);
        }
        
        private void DisablePunchAnimation()
        {
            _animatorComponent.SetBool(Punch, false);
        }

        private void EnableKickingAnimation()
        {
            // _animatorComponent.SetBool(Kick, true);
            _animatorComponent.SetTrigger(KickTransition);
        }

        private void EnableShootingAnimation()
        {
            _animatorComponent.SetBool(Shoot, true);
        }
        
        private void DisableShootingAnimation()
        {
            _animatorComponent.SetBool(Shoot, false);
        }
        
        private void DisableKickingAnimation()
        {
            _animatorComponent.SetBool(Kick, false);
        }

        private void PunchMechanics()
        {
            if (Input.GetKeyDown(punchKey))
            {
                EnablePunchAnimation();
            }
            else if (Input.GetKeyUp(punchKey))
            {
                DisablePunchAnimation();
            }
        }
        
        private void KickMechanics()
        {
            if (Input.GetKeyDown(kickKey))
            {
                _initialRotation = transform.rotation;
                EnableKickingAnimation();
                transform.rotation = _initialRotation;
            }
            else if (Input.GetKeyUp(kickKey))
            {
                DisableKickingAnimation();
                transform.rotation = _initialRotation;
            }
        }

        private void ShootMechanics()
        {
            if(Input.GetKeyDown(shootKey))
            {
                EnableShootingAnimation();
                Invoke("SpawnBullet", 0.3f);
            }
            else if (Input.GetKeyUp(shootKey))
            {
                DisableShootingAnimation();
            }
        }

        private void SpawnBullet()
        {
            var bullet = Instantiate(bulletGameObject, bulletPoint.position, Quaternion.identity);
            var bulletRigidBodyComponent = bullet.GetComponent<Rigidbody>();
            bulletRigidBodyComponent.AddForce(transform.forward * 4f, ForceMode.Impulse);
            bulletRigidBodyComponent.AddForce(transform.up * -0.1f, ForceMode.Impulse);
        }

        private void Update()
        {
            PunchMechanics();
            KickMechanics();
            ShootMechanics();
        }
    }
}
