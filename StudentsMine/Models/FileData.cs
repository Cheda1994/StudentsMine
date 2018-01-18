using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsMine.Models
{
    //TODO: Continuse FileData
    public class FileData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Format { get; set; }

        public static string FormatBase64(string base64) {
            string[] formatedString = base64.Split(',');
            if (formatedString.Length == 2)
            {
                return formatedString[1];
            }
            else if (formatedString.Length > 2)
            {
                return "Exception";
            }
            return formatedString[0];
        }

    }
}
 