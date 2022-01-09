using UnityEngine;
using System.Collections;           // 引用 系統.集合
using System.Collections.Generic;   // 引用 系統.集合.一般 (包含 List)

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

    /// <summary>
    /// 所有彈珠數量
    /// </summary>
    public static int allMarbles;
    #endregion

    #region 事件
    private void Start()
    {
        for (int i = 0; i < 2; i++) SpawnMarble();
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
                print("滑鼠射線打到物件：" + hit.collider.name);
                
                Vector3 hitPosition = hit.point;                // 取得碰撞資訊的座標
                hitPosition.y = 0.5f;                           // 調整高度軸向
                traTestMousePosition.position = hitPosition;    // 更新測試物件座標

                // 角色 的 Z 軸 = 測試物件的座標 - 角色的座標 (向量)
                transform.forward = traTestMousePosition.position - transform.position;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StartCoroutine(FireMarble());
        }
    }

    /// <summary>
    /// 發射彈珠
    /// </summary>
    private IEnumerator FireMarble()
    {
        for (int i = 0; i < listMarbles.Count; i++)
        {
            GameObject temp = listMarbles[i];
            temp.transform.position = traSpawnPoint.position;
            temp.transform.rotation = traSpawnPoint.rotation;
            temp.GetComponent<Rigidbody>().velocity = Vector3.zero;
            temp.GetComponent<Rigidbody>().AddForce(traSpawnPoint.forward * speedShoot);    // 發射 彈珠
            yield return new WaitForSeconds(fireInterval);                                  //間隔
        }
        goArrow.SetActive(false);
    }
    #endregion
}
