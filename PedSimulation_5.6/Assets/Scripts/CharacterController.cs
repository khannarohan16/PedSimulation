using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterController : MonoBehaviour
{
    Animator animator;

    NavMeshAgent navAgent;

    [SerializeField]
    AnimatorOverrideController overrideController;

    [SerializeField] Waypoint wayPoint;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float minDelay = 1f;    //Minimum interval between changing State
    [SerializeField] float maxDelay = 5f;    //Maximum interval between changing State

    [SerializeField] bool hasAltIdle = false;
    [SerializeField] bool canRun = true;

    //float speed;  ? switch to rigid body and use speed as anim parameters
    bool isMoving;
    bool canMove = true;   //to determine whether character can move from idle state (in case of some idle animations having exit time

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        animator.runtimeAnimatorController = overrideController;
    }

    private void OnEnable()
    {
        isMoving = false;
        StartCoroutine("ProcessState");
    }

    private void Update()
    {
        if (isMoving && navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            isMoving = false;
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Run");
            animator.SetTrigger("Idle");
            StartCoroutine("ProcessState");
            wayPoint = wayPoint.nextWaypoint;
        }
    }

    int state;
    IEnumerator ProcessState()
    {
        if (!isMoving)
        {
            state = Random.Range(0, 2); // 0 for idle 1 for moving
            if (state == 0)
            {
                //Select Random Idle State
                animator.ResetTrigger("Walk");
                animator.ResetTrigger("Run");

                if (hasAltIdle)
                {
                    int idleState = Random.Range(0, 2);
                    if (idleState == 0)
                    {
                        canMove = true;
                        animator.SetTrigger("Idle");
                    }
                    else
                    {
                        canMove = false;
                        animator.SetTrigger("AltIdle");
                    }
                }
                else
                {
                    canMove = true;
                    animator.SetTrigger("Idle");
                }
               
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                StartCoroutine("ProcessState");
            }
            else
            {
                if (canMove)
                {
                    isMoving = true;

                    animator.ResetTrigger("Idle");

                    if (canRun)
                    {
                        int moveState = Random.Range(0, 2);

                        if (moveState == 0)
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
                    }
                    else
                    {
                        animator.SetTrigger("Walk");
                        navAgent.speed = walkSpeed;
                    }

                    if (wayPoint != null)
                    {
                        Vector3 destination = wayPoint.GetPosition();
                        navAgent.SetDestination(destination);
                    }
                }
                
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

