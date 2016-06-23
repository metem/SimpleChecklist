using System;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    public class DroidFileUtils : IFileUtils
    {
        public async Task<string> ReadTextAsync(object file, bool useApplicationDataPath = false)
        {
            if (useApplicationDataPath)
            {
                var fileName = file as string;
                if (fileName != null)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                    try
                    {
                        using (var streamReader = new StreamReader(Path.Combine(path, fileName)))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task SaveTextAsync(object file, string contents, bool useApplicationDataPath = false)
        {
            if (useApplicationDataPath)
            {
                var fileName = file as string;
                if (fileName != null)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                    using (var streamWriter = new StreamWriter(Path.Combine(path, fileName), false))
                    {
                        await streamWriter.WriteAsync(contents);
                    }
                }
            }
        }

        public Task<byte[]> ReadBytesAsync(object file, bool useApplicationDataPath = false)
        {
            throw new NotImplementedException();
        }

        public Task SaveBytesAsync(object file, byte[] contents, bool useApplicationDataPath = false)
        {
            throw new NotImplementedException();
        }
    }
}