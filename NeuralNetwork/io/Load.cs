using System.Text;
using System.IO;

namespace NeuralNetwork {
    /// <summary>
    /// Static class used to load content, saved by class 'Save', from the file.
    /// </summary>
    public static class Load {
        private static string path = "./"; 

        public static string FromDirectory(string name) {
            string _path = path + name;
            string read = string.Empty;
            string newLine = "\r\n";
            var files = Directory.GetFiles(_path);

            foreach (var file in files) {
                read += FromFile(file) + newLine;
            }

            return read;
        }

        public static string FromFile(string name) {
            string _path = path + name;
            string read = File.ReadAllText(_path, Encoding.Default);

            if (read != null)
                return read;
            return string.Empty;
        }

        public static void Delete(string name) {
            string _path = path + name + ".txt";
            if(File.Exists(_path))
                File.Delete(_path);
        }

        public static bool CheckForFile(string name) {
            string _path = path + name + ".txt";
            if(File.Exists(_path))
                return true;
            return false;
        }
    }
}