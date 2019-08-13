using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT7View
{
    public class AT7FileNode
    {
        private uint fileSize;
        private uint fileOffset;
        private string fileName;
        private string hexSize;
        private string hexOffset;
        private string fileExtension;
        
        

        public AT7FileNode(uint s, uint o, string n, string e)
        {
            fileSize = s;
            fileOffset = o;
            fileName = n;
            hexSize = s.ToString("X");
            hexOffset = o.ToString("X");
            fileExtension = e;
            
        }

        public uint getSize()
        {
            return fileSize;
        }

        public uint getOffset()
        {
            return fileOffset;
        }

        public string getName()
        {
            return fileName;
        }

        public string getExt()
        {
            return fileExtension;
        }

        

        public override string ToString()
        {
            return "Name: " + fileName + "\nSize: " + fileSize + $"/0x{hexSize} bytes" + "\nOffset: " + $"0x{hexOffset}" + "\nType: " + fileExtension;
            
        }
    }
}
