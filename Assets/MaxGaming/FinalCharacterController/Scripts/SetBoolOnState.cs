using UnityEngine;
namespace MaxGaming.FinalCharacterController
{
    public class SetBoolOnState : StateMachineBehaviour
    {
        public string boolName = "isPlayingAction";
        public bool valueOnEnter = true;
        public bool valueOnExit = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolName, valueOnEnter);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }
}