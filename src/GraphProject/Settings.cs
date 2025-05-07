
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
internal static class Settings
{
    public static string DotFileDirectory = string.Empty;
    public static List<string> DotFileNames = [];
    public static List<string> DotFilesPossibleExtensions = [];
    public static string StartElementName = string.Empty;

    static Settings()
    {
        if(!GetConfiguration(out var config))
        {
            return;
        }

        DotFileDirectory = GetStringVariable("DotFileDirectory", config!);
        DotFileNames.AddRange(
                GetListOfStringsVariable("DotFileNames", config!)
            );
        DotFilesPossibleExtensions.AddRange(
                GetListOfStringsVariable("DotFilesPossibleExtensions", config!)
            );
        StartElementName = GetStringVariable("StartElementName", config!);
    }

    private static List<string> GetListOfStringsVariable(
        string fieldName,
        IConfiguration config)
    {
        string? value = config.GetValue<string>(fieldName);
        return string.IsNullOrEmpty(value) 
            || string.IsNullOrWhiteSpace(value)
            ? []
            : [.. value.Split(";")];
    }

    private static string GetStringVariable(
        string fieldName,
        IConfiguration config)
    {
        return config.GetValue<string>(fieldName, string.Empty);
    }

    private static bool GetConfiguration(out IConfiguration? config)
    {
        try
        {
            //https://stackoverflow.com/questions/65110479/how-to-get-values-from-appsettings-json-in-a-console-application-using-net-core
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: false);

            config = builder.Build();
            return true;
        }
        catch(Exception)
        {
            config = null;
            return false;
        }
    } 
}