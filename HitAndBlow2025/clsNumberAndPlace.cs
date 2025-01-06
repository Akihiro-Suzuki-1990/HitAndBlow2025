using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// ある数字がどこに入りうるかを管理するクラス
    /// </summary>
    internal class clsNumberAndPlace
    {
        private class clsPlaceAndEnebled
        {
            public int m_place;
            public bool m_enabled;

            public clsPlaceAndEnebled(int place, bool enabled)
            {
                m_place = place;
                m_enabled = enabled;
            }
        }

        private int m_number;
        private int m_place;

        private List<clsPlaceAndEnebled> m_placeAndEnebleds;

        /// <summary>
        /// 数値
        /// </summary>
        public int Number { get { return m_number; } }

        /// <summary>
        /// 配置
        /// </summary>
        public int Place { get { return m_place; } }

        /// <summary>
        /// 残っている配置
        /// </summary>
        public int[] EnablePlaces
        {
            get
            {
                List<int> _enablePlaces = new List<int>();
                foreach (clsPlaceAndEnebled _current in m_placeAndEnebleds)
                {
                    if (_current.m_enabled)
                    {
                        _enablePlaces.Add(_current.m_place);
                    }
                }
                return _enablePlaces.ToArray();
            }
        }

        /// <summary>
        /// 削除した配置
        /// </summary>
        public int[] RemovedPlaces
        {
            get
            {
                List<int> _removedPlaces = new List<int>();
                foreach (clsPlaceAndEnebled _current in m_placeAndEnebleds)
                {
                    if (!_current.m_enabled)
                    {
                        _removedPlaces.Add(_current.m_place);
                    }
                }
                return _removedPlaces.ToArray();
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number">数値</param>
        /// <param name="place">配置</param>
        public clsNumberAndPlace(int number, int place)
        {
            m_number = number;
            m_place = place;
        }

        /// <summary>
        /// コンストラクタ(コピー)
        /// </summary>
        /// <param name="src"></param>
        public clsNumberAndPlace(clsNumberAndPlace src) 
        {
            m_number = src.Number;
            m_place = src.Place;
        }

        /// <summary>
        /// 残っている配置を生成する
        /// </summary>
        /// <param name="numberLength">回答に必要な長さ</param>
        public void Generate(int numberLength)
        {
            m_placeAndEnebleds = new List<clsPlaceAndEnebled>();
            for (int _i = 0; _i < numberLength; ++_i)
            {
                m_placeAndEnebleds.Add(new clsPlaceAndEnebled(_i, true));
            }
            m_placeAndEnebleds.RemoveAll((p) => p.m_place == m_place);
        }

        /// <summary>
        /// 他の数字に使われた場所を候補から消す
        /// </summary>
        /// <param name="place">消す場所</param>
        public void RemovePlace(int place)
        {
            foreach (clsPlaceAndEnebled _current in m_placeAndEnebleds)
            {
                if (_current.m_place == place)
                {
                    _current.m_enabled = false;
                    break;
                }
            }
        }

        /// <summary>
        /// 消された場所を候補に戻す
        /// </summary>
        /// <param name="place">戻す場所</param>
        public void RecoverPlace(int place)
        {
            foreach (clsPlaceAndEnebled _current in m_placeAndEnebleds)
            {
                if (_current.m_place == place)
                {
                    _current.m_enabled = true;
                    break;
                }
            }
        }

    }
}
