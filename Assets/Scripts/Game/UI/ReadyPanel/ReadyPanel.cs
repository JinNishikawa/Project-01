using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGame
{
    public class ReadyPanel : MonoBehaviour
    {
        public enum Type
        {
            Line,
            White,
            LineAlpha,
            CenterLine,
            Move,
            Thunder,
            FinAlpha,

            MaxType
        }


        private Type _type = Type.MaxType;

        /** 上下線 */
        [HideInInspector]
        public GameObject _line;

        /** 画面カバー */
        private GameObject _cover;

        /** キャラクター帯 */
        private GameObject[] _band;

        private GameObject _centerLine;

        private GameObject _textObj;

        [HideInInspector]
        public GameObject _thunder;

        // Start is called before the first frame update
        void Start()
        {
            _line = transform.Find("Line").gameObject;
            _cover = transform.Find("WhiteCover").gameObject;

            GameObject band = transform.Find("Band").gameObject;
            _band = new GameObject[band.transform.childCount];
            for(int i=0;i<_band.Length;i++)
            {
                _band[i] = band.transform.GetChild(i).gameObject;
                _band[i].AddComponent<BandMove>();
            }

            _centerLine = transform.Find("CenterLine").gameObject;

            _textObj = transform.Find("Text").gameObject;

            _thunder = transform.Find("Thunder").gameObject;
            _thunder.SetActive(false);

            NextType(Type.Line);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateType()
        {

        }

        public void NextType(Type next)
        {
            if (_type == next) return;

            _type = next;

            switch (_type)
            {
                case Type.Line:
                    _line.AddComponent<LineAppear>();
                    break;
                case Type.White:
                    _cover.AddComponent<WhiteOut>();
                    break;
                case Type.LineAlpha:
                    {
                        LineAppearAlpha appear = _line.AddComponent<LineAppearAlpha>();
                        appear.StartAlpha(true, Type.Move, 0.5f);
                        appear = _centerLine.AddComponent<LineAppearAlpha>();
                        appear.StartAlpha(true, Type.MaxType ,0.5f);
                    }
                    break;
                case Type.CenterLine:
                    {
                        //LineAppearAlpha appear = _centerLine.AddComponent<LineAppearAlpha>();
                        //appear.StartAlpha(true, Type.Move, 0.8f);
                    }
                    break;
                case Type.Move:
                    for (int i = 0; i < _band.Length; i++)
                    {
                        _band[i].GetComponent<BandMove>().MoveStart();
                    }
                    break;
                case Type.Thunder:
                    _thunder.SetActive(true);
                    _textObj.AddComponent<TextMove>();
                    break;
                case Type.FinAlpha:
                    {                   
                        BandAlpha panel = gameObject.AddComponent<BandAlpha>();
                    }

                    break;
            }
        }
    }
}