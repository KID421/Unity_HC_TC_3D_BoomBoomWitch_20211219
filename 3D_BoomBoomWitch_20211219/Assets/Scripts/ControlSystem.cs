using UnityEngine;

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
        if (Input.GetKey(KeyCode.Mouse0))
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
            }
        }
    }
    #endregion
}
