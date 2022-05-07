using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Project
{
    public class file_entry:Directory_Entry
    {
        directory parent;
        string content;
        file_entry(directory p, string Name, char Attr, int firstClust,int size,string con) : base(Name, Attr, firstClust,size)
        {
            parent = p;
            content = con;  
        }


    }
}
