using UnityEngine;

/// <summary>
/// �^���u�]�t��
/// </summary>
public class RecycleMarble : MonoBehaviour
{
    /// <summary>
    /// �^�����u�]�ƶq
    /// </summary>
    public static int recycleMarbles;
    public static RecycleMarble instance;

    public GameManager gm;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("�u�]"))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = new Vector3(0, 0, 100);

            // �^���u�]�ƶq �W�[
            recycleMarbles++;

            CheckIsRecycleAllMarbles();
        }
    }

    /// <summary>
    /// �ˬd�O�_�^���Ҧ��u�]
    /// </summary>
    public void CheckIsRecycleAllMarbles()
    {
        // �p�G �^���ƶq ���� �i�o�g�̤j�u�]�ƶq ������ �Ĥ�^�X
        if (recycleMarbles == ControlSystem.shootMarbles)
        {
            if (GameObject.FindGameObjectsWithTag("�ѽL�W������").Length == 0)
            {
                gm.allObjectDead = true;
            }

            gm.SwitchTurn(false);
        }
    }
}
