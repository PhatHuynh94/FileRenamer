using System;
using System.IO;


namespace FileFolder
{
    class Program
    {
       static int argCheck(string[] args) 
       {
            //if the file is executed by itself
            if (args.Length == 0)
                return 0;
            
            //if only uses -name and
            else if (args.Length == 2 && args[0].Equals("-name"))
                return 1;

            //if the user uses -name and -path are used and the directory exists
            else if (args.Length == 4 && args[0].Equals("-path") && Directory.Exists(args[1]) && args[2].Equals("-name"))
                return 2;

            //if incorrect number of arguments are given or file directory does not exist
            else
                return -1;
        }

        
        static void DisplayFilesName(ImageFolder folder)
        {
            Console.WriteLine("-------------------------------------------");
            foreach (ImageFile f in folder.GetImageList())
                Console.WriteLine(f.FileName+f.FileExtension);
            Console.WriteLine("-------------------------------------------");
        }
        
        static string GetDirectory()
        {
            Console.Write("Enter the directory Path(Leave blank to use the current directory): ");
            string dirPath = Console.ReadLine();

            //If the user enters nothing program uses the directory that the exe is in.
            if (string.IsNullOrWhiteSpace(dirPath))
                dirPath = Directory.GetCurrentDirectory();

            //checks to make sure that the directory exists.
            else if (!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException("Directory does not exist.");
            }

            return dirPath;
        }

        static int GetSortOptions()
        {
            char sortOption;
            int choice = 0;

            do
            {
                Console.WriteLine("1 - Sort By File Name");
                Console.WriteLine("2 - Sort By Last Modified Date");
                Console.WriteLine("3 - Sort By Created Date");
                Console.Write("Please Choose an Option: ");
                sortOption = Console.ReadKey().KeyChar;
                choice = Convert.ToInt32(sortOption);
                Console.Write("\n");
            }
            while (choice < 1 && choice > 3);

            return choice;
        }


        static void SortAndRename(int sortOption, ImageFolder folder)
        {
            if(sortOption == 2)
            {
                folder.SortByLastModifiedDate();
                DisplayFilesName(folder);
            }
            else if(sortOption == 3)
            {
                folder.SortByCreatedDate();
                DisplayFilesName(folder);
            }

            Console.Write("Enter the common name: ");
            string commonName = Console.ReadLine();
            folder.RenameFiles(commonName);
        }

        static void Main(string[] args)
        {
            ImageFolder folder;
            string dirPath;
            int argNum = argCheck(args);


            if (argNum == 0)
            {
                bool repeat = true;

                while (repeat == true)
                {
                    dirPath = GetDirectory();

                    folder = new ImageFolder(dirPath);
                    
                    DisplayFilesName(folder);

                    int sortOption = GetSortOptions();

                    SortAndRename(sortOption, folder);

                    //clean up after done renaming folder
                    folder = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Console.Write("Do you want to continue?(Y/N): ");
                    string cont = Console.ReadLine();
                    cont = cont.Trim();
                    if (cont.ToUpper().Equals("N"))
                        repeat = false;

                    Console.WriteLine("-------------------------------------------");
                }
            }
            else if (argNum ==  1)
            {
                //if the user does not use -path program uses the directory that the exe is in.
                dirPath = Directory.GetCurrentDirectory();

                folder = new ImageFolder(dirPath);
                folder.RenameFiles(args[1]);
            }
            else if(argNum == 2)
            {
                folder = new ImageFolder(args[1]);
                folder.RenameFiles(args[3]);
            }
            else
            {
                //error exit
                throw new ArgumentException("Error. Invalid Arguments or Directory does not exist");
            }
        }
    }
}
