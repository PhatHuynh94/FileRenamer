﻿using System.Collections.Generic;
using System.IO;

namespace FileFolder
{
    public class ImageFolder : IFolderOperations
    {
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".TIFF", ".TIF" };
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

            NumberOfItems = ImageList.Count;
        }

        ~ImageFolder()
        {
            ImageList = null;
            FolderPath = null;
        }

        public void RenameFiles(string commonName)
        {
            string newName;
            string digitCount = "D" + NumberOfItems.ToString().Length;

            for(int i = 0; i < NumberOfItems; i++)
            {
                newName = commonName + "_" + (i+1).ToString(digitCount);
                ImageList[i].RenameFile(newName);
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
            ImageList.Sort();
        }

        public void SortByCreatedDate()
        {
            ImageList.Sort((x, y) => x.CreatedDate.CompareTo(y.CreatedDate));
        }
    }
}