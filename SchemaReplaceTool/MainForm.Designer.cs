using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SqlSchemaReplacer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ComboBox cbEnvironment;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label lblEnv;
        private System.Windows.Forms.TextBox txtSchemaName;
        private System.Windows.Forms.Label lblSchemaName;
        private System.Windows.Forms.Label lblSchemaDisplay;
        private System.Windows.Forms.TextBox txtSchemaDisplay;
        private System.Windows.Forms.Button btnEditSchema;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.GroupBox grpInputSource;
        private System.Windows.Forms.RadioButton rbFileList;
        private System.Windows.Forms.RadioButton rbGitDiff;
        private System.Windows.Forms.TextBox txtRepoPath;
        private System.Windows.Forms.Button btnBrowseRepo;
        private System.Windows.Forms.Button btnGetGitDiff;
        private System.Windows.Forms.Button btnEditFileList;
        private System.Windows.Forms.TextBox txtFileList;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            txtFilePath = new TextBox();
            btnBrowse = new Button();
            cbEnvironment = new ComboBox();
            btnExecute = new Button();
            rtbLog = new RichTextBox();
            lblEnv = new Label();
            txtSchemaName = new TextBox();
            lblSchemaName = new Label();
            lblSchemaDisplay = new Label();
            txtSchemaDisplay = new TextBox();
            btnEditSchema = new Button();
            btnClearLog = new Button();
            grpInputSource = new GroupBox();
            rbFileList = new RadioButton();
            rbGitDiff = new RadioButton();
            txtRepoPath = new TextBox();
            btnBrowseRepo = new Button();
            btnGetGitDiff = new Button();
            btnEditFileList = new Button();
            txtFileList = new TextBox();
            grpInputSource.SuspendLayout();
            SuspendLayout();
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(90, 73);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.PlaceholderText = "Chọn file .txt chứa danh sách...";
            txtFilePath.Size = new Size(440, 23);
            txtFilePath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(535, 73);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(95, 23);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse...";
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lblEnv
            // 
            lblEnv.AutoSize = true;
            lblEnv.Location = new Point(12, 108);
            lblEnv.Name = "lblEnv";
            lblEnv.Size = new Size(75, 15);
            lblEnv.TabIndex = 7;
            lblEnv.Text = "Môi trường:";
            // 
            // cbEnvironment
            // 
            cbEnvironment.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEnvironment.FormattingEnabled = true;
            cbEnvironment.Location = new Point(90, 105);
            cbEnvironment.Name = "cbEnvironment";
            cbEnvironment.Size = new Size(100, 23);
            cbEnvironment.TabIndex = 8;
            cbEnvironment.SelectedIndexChanged += cbEnvironment_SelectedIndexChanged;
            // 
            // lblSchemaName
            // 
            lblSchemaName.AutoSize = true;
            lblSchemaName.Location = new Point(200, 108);
            lblSchemaName.Name = "lblSchemaName";
            lblSchemaName.Size = new Size(75, 15);
            lblSchemaName.TabIndex = 9;
            lblSchemaName.Text = "Tên schema:";
            // 
            // txtSchemaName
            // 
            txtSchemaName.Location = new Point(280, 105);
            txtSchemaName.Name = "txtSchemaName";
            txtSchemaName.PlaceholderText = "${HOST_SCHEMA}";
            txtSchemaName.Size = new Size(150, 23);
            txtSchemaName.TabIndex = 10;
            // 
            // btnExecute
            // 
            btnExecute.Location = new Point(535, 105);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(95, 23);
            btnExecute.TabIndex = 11;
            btnExecute.Text = "Thực hiện";
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Click += btnExecute_Click;
            // 
            // lblSchemaDisplay
            // 
            lblSchemaDisplay.Location = new Point(12, 140);
            lblSchemaDisplay.Name = "lblSchemaDisplay";
            lblSchemaDisplay.Size = new Size(80, 23);
            lblSchemaDisplay.TabIndex = 11;
            lblSchemaDisplay.Text = "Schema:";
            // 
            // txtSchemaDisplay
            // 
            txtSchemaDisplay.Enabled = false;
            txtSchemaDisplay.Location = new Point(95, 137);
            txtSchemaDisplay.Name = "txtSchemaDisplay";
            txtSchemaDisplay.ReadOnly = true;
            txtSchemaDisplay.Size = new Size(230, 23);
            txtSchemaDisplay.TabIndex = 12;
            // 
            // btnEditSchema
            // 
            btnEditSchema.Location = new Point(330, 137);
            btnEditSchema.Name = "btnEditSchema";
            btnEditSchema.Size = new Size(30, 23);
            btnEditSchema.TabIndex = 13;
            btnEditSchema.Text = "✏️";
            btnEditSchema.Click += btnEditSchema_Click;
            // 
            // txtRepoPath
            // 
            txtRepoPath.Enabled = false;
            txtRepoPath.Location = new Point(90, 73);
            txtRepoPath.Name = "txtRepoPath";
            txtRepoPath.PlaceholderText = "Chọn thư mục Git repository...";
            txtRepoPath.Size = new Size(340, 23);
            txtRepoPath.TabIndex = 4;
            txtRepoPath.Visible = false;
            // 
            // btnBrowseRepo
            // 
            btnBrowseRepo.Enabled = false;
            btnBrowseRepo.Location = new Point(435, 73);
            btnBrowseRepo.Name = "btnBrowseRepo";
            btnBrowseRepo.Size = new Size(95, 23);
            btnBrowseRepo.TabIndex = 5;
            btnBrowseRepo.Text = "Browse...";
            btnBrowseRepo.Visible = false;
            btnBrowseRepo.Click += btnBrowseRepo_Click;
            // 
            // btnGetGitDiff
            // 
            btnGetGitDiff.Enabled = false;
            btnGetGitDiff.Location = new Point(535, 73);
            btnGetGitDiff.Name = "btnGetGitDiff";
            btnGetGitDiff.Size = new Size(95, 23);
            btnGetGitDiff.TabIndex = 6;
            btnGetGitDiff.Text = "Lấy diff";
            btnGetGitDiff.Visible = false;
            btnGetGitDiff.Click += btnGetGitDiff_Click;
            // 
            // txtFileList
            // 
            txtFileList.Enabled = false;
            txtFileList.Font = new System.Drawing.Font("Consolas", 8F);
            txtFileList.Location = new Point(12, 170);
            txtFileList.Multiline = true;
            txtFileList.Name = "txtFileList";
            txtFileList.ReadOnly = true;
            txtFileList.ScrollBars = ScrollBars.Both;
            txtFileList.Size = new Size(588, 80);
            txtFileList.TabIndex = 14;
            txtFileList.WordWrap = false;
            txtFileList.Visible = false;
            // 
            // btnEditFileList
            // 
            btnEditFileList.Enabled = false;
            btnEditFileList.Location = new Point(605, 170);
            btnEditFileList.Name = "btnEditFileList";
            btnEditFileList.Size = new Size(25, 23);
            btnEditFileList.TabIndex = 15;
            btnEditFileList.Text = "✏️";
            btnEditFileList.Click += btnEditFileList_Click;
            // 
            // rtbLog
            // 
            rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            rtbLog.Location = new Point(12, 260);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(618, 260);
            rtbLog.TabIndex = 16;
            rtbLog.Text = "";
            // 
            // btnClearLog
            // 
            btnClearLog.Location = new Point(600, 260);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(30, 23);
            btnClearLog.TabIndex = 17;
            btnClearLog.Text = "🗑️";
            btnClearLog.Click += btnClearLog_Click;
            // 
            // grpInputSource
            // 
            grpInputSource.Controls.Add(rbGitDiff);
            grpInputSource.Controls.Add(rbFileList);
            grpInputSource.Location = new Point(12, 12);
            grpInputSource.Name = "grpInputSource";
            grpInputSource.Size = new Size(618, 50);
            grpInputSource.TabIndex = 0;
            grpInputSource.TabStop = false;
            grpInputSource.Text = "Chọn nguồn input";
            // 
            // rbFileList
            // 
            rbFileList.AutoSize = true;
            rbFileList.Checked = true;
            rbFileList.Location = new Point(20, 22);
            rbFileList.Name = "rbFileList";
            rbFileList.Size = new Size(150, 19);
            rbFileList.TabIndex = 0;
            rbFileList.TabStop = true;
            rbFileList.Text = "Sử dụng file list (.txt)";
            rbFileList.UseVisualStyleBackColor = true;
            rbFileList.CheckedChanged += rbFileList_CheckedChanged;
            // 
            // rbGitDiff
            // 
            rbGitDiff.AutoSize = true;
            rbGitDiff.Location = new Point(200, 22);
            rbGitDiff.Name = "rbGitDiff";
            rbGitDiff.Size = new Size(130, 19);
            rbGitDiff.TabIndex = 1;
            rbGitDiff.Text = "Sử dụng Git diff";
            rbGitDiff.UseVisualStyleBackColor = true;
            rbGitDiff.CheckedChanged += rbGitDiff_CheckedChanged;
            //             // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(642, 540);
            Controls.Add(btnEditFileList);
            Controls.Add(txtFileList);
            Controls.Add(btnGetGitDiff);
            Controls.Add(btnBrowseRepo);
            Controls.Add(txtRepoPath);
            Controls.Add(grpInputSource);
            Controls.Add(txtFilePath);
            Controls.Add(btnBrowse);
            Controls.Add(lblEnv);
            Controls.Add(cbEnvironment);
            Controls.Add(btnExecute);
            Controls.Add(lblSchemaName);
            Controls.Add(txtSchemaName);
            Controls.Add(lblSchemaDisplay);
            Controls.Add(txtSchemaDisplay);
            Controls.Add(btnEditSchema);
            Controls.Add(btnClearLog);
            Controls.Add(rtbLog);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SQL HOST_SCHEMA Replace";
            grpInputSource.ResumeLayout(false);
            grpInputSource.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
