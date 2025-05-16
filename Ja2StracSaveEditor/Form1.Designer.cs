namespace Ja2StracSaveEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            SettingsBtn = new Button();
            linkLabel1 = new LinkLabel();
            SaveBtn = new Button();
            OpenBtn = new Button();
            panel2 = new Panel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            inventoryControl1 = new InventoryControl();
            panel3 = new Panel();
            ReloadAllBtn = new Button();
            RepearAllBtn = new Button();
            SoldiersLst = new ListBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(SettingsBtn);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(SaveBtn);
            panel1.Controls.Add(OpenBtn);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1406, 47);
            panel1.TabIndex = 0;
            // 
            // SettingsBtn
            // 
            SettingsBtn.Location = new Point(172, 9);
            SettingsBtn.Name = "SettingsBtn";
            SettingsBtn.Size = new Size(75, 23);
            SettingsBtn.TabIndex = 3;
            SettingsBtn.Text = "Settings";
            SettingsBtn.UseVisualStyleBackColor = true;
            SettingsBtn.Click += SettingsBtn_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(268, 13);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(245, 15);
            linkLabel1.TabIndex = 2;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://github.com/PaulKuzmin/Ja2EditorLib";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(91, 9);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(75, 23);
            SaveBtn.TabIndex = 1;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // OpenBtn
            // 
            OpenBtn.Location = new Point(10, 9);
            OpenBtn.Name = "OpenBtn";
            OpenBtn.Size = new Size(75, 23);
            OpenBtn.TabIndex = 0;
            OpenBtn.Text = "Open";
            OpenBtn.UseVisualStyleBackColor = true;
            OpenBtn.Click += OpenBtn_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(tabControl1);
            panel2.Controls.Add(SoldiersLst);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 47);
            panel2.Name = "panel2";
            panel2.Size = new Size(1406, 640);
            panel2.TabIndex = 1;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(134, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1272, 640);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1264, 612);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Attributes & Stats";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(inventoryControl1);
            tabPage2.Controls.Add(panel3);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1264, 612);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Inventory";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // inventoryControl1
            // 
            inventoryControl1.BackgroundImage = (Image)resources.GetObject("inventoryControl1.BackgroundImage");
            inventoryControl1.BackgroundImageLayout = ImageLayout.Zoom;
            inventoryControl1.Location = new Point(3, 54);
            inventoryControl1.Name = "inventoryControl1";
            inventoryControl1.Size = new Size(1264, 552);
            inventoryControl1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(ReloadAllBtn);
            panel3.Controls.Add(RepearAllBtn);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(1258, 48);
            panel3.TabIndex = 1;
            // 
            // ReloadAllBtn
            // 
            ReloadAllBtn.Location = new Point(92, 11);
            ReloadAllBtn.Name = "ReloadAllBtn";
            ReloadAllBtn.Size = new Size(75, 23);
            ReloadAllBtn.TabIndex = 1;
            ReloadAllBtn.Text = "Reload All";
            ReloadAllBtn.UseVisualStyleBackColor = true;
            ReloadAllBtn.Click += ReloadAllBtn_Click;
            // 
            // RepearAllBtn
            // 
            RepearAllBtn.Location = new Point(10, 11);
            RepearAllBtn.Name = "RepearAllBtn";
            RepearAllBtn.Size = new Size(75, 23);
            RepearAllBtn.TabIndex = 0;
            RepearAllBtn.Text = "Repear All";
            RepearAllBtn.UseVisualStyleBackColor = true;
            RepearAllBtn.Click += RepearAllBtn_Click;
            // 
            // SoldiersLst
            // 
            SoldiersLst.Dock = DockStyle.Left;
            SoldiersLst.FormattingEnabled = true;
            SoldiersLst.ItemHeight = 15;
            SoldiersLst.Location = new Point(0, 0);
            SoldiersLst.Name = "SoldiersLst";
            SoldiersLst.Size = new Size(134, 640);
            SoldiersLst.TabIndex = 0;
            SoldiersLst.SelectedIndexChanged += SoldiersLst_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1406, 687);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Ja2StracSaveEditor";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListBox SoldiersLst;
        private LinkLabel linkLabel1;
        private Button SaveBtn;
        private Button OpenBtn;
        private Button SettingsBtn;
        private InventoryControl inventoryControl1;
        private Panel panel3;
        private Button ReloadAllBtn;
        private Button RepearAllBtn;
    }
}
