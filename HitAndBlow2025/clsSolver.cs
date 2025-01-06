using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// 過去の回答を基に次の候補を出し正解を考えるクラス
    /// 答え毎に候補を保存する方法と毎回イチから生成する方法がある
    /// メモリ使用量は毎回生成する方が少ないが、計算速度は保存する方が速いんじゃないかと思う
    /// 考えられる候補同士に矛盾がないかを計算するのだが、一つ一つ計算するのではなく、
    /// ありえないと分かった時点で次の候補に移ると速い。
    /// </summary>
    internal class clsSolver
    {
        /// <summary>
        /// 最大値
        /// </summary>
        int m_maxNumber;
        
        /// <summary>
        /// 数字の長さ
        /// </summary>
        int m_numberLength;

        /// <summary>
        /// 入る可能性があるかないかを管理する配列
        /// [数字,場所]で管理する2次元配列
        /// </summary>
        private bool[,] m_masterArray;

        /// <summary>
        /// 過去の回答
        /// </summary>
        private List<clsResult> m_results;
        
        /// <summary>
        /// 答えの生成方法
        /// </summary>
        private bool m_isAlter = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxNumber">最大値</param>
        /// <param name="numberLength">数字の数</param>
        public clsSolver(int maxNumber, int numberLength)
        {
            m_maxNumber = maxNumber;
            m_numberLength = numberLength;
            m_masterArray = new bool[maxNumber + 1, numberLength];

            for (int _number = 0; _number <= m_maxNumber; _number++)
            {
                for (int _place = 0; _place < numberLength; _place++)
                {
                    m_masterArray[_number, _place] = true;
                }
            }

            m_results = new List<clsResult>();
        }

        /// <summary>
        /// 回答を追加する
        /// </summary>
        /// <param name="srcResult">追加対象の回答</param>
        public void AddNewResult(clsResult srcResult) 
        {
            if (m_isAlter) 
            {
                AddNewResultAlt(srcResult);
            }
            else 
            { 
                AddNewResultNormal(srcResult);
            }
        }

        /// <summary>
        /// 回答を追加する
        /// </summary>
        /// <param name="srcResult">追加対象の回答</param>
        private void AddNewResultNormal(clsResult srcResult)
        {
            // マスターを更新する

            // 何も当たらなかった場合は回答に使った数は全てfalse
            if (srcResult.Hit + srcResult.Blow == 0)
            {
                // 何も当たらなかった場合は回答に使った数は全てfalse
                for (int _place = 0; _place < m_numberLength; _place++)
                {
                    foreach (int _number in srcResult.Numbers)
                    {
                        m_masterArray[_number, _place] = false;
                    }
                }
            }
            else
            {
                // Hitが無かった場合は数字と場所を指定してfalse
                if (srcResult.Hit == 0)
                {
                    for (int _place = 0; _place < srcResult.Numbers.Length; _place++)
                    {
                        m_masterArray[srcResult.Numbers[_place], _place] = false;
                    }
                }

                // 入る数字が全て決まった場合
                if (srcResult.Hit + srcResult.Blow == m_numberLength)
                {
                    // 回答に入っていない数は全てfalse
                    for (int _number = 0; _number <= m_maxNumber; _number++)
                    {
                        if (!srcResult.Numbers.Contains(_number))
                        {
                            for (int _place = 0; _place < m_numberLength; _place++)
                            {
                                m_masterArray[_number, _place] = false;
                            }
                        }
                    }
                }
            }

            // 新しい回答を追加する
            clsResult _newResult = new clsResult(srcResult.Hit, srcResult.Blow, srcResult.Numbers);
            _newResult.MakeCandicates();
            m_results.Add(_newResult);
            
            // マスターの結果を基にありえない回答を削除していく
            foreach (clsResult _result in m_results)
            {
                _result.StrictCandicates(m_masterArray);
            }

            // 回答をいい順に並べる(次の候補を出すときに役にたつ?)
            m_results.Sort((a, b) => (b.Hit) - (a.Hit));
            m_results.Sort((a, b) => (b.Hit + b.Blow) - (a.Hit + a.Blow));
        }
        /// <summary>
        /// 新しい回答を作成する
        /// </summary>
        /// <returns></returns>
        public int[] GetNewAnswer()
        {
            if (m_isAlter)
            {
                return GetNewAnswerAlt();
            } 
            else
            {
                return GetNewAnswerNormal();
            }
        }

        /// <summary>
        /// 新しい回答を作成する
        /// </summary>
        /// <returns></returns>
        private int[] GetNewAnswerNormal()
        {
            // 未入力の候補を作成し、これまでの回答と組み合わせていく
            int[] _newAnswer = new int[m_numberLength];
            clsCandicate _newCandicate = new clsCandicate(m_numberLength);
            List<clsCandicate> _candicates = new List<clsCandicate>();
            List<clsResult> _results = m_results.FindAll((v) => 0 < (v.Hit + v.Blow));

            CombineResults(m_numberLength, _candicates, _results, 0, ref _newAnswer);

            return _newAnswer;
        }

        /// <summary>
        /// 過去の回答を組み合わせる(再帰呼び出し)
        /// </summary>
        /// <param name="numberLength">回数に必要な長さ</param>
        /// <param name="candicates">候補のリスト</param>
        /// <param name="results">過去の回答リスト</param>
        /// <param name="index">過去の回答のインデックス</param>
        /// <param name="answer">新しい回答</param>
        /// <returns></returns>
        private bool CombineResults(int numberLength, List<clsCandicate> candicates, List<clsResult> results, int index, ref int[] answer)
        {
            // 先にこれまでの結果を計算しておく
            clsCandicate _currentCandicate = new clsCandicate(numberLength);
            foreach (clsCandicate _current in candicates)
            {
                _currentCandicate.CombineWithoutCheck(_current);
            }

            // 全部の結果を読み終えたら空いてる所を埋めてみる
            if (index == results.Count())
            {
                answer = _currentCandicate.Candicate;
                // 回答を組み合わせた結果-1が残っている場合、空いている数字にマスターから数字を入れる
                for (int _place = 0; _place < answer.Length; _place++)
                {
                    if (answer[_place] == -1)
                    {
                        for (int _number = 0; _number <= m_maxNumber; _number++)
                        {
                            if (m_masterArray[_number, _place] && !answer.Contains(_number) && !_currentCandicate.DisabledNumbers.Contains(_number))
                            {
                                answer[_place] = _number;
                                break;
                            }
                        }
                    }
                }

                return !answer.Contains(-1);
            }

            foreach (clsCandicate _current in results[index].NextCandicates)
            {
                clsCandicate _temp = new clsCandicate(numberLength);
                _temp.CombineWithoutCheck(_current);
                // 参照中の結果が代入出来たら、次の値に進む
                if (_temp.Combine(_currentCandicate, m_maxNumber))
                {
                    candicates.Add(_current);
                    if (CombineResults(numberLength, candicates, results, index + 1, ref answer))
                    {
                        return true;
                    }
                    candicates.Remove(_current);
                }
            }
            return false;

        }

        /// <summary>
        /// 新しい回答を作成する(毎回計算)
        /// これの方がメモリを節約出来るが、時間がかかる
        /// </summary>
        /// <returns></returns>
        private int[] GetNewAnswerAlt()
        {
            return clsGenerateAnswer.GenerateAnswer(m_results, m_masterArray, m_maxNumber, m_numberLength);
        }

        /// <summary>
        /// 回答を追加する(毎回計算用)
        /// </summary>
        /// <param name="srcResult">追加対象の回答</param>
        private void AddNewResultAlt(clsResult srcResult)
        {
            // マスターを更新する

            // 何も当たらなかった場合は回答に使った数は全てfalse
            if (srcResult.Hit + srcResult.Blow == 0)
            {
                for (int _place = 0; _place < m_numberLength; _place++)
                {
                    foreach (int _number in srcResult.Numbers)
                    {
                        m_masterArray[_number, _place] = false;
                    }
                }
            }
            else
            {
                // Hitが無かった場合は数字と場所を指定してfalse
                if (srcResult.Hit == 0)
                {
                    for (int _place = 0; _place < srcResult.Numbers.Length; _place++)
                    {
                        m_masterArray[srcResult.Numbers[_place], _place] = false;
                    }
                }

                // 入る数字が全て決まった場合
                if (srcResult.Hit + srcResult.Blow == m_numberLength)
                {
                    // 回答に入っていない数は全てfalse
                    for (int _number = 0; _number <= m_maxNumber; _number++)
                    {
                        if (!srcResult.Numbers.Contains(_number))
                        {
                            for (int _place = 0; _place < m_numberLength; _place++)
                            {
                                m_masterArray[_number, _place] = false;
                            }
                        }
                    }
                }
            }

            // 新しい回答を追加する
            clsResult _newResult = new clsResult(srcResult.Hit, srcResult.Blow, srcResult.Numbers);
            m_results.Add(_newResult);

            // 回答を成績がいい順位並べる
            m_results.Sort((a, b) => (b.Hit) - (a.Hit));
            m_results.Sort((a, b) => (b.Hit + b.Blow) - (a.Hit + a.Blow));
        }
    }
}
