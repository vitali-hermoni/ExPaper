using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExPaper.SharedMethods.Lib.Helper;

public class JsonHelper
{

    public static bool IsValidJsonList(string jsonString)
    {
        try
        {
            JArray.Parse(jsonString);
            return true;
        }
        catch (JsonReaderException)
        {
            return false;
        }
    }
}
