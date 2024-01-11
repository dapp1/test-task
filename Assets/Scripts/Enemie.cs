using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;

    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    private bool isDead = false;

    public GameObject standartGoblin;

    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);

    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        float distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
     
        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.Hp -= Damage;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.isStopped = false;
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.speed); 
        Debug.Log(Agent.speed);
    }

    public void TakeDamage()
    {
        Debug.Log("Attack Player");
    }

    public void DieAnim()
    {
        if (gameObject.name == "SuperGoblin(Clone)")
        {
            Instantiate(standartGoblin, transform.position + new Vector3(2, 0, 0), Quaternion.identity);
            Instantiate(standartGoblin, transform.position - new Vector3(2,0,0), Quaternion.identity);
        }
    }

    private void Die()
    {
        SceneManager.Instance.RemoveEnemie(this);
        FindObjectOfType<Player>().Hp += 1f;
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }

}
