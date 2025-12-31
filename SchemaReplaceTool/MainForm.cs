using SchemaReplaceTool.Services;
using SqlSchemaReplacer.Services;
using SqlSchemaReplacer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSchemaReplacer
{
    public partial class MainForm : Form
    {
        private readonly SqlReplaceService _service = new();
        private readonly GitService _gitService = new();
        private List<string> _gitDiffFiles = new();

        public MainForm()
        {
            InitializeComponent();

            cbEnvironment.Items.AddRange(new[] { "SIT", "UAT", "PROD" });
            
            // Mặc định chọn File List mode
            rbFileList.Checked = true;
        }

        private void rbFileList_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFileList.Checked)
            {
                // Show File List controls
                txtFilePath.Visible = true;
                btnBrowse.Visible = true;
                txtFilePath.Enabled = true;
                btnBrowse.Enabled = true;
                
                // Hide Git Diff controls
                txtRepoPath.Visible = false;
                btnBrowseRepo.Visible = false;
                btnGetGitDiff.Visible = false;
                txtFileList.Visible = false;
                btnEditFileList.Visible = false;
                
                txtFileList.Clear();
                _gitDiffFiles.Clear();
            }
        }

        private void rbGitDiff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGitDiff.Checked)
            {
                // Hide File List controls
                txtFilePath.Visible = false;
                btnBrowse.Visible = false;
                
                // Show Git Diff controls
                txtRepoPath.Visible = true;
                btnBrowseRepo.Visible = true;
                btnGetGitDiff.Visible = true;
                txtFileList.Visible = true;
                txtRepoPath.Enabled = true;
                btnBrowseRepo.Enabled = true;
                btnGetGitDiff.Enabled = true;
            }
        }

        private void btnBrowseRepo_Click(object sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog
            {
                Description = "Chọn thư mục Git repository"
            };

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtRepoPath.Text = folderDialog.SelectedPath;
            }
        }

        private async void btnGetGitDiff_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRepoPath.Text) || !Directory.Exists(txtRepoPath.Text))
            {
                Log("Thư mục repository không hợp lệ", Color.Red);
                return;
            }

            if (cbEnvironment.SelectedItem == null)
            {
                Log("Chưa chọn môi trường", Color.Red);
                return;
            }

            btnGetGitDiff.Enabled = false;
            rtbLog.Clear();

            string env = cbEnvironment.SelectedItem.ToString();
            string branchName = _gitService.GetBranchNameForEnvironment(env);

            _gitDiffFiles = await Task.Run(() =>
                _gitService.GetChangedFiles(
                    txtRepoPath.Text,
                    branchName,
                    msg => Log(msg, Color.Black),
                    msg => Log(msg, Color.Red)
                )
            );

            if (_gitDiffFiles.Count > 0)
            {
                txtFileList.Text = string.Join(Environment.NewLine, _gitDiffFiles);
                txtFileList.Visible = true;
                txtFileList.Enabled = true;
                btnEditFileList.Visible = true;
                btnEditFileList.Enabled = true;
                Log($"✓ Đã lấy {_gitDiffFiles.Count} file .sql", Color.Green);
            }
            else
            {
                Log("Không tìm thấy file .sql nào thay đổi", Color.Orange);
            }

            btnGetGitDiff.Enabled = true;
        }

        private void btnEditFileList_Click(object sender, EventArgs e)
        {
            if (txtFileList.ReadOnly)
            {
                txtFileList.ReadOnly = false;
                txtFileList.BackColor = Color.White;
                txtFileList.Focus();
                btnEditFileList.Text = "✓";
            }
            else
            {
                txtFileList.ReadOnly = true;
                txtFileList.BackColor = SystemColors.Control;
                btnEditFileList.Text = "✏️";
                
                // Cập nhật lại list từ textbox
                _gitDiffFiles = txtFileList.Text
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToList();
            }
        }

        private void cbEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEnvironment.SelectedItem != null)
            {
                string env = cbEnvironment.SelectedItem.ToString();
                txtSchemaDisplay.Text = EnvironmentSchema.Map[env];
            }
        }

        private void btnEditSchema_Click(object sender, EventArgs e)
        {
            if (cbEnvironment.SelectedItem == null) return;
            if (txtSchemaDisplay.ReadOnly)
            {
                txtSchemaDisplay.ReadOnly = false;
                txtSchemaDisplay.Enabled = true;
                txtSchemaDisplay.Focus();
                btnEditSchema.Text = "✓";
            }
            else
            {
                txtSchemaDisplay.ReadOnly = true;
                txtSchemaDisplay.Enabled = false;
                btnEditSchema.Text = "✏️";
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Text files (*.txt)|*.txt"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
            }
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            if (cbEnvironment.SelectedItem == null)
            {
                Log("Chưa chọn môi trường", Color.Red);
                return;
            }

            List<string> filePaths;
            string baseDirectory;

            // Xác định nguồn file và base directory
            if (rbFileList.Checked)
            {
                // MODE 1: FILE LIST
                if (!File.Exists(txtFilePath.Text))
                {
                    Log("File txt không tồn tại", Color.Red);
                    return;
                }

                var lines = File.ReadAllLines(txtFilePath.Text);
                filePaths = lines
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Trim())
                    .ToList();

                baseDirectory = UniversalFilePathResolver.GetGitRootFromFileList(txtFilePath.Text);

                if (baseDirectory == null)
                {
                    Log("Không tìm thấy thư mục gốc", Color.Red);
                    return;
                }
            }
            else
            {
                // MODE 2: GIT DIFF
                if (string.IsNullOrWhiteSpace(txtRepoPath.Text) || !Directory.Exists(txtRepoPath.Text))
                {
                    Log("Thư mục repository không hợp lệ", Color.Red);
                    return;
                }

                if (_gitDiffFiles.Count == 0)
                {
                    Log("Chưa lấy danh sách file từ Git diff. Vui lòng nhấn 'Lấy diff' trước", Color.Red);
                    return;
                }

                filePaths = _gitDiffFiles;
                baseDirectory = txtRepoPath.Text;
            }

            btnExecute.Enabled = false;
            rtbLog.Clear();

            string env = cbEnvironment.SelectedItem.ToString();
            string schema = string.IsNullOrWhiteSpace(txtSchemaDisplay.Text)
                ? EnvironmentSchema.Map[env]
                : txtSchemaDisplay.Text.Trim();

            // Sử dụng tên schema tùy chỉnh nếu được nhập, ngược lại dùng ${HOST_SCHEMA}
            string searchPattern = string.IsNullOrWhiteSpace(txtSchemaName.Text)
                ? "${HOST_SCHEMA}"
                : txtSchemaName.Text.Trim();

            Log($"Bắt đầu replace '{searchPattern}' với schema: {schema}", Color.Blue);
            Log($"Số file cần xử lý: {filePaths.Count}", Color.Blue);

            var summary = await Task.Run(() =>
                _service.ProcessFiles(
                    filePaths,
                    baseDirectory,
                    schema,
                    searchPattern,
                    msg => Log(msg, Color.Black),
                    msg => Log(msg, Color.Green),
                    msg => Log(msg, Color.Red)
                )
            );

            Log("================================", Color.Black);
            Log("HOÀN THÀNH", Color.Blue);
            Log($"Tổng file: {summary.Total}", Color.Black);
            Log($"Thành công: {summary.Success}", Color.Green);
            Log($"Lỗi: {summary.Failed}", Color.Red);

            btnExecute.Enabled = true;
        }

        private void Log(string message, Color color)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => Log(message, color)));
                return;
            }

            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionColor = color;
            rtbLog.AppendText(message + Environment.NewLine);
            rtbLog.SelectionColor = rtbLog.ForeColor;
        }
    }
}
