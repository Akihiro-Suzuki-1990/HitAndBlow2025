using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// 使用する数と結果を渡して、次の候補となる並び順を生成するクラス
    /// </summary>
    internal static class clsPlaces
    {
        /// <summary>
        /// 数の配置
        /// </summary>
        private static List<int[]> m_candicatePlaces = new List<int[]>();

        /// <summary>
        /// 配置を生成する
        /// </summary>
        /// <param name="numbers">使用する数</param>
        /// <param name="numberLength">長さ</param>
        /// <param name="hit">ヒット</param>
        /// <param name="blow">ブロウ</param>
        /// <returns>数の配置のリスト</returns>
        public static List<int[]> Generate(clsNumberAndPlace[] numberAndPlaces, int numberLength, int hit, int blow)
        {
            m_candicatePlaces = new List<int[]>();

            if (blow == 0)
            {
                // blowがないので全てH(0H0Bはこのクラスを使わないから)
                int[] _currentCandicate = new int[numberLength];
                for (int _i = 0; _i < numberLength; _i++)
                {
                    _currentCandicate[_i] = -1;
                }

                foreach (clsNumberAndPlace _currentNumberAndPlace in numberAndPlaces)
                {
                    _currentCandicate[_currentNumberAndPlace.Place] = _currentNumberAndPlace.Number;
                }
                m_candicatePlaces.Add(_currentCandicate);

            }
            else if (hit == 0)
            {
                // hitがないので全てB
                int[] _currentCandicate = new int[numberLength];
                for (int _i = 0; _i < numberLength; _i++)
                {
                    _currentCandicate[_i] = -1;
                }
                CalcPlaces(numberAndPlaces, 0, _currentCandicate, 0);
            }
            else
            {
                // 選んだ数をH(位置を変えない数字)とB(位置を変える数字)に分ける
                // 先にHを出す(どっちが先でもよい)
                List<clsNumberAndPlace[]> _hitNumbersList = clsCombination.Generate(numberAndPlaces, hit);
                // H以外がBとなる
                List<clsNumberAndPlace[]> _blowNumberList = new List<clsNumberAndPlace[]>();
                foreach (clsNumberAndPlace[] _hitNumbers in _hitNumbersList)
                {
                    HashSet<clsNumberAndPlace> _currentBlows = new HashSet<clsNumberAndPlace>(numberAndPlaces);
                    foreach (clsNumberAndPlace _hitNumber in _hitNumbers)
                    {
                        _currentBlows.RemoveWhere((e) => e.Number == _hitNumber.Number);
                    }
                    _blowNumberList.Add(_currentBlows.ToArray());
                }

                for (int _i = 0; _i < _hitNumbersList.Count; _i++) 
                {
                    int[] _currentCandicate = new int[numberLength];
                    for (int _i2 = 0; _i2 < numberLength; _i2++)
                    {
                        _currentCandicate[_i2] = -1;
                    }

                    // Hを先に確定させる
                    foreach (clsNumberAndPlace _currentHit in _hitNumbersList[_i])
                    {
                        _currentCandicate[_currentHit.Place] = _currentHit.Number;
                        foreach (clsNumberAndPlace _cuurentBlow in _blowNumberList[_i])
                        {
                            _cuurentBlow.RemovePlace(_currentHit.Place);
                        }
                    }

                    // 残りのBを入れる
                    CalcPlaces(_blowNumberList[_i], hit, _currentCandicate, 0);
                }
            }
            return m_candicatePlaces;
        }

        /// <summary>
        /// 並び順を生成する関数
        /// </summary>
        /// <param name="numberAndPlaces">使用する数字の配列</param>
        /// <param name="hitCount">ヒット</param>
        /// <param name="candicate">作成中の候補</param>
        /// <param name="index">インデックス</param>
        private static void CalcPlaces(clsNumberAndPlace[] numberAndPlaces, int hitCount, int[] candicate, int index)
        {
            // 全ての数をあてはめたらリストに追加する
            if (numberAndPlaces.Length + hitCount == candicate.Count((e) => -1 < e))
            {
                m_candicatePlaces.Add(candicate);
                return;
            }

            // 候補の数字を配列に入れていく
            for (int _i = index; _i < numberAndPlaces.Length; _i++)
            {
                // 複数の場所が考えられるので、それぞれ入れたパターンを考える
                int[] _currentEnabled = numberAndPlaces[_i].EnablePlaces;
                for (int _i2 = index; _i2 < _currentEnabled.Length; _i2++)
                {
                    // ある場所に入れたら、その数字以外はそこに入れられなくなる
                    int _currentPlace = _currentEnabled[_i2];
                    candicate[_currentPlace] = numberAndPlaces[_i].Number;
                    foreach (clsNumberAndPlace _current in numberAndPlaces)
                    {
                        _current.RemovePlace(_currentPlace);
                    }

                    // 次の数字を入れていこう
                    CalcPlaces(numberAndPlaces, hitCount, candicate, _i + 1);

                    // 入れ終わったらいったん戻そう
                    candicate[_currentPlace] = -1;
                    foreach (clsNumberAndPlace _current in numberAndPlaces)
                    {
                        _current.RecoverPlace(_currentPlace);
                    }
                }
            }
        }
    }
}

