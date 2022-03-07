using KID;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ĤH��D�㱱�
/// </summary>
public class EnemyPropControl : MonoBehaviour
{
    [Header("�C�����ʪ��Z��")]
    public float moveDistance = 2;
    [Header("���ʪ��y�Щ��u")]
    public float moveUnderLine = -2;
    [Header("�u�]���W��")]
    public string nameMarble;
    [Header("�򥻦�q")]
    public float hpBase = 100;
    [Header("�C�@�h���ɦ�q")]
    public float hpIncrease = 100;
    [Header("�ˮ`")]
    public float damage = 100;
    [Header("�O�_������")]
    public bool hasUI;
    [Header("�O�_���i�H�Y���u�]")]
    public bool isMarble;
    [Header("���`����")]
    public AudioClip soundDead;
    [Header("����")]
    public GameObject goCoin;
    [Header("���������d��")]
    public Vector2Int v2CoinRange;

    [HideInInspector]
    public float hpCurrent = 0;

    [SerializeField, Header("�ʵe���")]
    private Animator ani;

    private float hpMax;
    private Image imgHp;
    private Text textHp;
    private GameManager gm;

    private void Awake()
    {
        if (!isMarble)
        {
            hpMax += hpBase + (GameManager.instance.floorCount - 1) * hpIncrease;
            hpCurrent = hpMax;
        }
        else
        {
            hpCurrent = hpBase;
        }

        if (hasUI)
        {
            imgHp = transform.Find("�e�����").Find("���").GetComponent<Image>();
            textHp = transform.Find("�e�����").Find("��q").GetComponent<Text>();
            textHp.text = hpCurrent.ToString();
        }

        gm = FindObjectOfType<GameManager>();
        gm.onEnemyTurn.AddListener(Move);
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Move()
    {
        transform.position += Vector3.forward * moveDistance;

        gm.SwitchTurn(true);                                        // ���ʫ��_�ڤ�^�X

        if (transform.position.z >= moveUnderLine) DestroyObject();
    }

    /// <summary>
    /// �R������
    /// </summary>
    private void DestroyObject()
    {
        if (!isMarble) HealthManager.instance.Hurt(damage);

        Destroy(gameObject);
    }

    /// <summary>
    /// ���`
    /// </summary>
    /// <param name="damage">�ˮ`</param>
    private void Hurt(float damage)
    {
        if (!isMarble) ani.SetTrigger("Ĳ�o����");

        hpCurrent -= damage;

        if (hasUI)
        {
            imgHp.fillAmount = hpCurrent / hpMax;
            textHp.text = hpCurrent.ToString();
        }

        if (hpCurrent <= 0) Dead();
    }

    /// <summary>
    /// ���`
    /// </summary>
    private void Dead()
    {
        Destroy(gameObject);
        RecycleMarble.instance.CheckIsRecycleAllMarbles();
        SoundManager.instance.PlaySoundRandomVolue(soundDead, 0.8f, 1.2f);

        if (gameObject.name.Contains("�u�]"))
        {
            ControlSystem.maxMarbles++;
        }
        else
        {
            DropCoin();
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void DropCoin()
    {
        int randomCoin = Random.Range(v2CoinRange.x, v2CoinRange.y);

        for (int i = 0; i < randomCoin; i++)
        {
            GameObject tempCoin = Instantiate(goCoin, transform.position + Vector3.up, Quaternion.Euler(0, Random.Range(0, 360), 0));

            float randomX = Random.Range(100, 300);
            float randomY = Random.Range(500, 800);
            float randomZ = Random.Range(100, 300);

            tempCoin.GetComponent<Rigidbody>().AddForce(randomX, randomY, randomZ);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains(nameMarble))
        {
            Hurt(collision.gameObject.GetComponent<Marble>().attack);
        }
    }
}
