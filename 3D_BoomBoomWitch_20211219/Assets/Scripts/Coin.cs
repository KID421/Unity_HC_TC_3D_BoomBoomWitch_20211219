using UnityEngine;

namespace KID
{
    /// <summary>
    /// 金幣管理
    /// </summary>
    public class Coin : MonoBehaviour
    {
        [SerializeField, Header("金幣飛行速度")]
        private float speed = 10;
        [SerializeField, Header("金幣生成後多久飛行")]
        private float flyAfterSpawn = 2;
        [SerializeField, Header("金幣音效")]
        private AudioClip soundCoin;

        /// <summary>
        /// 回收金幣的位置
        /// </summary>
        private Transform traCoinIcon;
        /// <summary>
        /// 開始飛
        /// </summary>
        private bool startFly;

        private void Awake()
        {
            traCoinIcon = GameObject.Find("回收金幣的位置").transform;

            Invoke("StartFly", flyAfterSpawn);
        }

        private void Update()
        {
            FlyToCoinIcon();
        }

        /// <summary>
        /// 開始飛行
        /// </summary>
        private void StartFly()
        {
            startFly = true;
        }

        /// <summary>
        /// 飛往金幣圖示
        /// </summary>
        private void FlyToCoinIcon()
        {
            if (!startFly) return;

            Vector3 v3CoinIcon = traCoinIcon.position;
            Vector3 v3Coin = transform.position;

            v3Coin = Vector3.Lerp(v3Coin, v3CoinIcon, Time.deltaTime * speed);

            transform.position = v3Coin;

            if (Vector3.Distance(transform.position, traCoinIcon.position) < 3)
            {
                DestroyAndAddCoinCount();
            }
        }

        /// <summary>
        /// 刪除並且添加金幣數量
        /// </summary>
        private void DestroyAndAddCoinCount()
        {
            SoundManager.instance.PlaySoundRandomVolue(soundCoin, 0.5f, 0.7f);
            GameManager.instance.AddCoinAndUpdateUI();
            Destroy(gameObject);
        }
    }
}

