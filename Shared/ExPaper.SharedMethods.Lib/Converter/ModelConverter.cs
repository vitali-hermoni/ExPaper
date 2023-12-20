using ExPaper.SharedMethods.Lib.Helper;
using System.Text.Json;

namespace ExPaper.SharedMethods.Lib.Converter;

#nullable disable
public static class ModelConverter
{
    public static async Task<string> SerializeAsync<ModelClass>(ModelClass modelClass) where ModelClass : class
    {
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, modelClass);
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                string ret = EncryptionHelper.EncryptString(await reader.ReadToEndAsync());
                return ret;
            }
        }
    }



    public static async Task<string> SerializeToListAsync<ModelClass>(List<ModelClass> modelClassList) where ModelClass : class
    {
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, modelClassList);
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                var ret = EncryptionHelper.EncryptString(await reader.ReadToEndAsync());
                return ret;
            }
        }
    }




    public static async Task<ModelClass> DeserializeAsync<ModelClass>(string jsonString) where ModelClass : class
    {
        string jsonStrinDecrypt = EncryptionHelper.DecryptString(jsonString);
        using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStrinDecrypt)))
        {
            var ret = await JsonSerializer.DeserializeAsync<ModelClass>(stream);
            return ret;
        }
    }



    public static async Task<List<ModelClass>> DeserializeToListAsync<ModelClass>(string jsonString) where ModelClass : class
    {
        List<ModelClass> ret = new();
        string jsonStrinDecrypt = EncryptionHelper.DecryptString(jsonString);

        using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStrinDecrypt)))
        {
            ret = await JsonSerializer.DeserializeAsync<List<ModelClass>>(stream);
        }

        return ret;
    }
}
