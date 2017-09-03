using System.Collections.Generic;
using System.IO;

namespace FileFolder
{
    public class ImageFolder : IFolderOperations
    {
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".JPEG", ".BMP", ".GIF", ".PNG", ".TIFF", ".TIF" };
        public string FolderPath { get; protected set; }
        public int NumberOfItems { get; private set; }
        private List<ImageFile> ImageList;

        public ImageFolder()
        {
            ImageList = new List<ImageFile>();
            FolderPath = Directory.GetCurrentDirectory();

            foreach (string s in Directory.GetFiles(FolderPath))
            {
                if (ImageExtensions.Contains(Path.GetExtension(s).ToUpperInvariant()))
                    ImageList.Add(new ImageFile(s));
            }

            ImageList.Sort();
            NumberOfItems = ImageList.Count;
        }

        public ImageFolder(string dir)
        {
            ImageList = new List<ImageFile>();
            FolderPath = dir;

            foreach (string s in Directory.GetFiles(FolderPath))
            {
                if (ImageExtensions.Contains(Path.GetExtension(s).ToUpperInvariant()))
                    ImageList.Add(new ImageFile(s));
            }

            ImageList.Sort();
            NumberOfItems = ImageList.Count;
        }

        ~ImageFolder()
        {
            ImageList = null;
            FolderPath = null;
        }

        public List<ImageFile> GetImageList()
        {
            return ImageList;
        }

        public void RenameFiles(string commonName)
        {
            string newName;
            string digitCount = "D" + NumberOfItems.ToString().Length;

            for(int i = 0; i < NumberOfItems; i++)
            {
                newName = commonName + "_" + (i+1).ToString(digitCount);

                if (!ImageList[i].FileName.Equals(newName))
                {
                    bool fileExist = ImageList[i].RenameFile(newName);

                    //if a file with the new names already exist renames the file.
                    if (fileExist)
                    {
                        for (int j = i + 1; j < NumberOfItems; j++)
                        {
                            if (ImageList[j].FileName.Equals(newName))
                                ImageList[j].RenameFile(newName + "$(Copy)");

                        }
                    }
                }
            }
        }

        public void ChangeFilesExt(string newExt)
        {
            for (int i = 0; i < NumberOfItems; i++)
            {
                ImageList[i].ChangeExtension(newExt);
            }
        }

        public void SortByLastModifiedDate()
        {
            ImageList.Sort((x, y) => x.LastModifiedDate.CompareTo(y.LastModifiedDate));
        }

        public void SortByCreatedDate()
        {
            ImageList.Sort((x, y) => x.CreatedDate.CompareTo(y.CreatedDate));
        }
    }
}
