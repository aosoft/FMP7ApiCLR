namespace FMP7ApiSample
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.listboxMusicList = new System.Windows.Forms.ListBox();
			this.labelMusicTitle = new System.Windows.Forms.Label();
			this.labelMusicCreator = new System.Windows.Forms.Label();
			this.checkAutoPlay = new System.Windows.Forms.CheckBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnAllClearList = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.panelParameterView = new System.Windows.Forms.Panel();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listboxMusicList
			// 
			this.listboxMusicList.AllowDrop = true;
			this.listboxMusicList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listboxMusicList.FormattingEnabled = true;
			this.listboxMusicList.ItemHeight = 12;
			this.listboxMusicList.Location = new System.Drawing.Point(3, 42);
			this.listboxMusicList.Name = "listboxMusicList";
			this.listboxMusicList.Size = new System.Drawing.Size(502, 208);
			this.listboxMusicList.TabIndex = 0;
			this.listboxMusicList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listboxMusicList_MouseDoubleClick);
			this.listboxMusicList.DragDrop += new System.Windows.Forms.DragEventHandler(this.listboxMusicList_DragDrop);
			this.listboxMusicList.DragEnter += new System.Windows.Forms.DragEventHandler(this.listboxMusicList_DragEnter);
			// 
			// labelMusicTitle
			// 
			this.labelMusicTitle.AutoSize = true;
			this.labelMusicTitle.Location = new System.Drawing.Point(3, 6);
			this.labelMusicTitle.Name = "labelMusicTitle";
			this.labelMusicTitle.Size = new System.Drawing.Size(28, 12);
			this.labelMusicTitle.TabIndex = 1;
			this.labelMusicTitle.Text = "Title";
			// 
			// labelMusicCreator
			// 
			this.labelMusicCreator.AutoSize = true;
			this.labelMusicCreator.Location = new System.Drawing.Point(3, 26);
			this.labelMusicCreator.Name = "labelMusicCreator";
			this.labelMusicCreator.Size = new System.Drawing.Size(43, 12);
			this.labelMusicCreator.TabIndex = 2;
			this.labelMusicCreator.Text = "Creator";
			// 
			// checkAutoPlay
			// 
			this.checkAutoPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkAutoPlay.AutoSize = true;
			this.checkAutoPlay.Location = new System.Drawing.Point(3, 298);
			this.checkAutoPlay.Name = "checkAutoPlay";
			this.checkAutoPlay.Size = new System.Drawing.Size(232, 16);
			this.checkAutoPlay.TabIndex = 3;
			this.checkAutoPlay.Text = "停止、ループしたら自動的に次曲を再生する";
			this.checkAutoPlay.UseVisualStyleBackColor = true;
			// 
			// timer1
			// 
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnAllClearList
			// 
			this.btnAllClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAllClearList.Location = new System.Drawing.Point(3, 269);
			this.btnAllClearList.Name = "btnAllClearList";
			this.btnAllClearList.Size = new System.Drawing.Size(99, 23);
			this.btnAllClearList.TabIndex = 4;
			this.btnAllClearList.Text = "リストクリア";
			this.btnAllClearList.UseVisualStyleBackColor = true;
			this.btnAllClearList.Click += new System.EventHandler(this.btnAllClearList_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 2);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
			this.splitContainer1.Panel1.Controls.Add(this.panelParameterView);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.listboxMusicList);
			this.splitContainer1.Panel2.Controls.Add(this.btnAllClearList);
			this.splitContainer1.Panel2.Controls.Add(this.checkAutoPlay);
			this.splitContainer1.Panel2.Controls.Add(this.labelMusicTitle);
			this.splitContainer1.Panel2.Controls.Add(this.labelMusicCreator);
			this.splitContainer1.Size = new System.Drawing.Size(981, 320);
			this.splitContainer1.SplitterDistance = 469;
			this.splitContainer1.TabIndex = 5;
			// 
			// panelParameterView
			// 
			this.panelParameterView.AutoScroll = true;
			this.panelParameterView.BackColor = System.Drawing.SystemColors.Window;
			this.panelParameterView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelParameterView.Location = new System.Drawing.Point(0, 0);
			this.panelParameterView.Name = "panelParameterView";
			this.panelParameterView.Size = new System.Drawing.Size(469, 320);
			this.panelParameterView.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(986, 323);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "FMP7Api Sample";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listboxMusicList;
		private System.Windows.Forms.Label labelMusicTitle;
		private System.Windows.Forms.Label labelMusicCreator;
		private System.Windows.Forms.CheckBox checkAutoPlay;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnAllClearList;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Panel panelParameterView;
	}
}

