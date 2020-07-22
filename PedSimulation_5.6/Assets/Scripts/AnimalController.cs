using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class AnimalController : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    AnimatorOverrideController overrideController;

    NavMeshAgent navAgent;

    

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float minDelay = 1f;    //Minimum interval between changing State
    [SerializeField] float maxDelay = 5f;    //Maximum interval between changing State

    //float speed;  ? switch to rigid body and use speed as anim parameters
    bool isMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        animator.runtimeAnimatorController = overrideController;
    }

    private void OnEnable()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            print("Hit " + hit.collider.gameObject.name);
            //destinationPointComputer = hit.collider.transform.GetComponentInParent<SplineComputer>();
        }

        isMoving = false;
        StartCoroutine("ProcessState");
    }

    private void Update()
    {
        if (isMoving && navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            isMoving = false;
            StartCoroutine("ProcessState");
        }
    }

    IEnumerator ProcessState()
    {
        if (!isMoving)
        {
            int state = Random.Range(0, 2); // 0 for idle 1 for moving
            if(state == 0)
            {
                //Select Random Idle State
                int idleState = Random.Range(0, 2);
                if(idleState == 0)
                {
                    animator.SetTrigger("Idle");
                }
                else
                {
                    animator.SetTrigger("Eat");
                }
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                StartCoroutine("ProcessState");
            }
            else
            {
                isMoving = true;

                int moveState = Random.Range(0, 2);

                //Vector3 destination = GetRandomDestination();

                if(moveState == 0)
                {
                    //Walk
                    animator.SetTrigger("Walk");
                    navAgent.speed = walkSpeed;
                }
                else
                {
                    //Run
                    animator.SetTrigger("Run");
                    navAgent.speed = runSpeed;
                }

                //navAgent.SetDestination(destination);
            }
        }
        yield return new WaitForSeconds(0f);
    }

    //Vector3 GetRandomDestination()
    //{
    //    SplinePoint[] splinePoints = destinationPointComputer.GetPoints();
    //    int randomPoint = Random.Range(0, splinePoints.Length);
    //    Vector3 randomPosition = splinePoints[randomPoint].position;
    //    return randomPosition;
    //}
}
