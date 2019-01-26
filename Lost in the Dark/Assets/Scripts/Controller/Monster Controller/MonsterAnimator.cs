using UnityEngine;
using UnityEngine.AI;

public class MonsterAnimator : MonoBehaviour {

    //Thee animator
    private Animator anim;

    // The agent
    private NavMeshAgent agent;

    // Monster health to know if he died
    private MonsterHealth health;

    private bool stopped;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<MonsterHealth>();
        stopped = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!health.IsDead()) {
            if (agent.velocity == Vector3.zero && Vector3.Distance(agent.transform.position, transform.position) <= MonsterController.LIMIT && !anim.GetBool("isAttacking")) {
                anim.SetBool("isAttacking", true);
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", false);
            } else if (agent.velocity != Vector3.zero && !anim.GetBool("isWalking")) {
                anim.SetBool("isWalking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttacking", false);
            }
        } else if (!stopped){
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isDead", true);
            stopped = true;
        }

        // Remove the monster from the game when death animation finish
        if (health.IsDead()) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) 
                Destroy(gameObject);
            
        }
    }
}
