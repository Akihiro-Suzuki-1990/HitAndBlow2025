using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitAndBlow2025
{
    /// <summary>
    /// 結果とそこか考えられる次の候補を管理するクラス
    /// </summary>
    internal class clsResult
    {
        private int m_hit;

        private int m_blow;

        private int[] m_numbers;

        private List<clsCandicate> m_nextCandicates;

        /// <summary>
        /// ヒット
        /// </summary>
        public int Hit { get { return m_hit; } }

        /// <summary>
        /// ブロウ
        /// </summary>
        public int Blow { get { return m_blow; } }

        /// <summary>
        /// 回答の数字
        /// </summary>
        public int[] Numbers { get { return m_numbers; } }

        /// <summary>
        /// 正解か否か
        /// </summary>
        public bool IsCorrect { get { return m_numbers.Length == Hit; } }

        /// <summary>
        /// 次に考えられる候補
        /// </summary>
        public clsCandicate[] NextCandicates { get { return m_nextCandicates.ToArray(); } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hit">ヒット</param>
        /// <param name="blow">ブロウ</param>
        /// <param name="numbers">回答の数字</param>
        public clsResult(int hit, int blow, int[] numbers)
        {
            m_hit = hit;
            m_blow = blow;
            m_numbers = numbers;
        }

        /// <summary>
        /// 候補を探す(人がプレイする場合は不要)
        /// </summary>
        public void MakeCandicates()
        {
            // 数値をその場所を一緒に管理する事で並び順を求めやすくする
            clsNumberAndPlace[] _clsNumberAndPlaces = new clsNumberAndPlace[m_numbers.Length];
            for (int _i = 0; _i < m_numbers.Length; ++_i)
            {
                _clsNumberAndPlaces[_i] = new clsNumberAndPlace(m_numbers[_i], _i);
                _clsNumberAndPlaces[_i].Generate(m_numbers.Length);
            }

            m_nextCandicates = clsGenerateCandicate.GenerateCandicate(_clsNumberAndPlaces, m_hit, m_blow);
        }

        /// <summary>
        /// 作成した候補から、更にありえない組み合わせを削除する
        /// </summary>
        /// <param name="masterArray">[数字,場所]で候補を管理する配列(true=入る可能性がある,false=入る可能性がない)</param>
        public void StrictCandicates(bool[,] masterArray)
        {
            foreach (clsCandicate _current in m_nextCandicates)
            {
                int[] _currentCandicate = _current.Candicate;
                for (int _place = 0; _place < _currentCandicate.Length; _place++)
                {
                    if (-1 < _currentCandicate[_place] && !masterArray[_currentCandicate[_place], _place])
                    {
                        _current.Enabled = false;
                        break;
                    }
                }
            }

            m_nextCandicates.RemoveAll(x => !x.Enabled);
        }
    }
}
