using UnityEngine;

/// <summary>
/// �u�]�t��
/// </summary>
public class Marble : MonoBehaviour
{
    private void Awake()
    {
        // ���z.�����ϼh�I��(A �ϼh�AB�ϼh) ���� A B �ϼh�I��
        Physics.IgnoreLayerCollision(6, 6);
    }
}
