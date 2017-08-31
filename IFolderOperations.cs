namespace FileFolder
{
    interface IFolderOperations
    {
        void RenameFiles(string commonName);
        void ChangeFilesExt(string newExt);
        void SortByLastModifiedDate();
        void SortByCreatedDate();
    }
}
