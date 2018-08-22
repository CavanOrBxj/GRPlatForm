using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace HttpModel
{
    public class CommonFunc
    {
        public void SaveXmlWithUTF8NotBOM(XmlDocument xml, string savePath)
        {
            using (StreamWriter sw = new StreamWriter(savePath, false, new UTF8Encoding(false)))
            {
                xml.Save(sw);
                sw.WriteLine();
                sw.Close();
            }
        }
    }
}
