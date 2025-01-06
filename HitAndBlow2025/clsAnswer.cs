using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// 答えの生成と答え合わせをするクラス
    /// </summary>
    internal class clsAnswer
    {
        /// <summary>
        /// 正解
        /// </summary>
        private readonly int[] m_answer;

        /// <summary>
        /// 最大値
        /// </summary>
        private int m_maxNumber = 0;

        /// <summary>
        /// 答え
        /// </summary>
        public int[] Answer { get { return m_answer; } }

        /// <summary>
        /// コンストラクタ(0～maxまでをN個使った正解を作る)
        /// </summary>
        /// <param name="maxNumber">最大値</param>
        /// <param name="numberLength">個数(10以上の数も使うのでケタ数とは言わない)</param>
        public clsAnswer(int maxNumber,int numberLength) 
        {
            m_answer = new int[numberLength];
            m_maxNumber = maxNumber;
            // ランダムの衝突を避けるため、リストを生成し
            // 数値を入れてはリストから消して行く方式
            List<int> _list = new List<int>();
            for (int _i =0 ; _i <= maxNumber; _i++) 
            {
                _list.Add(_i);
            }
            Random _rnd = new Random();
            for (int _i = 0; _i < numberLength; _i++)
            {
                int _currentNumber = _list[_rnd.Next(_list.Count)];
                m_answer[_i] = _currentNumber;
                _list.Remove(_currentNumber);
            }

        }

        /// <summary>
        /// コンストラクタテストのために答えを決めて生成する
        /// </summary>
        /// <param name="maxNumber"></param>
        /// <param name="answer"></param>
        public clsAnswer(int maxNumber, int[] answer)
        {
            m_answer = answer;
            m_maxNumber = maxNumber;
        }

        /// <summary>
        /// 正解と照らし合わせて結果を返す
        /// </summary>
        /// <param name="numbers">プレイヤーが提示した値</param>
        /// <returns>結果</returns>
        public clsResult CheckAnswer(int[] numbers) 
        {
            // 配列の長さ
            if (numbers.Length != m_answer.Length)
            {
                return null;
            }

            // それぞれの数値の条件
            foreach (int _i in numbers)
            {
                if (_i < 0 || m_maxNumber < _i || 1 < numbers.Count(current => current == _i))
                {
                    return null;
                }
            }

            // 問題ない回答だったら検証する
            int _hit = 0;
            int _blow = 0;
            for (int _i = 0; _i < numbers.Length; _i++)
            {
                if (numbers[_i] == m_answer[_i])
                {
                    _hit++;
                }
                else if (m_answer.Contains(numbers[_i]))
                {
                    _blow++;
                }
            }
            return new clsResult(_hit, _blow, numbers);
        }
    }
}
