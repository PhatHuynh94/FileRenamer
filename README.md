Renames all files in a directory to a common name. Can be used via command line or double click on .exe file

If using normal execution method:
  - Enter the directory path in the first prompt. Enter the common name at the second prompt

If using via command line method:
  - There are two commands -name and -path:
    - -name is the common name for the files. Needs to be used while excuting program
    - -path is the directory path. Does not need to be used while excuting program
    - currently does not have the option to sort file by last modifed date or creation date
  - If -path is not used the program will rename all files in the same directory as the program.

examples:
- FileRename -name "Hello World"
- FileRename -path C:\blah -name "Red Rocket"

Note: if using via command line it is recommemded to put quotation marks around directory path and common name.

The string comparison algorithm used for the program can be found here: 
- https://www.codeproject.com/Articles/11016/Numeric-String-Sort-in-C
- This allowed the program to replicate the sorting algorithm used by windows directory.
