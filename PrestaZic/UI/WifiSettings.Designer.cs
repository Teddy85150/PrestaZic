namespace PrestaZic.UI
{
    partial class WifiSettings
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
            this.listBoxNetworks = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxNetworks
            // 
            this.listBoxNetworks.FormattingEnabled = true;
            this.listBoxNetworks.ItemHeight = 20;
            this.listBoxNetworks.Location = new System.Drawing.Point(12, 12);
            this.listBoxNetworks.Name = "listBoxNetworks";
            this.listBoxNetworks.Size = new System.Drawing.Size(373, 684);
            this.listBoxNetworks.TabIndex = 0;
            this.listBoxNetworks.SelectedIndexChanged += new System.EventHandler(this.listBoxNetworks_SelectedIndexChanged);
            // 
            // WifiSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 732);
            this.Controls.Add(this.listBoxNetworks);
            this.Name = "WifiSettings";
            this.ShowIcon = false;
            this.Text = "Paramètres wifi";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxNetworks;
    }
}