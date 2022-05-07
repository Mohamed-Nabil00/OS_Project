using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    public class Commands
    {
        public static object Virual_Disk;

        public static string[] input(string str)
        {
            bool f = false; string y = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ' ') { y += str[i]; f = false; }
                else { if (f == false) { y += str[i]; f = true; } }
            }
            string[] s = y.Split(' ');
            return s;
        }
        public static void md(string name)
        {
            int position = Program.current.search_directory(name);
            Program.current.read_direcotry();
            if (position == -1)
            {
                Directory_Entry d=new Directory_Entry(name,'1',0,0);
                Program.current.Directory_Table.Add(d);
                Program.current.write_directory();
                Fat_Table.set_next(d.firstCluster, -1);
                Fat_Table.write();
                if (Program.current.parent != null)
                {
                    Program.current.parent.update_content(Program.current.get_dirctory());
                }
            }
            else
            {
                Console.WriteLine("directory already exists");
            }
        }
        public static void rd(string name)
        {
            int index = Program.current.search_directory(name);
            if (index != -1)
            {
                if (Program.current.Directory_Table[index].fileAttribute == '1')
                {
                    int fc = Program.current.Directory_Table[index].firstCluster;
                    directory d = new directory(Program.current, name, '1', fc, 0);
                    d.write_directory();
                    d.delete_directory();
                }
                else
                {
                    Console.WriteLine("cannot delete file");
                }
            }
            else
            {
                Console.WriteLine("Not Exist");
            }
        }
        public static void cd(string name)
        {

            int index = Program.current.search_directory(name);
            if (index != -1)
            {
                if (Program.current.Directory_Table[index].fileAttribute == '1')
                {
                    int fc = Program.current.Directory_Table[index].firstCluster;
                    directory d = new directory(Program.current, name, '1', fc, 0);
                    d.write_directory();
                    Program.current = d;
                    Program.curpath +=  (new string(Program.current.filename)+ @"\");
                }
                else
                {
                    Console.WriteLine("Can't change current directory ");
                }
            }
            else
            {
                if (Program.current.parent != null)
                {
                    string n = new string(Program.current.parent.filename);
                    if (n == name)
                    {
                        directory d = Program.current.parent;
                        d.read_direcotry();
                        Program.current = d;
                        Program.curpath = Program.curpath.Remove(Program.curpath.Length - 1);
                        while (Program.curpath[Program.curpath.Length - 1] != '\\')
                        {
                            Program.curpath = Program.curpath.Remove(Program.curpath.Length - 1);
                        }
                        
                    }
                }
                else
                    Console.WriteLine("Not Exist");
            }
        }
        public static void clear()
        {
            Console.Clear();
        }
        public static string getpath()
        {
            return Program.curpath;
        }
        public static void dir()
        {
            Program.current.read_direcotry();
            int numf = 0, numd = 0, size_file = 0;
            int n=Program.current.Directory_Table.Count();
            for (int i = 0; i < n; i++)
            {

                if (Program.current.Directory_Table[i].fileAttribute == 0)
                {
                    Console.WriteLine(Program.current.Directory_Table[i].fileSize + "      " + Program.current.Directory_Table[i].filename);
                    numf++;
                    size_file += Program.current.Directory_Table[i].fileSize;
                }
                else
                {
                    string na=new string(Program.current.Directory_Table[i].filename);
                    Console.WriteLine("<DIR>        " + na+"   "+ Program.current.Directory_Table[i].firstCluster);
                    

                    numd++;
                }
            }
            Console.WriteLine(numf + " File(s)         " + size_file + " Bytes");
            Console.WriteLine(numd + " Dir(s)         " + (Fat_Table.get_free_space()-size_file) + " Bytes");
        }
        public static void help(string arg = "")
        {
            if (arg == "")
            {
                Console.WriteLine("\ncls         Clears the screen.\n");
                Console.WriteLine("quit        Quits the CMD.EXE program (command interpreter) or the current batch script.\n");
                Console.WriteLine("help        Provides help information for Windows commands.\n");
            }
            else
            {
                if (arg == "cls")
                {
                    Console.WriteLine("\ncls         Clears the screen.\n");
                }
                else if (arg == "quit")
                {
                    Console.WriteLine("\nquit        Quits the CMD.EXE program (command interpreter) or the current batch script.\n");
                }
                else if (arg == "help")
                {
                    Console.WriteLine("\nhelp        Provides help information for Windows commands.\nHELP [command]\n" +
                        " command - displays help information on that command.\n");
                }
              
                else
                {
                    Console.WriteLine($"\nThis command is not supported by the help utility.  Try { arg} /? .\n");
                }
            }
        }
        public static void exit()
        {

            Environment.Exit(0);
        }
        public static void command(string[] str, string orgi)
        {
            if (str[0] == "quit")
            {
                exit();
            }
            else if (str[0] == "help")
            {
                if (str.Length > 1)
                    help(str[1]);
                else
                    help();
            }
            else if (str[0] == "cls")
            {
                clear();
            }
            else if (str[0] == "cd") 
            {
                if (str.Length == 1)
                {

                    Console.WriteLine(Program.curpath+":");
                }
                else if(str.Length ==2)
                {
                    cd(str[1]);
                }
                else
                {
                    Console.WriteLine("error");
                }
                //-------------------
            }
            else if (str[0] == "md")
            {
                if (str.Length == 2)
                    md(str[1]);
                else
                    Console.WriteLine("error");
            }
            else if (str[0] == "dir") {
                dir();
            }
            else if(str[0] == "rd")
            {
                if (str.Length > 1)
                    rd(str[1]);
                else
                    Console.WriteLine("error");
            }
            else if (str[0] == "")
            {
                return;
            }
            else
            {
                Console.WriteLine($"\'{orgi}\' is not recognized as an internal or external command, \noperable program or batch file. \n");

            }
        }
    }
}
