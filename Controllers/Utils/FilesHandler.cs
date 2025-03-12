namespace EPlatformWebApp.Controllers.Utils
{
    using Elfie.Serialization;
    using EPlatformWebApp.DTO;
    using EPlatformWebApp.Model;
    using System;
    using System.Diagnostics;
    using System.IO;
    public static class FilesHandler
    {
        public static bool CreteSourceFolders(int sourceType, int ID)
        {
            if (ID == 0)
                return false;

            bool areDirectoriesCreated = false;
            if (sourceType == (int)FilePathHelper.SourceType.Course)
            {
                for (int source = 0; source <= 4; source++)
                {
                    areDirectoriesCreated = CreateFolder(FilePathHelper.GetSourcePath(ID, sourceType, source));
                }
            }
            else if (sourceType == (int)FilePathHelper.SourceType.User)
            {
                areDirectoriesCreated = CreateFolder(FilePathHelper.GetSourcePath(ID, sourceType));
            }
            
            return areDirectoriesCreated;
        }

        private static bool CreateFolder(string path) 
        {
            try
            {
                if (path == null) return false;

                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path already exists.");
                    return false;
                }

                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }
        }

        public static async Task<Boolean> SaveFile(FileDataDTO fileData)
        {
            try
            {
                string path = FilePathHelper.GetSourcePath(int.Parse(fileData.SourceID), int.Parse(fileData.SourceType),
                    int.Parse(fileData.CourseSource), int.Parse(fileData.imageType));
                string completeFileName = string.Format("{0}\\{1}", path, fileData.File.FileName);

                if (!File.Exists(completeFileName))
                {
                    string fileName = fileData.File.FileName;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileData.File.CopyToAsync(stream);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error has occurred while file was persisted", ex);
                return false;
            }
        }

        public static async Task<String>  loadFileToArray(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    Task<byte[]> task = File.ReadAllBytesAsync(path);
                    byte[] data = await task;
                    string imgB64 = Convert.ToBase64String(data);
                    return imgB64;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while obtaining data", ex);
            }

            return String.Empty;
        }

        public static async Task<bool> deleteFile(String path)
        {
            try
            {
                if (File.Exists(path))
                { 
                    File.Delete(path);
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete file: ", ex);
            }
        }
    }
}
