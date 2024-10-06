namespace PrestaZic
{
    partial class mainUI
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
            this.img_loader = new System.Windows.Forms.PictureBox();
            this.img_logo = new System.Windows.Forms.PictureBox();
            this.lbl_startArg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.img_loader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // img_loader
            // 
            this.img_loader.Image = global::PrestaZic.Properties.Resources.loader;
            this.img_loader.Location = new System.Drawing.Point(462, 463);
            this.img_loader.Name = "img_loader";
            this.img_loader.Size = new System.Drawing.Size(50, 25);
            this.img_loader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img_loader.TabIndex = 1;
            this.img_loader.TabStop = false;
            // 
            // img_logo
            // 
            this.img_logo.Image = global::PrestaZic.Properties.Resources.penup_20230819_192112;
            this.img_logo.Location = new System.Drawing.Point(245, 119);
            this.img_logo.Name = "img_logo";
            this.img_logo.Size = new System.Drawing.Size(464, 308);
            this.img_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img_logo.TabIndex = 0;
            this.img_logo.TabStop = false;
            // 
            // lbl_startArg
            // 
            this.lbl_startArg.AutoSize = true;
            this.lbl_startArg.Location = new System.Drawing.Point(472, 518);
            this.lbl_startArg.Name = "lbl_startArg";
            this.lbl_startArg.Size = new System.Drawing.Size(70, 13);
            this.lbl_startArg.TabIndex = 2;
            this.lbl_startArg.Text = "Please wait...";
            // 
            // mainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(977, 566);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_startArg);
            this.Controls.Add(this.img_loader);
            this.Controls.Add(this.img_logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinimizeBox = false;
            this.Name = "mainUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PrestaZic";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.mainUI_Shown);
            this.Resize += new System.EventHandler(this.mainUI_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.img_loader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox img_logo;
        private System.Windows.Forms.PictureBox img_loader;
        private System.Windows.Forms.Label lbl_startArg;
    }
}