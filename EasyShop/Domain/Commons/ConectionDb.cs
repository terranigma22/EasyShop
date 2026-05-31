namespace EasyShop.Domain.Commons;

internal static class ConectionDb
{
    public static string GetConectionString(string name)
    {
        string folderPath = $"{name}";

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folderPath, name);
        }

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(folderPath, "..", "Library", name);
        }

        if (DeviceInfo.Platform == DevicePlatform.macOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(folderPath, "..", "Library", name);
        }

        return folderPath;
    }

    public static async Task<string?> ExportToDownloadsAsync()
    {
        var dbName = "_easyshop.db";
        var sourcePath = GetConectionString(dbName);

        if (!Path.IsPathRooted(sourcePath))
        {
            sourcePath = Path.Combine(Environment.CurrentDirectory, sourcePath);

            if (!File.Exists(sourcePath))
                sourcePath = Path.Combine(AppContext.BaseDirectory, dbName);
        }

        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Base de datos no encontrada", sourcePath);

        using var con = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={sourcePath}");
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "PRAGMA wal_checkpoint(TRUNCATE)";
        cmd.ExecuteNonQuery();

        var destName = $"EasyShop_{DateTime.Now:yyyy-MM-dd}.db";

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            var downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            var destPath = Path.Combine(downloads, destName);
            File.Copy(sourcePath, destPath, overwrite: true);
            return destPath;
        }

        var cachePath = Path.Combine(FileSystem.CacheDirectory, destName);
        File.Copy(sourcePath, cachePath, overwrite: true);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Exportar base de datos",
            File = new ShareFile(cachePath)
        });

        return null;
    }
}
