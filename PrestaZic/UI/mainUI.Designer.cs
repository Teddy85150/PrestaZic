﻿namespace PrestaZic
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
            this.lbl_help = new System.Windows.Forms.Label();
            this.btn_shutdown = new System.Windows.Forms.Button();
            this.btn_wifi = new System.Windows.Forms.Button();
            this.btn_sendToDrive = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.img_loader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // img_loader
            // 
            this.img_loader.Image = global::PrestaZic.Properties.Resources.loader;
            this.img_loader.Location = new System.Drawing.Point(924, 890);
            this.img_loader.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.img_loader.Name = "img_loader";
            this.img_loader.Size = new System.Drawing.Size(100, 48);
            this.img_loader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img_loader.TabIndex = 1;
            this.img_loader.TabStop = false;
            // 
            // img_logo
            // 
            this.img_logo.Image = global::PrestaZic.Properties.Resources.penup_20230819_192112;
            this.img_logo.Location = new System.Drawing.Point(491, 229);
            this.img_logo.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.img_logo.Name = "img_logo";
            this.img_logo.Size = new System.Drawing.Size(928, 592);
            this.img_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img_logo.TabIndex = 0;
            this.img_logo.TabStop = false;
            // 
            // lbl_startArg
            // 
            this.lbl_startArg.AutoSize = true;
            this.lbl_startArg.Location = new System.Drawing.Point(944, 996);
            this.lbl_startArg.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbl_startArg.Name = "lbl_startArg";
            this.lbl_startArg.Size = new System.Drawing.Size(140, 25);
            this.lbl_startArg.TabIndex = 2;
            this.lbl_startArg.Text = "Please wait...";
            // 
            // lbl_help
            // 
            this.lbl_help.AutoSize = true;
            this.lbl_help.Location = new System.Drawing.Point(1676, 411);
            this.lbl_help.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_help.Name = "lbl_help";
            this.lbl_help.Size = new System.Drawing.Size(0, 25);
            this.lbl_help.TabIndex = 3;
            // 
            // btn_shutdown
            // 
            this.btn_shutdown.Location = new System.Drawing.Point(216, 332);
            this.btn_shutdown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_shutdown.Name = "btn_shutdown";
            this.btn_shutdown.Size = new System.Drawing.Size(128, 62);
            this.btn_shutdown.TabIndex = 4;
            this.btn_shutdown.Text = "Arrêter";
            this.btn_shutdown.UseVisualStyleBackColor = true;
            this.btn_shutdown.Visible = false;
            // 
            // btn_wifi
            // 
            this.btn_wifi.Location = new System.Drawing.Point(216, 459);
            this.btn_wifi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_wifi.Name = "btn_wifi";
            this.btn_wifi.Size = new System.Drawing.Size(128, 62);
            this.btn_wifi.TabIndex = 5;
            this.btn_wifi.Text = "Wifi";
            this.btn_wifi.UseVisualStyleBackColor = true;
            this.btn_wifi.Visible = false;
            this.btn_wifi.Click += new System.EventHandler(this.btn_wifi_Click);
            // 
            // btn_sendToDrive
            // 
            this.btn_sendToDrive.Location = new System.Drawing.Point(216, 564);
            this.btn_sendToDrive.Margin = new System.Windows.Forms.Padding(4);
            this.btn_sendToDrive.Name = "btn_sendToDrive";
            this.btn_sendToDrive.Size = new System.Drawing.Size(128, 62);
            this.btn_sendToDrive.TabIndex = 6;
            this.btn_sendToDrive.Text = "Send to drive";
            this.btn_sendToDrive.UseVisualStyleBackColor = true;
            this.btn_sendToDrive.Visible = false;
            this.btn_sendToDrive.Click += new System.EventHandler(this.btn_sendToDrive_ClickAsync);
            // 
            // mainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1955, 1089);
            this.ControlBox = false;
            this.Controls.Add(this.btn_sendToDrive);
            this.Controls.Add(this.btn_wifi);
            this.Controls.Add(this.btn_shutdown);
            this.Controls.Add(this.lbl_help);
            this.Controls.Add(this.lbl_startArg);
            this.Controls.Add(this.img_loader);
            this.Controls.Add(this.img_logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Label lbl_help;
        private System.Windows.Forms.Button btn_shutdown;
        private System.Windows.Forms.Button btn_wifi;
        private System.Windows.Forms.Button btn_sendToDrive;
    }
}