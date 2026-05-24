using SkiaSharp;

namespace EasyShop.Domain.Commons;

internal static class SKHelper
{
    public static async Task<string?> ProcessImageBytesToUri(byte[] value, long sizeInKB = 100)
    {
        try
        {
            var imageBytes = value;

            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using var original = SKBitmap.Decode(imageBytes);

            if (original == null) 
                return null;

            int w = original.Width, h = original.Height;
            const int maxDim = 1200;
            if (w > maxDim || h > maxDim)
            {
                float scale = Math.Min((float)maxDim / w, (float)maxDim / h);
                w = (int)(w * scale);
                h = (int)(h * scale);
            }

            var samplingOptions = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Nearest);
            using var resized = original.Resize(new SKImageInfo(w, h), samplingOptions);

            using var image = SKImage.FromBitmap(resized);

            int quality = 85;
            byte[] result;

            do
            {
                using var data = image.Encode(SKEncodedImageFormat.Jpeg, quality);
                result = data.ToArray();
                quality -= 10;
            } 
            while (result.Length > sizeInKB * 1024 && quality > 10);

            var finalBase64 = Convert.ToBase64String(result);
            var uri = $"data:image/jpeg;base64,{finalBase64}";

            return uri;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<byte[]?> ProcessImageBytesToBytes(byte[] value, long sizeInKB = 100)
    {
        try
        {
            var imageBytes = value;

            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using var original = SKBitmap.Decode(imageBytes);

            if (original == null)
                return null;

            int w = original.Width, h = original.Height;
            const int maxDim = 1200;
            if (w > maxDim || h > maxDim)
            {
                float scale = Math.Min((float)maxDim / w, (float)maxDim / h);
                w = (int)(w * scale);
                h = (int)(h * scale);
            }

            var samplingOptions = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);
            using var resized = original.Resize(new SKImageInfo(w, h), samplingOptions);

            using var image = SKImage.FromBitmap(resized);

            int quality = 85;
            byte[] result;

            do
            {
                using var data = image.Encode(SKEncodedImageFormat.Jpeg, quality);
                result = data.ToArray();
                quality -= 10;
            }
            while (result.Length > sizeInKB * 1024 && quality > 10);

            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
