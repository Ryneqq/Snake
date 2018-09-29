using System.IO;

namespace NeuralNetwork {
    /// <summary>
    /// Static class used for saving content to the file.
    /// </summary>
    public static class Save {
        private static string path = "./";
        
        public static void ToFile(string name, string content)
        {
            string _path = path + name + ".txt";
            if (System.IO.File.Exists(_path))
            {
                System.IO.File.Delete(_path);
            }

            using (StreamWriter file = new StreamWriter(_path, true))
            {
                file.Write(content);
                file.Close();
            }
        }
    }
}