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
}
