using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵人跟道具控制器
/// </summary>
public class EnemyPropControl : MonoBehaviour
{
    private GameManager gm;

    [Header("每次移動的距離")]
    public float moveDistance = 2;
    [Header("移動的座標底線")]
    public float moveUnderLine = -2;
    [Header("彈珠的名稱")]
    public string nameMarble;
    [Header("血量")]
    public float hp = 100;
    [Header("是否有介面")]
    public bool hasUI;
    [Header("死亡音效")]
    public AudioClip soundDead;

    private float hpMax;
    private Image imgHp;
    private Text textHp;

    private void Awake()
    {
        hpMax = hp;

        if (hasUI)
        {
            imgHp = transform.Find("畫布血條").Find("血條").GetComponent<Image>();
            textHp = transform.Find("畫布血條").Find("血量").GetComponent<Text>();
            textHp.text = hp.ToString();
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
        Destroy(gameObject);
    }

    private void Hurt(float damage)
    {
        hp -= damage;
        if (hasUI)
        {
            imgHp.fillAmount = hp / hpMax;
            textHp.text = hp.ToString();
        }
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        Destroy(gameObject);
        RecycleMarble.instance.CheckIsRecycleAllMarbles();
        SoundManager.instance.PlaySoundRandomVolue(soundDead, 0.8f, 1.2f);

        if (gameObject.name.Contains("彈珠"))
        {
            ControlSystem.maxMarbles++;
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
