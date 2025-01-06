namespace HitAndBlow2025
{
    partial class frmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.lblCaptionPastResult = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.btnAnswer = new System.Windows.Forms.Button();
            this.dgvNumbers = new System.Windows.Forms.DataGridView();
            this.lblCaptionChoice = new System.Windows.Forms.Label();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.nudLength = new System.Windows.Forms.NumericUpDown();
            this.btnAutoSolve = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNumbers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLength)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Location = new System.Drawing.Point(12, 173);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowTemplate.Height = 21;
            this.dgvResults.Size = new System.Drawing.Size(1232, 265);
            this.dgvResults.TabIndex = 10;
            // 
            // lblCaptionPastResult
            // 
            this.lblCaptionPastResult.AutoSize = true;
            this.lblCaptionPastResult.Location = new System.Drawing.Point(12, 158);
            this.lblCaptionPastResult.Name = "lblCaptionPastResult";
            this.lblCaptionPastResult.Size = new System.Drawing.Size(29, 12);
            this.lblCaptionPastResult.TabIndex = 9;
            this.lblCaptionPastResult.Text = "結果";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(353, 10);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "ゲーム開始";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Location = new System.Drawing.Point(12, 15);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(43, 12);
            this.lblMax.TabIndex = 0;
            this.lblMax.Text = "最大値:";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(167, 15);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(74, 12);
            this.lblLength.TabIndex = 2;
            this.lblLength.Text = "使用する個数:";
            // 
            // btnAnswer
            // 
            this.btnAnswer.Location = new System.Drawing.Point(12, 132);
            this.btnAnswer.Name = "btnAnswer";
            this.btnAnswer.Size = new System.Drawing.Size(75, 23);
            this.btnAnswer.TabIndex = 7;
            this.btnAnswer.Text = "回答";
            this.btnAnswer.UseVisualStyleBackColor = true;
            this.btnAnswer.Click += new System.EventHandler(this.btnAnswer_Click);
            // 
            // dgvNumbers
            // 
            this.dgvNumbers.AllowUserToAddRows = false;
            this.dgvNumbers.AllowUserToDeleteRows = false;
            this.dgvNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNumbers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNumbers.Location = new System.Drawing.Point(12, 54);
            this.dgvNumbers.Name = "dgvNumbers";
            this.dgvNumbers.RowTemplate.Height = 21;
            this.dgvNumbers.Size = new System.Drawing.Size(1232, 72);
            this.dgvNumbers.TabIndex = 6;
            // 
            // lblCaptionChoice
            // 
            this.lblCaptionChoice.AutoSize = true;
            this.lblCaptionChoice.Location = new System.Drawing.Point(12, 39);
            this.lblCaptionChoice.Name = "lblCaptionChoice";
            this.lblCaptionChoice.Size = new System.Drawing.Size(113, 12);
            this.lblCaptionChoice.TabIndex = 5;
            this.lblCaptionChoice.Text = "数字を入力してください";
            // 
            // nudMax
            // 
            this.nudMax.Location = new System.Drawing.Point(61, 12);
            this.nudMax.Maximum = new decimal(new int[] {
            2025,
            0,
            0,
            0});
            this.nudMax.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(100, 19);
            this.nudMax.TabIndex = 1;
            this.nudMax.Value = new decimal(new int[] {
            2025,
            0,
            0,
            0});
            // 
            // nudLength
            // 
            this.nudLength.Location = new System.Drawing.Point(247, 12);
            this.nudLength.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudLength.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudLength.Name = "nudLength";
            this.nudLength.Size = new System.Drawing.Size(100, 19);
            this.nudLength.TabIndex = 3;
            this.nudLength.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // btnAutoSolve
            // 
            this.btnAutoSolve.Location = new System.Drawing.Point(93, 132);
            this.btnAutoSolve.Name = "btnAutoSolve";
            this.btnAutoSolve.Size = new System.Drawing.Size(75, 23);
            this.btnAutoSolve.TabIndex = 8;
            this.btnAutoSolve.Text = "自動回答";
            this.btnAutoSolve.UseVisualStyleBackColor = true;
            this.btnAutoSolve.Click += new System.EventHandler(this.btnAutoSolve_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(174, 137);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(330, 12);
            this.lblInfo.TabIndex = 11;
            this.lblInfo.Text = "[自動回答]は[最大値20][使用する個数8]くらいまでならまぁ快適かな";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 450);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnAutoSolve);
            this.Controls.Add(this.nudLength);
            this.Controls.Add(this.nudMax);
            this.Controls.Add(this.lblCaptionChoice);
            this.Controls.Add(this.dgvNumbers);
            this.Controls.Add(this.btnAnswer);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblCaptionPastResult);
            this.Controls.Add(this.dgvResults);
            this.Name = "frmMain";
            this.Text = "HappyNewYear";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNumbers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Label lblCaptionPastResult;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Button btnAnswer;
        private System.Windows.Forms.DataGridView dgvNumbers;
        private System.Windows.Forms.Label lblCaptionChoice;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.NumericUpDown nudLength;
        private System.Windows.Forms.Button btnAutoSolve;
        private System.Windows.Forms.Label lblInfo;
    }
}

