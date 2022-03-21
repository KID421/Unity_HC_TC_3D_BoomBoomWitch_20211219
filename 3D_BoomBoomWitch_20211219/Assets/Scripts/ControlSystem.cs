using UnityEngine;
using System.Collections;           // 引用 系統.集合
using System.Collections.Generic;   // 引用 系統.集合.一般 (包含 List)
using UnityEngine.UI;

/// <summary>
/// 控制系統
/// 指向滑鼠位置
/// 發射彈珠
/// 回合控制
/// </summary>
public class ControlSystem : MonoBehaviour
{
    #region 欄位
    [Header("箭頭")]
    public GameObject goArrow;
    [Header("生成彈珠位置")]
    public Transform traSpawnPoint;
    [Header("彈珠預製物")]
    public GameObject goMarbles;
    [Header("發射速度"), Range(0, 5000)]
    public float speedShoot = 750;
    [Header("射線要碰撞的圖層")]
    public LayerMask layerToHit;
    [Header("測試滑鼠位置")]
    public Transform traTestMousePosition;
    [Header("所有彈珠")]
    public List<GameObject> listMarbles = new List<GameObject>();
    [Header("發射間隔"), Range(0, 5)]
    public float fireInterval = 0.5f;
    [Header("發射音效")]
    public AudioClip soundShoot;

    /// <summary>
    /// 所有彈珠數量
    /// </summary>
    public static int allMarbles;
    /// <summary>
    /// 可以發射的最大彈珠數量
    /// </summary>
    public static int maxMarbles = 10;
    /// <summary>
    /// 每次發射出去的彈珠數量
    /// </summary>
    public static int shootMarbles;

    public static ControlSystem instance;

    /// <summary>
    /// 是否能發射
    /// </summary>
    public bool canShoot = true;

    [SerializeField, Header("動畫控制器")]
    private Animator ani;

    /// <summary>
    /// 彈珠數量
    /// </summary>
    private Text textMarbleCount;
    #endregion

    #region 事件
    private void Awake()
    {
        instance = this;
        textMarbleCount = GameObject.Find("彈珠數量").GetComponent<Text>();
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

    #region 方法
    /// <summary>
    /// 生成彈珠存放到清單內
    /// </summary>
    private void SpawnMarble()
    {
        // 彈珠總數增加
        allMarbles++;
        // 所有彈珠清單.添加(生成彈珠)
        listMarbles.Add(Instantiate(goMarbles, new Vector3(0, 0, 100), Quaternion.identity));
    }

    /// <summary>
    /// 滑鼠控制
    /// </summary>
    private void MouseControl()
    {
        if (!canShoot) return;              // 如果 現在是 敵方回合 就跳出

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            goArrow.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 v3Mouse = Input.mousePosition;

            // print("滑鼠座標：" + v3Mouse);

            // 射線 = 主要攝影機.螢幕座標轉射線(滑鼠座標)
            Ray rayMouse = Camera.main.ScreenPointToRay(v3Mouse);
            // 射線碰撞資訊
            RaycastHit hit;

            // 如果 射線打到物件就處理
            // 物理 射線碰撞(射線，射線碰撞資訊，距離，圖層)
            if (Physics.Raycast(rayMouse, out hit, 100, layerToHit))
            {
                // print("滑鼠射線打到物件：" + hit.collider.name);
                
                Vector3 hitPosition = hit.point;                // 取得碰撞資訊的座標
                hitPosition.y = 0.5f;                           // 調整高度軸向
                traTestMousePosition.position = hitPosition;    // 更新測試物件座標

                // 角色 的 Z 軸 = 測試物件的座標 - 角色的座標 (向量)
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
    /// 發射彈珠
    /// </summary>
    private IEnumerator FireMarble()
    {
        shootMarbles = 0;

        for (int i = 0; i < maxMarbles; i++)
        {
            ani.SetTrigger("觸發攻擊");
            shootMarbles++;
            GameObject temp = listMarbles[i];
            temp.transform.position = traSpawnPoint.position;
            temp.transform.rotation = traSpawnPoint.rotation;
            temp.GetComponent<Rigidbody>().velocity = Vector3.zero;
            temp.GetComponent<Rigidbody>().AddForce(traSpawnPoint.forward * speedShoot);    // 發射 彈珠
            SoundManager.instance.PlaySoundRandomVolue(soundShoot, 0.8f, 1.2f);
            temp.GetComponent<Marble>().FlyToBottomCountDown();                             // 避免停留太久進入倒數飛回底部回收區
            UpdateUIMarbleCount();

            yield return new WaitForSeconds(fireInterval);                                  //間隔
        }
        goArrow.SetActive(false);
    }

    /// <summary>
    /// 更新介面：彈珠數量
    /// </summary>
    public void UpdateUIMarbleCount()
    {
        int marblesLess = maxMarbles - shootMarbles;
        string content = marblesLess != 0 ? "x " + marblesLess : "";
        textMarbleCount.text = content;
    }

    /// <summary>
    /// 重設介面：彈珠數量恢復為最大值
    /// </summary>
    public void ResetUIMarbleCountToMax()
    {
        textMarbleCount.text = "x " + maxMarbles;
    }
    #endregion
}
