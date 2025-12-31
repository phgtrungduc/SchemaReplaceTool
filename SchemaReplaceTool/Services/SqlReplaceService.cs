using SqlSchemaReplacer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SqlSchemaReplacer.Services
{
    public class SqlReplaceService
    {
        const string resultFolderPath = "results";

        public ReplaceSummary ProcessFiles(
            List<string> relativePaths,
            string baseDirectory,
            string schema,
            string searchPattern,
            Action<string> logInfo,
            Action<string> logSuccess,
            Action<string> logError)
        {
            var summary = new ReplaceSummary();
            var now = DateTime.Now;
            var folderResultName = resultFolderPath + "/replace-result-" + now.ToFileTimeUtc();

            foreach (var filePath in relativePaths)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    continue;

                summary.Total++;
                string sqlFilePath = Path.Combine(baseDirectory, filePath);

                try
                {
                    if (!File.Exists(sqlFilePath))
                    {
                        logError($"Không tìm thấy file: {sqlFilePath}");
                        continue;
                    }

                    logInfo($"Đang xử lý file: {sqlFilePath}");

                    string content = File.ReadAllText(sqlFilePath);
                    content = Regex.Replace(content, searchPattern, schema, RegexOptions.IgnoreCase);

                    // Tạo đường dẫn file kết quả, giữ nguyên cấu trúc thư mục
                    string outputFilePath = Path.Combine(baseDirectory, folderResultName, filePath);
                    string? outputDirectory = Path.GetDirectoryName(outputFilePath);

                    if (outputDirectory != null && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    File.WriteAllText(outputFilePath, content);

                    summary.Success++;
                    logSuccess($"Replace thành công: {outputFilePath}");
                }
                catch (Exception ex)
                {
                    logError($"Lỗi xử lý file: {sqlFilePath}");
                    logError($"→ {ex.Message}");
                }
            }

            return summary;
        }
    }
}
