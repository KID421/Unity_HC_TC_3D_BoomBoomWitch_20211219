using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ����t��
/// ���V�ƹ���m
/// �o�g�u�]
/// �^�X����
/// </summary>
public class ControlSystemTutorial : MonoBehaviour
{
    #region �w������
    #region ���
    [Header("�g�u�n�I�����ϼh")]
    public LayerMask layerToHit;
    [Header("�ƹ���m")]
    public Transform traTestMousePosition;
    [Header("�b�Y")]
    public GameObject goArrow;

    /// <summary>
    /// �Ҧ��u�]�ƶq
    /// </summary>
    public static int allMarbles;
    /// <summary>
    /// �i�H�o�g���̤j�u�]�ƶq
    /// </summary>
    public static int maxMarbles = 2;
    /// <summary>
    /// �C���o�g�X�h���u�]�ƶq
    /// </summary>
    public static int shootMarbles;

    /// <summary>
    /// �O�_��o�g
    /// </summary>
    private bool canShoot = true;
    #endregion

    #region �ƥ�
    private void Update()
    {
        MouseControl();
    }
    #endregion

    #region ��k
    /// <summary>
    /// �ƹ�����
    /// </summary>
    private void MouseControl()
    {
        if (!canShoot) return;              // �p�G �{�b�O �Ĥ�^�X �N���X

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            goArrow.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 v3Mouse = Input.mousePosition;
            Ray rayMouse = Camera.main.ScreenPointToRay(v3Mouse);
            RaycastHit hit;

            if (Physics.Raycast(rayMouse, out hit, 100, layerToHit))
            {
                Vector3 hitPosition = hit.point;                // ���o�I����T���y��
                hitPosition.y = 0.5f;                           // �վ㰪�׶b�V
                traTestMousePosition.position = hitPosition;    // ��s���ժ���y��
                transform.forward = traTestMousePosition.position - transform.position;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StartCoroutine(FireMarble());
        }
    }
    #endregion
    #endregion

    #region �оǰ�
    public Transform point;
    public int speed = 700;
    public GameObject marble;

    // 1. Instantiate - �ͦ�����
    // Instantiate(�n�ͦ�������A�ͦ����y�СA�ͦ�������)

    // 2. SetActive - ��ܩ����ê���
    // SetActive(���L��) - ���L�� true ��ܡAfalse ����

    // 3. GetComponent - ���o����
    // GetComponent<����>()

    // 4. AddForce - �K�[���O
    // AddForce(���O)

    /// <summary>
    /// �o�g�u�]
    /// </summary>
    private IEnumerator FireMarble()
    {
        GameObject temp = Instantiate(marble, point.position, point.rotation);
        temp.GetComponent<Rigidbody>().AddForce(goArrow.transform.up * speed);
        yield return new WaitForSeconds(0.5f);         // ���ݶ��j�ɶ�
        goArrow.SetActive(false);
    }
    #endregion
}
