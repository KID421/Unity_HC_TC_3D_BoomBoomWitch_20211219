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

    public GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("紆痌"))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = new Vector3(0, 0, 100);

            // Μ紆痌计秖 糤
            recycleMarbles++;
            // 狦 Μ计秖 单 ┮Τ紆痌计秖 ち传 寄よ
            if (recycleMarbles == ControlSystem.allMarbles) gm.SwitchTurn(false);
        }
    }
}
