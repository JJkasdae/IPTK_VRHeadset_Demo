using UnityEngine;

public class DisableAnimatorIK : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // ��ȡ Animator ���
        animator = GetComponent<Animator>();
    }

    // Unity ��ÿһ֡���� IK ʱ�����������
    void OnAnimatorIK(int layerIndex)
    {
        // �������е� IK ������ȷ�� Animator ������ IK
        for (int i = 0; i < 4; i++)
        {
            AvatarIKGoal goal = (AvatarIKGoal)i;
            animator.SetIKPositionWeight(goal, 0f);
            animator.SetIKRotationWeight(goal, 0f);
        }

        // �������е� IK ��ʾ (hint) ����
        for (int i = 0; i < 2; i++)
        {
            AvatarIKHint hint = (AvatarIKHint)i;
            animator.SetIKHintPositionWeight(hint, 0f);
        }
    }
}
