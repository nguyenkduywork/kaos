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
            //if the target isnt null and the distance to target is less than chase range then set the destination of agent to the target position
            if (target != null && distanceToTarget <= chaseRange)
            {
                agent.SetDestination(target.position);
            }
            
            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
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
