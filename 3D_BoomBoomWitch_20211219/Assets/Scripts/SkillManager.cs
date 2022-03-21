using UnityEngine;

namespace KID
{
    /// <summary>
    /// �ޯ�޲z
    /// </summary>
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void GetMarbles(int getCount)
        {
            ControlSystem.maxMarbles += getCount;
            ControlSystem.instance.UpdateUIMarbleCount();
        }
    }
}

