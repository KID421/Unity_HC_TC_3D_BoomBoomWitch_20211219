using UnityEngine;

/// <summary>
/// 彈珠系統
/// </summary>
public class Marble : MonoBehaviour
{
    private void Awake()
    {
        // 物理.忽略圖層碰撞(A 圖層，B圖層) 忽略 A B 圖層碰撞
        Physics.IgnoreLayerCollision(6, 6);
    }
}
