using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
	[Space(5)]
    public PlayerCombatData CombatData;
    public Animator Animator { get; private set; } 
    public PlayerMovement PlayerMovement { get; private set; }
    public Health HealthBar;
    public Mana ManaBar;
    public FollowCamera Camera;
    [Space(10)]


    [Header("AttackPoints")]
	[Space(5)]
    public Transform normalAttackPoint;
    public Transform strongAttackPoint;
    public Transform groundSlamPoint;
    public Transform rangeAttackPoint;
    [Space(10)]


    [Header("Attack Statement")]
	[Space(5)]
    public bool isAttacking;
    [Space(10)]


    [Header("Layers & Tags")]
	[Space(5)]
    public LayerMask enemyLayers;
    [Space(10)]


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
            NormalAttack();
        }

        if(Input.GetKeyDown(KeyCode.Q) && CanAttack() && ManaBar.HasMana(CombatData.strongAttackCost)){
            StrongAttack();
        }

        if(Input.GetKeyDown(KeyCode.E) && CanAttack() && ManaBar.HasMana(CombatData.groundSlamCost)){
            GroundSlam();
        }

        if(Input.GetKeyDown(KeyCode.Mouse2) && CanAttack() && ManaBar.HasMana(CombatData.rangeAttackCost)){
            RangeAttack();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            HealthBar.TakeDamage(20);
        }

        HealthBar.Die();
    }

    public bool CanAttack()
    {
        return (!PlayerMovement.IsJumping && !PlayerMovement.IsDoubleJumping && !PlayerMovement.IsDashing && !PlayerMovement.IsFalling && !AttackOnCooldown());
    }

    public bool AttackOnCooldown()
    {
       return (Time.time - lastAttackTime < CombatData.attackCooldown);
    }

    void NormalAttack() 
    {   
        lastAttackTime = Time.time;
        CombatData.normalAttackCounter++;

        isAttacking = true;

        if (CombatData.normalAttackCounter == 1){
            Animator.SetTrigger("Attack01");
        } else if(CombatData.normalAttackCounter == 2) {
            Animator.SetTrigger("Attack02");
        } else if(CombatData.normalAttackCounter == 3) {
            Animator.SetTrigger("Attack04");
            CombatData.normalAttackCounter = 0;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(normalAttackPoint.position, CombatData.normalAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
            ManaBar.AbsorbMana(CombatData.normalAttackCost);
        }

        StartCoroutine(ResetAttackState(0.6f));
    }

    void StrongAttack() 
    {
        lastAttackTime = Time.time;

        ManaBar.ConsumeMana(CombatData.strongAttackCost);

        isAttacking = true;

        Animator.SetTrigger("StrongAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(strongAttackPoint.position, CombatData.strongAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
        }

        StartCoroutine(ResetAttackState(0.6f));
    }

    void GroundSlam() 
    {
        lastAttackTime = Time.time;

        isAttacking = true;

        ManaBar.ConsumeMana(CombatData.groundSlamCost);

        Animator.SetTrigger("GroundSlam");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(groundSlamPoint.position, CombatData.groundSlamRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
        }

        StartCoroutine(ResetAttackState(0.8f));
    }

    void RangeAttack() 
    {
        lastAttackTime = Time.time;

        ManaBar.ConsumeMana(CombatData.rangeAttackCost);

        isAttacking = true;

        Animator.SetTrigger("RangeAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rangeAttackPoint.position, CombatData.rangeAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
        }

        StartCoroutine(ResetAttackState(0.8f)); 
    }

    private IEnumerator ResetAttackState(float delay)
    {
        // Start a coroutine to reset isAttacking when the animation finishes
        Camera.CameraShake();
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    void OnDrawGizmosSelected() 
    {
        if(normalAttackPoint == null){
            Gizmos.DrawWireSphere(normalAttackPoint.position, CombatData.normalAttackRange);
        }

        if(strongAttackPoint == null){
            Gizmos.DrawWireSphere(strongAttackPoint.position, CombatData.strongAttackRange);
        }

        if(groundSlamPoint == null){
            Gizmos.DrawWireSphere(groundSlamPoint.position, CombatData.groundSlamRange);
        }

        if(rangeAttackPoint == null){
            Gizmos.DrawWireSphere(rangeAttackPoint.position, CombatData.rangeAttackRange);
        }
    }
}
