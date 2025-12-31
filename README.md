# Schema Replace Tool

Tool hỗ trợ thay thế schema trong các file SQL/PL-SQL một cách tự động, hữu ích cho việc migrate database giữa các môi trường khác nhau.

## Tính năng chính

- ✅ **Tự động phát hiện Git Repository**: Tìm Git root và resolve đường dẫn file tự động
- ✅ **Replace Schema**: Thay thế schema name trong SQL files (ví dụ: `DEV_SCHEMA` → `PROD_SCHEMA`)
- ✅ **File List Support**: Xử lý nhiều file từ danh sách (file_list.txt)
- ✅ **Git Integration**: Tích hợp với Git để quản lý versions và changes
- ✅ **Replace Summary**: Tổng hợp kết quả replace và lưu báo cáo

## Cấu trúc dự án

```
SchemaReplaceTool/
├── Services/
│   ├── UniversalFilePathResolver.cs  # Resolver đường dẫn dựa trên Git root
│   ├── SqlReplaceService.cs          # Service xử lý replace schema
│   └── GitService.cs                 # Service tích hợp với Git
├── Models/
│   └── ReplaceSummary.cs             # Model chứa thông tin kết quả replace
├── Utils/
│   └── EnvironmentSchema.cs          # Quản lý schema theo environment
├── MainForm.cs                       # Giao diện Windows Forms
└── Program.cs                        # Entry point
```

## Yêu cầu hệ thống

- Windows OS
- .NET 9.0 Runtime
- Git (nếu sử dụng tính năng Git integration)

## Cách sử dụng

### 1. Chuẩn bị file_list.txt

Tạo file `file_list.txt` trong repository Git của bạn (ví dụ: `changelog/file_list.txt`), liệt kê các file cần replace:

```plaintext
07.packages/PKG_BOAPI_CASHAPI.sql
02.tables/t_boapi_call_log/init/t_boapi_call_log.sql
08.sequence/SEQ_BOAPI_BATCHTXNUM.sql
```

**Lưu ý**: Đường dẫn trong file_list.txt phải tương đối so với Git root directory.

### 2. Chạy tool

1. Mở SchemaReplaceTool
2. Chọn file `file_list.txt`
3. Nhập schema nguồn (ví dụ: `DEV_SCHEMA`)
4. Nhập schema đích (ví dụ: `PROD_SCHEMA`)
5. Nhấn "Replace" để thực hiện

### 3. Xem kết quả

Tool sẽ tự động:
- Phát hiện Git root directory
- Resolve đường dẫn tuyệt đối của các file
- Thực hiện replace schema
- Tạo báo cáo kết quả trong thư mục `results/`

## UniversalFilePathResolver

Service đặc biệt giúp resolve đường dẫn file dựa trên Git repository:

```csharp
// Lấy Git root từ đường dẫn file_list
string gitRoot = UniversalFilePathResolver.GetGitRootFromFileList(
    @"d:\Projects\MyDB\changelog\file_list.txt"
);
// Output: d:\Projects\MyDB

// Resolve đường dẫn tương đối
string fullPath = UniversalFilePathResolver.ResolveFullPath(
    gitRoot, 
    "07.packages/PKG_BOAPI_CASHAPI.sql"
);
// Output: d:\Projects\MyDB\07.packages\PKG_BOAPI_CASHAPI.sql

// Kiểm tra file tồn tại
bool exists = UniversalFilePathResolver.FileExists(
    gitRoot, 
    "07.packages/PKG_BOAPI_CASHAPI.sql"
);
```

## Build & Publish

### Build Debug

```powershell
dotnet build SchemaReplaceTool/SchemaReplaceTool.csproj
```

### Publish (Single-file executable)

Chạy file `publish.bat` hoặc:

```powershell
dotnet publish SchemaReplaceTool/SchemaReplaceTool.csproj `
    -c Release `
    -r win-x86 `
    --self-contained true `
    -p:PublishSingleFile=true
```

Output sẽ được tạo trong: `SchemaReplaceTool/bin/Release/net9.0-windows/win-x86/publish/`

## Ví dụ sử dụng

### Ví dụ 1: Migrate từ DEV sang UAT

```
Source Schema: DEV_SCHEMA
Target Schema: UAT_SCHEMA
File List: d:\Projects\MyDB\changelog\file_list.txt
```

### Ví dụ 2: Migrate từ UAT sang PROD

```
Source Schema: UAT_SCHEMA
Target Schema: PROD_SCHEMA
File List: d:\Projects\MyDB\changelog\file_list.txt
```

## Cấu trúc file_list.txt

File `file_list.txt` cần đặt trong Git repository và liệt kê các file theo đường dẫn tương đối:

```plaintext
# Comment lines start with #
07.packages/PKG_BOAPI_CASHAPI.sql
07.packages/pkg_boapi_common.sql
02.tables/t_boapi_call_log/init/t_boapi_call_log.sql

# Có thể để dòng trống
08.sequence/SEQ_BOAPI_BATCHTXNUM.sql
```

## Lưu ý quan trọng

⚠️ **Git Repository**: File `file_list.txt` phải nằm trong một Git repository. Tool sẽ tự động tìm thư mục `.git` và sử dụng làm root directory.

⚠️ **Backup**: Luôn backup dữ liệu trước khi chạy replace trên production.

⚠️ **Encoding**: Đảm bảo các file SQL sử dụng encoding UTF-8 để tránh lỗi ký tự.

## Troubleshooting

### Lỗi "Không tìm thấy thư mục .git"

**Nguyên nhân**: File_list.txt không nằm trong Git repository.

**Giải pháp**: 
- Đảm bảo file_list.txt nằm trong thư mục đã được git init
- Kiểm tra có thư mục `.git` trong các thư mục cha

### Lỗi "File not found"

**Nguyên nhân**: Đường dẫn trong file_list.txt không đúng.

**Giải pháp**:
- Kiểm tra đường dẫn trong file_list.txt phải tương đối so với Git root
- Sử dụng `/` thay vì `\` trong đường dẫn
- Đảm bảo file thực sự tồn tại

## License

[Specify your license here]

## Author

[Your name/team]

## Version History

- **1.0.0** - Initial release with UniversalFilePathResolver
  - Tự động phát hiện Git root
  - Hỗ trợ đường dẫn tương đối trong file_list.txt
  - Replace schema với báo cáo chi tiết
