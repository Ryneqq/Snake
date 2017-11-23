using System.Text;


/// <summary>
/// Static class used to load content, saved by class 'Save', from the file.
/// </summary>
public static class Load {
    private static string path = "./"; 

    public static string FromFile(string name) {
        string _path = path + name + ".txt";
        string read = System.IO.File.ReadAllText(_path, Encoding.Default);

        if (read != null)
            return read;
        return string.Empty;
    }

    public static void Delete(string name) {
        string _path = path + name + ".txt";
        if(System.IO.File.Exists(_path))
            System.IO.File.Delete(_path);
    }

    public static bool CheckForFile(string name) {
        string _path = path + name + ".txt";
        if(System.IO.File.Exists(_path))
            return true;
        return false;
    }
}