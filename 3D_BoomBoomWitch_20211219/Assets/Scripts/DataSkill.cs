using UnityEngine;

namespace KID
{
    /// <summary>
    /// �ޯ���
    /// </summary>
    [CreateAssetMenu(menuName = "KID/Data Skill")]
    public class DataSkill : ScriptableObject
    {
        [Header("�ޯ�W��")]
        public string skillName;
        [Header("�ޯ�����")]
        public TypeSkill typeSkill;
        [Header("�ޯ໡��")]
        public string skillDescription;
        [Header("�ޯ�ϥ�")]
        public Sprite skillSprite;
    }

    /// <summary>
    /// �ޯ�����
    /// </summary>
    public enum TypeSkill
    {
        addMarble1, addMarble3, addMarble5
    }
}
