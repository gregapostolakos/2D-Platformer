using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator Animator { get; private set; } 

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    int attackCounter = 0;
    int attackCounterAnimations = 0;
    float attackCooldown = 0.5f;
    float lastAttackTime = -1f;

    private void Awake()
	{
		Animator = GetComponent<Animator>(); 
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Attack();
        }
    }

    void Attack()
    {   
        if (Time.time - lastAttackTime >= attackCooldown) { // check if enough time has passed since last dash
            lastAttackTime = Time.time;
            attackCounterAnimations++;

            if (attackCounterAnimations == 1){
                Animator.SetTrigger("Attack01");
            } else if(attackCounterAnimations == 2) {
                Animator.SetTrigger("Attack02");
            } else if(attackCounterAnimations == 3) {
                Animator.SetTrigger("Attack03");
                attackCounterAnimations = 0;
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {   
                Debug.Log("We hit" + enemy.name);
                attackCounter++; 
            }

            // if (attackCounter == 1){
            //     Attack01.SetActive(true);
            // } else if(attackCounter == 2) {
            //     Attack02.SetActive(true);
            // } else if(attackCounter == 3) {
            //     Attack03.SetActive(true);
            // } else if(attackCounter == 4) {
            //     StrongAttack.SetActive(true);
            // } else if(attackCounter == 5) {
            //     Attack01.SetActive(false);
            //     Attack02.SetActive(false);
            //     Attack03.SetActive(false);
            //     StrongAttack.SetActive(false);
            //     attackCounter = 0;
            // }
        }  
    }

    void OnDrawGizmosSelected() 
    {
        if(attackPoint == null){
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
