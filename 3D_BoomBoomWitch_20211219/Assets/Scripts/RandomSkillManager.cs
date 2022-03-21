using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace KID
{
    /// <summary>
    /// �H���ޯ�
    /// </summary>
    public class RandomSkillManager : MonoBehaviour
    {
        public static RandomSkillManager instance;

        [SerializeField, Header("�Ҧ��ޯ�")]
        private List<DataSkill> dataSkills = new List<DataSkill>();
        [SerializeField, Header("�ޯફ��")]
        private GameObject goSkill;
        [SerializeField, Header("�H���ޯ��l")]
        private Transform[] traRandomSkillBox;
        [SerializeField, Header("�ޯફ��ؤo")]
        private float sizeSkillObject = 100;
        [SerializeField, Header("�ޯ�C�����ʳ��")]
        private float unitSkillMove = 20;
        [SerializeField, Header("���ʶ��j")]
        private float intervalMove = 0.1f;
        [SerializeField, Header("���ʦ���")]
        private float countMove = 30;
        [Header("����")]
        [SerializeField]private AudioClip soundRandom;
        [SerializeField]private AudioClip soundGetSkill;

        private List<GameObject>[] listSkillInBox = { new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };
        private List<RectTransform>[] listSkillRectInBox = { new List<RectTransform>(), new List<RectTransform>(), new List<RectTransform>() };

        /// <summary>
        /// �H���ޯ�
        /// </summary>
        private CanvasGroup groupRandomSkill;
        private AudioSource aud;

        private void Awake()
        {
            instance = this;

            aud = GetComponent<AudioSource>();
            groupRandomSkill = GameObject.Find("�H���ޯ�").GetComponent<CanvasGroup>();

            InitializeSkill();
            StartCoroutine(ShowRandomSkill());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ShowRandomSkill());
        }

        public IEnumerator ShowRandomSkill()
        {
            yield return StartCoroutine(FadeRandomSkillGroup(true));

            RandomSkill();

            for (int i = 0; i < listSkillRectInBox.Length; i++)
            {
                yield return new WaitForSeconds(0.1f);

                StartCoroutine(MoveSkill(listSkillRectInBox[i]));
            }
        }

        /// <summary>
        /// ��l�Ƨޯફ��
        /// </summary>
        private void InitializeSkill()
        {
            for (int i = 0; i < listSkillInBox.Length; i++)
            {
                int index = 0;

                foreach (var item in dataSkills)
                {
                    GameObject tempSkill = Instantiate(goSkill, traRandomSkillBox[i]);
                    tempSkill.GetComponent<Image>().sprite = item.skillSprite;
                    tempSkill.GetComponent<SkillUIObject>().dataSkill = item;
                    listSkillRectInBox[i].Add(tempSkill.GetComponent<RectTransform>());
                    listSkillInBox[i].Add(tempSkill);
                    index++;
                }
            }
        }

        /// <summary>
        /// �H���ޯ�
        /// </summary>
        private void RandomSkill()
        {
            for (int i = 0; i < listSkillInBox.Length; i++)
            {
                int randomSeed = Random.Range(0, 100);
                var random = new System.Random(randomSeed);
                listSkillInBox[i] = listSkillInBox[i].OrderBy(item => random.Next()).ToList();
                listSkillRectInBox[i] = listSkillRectInBox[i].OrderBy(item => random.Next()).ToList();

                for (int j = 0; j < listSkillRectInBox[i].Count; j++)
                {
                    print(j);
                    listSkillRectInBox[i][j].anchoredPosition = Vector2.up * j * sizeSkillObject;
                }
            }
        }

        /// <summary>
        /// ���ʧޯ�
        /// </summary>
        private IEnumerator MoveSkill(List<RectTransform> listSkillRect)
        {
            for (int i = 0; i < countMove; i++)
            {
                MovePerSKillRect(listSkillRect, -unitSkillMove, soundRandom, new Vector2(0.1f, 0.25f));

                for (int j = 0; j < listSkillRect.Count; j++)
                {
                    //listSkillRect[j].anchoredPosition += Vector2.up * -unitSkillMove;
                    //aud.PlayOneShot(soundRandom, Random.Range(0.1f, 0.3f));

                    if (listSkillRect[j].anchoredPosition.y == -sizeSkillObject)
                    {
                        listSkillRect[j].anchoredPosition += Vector2.up * listSkillRect.Count * sizeSkillObject;
                    }
                }
                yield return new WaitForSeconds(intervalMove);
            }

            yield return new WaitForSeconds(0.1f);

            MovePerSKillRect(listSkillRect, -20, soundRandom, new Vector2(0.1f, 0.25f));

            yield return new WaitForSeconds(0.3f);

            MovePerSKillRect(listSkillRect, 25, soundRandom, new Vector2(0.1f, 0.25f));

            yield return new WaitForSeconds(0.3f);

            MovePerSKillRect(listSkillRect, -5, soundGetSkill, new Vector2(0.5f, 0.7f));
        }

        private void MovePerSKillRect(List<RectTransform> listSkillRect, float moveDistance, AudioClip sound, Vector2 volumeRange)
        {
            for (int i = 0; i < listSkillRect.Count; i++)
            {
                listSkillRect[i].anchoredPosition += Vector2.up * moveDistance;
                aud.PlayOneShot(sound, Random.Range(volumeRange.x, volumeRange.y));
            }
        }

        /// <summary>
        /// �H�J�H�X�H���ޯ�s��
        /// </summary>
        /// <param name="fadeIn">�H�J</param>
        public IEnumerator FadeRandomSkillGroup(bool fadeIn)
        {
            ControlSystem.instance.enabled = !fadeIn;

            float increase = fadeIn ? 0.1f : -0.1f;

            for (int i = 0; i < 10; i++)
            {
                groupRandomSkill.alpha += increase;
                yield return new WaitForSeconds(0.01f);
            }

            groupRandomSkill.interactable = fadeIn;
            groupRandomSkill.blocksRaycasts = fadeIn;
        }
    }
}
