using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/DeckBase", fileName = "DeckBase")]
    public class DeckData : ScriptableObject
    {
        public List<CardData> _deck = new List<CardData>();
        public List<int> _count = new List<int>();

        public string _costText = "0";
        private int _cost  = 0;

        private List<int> _deleteIndex = new List<int>();
        /* ---- ��������g���R�[�h ---- */
#if UNITY_EDITOR
        /**
         * Inspector�g���N���X
         */
        [CustomEditor(typeof(DeckData))]               //!< �g������Ƃ��̂����܂�Ƃ��ď����Ă�
        public class DeckEditor : Editor           //!< Editor���p�������I
        {
            bool folding = true;

            public override void OnInspectorGUI()
            {
                DeckData deckData = target as DeckData;

                //
                List<CardData> list = deckData._deck;
                List<int> countList = deckData._count;
                int i, len = list.Count;
                int cost = deckData._cost;

                deckData._cost = cost;
                deckData._costText = EditorGUILayout.TextField("�R�X�g", cost.ToString());

                List<int> deleteIndexes = deckData._deleteIndex;

                // ��������ύX�����m
                EditorGUI.BeginChangeCheck();

                // �܂肽���ݕ\��
                if (folding = EditorGUILayout.Foldout(folding, "�J�[�h�f�[�^"))
                {
                    // ���X�g�\��
                    cost = 0;
                    deleteIndexes.Clear();
                    for (i = 0; i < len; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();
                        CardData inputCardData = EditorGUILayout.ObjectField(list[i], typeof(CardData), true) as CardData;
                        if (inputCardData)
                        {
                            list[i] = inputCardData;
                            countList[i] = EditorGUILayout.IntField(countList[i], GUILayout.Width(48));
                            cost += inputCardData._Cost * countList[i];
                        }
                        else
                        {
                            deleteIndexes.Add(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    deckData._cost = cost;

                    foreach(int index in deleteIndexes)
                    {
                        list.RemoveAt(index);
                        countList.RemoveAt(index);
                    }
                    deleteIndexes.Clear();


                    CardData newData = EditorGUILayout.ObjectField("�ǉ�", null, typeof(CardData), true) as CardData;
                    if (newData != null)
                    {
                        list.Add(newData);
                        countList.Add(1);
                    }
                    
                }

                if (GUILayout.Button("�N���A", GUILayout.Width(48)))
                {
                    list.Clear();
                    countList.Clear();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    deckData._deck = list;
                    deckData._count = countList;
                    deckData._cost = cost;
                    EditorUtility.SetDirty(deckData);
                }
            }
        }
#endif
    }
}