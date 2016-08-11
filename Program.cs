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

        static void processDir(ref List<string> fileNames, string dirPath)
        {
            //gets all file names in the current directory and puts it into a list
            fileNames = Directory.GetFiles(dirPath).ToList();

            //looks for the program and if it is the folder delete it from the list
            string programName = dirPath + "\\FileRenamer.exe";
            if(File.Exists(programName))
                fileNames.Remove(programName);

            //count the files in at the start of the process
            Console.WriteLine("There are {0} file(s) in the folder", fileNames.Count);
        }

        static void rename(List<string> fileNames, string path, string commonName)
        {
            string name, ext;
            int count = 1, temp;

            for(int i = 0; i < fileNames.Count; i++)
            {
                ext = Path.GetExtension(fileNames[i]);

                //creates the files new name.
                name = commonName + count.ToString("D6");

                //test to see if the file already has that name if it does then skips.
                if (Path.GetFileNameWithoutExtension(fileNames[i]).Equals(name))
                {
                    count++;
                    continue;
                }
                else
                {
                    temp = count;    
                    //check to see if this file name will conflict with anyother file name if it does gives it a new name.
                    for (int j = 0; j < fileNames.Count; j++)
                    {
                        if (name.Equals(Path.GetFileNameWithoutExtension(fileNames[j])))
                        {
                            count++;
                            name = commonName + count.ToString("D6");
                            j = 0;
                        }
                    }


                    //puts in full name path and move the file via copy and delete
                    name = path + '\\' + commonName + count.ToString("D6") + ext;
                    File.Copy(fileNames[i], name);
                    File.Delete(fileNames[i]);
                    fileNames[i] = name;
                    fileNames.Sort();

                    count = temp;
                    count++;

                }
            }
            Console.WriteLine("Rename Complete.");
        }

        static int Main(string[] args)
        {
            List<string> fileNames = new List<string>();
            string dirPath, commonName;
            int argNum = argCheck(args);


            if (argNum == 0)
            {
                Console.Write("Enter the directory Path: ");
                dirPath = Console.ReadLine();

                //If the user enters nothing program uses the directory that the exe is in.
                if (string.IsNullOrWhiteSpace(dirPath))
                    dirPath = Directory.GetCurrentDirectory();

                //checks to make sure that the directory exists.
                else if(!Directory.Exists(dirPath))
                {
                    Console.WriteLine("Directory does not exist.");
                    return 1;
                }

                Console.Write("Enter the common name: ");
                commonName = Console.ReadLine();

                processDir(ref fileNames, dirPath);
                rename(fileNames, dirPath, commonName);

                Console.WriteLine("Press Any key to exit.");
                Console.ReadKey();

            }
            else if (argNum ==  1)
            {
                //if the user does not use -path program uses the directory that the exe is in.
                dirPath = Directory.GetCurrentDirectory();
                processDir(ref fileNames, dirPath);
       
                rename(fileNames, dirPath, args[1]);

            }
            else if(argNum == 2)
            {
                processDir(ref fileNames, args[1]);
                rename(fileNames, args[1], args[3]);
            }
            else
            {
                //error exit
                Console.WriteLine("Error. Invalid Arguments or Directory does not exist");
                return 1;
            }
            return 0;
        }
    }
}
