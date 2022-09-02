using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 控制系統
/// 指向滑鼠位置
/// 發射彈珠
/// 回合控制
/// </summary>
public class ControlSystemTutorial : MonoBehaviour
{
    #region 已完成區
    #region 欄位
    [Header("射線要碰撞的圖層")]
    public LayerMask layerToHit;
    [Header("滑鼠位置")]
    public Transform traTestMousePosition;
    [Header("箭頭")]
    public GameObject goArrow;

    /// <summary>
    /// 所有彈珠數量
    /// </summary>
    public static int allMarbles;
    /// <summary>
    /// 可以發射的最大彈珠數量
    /// </summary>
    public static int maxMarbles = 2;
    /// <summary>
    /// 每次發射出去的彈珠數量
    /// </summary>
    public static int shootMarbles;

    /// <summary>
    /// 是否能發射
    /// </summary>
    private bool canShoot = true;
    #endregion

    #region 事件
    private void Update()
    {
        MouseControl();
    }
    #endregion

    #region 方法
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
            Ray rayMouse = Camera.main.ScreenPointToRay(v3Mouse);
            RaycastHit hit;

            if (Physics.Raycast(rayMouse, out hit, 100, layerToHit))
            {
                Vector3 hitPosition = hit.point;                // 取得碰撞資訊的座標
                hitPosition.y = 0.5f;                           // 調整高度軸向
                traTestMousePosition.position = hitPosition;    // 更新測試物件座標
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

    #region 教學區
    public Transform point;
    public int speed = 700;
    public GameObject marble;

    // 1. Instantiate - 生成物件
    // Instantiate(要生成的物件，生成的座標，生成的角度)

    // 2. SetActive - 顯示或隱藏物件
    // SetActive(布林值) - 布林值 true 顯示，false 隱藏

    // 3. GetComponent - 取得元件
    // GetComponent<元件>()

    // 4. AddForce - 添加推力
    // AddForce(推力)

    /// <summary>
    /// 發射彈珠
    /// </summary>
    private IEnumerator FireMarble()
    {
        GameObject temp = Instantiate(marble, point.position, point.rotation);
        temp.GetComponent<Rigidbody>().AddForce(goArrow.transform.up * speed);
        yield return new WaitForSeconds(0.5f);         // 等待間隔時間
        goArrow.SetActive(false);
    }
    #endregion
}
