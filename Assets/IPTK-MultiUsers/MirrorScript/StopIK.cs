using UnityEngine;

public class DisableAnimatorIK : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // 获取 Animator 组件
        animator = GetComponent<Animator>();
    }

    // Unity 在每一帧处理 IK 时调用这个方法
    void OnAnimatorIK(int layerIndex)
    {
        // 禁用所有的 IK 操作，确保 Animator 不处理 IK
        for (int i = 0; i < 4; i++)
        {
            AvatarIKGoal goal = (AvatarIKGoal)i;
            animator.SetIKPositionWeight(goal, 0f);
            animator.SetIKRotationWeight(goal, 0f);
        }

        // 禁用所有的 IK 提示 (hint) 处理
        for (int i = 0; i < 2; i++)
        {
            AvatarIKHint hint = (AvatarIKHint)i;
            animator.SetIKHintPositionWeight(hint, 0f);
        }
    }
}
