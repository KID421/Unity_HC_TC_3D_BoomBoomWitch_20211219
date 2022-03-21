using UnityEngine;
using System.Collections;           // �ޥ� �t��.���X
using System.Collections.Generic;   // �ޥ� �t��.���X.�@�� (�]�t List)
using UnityEngine.UI;

/// <summary>
/// ����t��
/// ���V�ƹ���m
/// �o�g�u�]
/// �^�X����
/// </summary>
public class ControlSystem : MonoBehaviour
{
    #region ���
    [Header("�b�Y")]
    public GameObject goArrow;
    [Header("�ͦ��u�]��m")]
    public Transform traSpawnPoint;
    [Header("�u�]�w�s��")]
    public GameObject goMarbles;
    [Header("�o�g�t��"), Range(0, 5000)]
    public float speedShoot = 750;
    [Header("�g�u�n�I�����ϼh")]
    public LayerMask layerToHit;
    [Header("���շƹ���m")]
    public Transform traTestMousePosition;
    [Header("�Ҧ��u�]")]
    public List<GameObject> listMarbles = new List<GameObject>();
    [Header("�o�g���j"), Range(0, 5)]
    public float fireInterval = 0.5f;
    [Header("�o�g����")]
    public AudioClip soundShoot;

    /// <summary>
    /// �Ҧ��u�]�ƶq
    /// </summary>
    public static int allMarbles;
    /// <summary>
    /// �i�H�o�g���̤j�u�]�ƶq
    /// </summary>
    public static int maxMarbles = 10;
    /// <summary>
    /// �C���o�g�X�h���u�]�ƶq
    /// </summary>
    public static int shootMarbles;

    public static ControlSystem instance;

    /// <summary>
    /// �O�_��o�g
    /// </summary>
    public bool canShoot = true;

    [SerializeField, Header("�ʵe���")]
    private Animator ani;

    /// <summary>
    /// �u�]�ƶq
    /// </summary>
    private Text textMarbleCount;
    #endregion

    #region �ƥ�
    private void Awake()
    {
        instance = this;
        textMarbleCount = GameObject.Find("�u�]�ƶq").GetComponent<Text>();
    }

    private void Start()
    {
        for (int i = 0; i < 50; i++) SpawnMarble();

        UpdateUIMarbleCount();
    }

    private void Update()
    {
        MouseControl();
    }
    #endregion

    #region ��k
    /// <summary>
    /// �ͦ��u�]�s���M�椺
    /// </summary>
    private void SpawnMarble()
    {
        // �u�]�`�ƼW�[
        allMarbles++;
        // �Ҧ��u�]�M��.�K�[(�ͦ��u�])
        listMarbles.Add(Instantiate(goMarbles, new Vector3(0, 0, 100), Quaternion.identity));
    }

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

            // print("�ƹ��y�СG" + v3Mouse);

            // �g�u = �D�n��v��.�ù��y����g�u(�ƹ��y��)
            Ray rayMouse = Camera.main.ScreenPointToRay(v3Mouse);
            // �g�u�I����T
            RaycastHit hit;

            // �p�G �g�u���쪫��N�B�z
            // ���z �g�u�I��(�g�u�A�g�u�I����T�A�Z���A�ϼh)
            if (Physics.Raycast(rayMouse, out hit, 100, layerToHit))
            {
                // print("�ƹ��g�u���쪫��G" + hit.collider.name);
                
                Vector3 hitPosition = hit.point;                // ���o�I����T���y��
                hitPosition.y = 0.5f;                           // �վ㰪�׶b�V
                traTestMousePosition.position = hitPosition;    // ��s���ժ���y��

                // ���� �� Z �b = ���ժ��󪺮y�� - ���⪺�y�� (�V�q)
                transform.forward = traTestMousePosition.position - transform.position;
                Vector3 angle = transform.eulerAngles;
                angle.x = 0;
                angle.z = 0;
                transform.eulerAngles = angle;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StartCoroutine(FireMarble());
            canShoot = false;
        }
    }

    /// <summary>
    /// �o�g�u�]
    /// </summary>
    private IEnumerator FireMarble()
    {
        shootMarbles = 0;

        for (int i = 0; i < maxMarbles; i++)
        {
            ani.SetTrigger("Ĳ�o����");
            shootMarbles++;
            GameObject temp = listMarbles[i];
            temp.transform.position = traSpawnPoint.position;
            temp.transform.rotation = traSpawnPoint.rotation;
            temp.GetComponent<Rigidbody>().velocity = Vector3.zero;
            temp.GetComponent<Rigidbody>().AddForce(traSpawnPoint.forward * speedShoot);    // �o�g �u�]
            SoundManager.instance.PlaySoundRandomVolue(soundShoot, 0.8f, 1.2f);
            temp.GetComponent<Marble>().FlyToBottomCountDown();                             // �קK���d�Ӥ[�i�J�˼ƭ��^�����^����
            UpdateUIMarbleCount();

            yield return new WaitForSeconds(fireInterval);                                  //���j
        }
        goArrow.SetActive(false);
    }

    /// <summary>
    /// ��s�����G�u�]�ƶq
    /// </summary>
    public void UpdateUIMarbleCount()
    {
        int marblesLess = maxMarbles - shootMarbles;
        string content = marblesLess != 0 ? "x " + marblesLess : "";
        textMarbleCount.text = content;
    }

    /// <summary>
    /// ���]�����G�u�]�ƶq��_���̤j��
    /// </summary>
    public void ResetUIMarbleCountToMax()
    {
        textMarbleCount.text = "x " + maxMarbles;
    }
    #endregion
}
