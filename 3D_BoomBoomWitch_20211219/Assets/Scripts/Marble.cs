using UnityEngine;

/// <summary>
/// 紆痌╰参
/// </summary>
public class Marble : MonoBehaviour
{
    /// <summary>
    /// ю阑
    /// </summary>
    public float attack;

    [SerializeField, Header("ネΘ┕┏场")]
    private float flyToBottomAfterSpawn = 7;

    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ┕┏场计
    /// </summary>
    public void FlyToBottomCountDown()
    {
        CancelInvoke();
        Invoke("FlyToBottom", flyToBottomAfterSpawn);
    }

    /// <summary>
    /// ネΘ┕┏场
    /// </summary>
    private void FlyToBottom()
    {
        rig.velocity = Vector3.zero;
        rig.AddForce(0, 0, 1000);
    }
}
