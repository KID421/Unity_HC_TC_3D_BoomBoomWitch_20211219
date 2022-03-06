using UnityEngine;

namespace KID
{
    /// <summary>
    /// �����޲z
    /// </summary>
    public class Coin : MonoBehaviour
    {
        [SerializeField, Header("��������t��")]
        private float speed = 10;
        [SerializeField, Header("�����ͦ���h�[����")]
        private float flyAfterSpawn = 2;

        /// <summary>
        /// �^����������m
        /// </summary>
        private Transform traCoinIcon;
        /// <summary>
        /// �}�l��
        /// </summary>
        private bool startFly;

        private void Awake()
        {
            traCoinIcon = GameObject.Find("�^����������m").transform;

            Invoke("StartFly", flyAfterSpawn);
        }

        private void Update()
        {
            FlyToCoinIcon();
        }

        /// <summary>
        /// �}�l����
        /// </summary>
        private void StartFly()
        {
            startFly = true;
        }

        /// <summary>
        /// ���������ϥ�
        /// </summary>
        private void FlyToCoinIcon()
        {
            if (!startFly) return;

            Vector3 v3CoinIcon = traCoinIcon.position;
            Vector3 v3Coin = transform.position;

            v3Coin = Vector3.Lerp(v3Coin, v3CoinIcon, Time.deltaTime * speed);

            transform.position = v3Coin;

            if (Vector3.Distance(transform.position, traCoinIcon.position) < 2)
            {
                DestroyAndAddCoinCount();
            }
        }

        /// <summary>
        /// �R���åB�K�[�����ƶq
        /// </summary>
        private void DestroyAndAddCoinCount()
        {
            GameManager.instance.AddCoinAndUpdateUI();
            Destroy(gameObject);
        }
    }
}

