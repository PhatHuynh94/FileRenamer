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

        public void RenameFile(string newName)
        {
            string newFilePath = FileDirectory + "\\" + newName + FileExtension;
            
            File.Copy(FullFilePath, newFilePath);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            File.Delete(FullFilePath);
            FullFilePath = newFilePath;
            FileName = newName;
            LastModifiedDate = DateTime.Now;
        }

        public int CompareTo(ImageFile file)
        {
            if (file == null)
                return 1;
            else
                return this.LastModifiedDate.CompareTo(file.LastModifiedDate);
        }

        //test to see if file is locked
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
