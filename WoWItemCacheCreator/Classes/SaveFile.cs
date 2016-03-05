using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWItemCacheCreator.Classes
{
    class SaveFile
    {
        /// <summary>
        /// write a logfile to the disk for help with testing    
        /// </summary>
        /// <param name="text"></param>
        public string writeToLogFile(string json, string id, string path, string ext)
        {
            // Write the string to a file.
            string filePath = path + "\\" + id + "." + ext;
            try
            {
                if (File.Exists(filePath)) 
                { 
                    using (System.IO.StreamWriter file = File.AppendText(filePath))
                    {
                        file.Write(json + Environment.NewLine);     //WriteLine(text + Environment.NewLine);
                    }
                }
                else
                {
                    //File.Create(filePath);
                    using (System.IO.StreamWriter file = File.CreateText(filePath))
                    {
                        file.Write(json + Environment.NewLine);          //.WriteLine(text + Environment.NewLine);
                    }
                }
                return "yes";
            }
            catch (Exception ex)
            {
                return "fpath " + filePath + " " + ex.Message;
            }
        }
    }
}
