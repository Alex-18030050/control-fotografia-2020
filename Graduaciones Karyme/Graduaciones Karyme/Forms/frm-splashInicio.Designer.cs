namespace Graduaciones_Karyme.Forms
{
    partial class frm_splashInicio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_splashInicio));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.progressbar = new CircularProgressBar.CircularProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(67, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(803, 321);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Script", 26.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Indigo;
            this.label1.Location = new System.Drawing.Point(158, 589);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(652, 55);
            this.label1.TabIndex = 2;
            this.label1.Text = "Iniciando... un momento porfavor";
            // 
            // progressbar
            // 
            this.progressbar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.progressbar.AnimationSpeed = 1000;
            this.progressbar.BackColor = System.Drawing.Color.Transparent;
            this.progressbar.Font = new System.Drawing.Font("Segoe Script", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressbar.ForeColor = System.Drawing.Color.Indigo;
            this.progressbar.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.progressbar.InnerMargin = 2;
            this.progressbar.InnerWidth = -1;
            this.progressbar.Location = new System.Drawing.Point(379, 391);
            this.progressbar.MarqueeAnimationSpeed = 2000;
            this.progressbar.Name = "progressbar";
            this.progressbar.OuterColor = System.Drawing.Color.Gray;
            this.progressbar.OuterMargin = -25;
            this.progressbar.OuterWidth = 26;
            this.progressbar.ProgressColor = System.Drawing.Color.Indigo;
            this.progressbar.ProgressWidth = 14;
            this.progressbar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.progressbar.Size = new System.Drawing.Size(192, 182);
            this.progressbar.StartAngle = 270;
            this.progressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressbar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.progressbar.SubscriptText = "";
            this.progressbar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SuperscriptMargin = new System.Windows.Forms.Padding(0);
            this.progressbar.SuperscriptText = "";
            this.progressbar.TabIndex = 3;
            this.progressbar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.progressbar.Value = 68;
            // 
            // frm_splashInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(909, 669);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_splashInicio";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frm_splashInicio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private CircularProgressBar.CircularProgressBar progressbar;
    }
}