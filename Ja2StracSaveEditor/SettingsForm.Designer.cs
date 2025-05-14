namespace Ja2StracSaveEditor
{
    partial class SettingsForm
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
            OkBtn = new Button();
            CancelBtn = new Button();
            label1 = new Label();
            label2 = new Label();
            Ja2StracEdit = new TextBox();
            Ja2DataEdit = new TextBox();
            ChooseJa2StracBtn = new Button();
            ChooseJa2DataBtn = new Button();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(439, 78);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(75, 23);
            OkBtn.TabIndex = 0;
            OkBtn.Text = "Ok";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += button1_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(520, 78);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(75, 23);
            CancelBtn.TabIndex = 1;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "label1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 46);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // Ja2StracEdit
            // 
            Ja2StracEdit.Location = new Point(161, 10);
            Ja2StracEdit.Name = "Ja2StracEdit";
            Ja2StracEdit.Size = new Size(400, 23);
            Ja2StracEdit.TabIndex = 4;
            // 
            // Ja2DataEdit
            // 
            Ja2DataEdit.Location = new Point(161, 42);
            Ja2DataEdit.Name = "Ja2DataEdit";
            Ja2DataEdit.Size = new Size(400, 23);
            Ja2DataEdit.TabIndex = 5;
            // 
            // ChooseJa2StracBtn
            // 
            ChooseJa2StracBtn.Location = new Point(567, 10);
            ChooseJa2StracBtn.Name = "ChooseJa2StracBtn";
            ChooseJa2StracBtn.Size = new Size(28, 23);
            ChooseJa2StracBtn.TabIndex = 6;
            ChooseJa2StracBtn.Text = "...";
            ChooseJa2StracBtn.UseVisualStyleBackColor = true;
            ChooseJa2StracBtn.Click += ChooseJa2StracBtn_Click;
            // 
            // ChooseJa2DataBtn
            // 
            ChooseJa2DataBtn.Location = new Point(567, 42);
            ChooseJa2DataBtn.Name = "ChooseJa2DataBtn";
            ChooseJa2DataBtn.Size = new Size(28, 23);
            ChooseJa2DataBtn.TabIndex = 7;
            ChooseJa2DataBtn.Text = "...";
            ChooseJa2DataBtn.UseVisualStyleBackColor = true;
            ChooseJa2DataBtn.Click += ChooseJa2DataBtn_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(609, 111);
            Controls.Add(ChooseJa2DataBtn);
            Controls.Add(ChooseJa2StracBtn);
            Controls.Add(Ja2DataEdit);
            Controls.Add(Ja2StracEdit);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowIcon = false;
            Text = "Settings";
            FormClosed += SettingsForm_FormClosed;
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OkBtn;
        private Button CancelBtn;
        private Label label1;
        private Label label2;
        private TextBox Ja2StracEdit;
        private TextBox Ja2DataEdit;
        private Button ChooseJa2StracBtn;
        private Button ChooseJa2DataBtn;
    }
}