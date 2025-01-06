namespace HitAndBlow2025
{
    internal class clsHitAndBlowGame
    {
        /// <summary>
        /// 答え
        /// </summary>
        private clsAnswer m_Answer;

        /// <summary>
        ///　コンストラクタ(答えを生成する)
        /// </summary>
        /// <param name="max">最大値</param>
        /// <param name="length">長さ</param>
        public clsHitAndBlowGame(int max, int length)
        {
            m_Answer = new clsAnswer(max, length);
        }

        /// <summary>
        /// 回答をチェックする
        /// </summary>
        /// <param name="numbers">回答</param>
        /// <returns>結果</returns>
        public clsResult CheckYourAnswer(int[] numbers)
        {
            return m_Answer.CheckAnswer(numbers);
        }

        /// <summary>
        /// デバッグ用に回答を教えてもらう
        /// </summary>
        /// <returns></returns>
        public int[] GetAnswer()
        {
            return m_Answer.Answer;
        }
    }
}
