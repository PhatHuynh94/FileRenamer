using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileRenamer
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

        static void processDir(string path, string commonName)
        {

            string newName;
            string[] fileEntries = Directory.GetFiles(path);
            int counter = 1;
 
            Console.WriteLine("\nThis folder started with {0} file(s)\n", fileEntries.Length);


            for(int i = 0; i < fileEntries.Length; i++)
            {
                //creates new name for the file
                newName = rename(fileEntries[i], commonName, i);

                //if the file already exist increases the number after the file name until it is a name that does not exist
                while (File.Exists(newName))
                {
                    newName = rename(fileEntries[i], commonName, i + counter);
                    counter++;
                }
                counter = 0;

                //renaming the file
                File.Move(fileEntries[i], newName);
            }

            //number of files after
            fileEntries = Directory.GetFiles(path);
            Console.WriteLine("This folder now contains {0} file(s)\n", fileEntries.Length);
        }

        static string rename(string oldName, string commonName, int fileNum)
        {
            //dir - contains the file's current directory
            //ext -  contains file extention
            //numExt - the file number
            //newName - new name for the file
            string dir, ext, numExt, newName;

            //puts all separate parts of the file name into a string field
            int index = oldName.LastIndexOf('.');
            ext = oldName.Substring(index + 1);
            dir = Path.GetDirectoryName(oldName) + '\\';

            //makes sure that all files stay in the current order
            //only works if there are less than 1000000 files in the folder
            numExt = fileNum.ToString("D6");

            //renames the file
            newName = dir + '\\' + commonName + "-" + numExt + '.' + ext;
            return newName;
        }

        static void Main(string[] args)
        {
            int argNum = argCheck(args);
            string dirPath, commonName;

            if(argNum == 0)
            {
                Console.Write("Enter the directory Path: ");
                dirPath = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(dirPath))
                    dirPath = Directory.GetCurrentDirectory();

                Console.Write("Enter the common name: ");
                commonName = Console.ReadLine();

                processDir(dirPath, commonName);

                Console.WriteLine("Press Any key to exit.");
                Console.ReadKey();
            }
            else if (argNum ==  1)
            {
                dirPath = Directory.GetCurrentDirectory();
                processDir(dirPath, args[1]);
            }
            else if(argNum == 2)
            {
                processDir(args[1], args[3]);
            }
            else
            {
                Console.WriteLine("Error. Invalid Arguments or Directory does not exist");
            }

            Console.WriteLine("Renaming Complete.");
        }
    }
}
