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

    public static string ExportToDownloads()
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

        var downloads = DeviceInfo.Platform == DevicePlatform.WinUI
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        var destPath = Path.Combine(downloads, $"EasyShop_{DateTime.Now:yyyy-MM-dd}.db");
        File.Copy(sourcePath, destPath, overwrite: true);
        return destPath;
    }
}
