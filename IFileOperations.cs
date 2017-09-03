namespace FileFolder
{
    interface IFileOperations
    {
        bool RenameFile(string newName);
        void ChangeExtension(string newExtension);
        void ChangeLastModifiedDate();
    }
}
