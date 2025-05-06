
using Microsoft.Extensions.Configuration;
internal static class Settings
{
    public static string DotFileDirectory = string.Empty;
    public static List<string> DotFileNames = [];
    public static List<string> DotFilesPossibleExtensions = [];

    static Settings()
    {
        IConfiguration config;
        //https://stackoverflow.com/questions/65110479/how-to-get-values-from-appsettings-json-in-a-console-application-using-net-core
        try
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: false);

            config = builder.Build();
        }
        catch(Exception)
        {
            return;
        }

        DotFileDirectory = GetStringVariable("DotFileDirectory", config);
        DotFileNames.AddRange(
                GetListOfStringsVariable("DotFileNames", config)
            );
        DotFilesPossibleExtensions.AddRange(
                GetListOfStringsVariable("DotFilesPossibleExtensions", config)
            );
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
}