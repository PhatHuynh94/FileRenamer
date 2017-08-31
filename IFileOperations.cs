namespace FileFolder
{
    interface IFileOperations
    {
        void RenameFile(string newName);
        void ChangeExtension(string newExtension);
        void ChangeLastModifiedDate();
    }
}
