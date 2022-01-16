using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵人跟道具控制器
/// </summary>
public class EnemyPropControl : MonoBehaviour
{
    public GameManager gm;

    [Header("每次移動的距離")]
    public float moveDistance = 2;
    [Header("移動的座標底線")]
    public float moveUnderLine = -2;
    [Header("彈珠的名稱")]
    public string nameMarble;
    [Header("血量")]
    public float hp = 100;

    private float hpMax;
    private Image imgHp;
    private Text textHp;

    private void Awake()
    {
        hpMax = hp;

        imgHp = transform.Find("畫布血條").Find("血條").GetComponent<Image>();
        textHp = transform.Find("畫布血條").Find("血量").GetComponent<Text>();
        textHp.text = hp.ToString();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position += Vector3.forward * moveDistance;

        if (transform.position.z <= moveUnderLine) DestroyObject();
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
        imgHp.fillAmount = hp / hpMax;
        textHp.text = hp.ToString();
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains(nameMarble))
        {
            Hurt(collision.gameObject.GetComponent<Marble>().attack);
        }
    }
}
