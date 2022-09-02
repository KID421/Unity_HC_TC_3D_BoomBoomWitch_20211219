using UnityEngine;

/// <summary>
/// 回收彈珠系統
/// </summary>
public class RecycleMarble : MonoBehaviour
{
    /// <summary>
    /// 回收的彈珠數量
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
        if (other.name.Contains("彈珠"))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = new Vector3(0, 0, 100);

            // 回收彈珠數量 增加
            recycleMarbles++;

            CheckIsRecycleAllMarbles();
        }
    }

    /// <summary>
    /// 檢查是否回收所有彈珠
    /// </summary>
    public void CheckIsRecycleAllMarbles()
    {
        // 如果 回收數量 等於 可發射最大彈珠數量 切換為 敵方回合
        if (recycleMarbles == ControlSystem.shootMarbles)
        {
            if (GameObject.FindGameObjectsWithTag("棋盤上的物件").Length == 0)
            {
                gm.allObjectDead = true;
            }

            gm.SwitchTurn(false);
        }
    }
}
