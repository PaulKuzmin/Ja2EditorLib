namespace Ja2StracSaveEditor
{
    partial class ItemsForm
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
            search = new Zuby.ADGV.AdvancedDataGridViewSearchToolBar();
            SuspendLayout();
            // 
            // search
            // 
            search.AllowMerge = false;
            search.GripStyle = ToolStripGripStyle.Hidden;
            search.Location = new Point(0, 0);
            search.MaximumSize = new Size(0, 27);
            search.MinimumSize = new Size(0, 27);
            search.Name = "search";
            search.RenderMode = ToolStripRenderMode.Professional;
            search.Size = new Size(800, 27);
            search.TabIndex = 1;
            search.Text = "advancedDataGridViewSearchToolBar1";
            // 
            // ItemsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(search);
            Name = "ItemsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Items";
            Load += ItemsForm_Load;
            Shown += ItemsForm_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Zuby.ADGV.AdvancedDataGridViewSearchToolBar search;
    }
}