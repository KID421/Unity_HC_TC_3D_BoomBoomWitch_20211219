using UnityEngine;
using UnityEngine.UI;

namespace KID
{
    /// <summary>
    /// �ޯश������
    /// </summary>
    public class SkillUIObject : MonoBehaviour
    {
        [Header("�ޯ���")]
        public DataSkill dataSkill;

        private Button btn;

        private void Awake()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(ChooseSkill);
        }

        /// <summary>
        /// ����ޯ�
        /// </summary>
        private void ChooseSkill()
        {
            StartCoroutine(RandomSkillManager.instance.FadeRandomSkillGroup(false));

            switch (dataSkill.typeSkill)
            {
                case TypeSkill.addMarble1:
                    SkillManager.instance.GetMarbles(1);
                    break;
                case TypeSkill.addMarble3:
                    SkillManager.instance.GetMarbles(3);
                    break;
                case TypeSkill.addMarble5:
                    SkillManager.instance.GetMarbles(5);
                    break;
                default:
                    break;
            }
        }
    }
}

