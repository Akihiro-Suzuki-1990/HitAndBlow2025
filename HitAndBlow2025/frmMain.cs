using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitAndBlow2025
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// ゲームを遊ぶためのクラス
        /// </summary>
        clsHitAndBlowGame m_hitAndBlow;

        /// <summary>
        /// 回答するためのクラス
        /// </summary>
        clsSolver m_solver;

        /// <summary>
        /// デバッグ用(回答を見せるか否か)
        /// </summary>
        bool m_viewAnswer = false;

        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロード後イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            ResetGame();
        }

        /// <summary>
        /// リセット(問題を作り直す)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        /// <summary>
        /// ユーザーが回答する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnswer_Click(object sender, EventArgs e)
        {
            btnAutoSolve.Enabled = false;
            int[] _yourAnswer = new int[dgvNumbers.Columns.Count];
            for (int _i = 0; _i < dgvNumbers.Columns.Count; _i++)
            {
                if (dgvNumbers.Rows[0].Cells[_i].Value != null)
                {
                    int.TryParse(dgvNumbers.Rows[0].Cells[_i].Value.ToString(), out _yourAnswer[_i]);
                }
                else
                {
                    MessageBox.Show("明けましておめでとうございます。有効な値を入力してください。");
                    return;
                }
            }

            clsResult _newResult = m_hitAndBlow.CheckYourAnswer(_yourAnswer);
            if (_newResult != null)
            {
                if (_newResult.IsCorrect)
                {
                    MessageBox.Show("明けましておめでとうございます。正解です。");
                }
                dgvResults.Rows.Insert(0);
                dgvResults.Rows[0].Cells[0].Value = _newResult.Hit;
                dgvResults.Rows[0].Cells[1].Value = _newResult.Blow;
                for (int _i = 0; _i < _yourAnswer.Length; _i++)
                {
                    dgvResults.Rows[0].Cells[_i + 2].Value = _yourAnswer[_i];
                }
            }
            else
            {
                MessageBox.Show("明けましておめでとうございます。有効な値を入力してください。");
            }
        }

        /// <summary>
        /// 自動で回答する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoSolve_Click(object sender, EventArgs e)
        {
            btnAnswer.Enabled = false;
            int[] _newAnswer = m_solver.GetNewAnswer();

            for (int _i = 0; _i < _newAnswer.Length; _i++)
            {
                dgvNumbers.Rows[0].Cells[_i].Value = _newAnswer[_i];
            }

            clsResult _newResult = m_hitAndBlow.CheckYourAnswer(_newAnswer);
            if (_newResult != null)
            {
                if (_newResult.IsCorrect)
                {
                    MessageBox.Show("明けましておめでとうございます。正解です。");
                }
                dgvResults.Rows.Insert(0);
                dgvResults.Rows[0].Cells[0].Value = _newResult.Hit;
                dgvResults.Rows[0].Cells[1].Value = _newResult.Blow;
                for (int _i = 0; _i < _newAnswer.Length; _i++)
                {
                    dgvResults.Rows[0].Cells[_i + 2].Value = _newAnswer[_i];
                }

                m_solver.AddNewResult(_newResult);
            }
            else
            {
                MessageBox.Show("明けましておめでとうございます。有効な値を入力してください。");
            }
        }

        /// <summary>
        /// リセット
        /// </summary>
        private void ResetGame()
        {
            if (nudMax.Value < nudLength.Value)
            {
                MessageBox.Show("明けましておめでとうございます。最大値が個数より大きくないとゲームになりませんよ。");
            }
            else
            {
                m_hitAndBlow = new clsHitAndBlowGame((int)nudMax.Value, (int)nudLength.Value);
                m_solver = new clsSolver((int)nudMax.Value, (int)nudLength.Value);
                dgvNumbers.Rows.Clear();
                dgvResults.Rows.Clear();

                dgvNumbers.Columns.Clear();
                dgvResults.Columns.Clear();

                dgvResults.Columns.Add("Hit", "Hit");
                dgvResults.Columns.Add("Blow", "Blow");
                for (int _i = 0; _i < nudLength.Value; _i++)
                {
                    dgvNumbers.Columns.Add("number" + _i.ToString(), _i.ToString());
                    dgvResults.Columns.Add("number" + _i.ToString(), _i.ToString());
                }
                dgvNumbers.Rows.Add();
                if (m_viewAnswer)
                {
                    dgvNumbers.Rows.Add();
                    int[] _answer = m_hitAndBlow.GetAnswer();
                    for (int _i = 0; _i < _answer.Length; _i++)
                    {
                        dgvNumbers.Rows[dgvNumbers.Rows.Count - 1].Cells[_i].Value = _answer[_i];
                    }

                }

                // 両方押せるようにする
                btnAutoSolve.Enabled = true;
                btnAnswer.Enabled = true;
            }
        }
    }
}
