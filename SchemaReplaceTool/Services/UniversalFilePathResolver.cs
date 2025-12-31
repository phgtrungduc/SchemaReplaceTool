using System;
using System.IO;

namespace SchemaReplaceTool.Services
{
    /// <summary>
    /// Resolver đường dẫn file dựa trên Git root directory.
    /// Các đường dẫn trong file_list được coi là tương đối so với thư mục chứa .git
    /// </summary>
    public static class UniversalFilePathResolver
    {
        /// <summary>
        /// Tìm thư mục chứa .git bằng cách đi lên các thư mục cha
        /// </summary>
        /// <param name="startDirectory">Thư mục bắt đầu tìm kiếm</param>
        /// <returns>Đường dẫn tuyệt đối đến Git root, hoặc null nếu không tìm thấy</returns>
        public static string FindGitRoot(string startDirectory)
        {
            if (string.IsNullOrWhiteSpace(startDirectory))
                return null;

            DirectoryInfo dir = new DirectoryInfo(startDirectory);
            
            while (dir != null)
            {
                string gitPath = Path.Combine(dir.FullName, ".git");
                
                // Kiểm tra .git là thư mục hoặc file (trong trường hợp submodule)
                if (Directory.Exists(gitPath) || File.Exists(gitPath))
                {
                    return dir.FullName;
                }
                
                dir = dir.Parent;
            }
            
            return null;
        }

        /// <summary>
        /// Lấy Git root directory từ đường dẫn file_list
        /// </summary>
        /// <param name="fileListPath">Đường dẫn tuyệt đối đến file_list.txt</param>
        /// <returns>Đường dẫn tuyệt đối đến Git root</returns>
        /// <exception cref="InvalidOperationException">Ném ra khi không tìm thấy Git root</exception>
        public static string GetGitRootFromFileList(string fileListPath)
        {
            if (string.IsNullOrWhiteSpace(fileListPath))
                throw new ArgumentNullException(nameof(fileListPath));

            string fileListDirectory = Path.GetDirectoryName(fileListPath);
            string gitRoot = FindGitRoot(fileListDirectory);
            
            if (string.IsNullOrEmpty(gitRoot))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy thư mục .git từ: {fileListDirectory}. " +
                    "Đảm bảo file_list.txt nằm trong một Git repository.");
            }

            return gitRoot;
        }

        /// <summary>
        /// Chuyển đổi đường dẫn tương đối thành đường dẫn tuyệt đối dựa trên Git root
        /// </summary>
        /// <param name="gitRootDirectory">Đường dẫn Git root directory</param>
        /// <param name="relativePath">Đường dẫn tương đối từ file_list (ví dụ: 07.packages/PKG_BOAPI_CASHAPI.sql)</param>
        /// <returns>Đường dẫn tuyệt đối của file, hoặc null nếu path không hợp lệ</returns>
        public static string ResolveFullPath(string gitRootDirectory, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(gitRootDirectory))
                throw new ArgumentNullException(nameof(gitRootDirectory));

            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            relativePath = relativePath.Trim();

            // Bỏ qua comment và dòng trống
            if (relativePath.StartsWith("#") || string.IsNullOrEmpty(relativePath))
                return null;

            // Chuẩn hóa separator (/ hoặc \)
            relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar)
                                       .Replace('\\', Path.DirectorySeparatorChar);

            try
            {
                // Kết hợp với Git root directory
                string fullPath = Path.Combine(gitRootDirectory, relativePath);
                return Path.GetFullPath(fullPath);
            }
            catch (Exception ex)
            {
                // Path không hợp lệ
                throw new ArgumentException($"Đường dẫn không hợp lệ: {relativePath}", nameof(relativePath), ex);
            }
        }

        /// <summary>
        /// Kiểm tra file có tồn tại không
        /// </summary>
        /// <param name="gitRootDirectory">Đường dẫn Git root directory</param>
        /// <param name="relativePath">Đường dẫn tương đối từ file_list</param>
        /// <returns>True nếu file tồn tại, False nếu không</returns>
        public static bool FileExists(string gitRootDirectory, string relativePath)
        {
            try
            {
                string fullPath = ResolveFullPath(gitRootDirectory, relativePath);
                return !string.IsNullOrEmpty(fullPath) && File.Exists(fullPath);
            }
            catch
            {
                return false;
            }
        }
    }
}