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
            //if only uses -name and
            if (args.Length == 2 && args[0].Equals("-name"))
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
                if (File.Exists(newName))
                {
                    newName = rename(fileEntries[i], commonName, i + counter);
                    counter++;
                }
                else
                {
                    counter = 0;

                    //renaming the file
                    File.Move(fileEntries[i], newName);
                }
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


            if (fileNum >= 0 && fileNum < 10)
                numExt = "000" + fileNum.ToString();
            else if (fileNum >= 10 && fileNum < 100)
                numExt = "00" + fileNum.ToString();
            else if (fileNum >= 100 && fileNum < 1000)
                numExt = "0" + fileNum.ToString();
            else
                numExt = fileNum.ToString();

            //renames the file
            newName = dir + '\\' + commonName + "-" + numExt + '.' + ext;
            return newName;
        }

        static void Main(string[] args)
        {
            int argNum = argCheck(args);
           
            if (argNum ==  1)
            {
                string currentDir = Directory.GetCurrentDirectory();
                processDir(currentDir, args[1]);
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
