using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// 候補と使ってはいけない数の組み合わせを管理する
    /// </summary>
    internal class clsCandicate
    {
        /// <summary>
        /// 候補
        /// </summary>
        private int[] m_candicate;

        /// <summary>
        /// この候補を使う際に使ってはいけない数
        /// </summary>
        private List<int> m_disabledNumbers;

        /// <summary>
        /// これが使用可能か否か
        /// </summary>
        private bool m_enabled = true;

        /// <summary>
        /// 候補
        /// </summary>
        public int[] Candicate { get { return m_candicate; } }

        /// <summary>
        /// この候補を使う際に使ってはいけない数
        /// </summary>
        public List<int> DisabledNumbers { get { return m_disabledNumbers; } }

        /// <summary>
        /// これが使用可能か否か
        /// </summary>
        public bool Enabled { get { return m_enabled; } set { m_enabled = value; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="candicate">候補</param>
        /// <param name="numbers">回答で使った数</param>
        public clsCandicate(int[] candicate, int[] numbers)
        {
            m_candicate = candicate;
            HashSet<int> hs1 = new HashSet<int>(numbers);
            hs1.ExceptWith(candicate);
            m_disabledNumbers = hs1.ToList();
        }

        /// <summary>
        /// 空のコンストラクタ
        /// </summary>
        /// <param name="numberLength">回答に必要な長さ</param>
        public clsCandicate(int numberLength)
        {
            m_disabledNumbers = new List<int>();
            m_candicate = new int[numberLength];
            for (int _place = 0; _place < numberLength; _place++)
            {
                m_candicate[_place] = -1;
            }
        }

        /// <summary>
        /// 候補同士を組み合わせる
        /// </summary>
        /// <param name="srcCandicate">組み合わせる候補</param>
        public bool Combine(clsCandicate srcCandicate, int maxNumber)
        {
            for (int _i = 0; srcCandicate.m_candicate.Length > _i; _i++)
            {
                // 組み合わせとして成り立たない条件
                // 確認のために分けてますー。

                // 作成中の回答と現在参照中の回答の両方で候補が決まっていてそれが一致しない場合
                if ((srcCandicate.m_candicate[_i] != -1 && m_candicate[_i] != -1 && srcCandicate.m_candicate[_i] != m_candicate[_i])
)
                {
                    return false;
                }

                // 数字が重複してしまう場合
                if ((srcCandicate.m_candicate[_i] != -1 && m_candicate.Contains(srcCandicate.m_candicate[_i]) && srcCandicate.m_candicate[_i] != m_candicate[_i]))
                {

                    return false;
                }

                // 入れようとしている数が使用不可能になっている場合
                if (srcCandicate.m_disabledNumbers.Contains(m_candicate[_i]) || m_disabledNumbers.Contains(srcCandicate.m_candicate[_i]))
                {

                    return false;
                }
            }

            // 組み合わせ可能
            //  参照中の回答を組み合わせる
            clsCandicate _tempCandicate = new clsCandicate(m_candicate.Length);
            for (int _i = 0; m_candicate.Length > _i; _i++)
            {
                _tempCandicate.m_candicate[_i] = m_candicate[_i];
            }
            _tempCandicate.m_disabledNumbers.AddRange(m_disabledNumbers);

            for (int _i = 0; srcCandicate.m_candicate.Length > _i; _i++)
            {
                if (_tempCandicate.m_candicate[_i] == -1 && srcCandicate.m_candicate[_i] != -1)
                {
                    _tempCandicate.m_candicate[_i] = srcCandicate.m_candicate[_i];
                }
            }

            // 使ってはいけない数も組み合わせる
            _tempCandicate.m_disabledNumbers.AddRange(srcCandicate.m_disabledNumbers);
            _tempCandicate.m_disabledNumbers = _tempCandicate.m_disabledNumbers.Distinct().ToList();

            // 使用可能な数が空いている数より少ない場合、組み合わせとして成り立たない
            if (maxNumber + 1 - (_tempCandicate.m_candicate.Count((v) => -1 < v) + _tempCandicate.m_disabledNumbers.Count()) < _tempCandicate.m_candicate.Count((v) => -1 == v))
            {
                return false;
            }

            this.m_candicate = _tempCandicate.m_candicate;
            this.m_disabledNumbers = _tempCandicate.m_disabledNumbers;
            return true;
        }

        /// <summary>
        /// チェックなしで組み合わせる
        /// </summary>
        /// <param name="srcCandicate"></param>
        public void CombineWithoutCheck(clsCandicate srcCandicate)
        {
            for (int _i = 0; srcCandicate.m_candicate.Length > _i; _i++)
            {
                if (srcCandicate.m_candicate[_i] != -1)
                {
                    m_candicate[_i] = srcCandicate.m_candicate[_i];
                }
            }

            // 使ってはいけない数も組み合わせる
            m_disabledNumbers.AddRange(srcCandicate.m_disabledNumbers);
            m_disabledNumbers = m_disabledNumbers.Distinct().ToList();

        }
    }

}
