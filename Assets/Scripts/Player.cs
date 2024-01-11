using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAnimations
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;

    private bool isDead = false;
    public Animator AnimatorController;

    public float doubleAttackCooldown;
    private bool isDoubleAttack;
    private bool isAttack;

    private Enemie closestEnemy;
    private void Update()
    {
        UIController.Instance.attack += Attack;
        UIController.Instance.doubleAttack += DoubleAttack;
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }

        Move();

        UIController.Instance.SetActiveButton(CloserEnemies() && !isDoubleAttack);
    }

    private void DoubleAttack()
    {
        if (!isAttack && !isDoubleAttack)
            AnimatorController.SetTrigger("DoubleAttack");

        closestEnemy = FindCloserEnemies();

        if (closestEnemy != null)
            transform.transform.rotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);

        StartCoroutine(UIController.Instance.FillImage(doubleAttackCooldown));
        isDoubleAttack = true;
    }

    private void Attack()
    {
        if (!isAttack && !isDoubleAttack)
            AnimatorController.SetTrigger("Attack");

        closestEnemy = FindCloserEnemies();

        if (closestEnemy != null)
            transform.transform.rotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);

        isAttack = true;
    }

    bool CloserEnemies()
    {
        if (FindCloserEnemies() != null)
            return true;
        else
            return false;
    }

    Enemie FindCloserEnemies()
    {
        Enemie enemie = null;

        foreach (Enemie enemy in SceneManager.Instance.Enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= AttackRange)
            {
                enemie = enemy;
            } 
        }

        return enemie;
    }

    //Animation Event
    public void TakeDamage()
    {
        if (closestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
            if (distance <= AttackRange)
            {
                float damage = Damage;

                if (isDoubleAttack)
                    damage *= 2;
                else if (isAttack)
                    damage = Damage;

                //transform.LookAt(closestEnemie.transform);

                closestEnemy.Hp -= damage;
            }
        }

        isDoubleAttack = false;
        isAttack = false;
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

    public float speed = 5f;  // Скорость движения игрока

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            if (inputDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10 * Time.deltaTime);
            }

            Vector3 movement = inputDirection * speed;
            transform.Translate(movement * Time.deltaTime, Space.World);
            AnimatorController.SetFloat("Speed", 1);
        }
        else
        {
            AnimatorController.SetFloat("Speed", 0);
        }
    }


    public void DieAnim()
    {
        Debug.Log("Dead");
    }
}
