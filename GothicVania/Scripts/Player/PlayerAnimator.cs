using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator;
    public PlayerMovement PlayerMovement;
    public PlayerCombat PlayerCombat;
    private string currentState;

    // Animation States Movement
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_IDLE_TRANSITION = "IdleTransition";
    const string PLAYER_RUN = "Run";
    const string PLAYER_JUMP = "Jump";
    const string PLAYER_FALL = "JumpFall";
    const string PLAYER_LAND = "Land";
    const string PLAYER_DASH = "Dash";
    const string PLAYER_WALL_SLIDE = "WallSlide";

    // Animation States Combat
    const string PLAYER_ATTACK_COMBO_1 = "ComboAttack01";
    const string PLAYER_ATTACK_COMBO_2 = "ComboAttack02";
    const string PLAYER_ATTACK_COMBO_3 = "ComboAttack03";
    const string PLAYER_ATTACK_COMBO_4 = "ComboAttack04";
    const string PLAYER_GROUND_SLAM = "GroundSlam";
    const string PLAYER_STRONG_ATTACK = "SwordAttack";
    const string PLAYER_RANGE_ATTACK = "BowDraw";
    const string PLAYER_RANGE_ATTACK_FIRE = "BowFire";
    const string PLAYER_SWORD_GUARD = "SwordGuard";
    const string PLAYER_SWORD_GUARD_IMPACT = "SwordGuardImpact";

    // Animation States Utilities
    const string PLAYER_DIE = "Die";
    const string PLAYER_INTERACTION_PULL = "InteractionPull";
    const string PLAYER_KNOCKBACK = "Knockback";

    private bool rangeAttackFinished = false;
    private bool isLanding = false;
    private bool attackComboFinished = false;

    void ChangeAnimationState(string newState)
    {
        // Stop the same animation from interrupting itself
        if (currentState == newState) return;

        // Play the Animation
        Animator.Play(newState);

        // Reassign the current state
        currentState = newState;
    }

    void FixedUpdate()
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);

        // Reset isLanding flag if landing animation has finished
        if (stateInfo.IsName(PLAYER_LAND) && stateInfo.normalizedTime >= 1f)
        {
            isLanding = false;
            ChangeAnimationState(PLAYER_IDLE); // Ensure we go to idle after landing
        }

        // Movement Animations
        if (!isLanding)
        {
            if (PlayerMovement._moveInput.x == 0 && !PlayerMovement.IsRunning && !PlayerMovement.IsJumping && !PlayerMovement.IsDashing && !PlayerMovement.IsFalling && !PlayerCombat.IsAttacking())
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
            else if (PlayerMovement._moveInput.x != 0 && PlayerMovement.IsRunning && !PlayerMovement.IsJumping && !PlayerMovement.IsDashing && !PlayerMovement.IsFalling && !PlayerCombat.IsAttacking())
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else if (PlayerMovement.IsJumping && !PlayerMovement.IsDashing)
            {
                ChangeAnimationState(PLAYER_JUMP);
            }
            else if (PlayerMovement.IsFalling && !PlayerMovement.IsDashing && !PlayerMovement.IsJumping)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
        }

        // Dash Animation
        if (PlayerMovement.IsDashing && !PlayerCombat.IsAttacking())
        {
            ChangeAnimationState(PLAYER_DASH);
        }
        else if (stateInfo.IsName(PLAYER_DASH) && stateInfo.normalizedTime >= 1.0f)
        {
            // Ensure we exit the dash state
            if (PlayerMovement.IsFalling)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
            else if (PlayerMovement.IsRunning)
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        // Combat Animations
        if (PlayerCombat.isNormalAttacking && !attackComboFinished)
        {
            if (!stateInfo.IsName(PLAYER_ATTACK_COMBO_1))
            {
                ChangeAnimationState(PLAYER_ATTACK_COMBO_1);
            }
            else if (stateInfo.IsName(PLAYER_ATTACK_COMBO_1) && stateInfo.normalizedTime >= 1.0f)
            {
                attackComboFinished = true;
            }
        }

        if (stateInfo.IsName(PLAYER_ATTACK_COMBO_1) && stateInfo.normalizedTime >= 1.0f && attackComboFinished)
        {
            attackComboFinished = false;
            ChangeAnimationState(PLAYER_IDLE); // Ensure we go to idle after attack combo
        }

        if (PlayerCombat.isStrongAttacking)
        {
            ChangeAnimationState(PLAYER_STRONG_ATTACK);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                ChangeAnimationState(PLAYER_IDLE); // Ensure we go to idle after strong attack
            }
        }

        if (PlayerCombat.isGroundSlamAttacking)
        {
            ChangeAnimationState(PLAYER_GROUND_SLAM);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                ChangeAnimationState(PLAYER_IDLE); // Ensure we go to idle after ground slam
            }
        }

// Range Attack
if (PlayerCombat.isRangeAttacking && !rangeAttackFinished)
{
    ChangeAnimationState(PLAYER_RANGE_ATTACK);
    rangeAttackFinished = false;
}

if (stateInfo.IsName(PLAYER_RANGE_ATTACK) && stateInfo.normalizedTime >= 1f && !rangeAttackFinished)
{
    ChangeAnimationState(PLAYER_RANGE_ATTACK_FIRE);
    rangeAttackFinished = true;
}

if (stateInfo.IsName(PLAYER_RANGE_ATTACK_FIRE))
{
    if (stateInfo.normalizedTime >= 1f && rangeAttackFinished)
    {
        rangeAttackFinished = false;
        PlayerCombat.isRangeAttacking = false; // Reset range attack trigger
        ChangeAnimationState(PLAYER_IDLE); // Ensure we go to idle after range attack
    }
    else if (stateInfo.normalizedTime < 1f && !PlayerCombat.isRangeAttacking)
    {
        rangeAttackFinished = false;
        ChangeAnimationState(PLAYER_IDLE); // Ensure we go to idle if range attack is interrupted
    }
}
    }
}
