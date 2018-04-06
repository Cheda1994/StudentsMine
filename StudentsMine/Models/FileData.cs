using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudentsMine.Models
{
    //TODO: Continuse FileData
    public class FileData
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        [NotMapped]
        public string DataBase64 { get; set; }
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

        public static void DataBase64ToDataByteArr(IEnumerable<FileData> files) {
            foreach (var file in files)
            {
                file.Data = Convert.FromBase64String(FormatBase64(file.DataBase64));
            }
        }

        public static void BuindGuid(IEnumerable<FileData> files) {
            foreach (var file in files)
            {
                file.Guid = Guid.NewGuid();
            }
        }

    }

    public class AttachmentView
    {
        public AttachmentView()
        {

        }
        public AttachmentView(FileData fileData)
        {
            this.Guid = fileData.Guid;
            this.Name = fileData.Name;
            this.Format = fileData.Format;
        }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public int? Course_Id { get; set; }
        public long? Size { get; set; }

    }
}
 