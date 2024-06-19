using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.HeartHoarder.States
{
    public class HeartHoarderAttackSwitcherState : IState
    {
        public override string ToString() => "Switcher";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool WalkAttackAvailable { get; private set; }
        public bool AirAttackAvailable { get; private set; }
        public bool MeleeAttackAvailable { get; private set; }

        private readonly HeartHoarder _heartHoarder;
        private readonly CapsuleCollider2D _collider;

        private RaycastHit2D _leftCheck;
        private RaycastHit2D _rightCheck;
        private float _timer;

        public HeartHoarderAttackSwitcherState(HeartHoarder heartHoarder, CapsuleCollider2D collider)
        {
            _heartHoarder = heartHoarder;
            _collider = collider;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f) SidesVerification();
        }

        public void FixedTick()
        {
        }

        private RaycastHit2D CheckCollisionCustomDirection(Vector2 direction)
        {
            //return Physics2D.Raycast(_collider.bounds.center, direction, 30f, _heartHoarder.LayerMask);
            return Physics2D.CapsuleCast(_collider.bounds.center, _collider.bounds.size,
                CapsuleDirection2D.Vertical, 0f, direction, 30f, _heartHoarder.WallLayerMask);
        }

        private void SidesVerification()
        {
            /*_timer = 4f;
            _leftCheck = CheckCollisionCustomDirection(Vector2.left);
            _rightCheck = CheckCollisionCustomDirection(Vector2.right);

            var leftDistance = Mathf.Abs(_leftCheck.point.x - _heartHoarder.transform.position.x);
            var rightDistance = Mathf.Abs(_rightCheck.point.x - _heartHoarder.transform.position.x);

            var destination = leftDistance > rightDistance ? _leftCheck.point : _rightCheck.point;
            destination.x = (Mathf.Abs(destination.x) - _collider.bounds.size.x) * Mathf.Sign(destination.x);
            _heartHoarder.Teleport(destination);*/

            // WalkAttackAvailable = true;
            // AirAttackAvailable = true;
            // MeleeAttackAvailable = true;
            var result = Random.Range(0f, 1f);
            switch (result)
            {
                case < 0.33f:
                    WalkAttackAvailable = true;
                    break;
                case < .66f:
                    AirAttackAvailable = true;
                    break;
                default:
                    MeleeAttackAvailable = true;
                    break;
            }
        }

        public void OnEnter()
        {
            WalkAttackAvailable = false;
            AirAttackAvailable = false;
            MeleeAttackAvailable = false;
        }

        public void OnExit()
        {
        }
    }
}