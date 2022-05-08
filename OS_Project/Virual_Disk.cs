using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    public class Virual_Disk
    {
        public static string FileName = "F:\\OS_Project\\fat.txt";
        
        public static void intialize()
        {
            FileInfo Virtual_disk_txt = new FileInfo(FileName);
            directory root = new directory(null,"root", '1', 5,0);
            Fat_Table.set_next(5, -1);
            Program.current = root;
            Program.curpath = new string(Program.current.filename)+"\\";
            if (File.Exists(FileName))
            {
                Fat_Table.fat = Fat_Table.get();
                root.read_direcotry();
            }
            else
            {
                mk_file();
                Fat_Table.initialize_fat();
                root.write_directory();
                Fat_Table.write();
            }
        }
        public static void mk_file()
        {
            StreamWriter sw = new StreamWriter(FileName);
            for (int i = 0; i < 1024; i++)//super->1
                sw.Write('*');
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 1024; j++)//fat->4
                {
                    sw.Write('0');
                }
            }
            for (int i = 0; i < 1019; i++)
            {
                for (int j = 0; j < 1024; j++)
                    sw.Write('#');
            }
            sw.Close();
        }
        public static void write_block(byte[] data, int index)
        {   
            FileStream Virtual_disk_text = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_text.Seek(1024 * index, SeekOrigin.Begin);
            Virtual_disk_text.Write(data, 0, data.Length);
            Virtual_disk_text.Close();
        }

        public static byte[] get_block(int index)
        { 
            FileStream Virtual_disk_text = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_text.Seek(1024 * index, SeekOrigin.Begin);
            Byte[] bt = new Byte[1024];
            Virtual_disk_text.Read(bt, 0, bt.Length);
            Virtual_disk_text.Close();
            return bt;
        }
    }
}
