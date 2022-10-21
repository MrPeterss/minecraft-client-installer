// See https://aka.ms/new-console-template for more information
using System.Net;
using Newtonsoft.Json.Linq;
class jsonExporter
{
    static void Main(string[] args)
    {
        var launchername = "MrPeterss-Launcher";
        var minecraftdirname = "MrPeterss";

        //creates folders if they are not created already
        var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Roaming/.minecraft/";
        string[] folders = {path + "versions/" + launchername + "/", path+"planes/mods/", path + minecraftdirname + "/libraries/", path+""};
        CreateDirs(folders);

        //dict of req files; first is address, second is output
        Dictionary<string, string> files = new Dictionary<string, string>()
        {
            {"link/jar", path + "versions/" + launchername + ".jar"},
            {"link/json", path + "versions/" + launchername + ".json"}
        };

        //download req files
        DownloadFiles(files);

        //update json
        UpdateJSON();
    }

    static void CreateDirs(string[] dirs)
    {
        for (int i = 0; i < dirs.Length; i++)
        {
            CreateDirIfNotExist(dirs.ElementAt(i));
        }
    }
    static void CreateDirIfNotExist(string dir)
    {
        bool exists = Directory.Exists(dir);
        if (!exists) Directory.CreateDirectory(dir);
    }

    static void DownloadFiles(Dictionary<string, string> files)
    {
        var client = new WebClient();
        for (int i = 0; i < files.Count; i++)
        {
            client.DownloadFile(files.Keys.ElementAt(i), files.Values.ElementAt(i));
        }
    }

    static void UpdateJSON()
    {
        var jsonpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Roaming/.minecraft/launcher_profiles.json";
        var myJsonString = File.ReadAllText(jsonpath);
        var myJObject = JObject.Parse(myJsonString);
        var launcherObj = JObject.Parse("{\"created\" : \"2000-01-02T00:00:00.000Z\",\"gameDir\" : \"" + path + "/planes\",\"icon\" : \"data:image/png;base64, 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\",\"lastUsed\" : \"2000-01-02T00:00:00.000Z\",\"lastVersionId\" : \"rum-launcher-forge\",\"name\" : \"Rum\",\"type\" : \"custom\"}");
        var toAdd = new JProperty("Rum-Launcher", launcherObj);
        var obj = myJObject.SelectToken("profiles");
        var last = obj?.Last;
        last?.AddAfterSelf(toAdd);
        File.WriteAllText(jsonpath, myJObject.ToString());
        Console.WriteLine("Hello, World!");
    }
}
