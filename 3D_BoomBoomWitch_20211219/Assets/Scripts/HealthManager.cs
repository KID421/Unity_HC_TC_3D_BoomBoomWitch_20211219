using UnityEngine;
using UnityEngine.UI;

namespace KID
{
    /// <summary>
    /// 血量管理器
    /// </summary>
    public class HealthManager : MonoBehaviour
    {
        public static HealthManager instance;

        [SerializeField, Header("血量"), Range(0, 10000)]
        private float hp = 100;
        
        [SerializeField, Header("血條")]
        private Image imgHp;
        [SerializeField, Header("血量")]
        private Text textHp;
        [SerializeField, Header("動畫控制器")]
        private Animator ani;
        private float hpMax;

        private void Awake()
        {
            instance = this;
            hpMax = hp;
            UpdateUI();
        }

        /// <summary>
        /// 受傷
        /// </summary>
        /// <param name="damage">傷害</param>
        public void Hurt(float damage)
        {
            ani.SetTrigger("觸發受傷");
            hp -= damage;
            UpdateUI();

            if (hp <= 0) Dead();
        }

        /// <summary>
        /// 更新介面
        /// </summary>
        private void UpdateUI()
        {
            imgHp.fillAmount = hp / hpMax;
            textHp.text = hp.ToString();
        }

        /// <summary>
        /// 死亡
        /// </summary>
        private void Dead()
        {
            ani.SetBool("開關死亡", true);
        }
    }
}
