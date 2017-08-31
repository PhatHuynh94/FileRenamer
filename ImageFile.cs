using System;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Drawing;

namespace FileFolder
{
    public class ImageFile : IFileOperations, IComparable<ImageFile>
    {
        public string FullFilePath { get; protected set; }
        public string FileDirectory { get; protected set; }
        public string FileName { get; protected set; }
        public string FileExtension { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime LastModifiedDate { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public ImageFile(string imagePath)
        {
            FullFilePath = imagePath;
            FileInfo file = new FileInfo(imagePath);

            FileDirectory = Path.GetDirectoryName(imagePath);
            FileName = Path.GetFileNameWithoutExtension(imagePath);
            FileExtension = Path.GetExtension(imagePath);
            CreatedDate = file.CreationTime;
            LastModifiedDate = file.LastWriteTime;
            Width = Image.FromFile(imagePath).Width;
            Height = Image.FromFile(imagePath).Height;
        }

        public void ChangeLastModifiedDate()
        {
            LastModifiedDate = DateTime.Now;
        }

        public void ChangeExtension(string newExtension)
        {
            Path.ChangeExtension(FullFilePath, newExtension);
            FileExtension = newExtension;
            LastModifiedDate = DateTime.Now;
        }

        public bool RenameFile(string newName)
        {
            string newFilePath = FileDirectory + "\\" + newName + FileExtension;

            if (File.Exists(newFilePath))
                return false;
            else
            {
                //changing file name via copy and delete
                File.Copy(FullFilePath, newFilePath);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(FullFilePath);

                //updating new file information
                FullFilePath = newFilePath;
                FileName = newName;
                LastModifiedDate = DateTime.Now;

                return true;
            }
        }

        public int CompareTo(ImageFile file)
        {
            if (file == null)
                return 1;
            else
                return this.LastModifiedDate.CompareTo(file.LastModifiedDate);
        }
    }
}
