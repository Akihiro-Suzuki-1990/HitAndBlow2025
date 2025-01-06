using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// 組み合わせを生成するクラス
    /// </summary>
    internal static class clsCombination
    {
        /// <summary>
        /// 組み合わせのリスト
        /// </summary>
        private static List<clsNumberAndPlace[]> m_combinationNumberAndPlaces = new List<clsNumberAndPlace[]>();

        /// <summary>
        /// 組み合わせを生成する
        /// </summary>
        /// <param name="numbers">候補の配列</param>
        /// <param name="useCount">候補から使用する回数</param>
        /// <returns></returns>
        public static List<clsNumberAndPlace[]> Generate(clsNumberAndPlace[] numbers, int useCount)
        {
            m_combinationNumberAndPlaces = new List<clsNumberAndPlace[]>();
            CalcCombination(new List<clsNumberAndPlace>(), numbers, useCount, 0);
            return m_combinationNumberAndPlaces;
        }

        /// <summary>
        /// 組み合わせを生成する関数
        /// </summary>
        /// <param name="list">作製中の組み合わせ</param>
        /// <param name="numbers">使用する数字の配列</param>
        /// <param name="useCount">使用する数</param>
        /// <param name="index">インデックス</param>
        private static void CalcCombination(List<clsNumberAndPlace> list, clsNumberAndPlace[] numbers, int useCount, int index)
        {
            if (list.Count == useCount)
            {
                m_combinationNumberAndPlaces.Add(list.ToArray());
                return;
            }

            for (int _i = index; _i < numbers.Length; _i++)
            {
                list.Add(numbers[_i]);
                CalcCombination(list, numbers, useCount, _i + 1);
                list.Remove(numbers[_i]);
            }
        }
    }
}
