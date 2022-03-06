using UnityEngine;

/// <summary>
/// Μ紆痌╰参
/// </summary>
public class RecycleMarble : MonoBehaviour
{
    /// <summary>
    /// Μ紆痌计秖
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
        if (other.name.Contains("紆痌"))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = new Vector3(0, 0, 100);

            // Μ紆痌计秖 糤
            recycleMarbles++;

            CheckIsRecycleAllMarbles();
        }
    }

    /// <summary>
    /// 浪琩琌Μ┮Τ紆痌
    /// </summary>
    public void CheckIsRecycleAllMarbles()
    {
        // 狦 Μ计秖 单 祇甮程紆痌计秖 ち传 寄よ
        if (recycleMarbles == ControlSystem.shootMarbles)
        {
            if (GameObject.FindGameObjectsWithTag("囱絃ン").Length == 0)
            {
                gm.allObjectDead = true;
            }

            gm.SwitchTurn(false);
        }
    }
}
