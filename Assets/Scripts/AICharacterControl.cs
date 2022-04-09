using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        [SerializeField] private float chaseRange = 10f;
        private float distanceToTarget = Mathf.Infinity;
        
        bool isProvoked = false;
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            //make distanceToTarget the distance from target position to this object position
            distanceToTarget = Vector3.Distance(target.position, transform.position);

            if (isProvoked)
            {
                EngageTarget();
            }
            else if (distanceToTarget <= chaseRange)
            {
                isProvoked = true;
            }
            
        }
        
        private void EngageTarget()
        {
            //if the distance to target is more than the radius from which the ai can reach the target, move towards the target
            if (distanceToTarget >= agent.stoppingDistance)
                ChaseTarget();
            //if in range, attack the target
            if (distanceToTarget <= agent.stoppingDistance)
                AttackTarget();
        }
        
        private void ChaseTarget()
        {
            agent.SetDestination(target.position);
            character.Move(agent.desiredVelocity, false, false);
        }
        
        private void AttackTarget()
        {
            Debug.Log("Attacked " + target.name);
        }
        
        void OnDrawGizmosSelected()
        {
            // Display the explosion radius when selected
            Gizmos.color = new Color(211, 211, 211);
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
