using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SqlSchemaReplacer.Services
{
    public class GitService
    {
        public List<string> GetChangedFiles(string repoPath, string branchName, Action<string> logInfo, Action<string> logError)
        {
            var changedFiles = new List<string>();
            
            try
            {
                logInfo($"Đang lấy danh sách file thay đổi từ branch: {branchName}");
                
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = $"diff --name-only {branchName}",
                        WorkingDirectory = repoPath,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    logError($"Git command failed: {error}");
                    return changedFiles;
                }

                var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    
                    // Chỉ lấy file .sql
                    if (trimmedLine.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                    {
                        changedFiles.Add(trimmedLine);
                        logInfo($"  → {trimmedLine}");
                    }
                }

                logInfo($"Tìm thấy {changedFiles.Count} file .sql thay đổi");
            }
            catch (Exception ex)
            {
                logError($"Lỗi khi chạy git: {ex.Message}");
            }

            return changedFiles;
        }
        
        public string GetBranchNameForEnvironment(string environment)
        {
            return environment switch
            {
                "SIT" => "sit",
                "UAT" => "uat",
                "PROD" => "master",
                _ => "master"
            };
        }
    }
}
