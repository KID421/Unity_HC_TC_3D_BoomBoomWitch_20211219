using KID;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵人跟道具控制器
/// </summary>
public class EnemyPropControl : MonoBehaviour
{
    [Header("每次移動的距離")]
    public float moveDistance = 2;
    [Header("移動的座標底線")]
    public float moveUnderLine = -2;
    [Header("彈珠的名稱")]
    public string nameMarble;
    [Header("基本血量")]
    public float hpBase = 100;
    [Header("每一層提升血量")]
    public float hpIncrease = 100;
    [Header("傷害")]
    public float damage = 100;
    [Header("是否有介面")]
    public bool hasUI;
    [Header("是否為可以吃的彈珠")]
    public bool isMarble;
    [Header("死亡音效")]
    public AudioClip soundDead;
    [Header("金幣")]
    public GameObject goCoin;
    [Header("金幣掉落範圍")]
    public Vector2Int v2CoinRange;

    [HideInInspector]
    public float hpCurrent = 0;

    [SerializeField, Header("動畫控制器")]
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
            imgHp = transform.Find("畫布血條").Find("血條").GetComponent<Image>();
            textHp = transform.Find("畫布血條").Find("血量").GetComponent<Text>();
            textHp.text = hpCurrent.ToString();
        }

        gm = FindObjectOfType<GameManager>();
        gm.onEnemyTurn.AddListener(Move);
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position += Vector3.forward * moveDistance;

        gm.SwitchTurn(true);                                        // 移動後恢復我方回合

        if (transform.position.z >= moveUnderLine) DestroyObject();
    }

    /// <summary>
    /// 刪除物件
    /// </summary>
    private void DestroyObject()
    {
        if (!isMarble) HealthManager.instance.Hurt(damage);

        Destroy(gameObject);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="damage">傷害</param>
    private void Hurt(float damage)
    {
        if (!isMarble) ani.SetTrigger("觸發受傷");

        hpCurrent -= damage;

        if (hasUI)
        {
            imgHp.fillAmount = hpCurrent / hpMax;
            textHp.text = hpCurrent.ToString();
        }

        if (hpCurrent <= 0) Dead();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        Destroy(gameObject);
        RecycleMarble.instance.CheckIsRecycleAllMarbles();
        SoundManager.instance.PlaySoundRandomVolue(soundDead, 0.8f, 1.2f);

        if (gameObject.name.Contains("彈珠"))
        {
            ControlSystem.maxMarbles++;
        }
        else
        {
            DropCoin();
        }
    }

    /// <summary>
    /// 掉落金幣
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
