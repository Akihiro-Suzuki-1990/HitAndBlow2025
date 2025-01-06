using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitAndBlow2025
{
    /// <summary>
    /// clsGenerateCandicateとclsSolverのCombineResultを組み合わせて一つの回答を提示するクラス
    /// 一つの結果から一つの候補が出た時点で次の結果に進み、全て矛盾なく組み合わせられたらそれが回答となる
    /// 場合によっては最初の結果まで戻りつつ、再帰的に計算する
    /// </summary>
    internal static class clsGenerateAnswer
    {
        /// <summary>
        /// 新しい回答の基になる過去の回答
        /// </summary>
        private static List<clsResult> m_result;

        /// <summary>
        /// 全体を通して数字が入るか否かを判定する配列
        /// </summary>
        private static bool[,] m_masterArray;

        /// <summary>
        /// 最大値
        /// </summary>
        private static int m_maxNumber;

        /// <summary>
        /// 回答に必要な長さ
        /// </summary>
        private static int m_numberLength;

        /// <summary>
        /// 新しい回答
        /// </summary>
        private static int[] m_answer;

        /// <summary>
        /// 過去の回答から新しい答えを作成する(一つだけ作ってとっとと返したい)
        /// メモリは節約出来るが、全然とっとと返せない
        /// </summary>
        /// <param name="results">結果のリスト</param>
        /// <param name="masterArray">全体を通して数字が入るか否かを判定する配列</param>
        /// <param name="maxNumber">最大値</param>
        /// <param name="numberLength">回答に必要な長さ</param>
        /// <returns></returns>
        public static int[] GenerateAnswer(List<clsResult> results, bool[,] masterArray, int maxNumber, int numberLength)
        {
            if (0 < results.Count)
            {
                m_result = new List<clsResult>();
                m_masterArray = masterArray;
                m_maxNumber = maxNumber;
                m_numberLength = numberLength;
                foreach (clsResult _result in results)
                {
                    if (0 < _result.Hit + _result.Blow)
                    {
                        m_result.Add(new clsResult(_result.Hit, _result.Blow, _result.Numbers));
                    }
                }

                if (0 < m_result.Count)
                {
                    // 回答の0番目から組み合わせ→並び順と計算し、全ての回答で一つずつ並び順を揃えたら、回答を作成する
                    clsNumberAndPlace[] _usebleNumbers = new clsNumberAndPlace[m_numberLength];
                    for (int _i = 0; _i < m_numberLength; _i++)
                    {
                        _usebleNumbers[_i] = new clsNumberAndPlace(m_result[0].Numbers[_i], _i);
                    }
                    CalcCombination(new List<clsCandicate>(), new clsCandicate(m_numberLength), 0, new List<clsNumberAndPlace>(), _usebleNumbers, m_result[0], 0);
                }
                else
                {
                    // 0H0Bしか結果が出ていない場合は、マスターから回答を求める
                    int[] _answer = new int[numberLength];
                    for (int _place = 0; _place < _answer.Length; _place++)
                    {
                        for (int _number = 0; _number <= m_maxNumber; _number++)
                        {
                            if (m_masterArray[_number, _place] && !_answer.Contains(_number))
                            {
                                _answer[_place] = _number;
                                break;
                            }
                        }
                    }
                    m_answer = _answer;
                }
            }
            else
            {
                int[] _answer = new int[numberLength];
                for (int _place = 0; _place < _answer.Length; _place++)
                {
                    _answer[_place] = _place;
                }
                m_answer = _answer;
            }

            return m_answer;
        }

        /// <summary>
        /// 使用する数の組み合わせを求める(再帰的に呼び出す)
        /// </summary>
        /// <param name="candicates">抽出した候補のリスト</param>
        /// <param name="combinedCandicate">組み合わせ済みの候補(組み合わせや並び順を作成する途中で参照する)</param>
        /// <param name="resultIndex">対象とする回答のインデックス</param>
        /// <param name="list">組み合わせに使う数のリスト</param>
        /// <param name="usebleNumbers">組み合わせに使用可能な数字</param>
        ///  <param name="result">対象とする回答</param>
        /// <param name="index">回答内の数字のインデックス</param>
        private static bool CalcCombination(List<clsCandicate> candicates, clsCandicate combinedCandicate, int resultIndex, List<clsNumberAndPlace> list, clsNumberAndPlace[] usebleNumbers, clsResult result, int index)
        {
            // 組み合わせが決まったら並び順を生成する
            if (result.Hit + result.Blow <= list.Count)
            {
                return GenerateSort(candicates, combinedCandicate, resultIndex, list.ToArray(), result);
            }

            for (int _i = index; _i < usebleNumbers.Length; _i++)
            {
                bool _flg = false;
                for (int _place = 0; _place < m_numberLength; _place++)
                {
                    if (m_masterArray[usebleNumbers[_i].Number, _place])
                    {
                        _flg = true;
                        break;
                    }
                }

                if (_flg && !combinedCandicate.DisabledNumbers.Contains(usebleNumbers[_i].Number))
                {
                    clsNumberAndPlace _new = new clsNumberAndPlace(usebleNumbers[_i]);
                    _new.Generate(m_numberLength);
                    list.Add(_new);
                    if (CalcCombination(candicates, combinedCandicate, resultIndex, list, usebleNumbers, result, _i + 1))
                    {
                        return true;
                    }
                    list.Remove(_new);
                }
            }

            return false;
        }

        /// <summary>
        /// 並び順を生成する
        /// </summary>
        /// <param name="candicates">抽出した候補のリスト</param>
        /// <param name="combinedCandicate">組み合わせ済みの候補(組み合わせや並び順を作成する途中で参照する)</param>
        /// <param name="resultIndex">対象とする回答のインデックス</param>
        /// <param name="numberAndPlaces">使用する数組み合わせ</param>
        /// <param name="result">対象とする回答</param>
        private static bool GenerateSort(List<clsCandicate> candicates, clsCandicate combinedCandicate, int resultIndex, clsNumberAndPlace[] numberAndPlaces, clsResult result)
        {
            // 組み合わせが決まった段階で組み合わせが成り立つかを判定する
            int[] _tempNumbers = new int[m_numberLength];
            for (int _i = 0; _i < m_numberLength; _i++)
            {
                _tempNumbers[_i] = -1;
            }
            foreach (clsNumberAndPlace _curent in numberAndPlaces)
            {
                _tempNumbers[_curent.Place] = _curent.Number;
            }
            clsCandicate _temp = new clsCandicate(_tempNumbers, result.Numbers);

            for (int _i = 0; _i < m_numberLength; _i++)
            {
                if (_temp.DisabledNumbers.Contains(combinedCandicate.Candicate[_i]))
                {
                    return false;
                }
                else if (combinedCandicate.DisabledNumbers.Contains(_temp.Candicate[_i]))
                {
                    return false;
                }
            }

            if (result.Blow == 0)
            {
                // blowがないので全てH(0H0Bはこのクラスを使わないから)
                int[] _newCandicate = new int[m_numberLength];
                for (int _i = 0; _i < m_numberLength; _i++)
                {
                    _newCandicate[_i] = -1;
                }

                foreach (clsNumberAndPlace _currentNumberAndPlace in numberAndPlaces)
                {
                    _newCandicate[_currentNumberAndPlace.Place] = _currentNumberAndPlace.Number;
                }

                return CalcAnswer(candicates, combinedCandicate, resultIndex, _newCandicate, result.Numbers);
            }
            else
            {
                // HとBの組み合わせを作ってから、それぞれの並び順を計算する
                List<clsNumberAndPlace> _decidedNumberAndPlaces = new List<clsNumberAndPlace>();
                return CalcDevideHitAndBlow(candicates, combinedCandicate, resultIndex, numberAndPlaces, _decidedNumberAndPlaces, result, 0);
            }
        }

        /// <summary>
        /// 並び順を生成するための再帰呼び出し処理
        /// </summary>
        /// <param name="candicates">抽出した候補のリスト</param>
        /// <param name="combinedCandicate">組み合わせ済みの候補(組み合わせや並び順を作成する途中で参照する)</param>
        /// <param name="resultIndex">対象とする回答のインデックス</param>
        /// <param name="numberAndPlaces">Bで使用する数の組み合わせ</param>
        /// <param name="candicate">候補の配列</param>
        /// <param name="result">対象とする回答</param>
        /// <param name="index">インデックス</param>
        private static bool CalcSort(List<clsCandicate> candicates, clsCandicate combinedCandicate, int resultIndex, clsNumberAndPlace[] numberAndPlaces, int[] candicate, clsResult result, int index)
        {
            // 全ての数をあてはめたら候補リストに追加する
            if (numberAndPlaces.Length + result.Hit <= candicate.Count((e) => -1 < e))
            {
                return CalcAnswer(candicates, combinedCandicate, resultIndex, candicate, result.Numbers);
            }

            // 候補の数字を配列に入れていく
            for (int _i = index; _i < numberAndPlaces.Length; _i++)
            {
                // 複数の場所が考えられるので、それぞれ入れたパターンを考える
                int[] _currentEnabled = numberAndPlaces[_i].EnablePlaces;
                for (int _i2 = 0; _i2 < _currentEnabled.Length; _i2++)
                {
                    int _currentPlace = _currentEnabled[_i2];

                    // 同じ場所に別の数字が入っている場合
                    // 別の場所に同じ数字が入っている場合
                    // マスターで使用不可能な場合
                    // ここに数字は入れられない
                    // テストのために分けます。
                    if (-1 < combinedCandicate.Candicate[_currentPlace] && combinedCandicate.Candicate[_currentPlace] != numberAndPlaces[_i].Number)
                    {
                        continue;
                    }
                    if (combinedCandicate.Candicate.Contains(numberAndPlaces[_i].Number) && combinedCandicate.Candicate[_currentPlace] != numberAndPlaces[_i].Number)
                    {
                        continue;
                    }
                    if (!m_masterArray[numberAndPlaces[_i].Number, _currentPlace])
                    {
                        continue;
                    }


                    // ある場所に入れたら、その数字以外はそこに入れられなくなる
                    candicate[_currentPlace] = numberAndPlaces[_i].Number;
                    foreach (clsNumberAndPlace _current in numberAndPlaces)
                    {
                        _current.RemovePlace(_currentPlace);
                    }

                    // 次の数字を入れていこう
                    if (CalcSort(candicates, combinedCandicate, resultIndex, numberAndPlaces, candicate, result, _i + 1))
                    {
                        return true;
                    }

                    // 入れ終わったらいったん戻そう
                    foreach (clsNumberAndPlace _current in numberAndPlaces)
                    {
                        _current.RecoverPlace(_currentPlace);
                    }
                    candicate[_currentPlace] = -1;
                }
            }

            return false;
        }

        /// <summary>
        /// 候補をHitとBlowに分けるための再帰呼び出し処理
        /// </summary>
        /// <param name="candicates">抽出した候補のリスト</param>
        /// <param name="combinedCandicate">組み合わせ済みの候補(組み合わせや並び順を作成する途中で参照する)</param>
        /// <param name="resultIndex">対象とする回答のインデックス</param>
        /// <param name="numberAndPlaces">使用する数の組み合わせ</param>
        /// <param name="hitNumbers">Hitのリスト</param>
        /// <param name="result">対象とする回答</param>
        /// <param name="index">インデックス</param>
        private static bool CalcDevideHitAndBlow(List<clsCandicate> candicates, clsCandicate combinedCandicate, int resultIndex, clsNumberAndPlace[] numberAndPlaces, List<clsNumberAndPlace> hitNumbers, clsResult result, int index)
        {
            // Hitが全部決まったらHとBを考慮した並び順を確定させる
            if (result.Hit <= hitNumbers.Count)
            {
                int[] _currentCandicate = new int[m_numberLength];
                for (int _i = 0; _i < _currentCandicate.Length; _i++)
                {
                    _currentCandicate[_i] = -1;
                }

                // ユーザーの回答からHを除いたモノがBの組み合わせになる
                HashSet<clsNumberAndPlace> _blowNumbers = new HashSet<clsNumberAndPlace>(numberAndPlaces);
                _blowNumbers.RemoveWhere(hitNumbers.Contains);

                clsNumberAndPlace[] _blowNumberArray = new clsNumberAndPlace[_blowNumbers.Count];
                int _index = 0;
                foreach (clsNumberAndPlace _blowNumber in _blowNumbers)
                {
                    _blowNumberArray[_index] = new clsNumberAndPlace(_blowNumber);
                    _blowNumberArray[_index].Generate(m_numberLength);
                    _index++;
                }

                // Hの並びを先に確定させる
                foreach (clsNumberAndPlace _currentHit in hitNumbers)
                {
                    _currentCandicate[_currentHit.Place] = _currentHit.Number;
                    foreach (clsNumberAndPlace _cuurentBlow in _blowNumberArray)
                    {
                        _cuurentBlow.RemovePlace(_currentHit.Place);
                    }
                }

                // 残りのBの並び順を生成する
                return CalcSort(candicates, combinedCandicate, resultIndex, _blowNumberArray, _currentCandicate, result, 0);
            }

            for (int _i = index; _i < numberAndPlaces.Length; _i++)
            {
                // 同じ場所に別の数字が入っている場合
                // 別の場所に同じ数字が入っている場合
                // マスターで使用不可能な場合
                // ここに数字は入れられない
                // テストのためにわけます
                if (-1 < combinedCandicate.Candicate[numberAndPlaces[_i].Place] && combinedCandicate.Candicate[numberAndPlaces[_i].Place] != numberAndPlaces[_i].Number)
                {
                    continue;
                }
                if (combinedCandicate.Candicate.Contains(numberAndPlaces[_i].Number) && combinedCandicate.Candicate[numberAndPlaces[_i].Place] != numberAndPlaces[_i].Number)
                {
                    continue;
                }
                if (!m_masterArray[numberAndPlaces[_i].Number, numberAndPlaces[_i].Place])
                {
                    continue;
                }
                hitNumbers.Add(numberAndPlaces[_i]);
                if (CalcDevideHitAndBlow(candicates, combinedCandicate, resultIndex, numberAndPlaces, hitNumbers, result, _i + 1))
                {
                    return true;
                }
                hitNumbers.Remove(numberAndPlaces[_i]);
            }
            return false;
        }

        /// <summary>
        /// 候補から回答を作成する
        /// </summary>
        /// <param name="candicates">過去の候補</param>
        /// <param name="combinedCandicate">組み合わせ済みの候補(組み合わせや並び順を作成する途中で参照する)</param>
        /// <param name="resultIndex">結果のインデックス</param>
        /// <param name="newCandicate">新しい候補</param>
        /// <param name="numbers">回答で使った数</param>
        private static bool CalcAnswer(List<clsCandicate> candicates, clsCandicate combinedCandicate, int resultIndex, int[] newCandicate, int[] numbers)
        {
            // 新しい候補が過去の候補と矛盾しないかを判定する

            // マスターで使用不可になっていないか
            for (int _i = 0; _i < newCandicate.Length; _i++)
            {
                if (-1 < newCandicate[_i] && !m_masterArray[newCandicate[_i], _i])
                {
                    return false;
                }
            }

            // 新しい候補を組み合わせて確認
            clsCandicate _tempCandicate = new clsCandicate(newCandicate.ToList().ToArray(), numbers);
            if (!_tempCandicate.Combine(combinedCandicate, m_maxNumber))
            {
                return false;
            }

            bool _result = false;
            clsCandicate _newCandicate = new clsCandicate(newCandicate.ToList().ToArray(), numbers);
            candicates.Add(_newCandicate);
            if (m_result.Count <= candicates.Count)
            {
                // これまでの結果を全て組み合わせたら回答を作る前の仕上げ
                int[] _newAnswer = _tempCandicate.Candicate;

                // 回答を組み合わせた結果-1が残っている場合、空いている数字にマスターから数字を入れる
                for (int _place = 0; _place < _newAnswer.Length; _place++)
                {
                    if (_newAnswer[_place] == -1)
                    {
                        for (int _number = 0; _number <= m_maxNumber; _number++)
                        {
                            if (m_masterArray[_number, _place] && !_newAnswer.Contains(_number) && !_tempCandicate.DisabledNumbers.Contains(_number))
                            {
                                _newAnswer[_place] = _number;
                                break;
                            }
                        }
                    }
                }

                if (_newAnswer.Contains(-1))
                {
                    _result = false;
                }
                else
                {
                    m_answer = _newAnswer;
                    _result = true;
                }
            }
            else
            {
                // まだ足りないので次の結果を見直す
                int _newResultIndex = resultIndex + 1;

                // 次の候補を探すため、これまでの候補を組み合わせておく
                clsCandicate _nextCandicate = new clsCandicate(m_numberLength);
                foreach (clsCandicate _current in candicates)
                {
                    _nextCandicate.CombineWithoutCheck(_current);
                }

                // これまでの候補で使用する事になっている数字と決まらない数字を分ける
                List<clsNumberAndPlace> _newDecidedNumbers = new List<clsNumberAndPlace>();
                List<clsNumberAndPlace> _newUsebleNumbers = new List<clsNumberAndPlace>();
                for (int _i = 0; _i < m_numberLength; _i++)
                {
                    clsNumberAndPlace _curentNumberAndPlace = new clsNumberAndPlace(m_result[_newResultIndex].Numbers[_i], _i);
                    _curentNumberAndPlace.Generate(m_numberLength);
                    if (_nextCandicate.Candicate.Contains(m_result[_newResultIndex].Numbers[_i]))
                    {
                        _newDecidedNumbers.Add(_curentNumberAndPlace);
                        if (m_result[_newResultIndex].Hit + m_result[_newResultIndex].Blow < _newDecidedNumbers.Count())
                        {
                            candicates.Remove(_newCandicate);
                            return false;
                        }

                    }
                    else
                    {
                        _newUsebleNumbers.Add(_curentNumberAndPlace);
                    }
                }
                _result = CalcCombination(candicates, _nextCandicate, _newResultIndex, _newDecidedNumbers, _newUsebleNumbers.ToArray(), m_result[_newResultIndex], 0);
            }
            candicates.Remove(_newCandicate);
            return _result;
        }
    }
}
