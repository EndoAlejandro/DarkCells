using DarkHavoc.PlayerComponents.States;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public class PlayerStateMachine : FiniteStateBehaviour
    {
        private Player _player;
        private PlayerAnimation _animation;
        private Rigidbody2D _rigidbody;
        private InputReader _input;

        protected override void References()
        {
            _animation = GetComponentInChildren<PlayerAnimation>();
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<InputReader>();
        }

        protected override void StateMachine()
        {
            var ground = new GroundState(_player, _rigidbody, _input);
            var air = new AirState(_player, _rigidbody, _input);
            var roll = new RollState(_player, _rigidbody, _input, _player.Stats.RollAction);
            var crouch = new CrouchState(_player, _rigidbody);
            var block = new BlockState(_player, _rigidbody, _input);
            var parry = new ParryState(_player, _player.Stats.ParryAction);

            var lightAttack = new AttackState(_player, _rigidbody, _input, _animation, AnimationState.LightAttack,
                _player.Stats.LightAttackAction);
            var parryAttack = new AttackState(_player, _rigidbody, _input, _animation, AnimationState.ParryAttack,
                _player.Stats.LightAttackAction);
            var heavyAttack = new AttackState(_player, _rigidbody, _input, _animation, AnimationState.HeavyAttack,
                _player.Stats.HeavyAttackAction);
            // TODO: Heavy attack maybe from combo.

            // Initial State.
            stateMachine.SetState(ground);

            // Locomotion.
            stateMachine.AddTransition(ground, air, () => !_player.Grounded);
            stateMachine.AddTransition(air, ground, () => _player.Grounded);

            // Roll.
            var toRollStates = new IState[] { ground, air, crouch, parry };
            stateMachine.AddManyTransitions(toRollStates, roll, () => _player.HasBufferedRoll);
            stateMachine.AddTransition(roll, ground,
                () => roll.Ended && _player.Grounded && !_player.CheckCeilingCollision());
            stateMachine.AddTransition(roll, air, () => roll.Ended && !_player.Grounded);
            stateMachine.AddTransition(roll, crouch,
                () => roll.Ended && _player.Grounded && _player.CheckCeilingCollision());
            stateMachine.AddTransition(roll, lightAttack,
                () => _player.HasBufferedAttack && !_player.CheckCeilingCollision());

            // Crouch.
            stateMachine.AddTransition(crouch, ground, () => !_player.CheckCeilingCollision());

            // Attack.
            var toAttackStates = new IState[] { ground, air };
            stateMachine.AddManyTransitions(toAttackStates, lightAttack, () => _player.HasBufferedAttack);

            stateMachine.AddTransition(lightAttack, lightAttack,
                () => _player.HasBufferedAttack && lightAttack.CanCombo && !lightAttack.CanHeavyAttack);
            stateMachine.AddTransition(lightAttack, heavyAttack,
                () => _player.HasBufferedAttack && lightAttack.CanCombo && lightAttack.CanHeavyAttack);

            stateMachine.AddTransition(lightAttack, ground, () => lightAttack.Ended);
            stateMachine.AddTransition(heavyAttack, ground, () => heavyAttack.Ended);
            //stateMachine.AddTransition(lightAttack, air, () => lightAttack.Ended && !_player.Grounded);


            // Block.
            var toBlockStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toBlockStates, block, () => _input.BlockHold);

            stateMachine.AddTransition(block, ground, () => !_input.BlockHold && block.Ended);
            stateMachine.AddTransition(block, air, () => _player.HasBufferedJump && block.Ended);
            stateMachine.AddTransition(block, parry, () => block.ParryAvailable);

            // Parry.
            stateMachine.AddTransition(parry, parry, () => parry.ParryAvailable);

            stateMachine.AddTransition(parry, parryAttack, () => _player.HasBufferedAttack);
            stateMachine.AddTransition(parry, block, () => parry.Ended && _input.BlockHold);
            stateMachine.AddTransition(parry, ground, () => parry.Ended && !_input.BlockHold);

            stateMachine.AddTransition(parryAttack, ground, () => parryAttack.Ended);
        }
    }
}