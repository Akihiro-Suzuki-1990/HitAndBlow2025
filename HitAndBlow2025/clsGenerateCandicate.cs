using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HitAndBlow2025
{
    /// <summary>
    /// 一つの結果から考えられる候補を計算するクラス
    /// 例えば「1H1B」なら「この結果から二つの数字を使って、一つは場所を変える」という条件で数字をあてはめて候補作る
    /// 何が入るか分からない場所は[-1]をいれておき、使った事がない数を後で入れたり、他の結果の候補と組み合わせて埋めていく。
    /// 最大値と使用する数によってメモリ使用量がトンでもない事になる可能性があるので
    /// 可能な限り配列で定義する要素を減らしたい
    /// </summary>
    internal static class clsGenerateCandicate
    {
        /// <summary>
        /// 数の配置
        /// </summary>
        private static List<clsCandicate> m_candicates = new List<clsCandicate>();

        private static int[] m_numbers;

        /// <summary>
        /// 候補を生成する
        /// </summary>
        /// <param name="numbers">プレイヤーの回答</param>
        /// <param name="hit">Hit</param>
        /// <param name="blow">Blow</param>
        /// <returns>候補のリスト</returns>
        public static List<clsCandicate> GenerateCandicate(clsNumberAndPlace[] numbers, int hit, int blow)
        {
            m_numbers = new int[numbers.Length];
            foreach (clsNumberAndPlace numberAndPlace in numbers)
            {
                m_numbers[numberAndPlace.Place] = numberAndPlace.Number;
            }

            m_candicates = new List<clsCandicate>();
            if (0 < hit + blow)
            {
                CalcCombination(new List<clsNumberAndPlace>(), numbers, hit, blow, 0);
            }
            return m_candicates;
        }

        /// <summary>
        /// 使用する数の組み合わせを求める(再帰的に呼び出す)
        /// </summary>
        /// <param name="list">組み合わせに使う数のリスト</param>
        /// <param name="numbers">プレイヤーの回答</param>
        /// <param name="hit">Hit</param>
        /// <param name="blow">Blow</param>
        /// <param name="index"></param>
        private static void CalcCombination(List<clsNumberAndPlace> list, clsNumberAndPlace[] numbers, int hit, int blow, int index)
        {
            // 組み合わせが決まったら並び順を生成する
            if (list.Count == hit + blow)
            {
                GenerateSort(list.ToArray(), numbers.Length, hit, blow);
                return;
            }

            for (int _i = index; _i < numbers.Length; _i++)
            {
                list.Add(numbers[_i]);
                CalcCombination(list, numbers, hit, blow, _i + 1);
                list.Remove(numbers[_i]);
            }
        }

        /// <summary>
        /// 並び順を生成する
        /// </summary>
        /// <param name="numbers">使用する数組み合わせ</param>
        /// <param name="numberLength">回答に必要な長さ</param>
        /// <param name="hit">Hit</param>
        /// <param name="blow">Blow</param>
        private static void GenerateSort(clsNumberAndPlace[] numberAndPlaces, int numberLength, int hit, int blow)
        {
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
                m_candicates.Add(new clsCandicate(_currentCandicate, m_numbers));

            }
            else if (hit == 0)
            {
                // 全てBの場合は並び順を計算する
                int[] _currentCandicate = new int[numberLength];
                for (int _i = 0; _i < numberLength; _i++)
                {
                    _currentCandicate[_i] = -1;
                }
                CalcSort(numberAndPlaces, _currentCandicate, 0, 0);
            }
            else
            {
                // HitとBlowの両方がある場合は
                // HとBの組み合わせを作ってから、それぞれの並び順を計算する
                DevideHitAndBlow(numberAndPlaces, numberLength, hit);
            }
        }

        /// <summary>
        /// 並び順を生成するための再帰呼び出し処理
        /// </summary>
        /// <param name="numberAndPlaces">Bで使用する数の組み合わせ</param>
        /// <param name="candicate">候補の配列</param>
        /// <param name="hit">Hit</param>
        /// <param name="index">インデックス</param>
        private static void CalcSort(clsNumberAndPlace[] numberAndPlaces, int[] candicate, int hit, int index)
        {
            // 全ての数をあてはめたら候補リストに追加する
            if (numberAndPlaces.Length + hit == candicate.Count((e) => -1 < e))
            {
                m_candicates.Add(new clsCandicate(candicate.ToList().ToArray(), m_numbers));
                return;
            }

            // 候補の数字を配列に入れていく
            for (int _i = index; _i < numberAndPlaces.Length; _i++)
            {
                // 複数の場所が考えられるので、それぞれ入れたパターンを考える
                int[] _currentEnabled = numberAndPlaces[_i].EnablePlaces;
                for (int _i2 = 0; _i2 < _currentEnabled.Length; _i2++)
                {
                    // ある場所に入れたら、その数字以外はそこに入れられなくなる
                    int _currentPlace = _currentEnabled[_i2];
                    candicate[_currentPlace] = numberAndPlaces[_i].Number;
                    foreach (clsNumberAndPlace _current in numberAndPlaces)
                    {
                        _current.RemovePlace(_currentPlace);
                    }

                    // 次の数字を入れていこう
                    CalcSort(numberAndPlaces, candicate, hit, _i + 1);

                    // 入れ終わったらいったん戻そう
                    foreach (clsNumberAndPlace _current in numberAndPlaces)
                    {
                        _current.RecoverPlace(_currentPlace);
                    }
                    candicate[_currentPlace] = -1;
                }
            }
        }


        /// <summary>
        /// 候補をHitとBlowに分ける
        /// </summary>
        /// <param name="numberAndPlaces">使用する数組み合わせ</param>
        /// <param name="numberLength">回答に必要な長さ</param>
        /// <param name="hit">Hit</param>
        private static void DevideHitAndBlow(clsNumberAndPlace[] numberAndPlaces, int numberLength, int hit)
        {
            CalcDevideHitAndBlow(numberAndPlaces, new List<clsNumberAndPlace>(), numberLength, hit, 0);
        }

        /// <summary>
        /// 候補をHitとBlowに分けるための再帰呼び出し処理
        /// </summary>
        /// <param name="numberAndPlaces">使用する数の組み合わせ</param>
        /// <param name="hitNumbers">Hitのリスト</param>
        /// <param name="numberLength">回答に必要な長さ</param>
        /// <param name="hit">Hit</param>
        /// <param name="index">インデックス</param>
        private static void CalcDevideHitAndBlow(clsNumberAndPlace[] numberAndPlaces, List<clsNumberAndPlace> hitNumbers, int numberLength, int hit, int index)
        {
            // Hitが全部決まったらHとBを考慮した並び順を確定させる
            if (hitNumbers.Count == hit)
            {
                int[] _currentCandicate = new int[numberLength];
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
                    _blowNumberArray[_index].Generate(numberLength);
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
                CalcSort(_blowNumberArray, _currentCandicate, hit, 0);
                return;
            }

            for (int _i = index; _i < numberAndPlaces.Length; _i++)
            {
                hitNumbers.Add(numberAndPlaces[_i]);
                CalcDevideHitAndBlow(numberAndPlaces, hitNumbers, numberLength, hit, _i + 1);
                hitNumbers.Remove(numberAndPlaces[_i]);
            }
        }
    }
}
