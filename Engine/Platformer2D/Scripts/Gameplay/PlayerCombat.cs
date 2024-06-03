using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    public Animator Animator { get; private set; } 
    public PlayerMovement PlayerMovement { get; private set; } 
    public bool isAttacking { get; private set; }

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    int attackCounter = 0;
    int attackCounterAnimations = 0;
    float attackCooldown = 1f;
    float lastAttackTime = 0f;

    private void Awake()
	{
		Animator = GetComponent<Animator>(); 
        PlayerMovement = GetComponent<PlayerMovement>(); 
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && CanAttack()){
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.E) && CanAttack()){
            GroundSlam();
        }

        if(Input.GetKeyDown(KeyCode.Q) && CanAttack()){
            SwordGuard();
        }

        if(Input.GetKeyUp(KeyCode.Q)){
            Animator.SetBool("SwordGuard", false);
        }

        if(Input.GetKeyDown(KeyCode.R) && CanAttack()){
            StrongAttack();
        }

        if(Input.GetKeyDown(KeyCode.Mouse2) && CanAttack()){
            RangeAttack();
        }
    }

    public bool CanAttack(){
        return (!PlayerMovement.IsJumping && !PlayerMovement.IsDoubleJumping && !PlayerMovement.IsDashing && !PlayerMovement.IsFalling);
    }

    void Attack() {   
        if (Time.time - lastAttackTime >= attackCooldown) {
            lastAttackTime = Time.time;
            attackCounterAnimations++;

            isAttacking = true; // Set isAttacking to true when an attack starts

            if (attackCounterAnimations == 1){
                Animator.SetTrigger("Attack01");
            } else if(attackCounterAnimations == 2) {
                Animator.SetTrigger("Attack02");
            } else if(attackCounterAnimations == 3) {
                Animator.SetTrigger("Attack04");
                attackCounterAnimations = 0;
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {   
                Debug.Log("We hit" + enemy.name);
                attackCounter++; 
            }

            // Start a coroutine to reset isAttacking when the animation finishes
            StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));
        }  
    }

    void GroundSlam() {
        if (Time.time - lastAttackTime >= attackCooldown) {
            lastAttackTime = Time.time;

            isAttacking = true; // Set isAttacking to true when an attack starts

            Animator.SetTrigger("GroundSlam");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {   
                Debug.Log("We hit" + enemy.name);
            }

            // Start a coroutine to reset isAttacking when the animation finishes
            StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));
        } 
    }

    void SwordGuard() {
        if (Time.time - lastAttackTime >= attackCooldown) {
            lastAttackTime = Time.time;

            isAttacking = true; // Set isAttacking to true when an attack starts

            Animator.SetBool("SwordGuard", true);

            StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));

        } 
    }

    void StrongAttack() {
        if (Time.time - lastAttackTime >= attackCooldown) {
            lastAttackTime = Time.time;

            isAttacking = true; // Set isAttacking to true when an attack starts

            Animator.SetTrigger("StrongAttack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {   
                Debug.Log("We hit" + enemy.name);
            }

            // Start a coroutine to reset isAttacking when the animation finishes
            StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));
        } 
    }

    void RangeAttack() {
        if (Time.time - lastAttackTime >= attackCooldown) {
            lastAttackTime = Time.time;

            isAttacking = true; // Set isAttacking to true when an attack starts

            Animator.SetTrigger("RangeAttack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {   
                Debug.Log("We hit" + enemy.name);
            }

            // Start a coroutine to reset isAttacking when the animation finishes
            StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));
        } 
    }

    private IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    void OnDrawGizmosSelected() {
        if(attackPoint == null){
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
