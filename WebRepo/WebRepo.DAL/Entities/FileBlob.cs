﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.DAL.Default;

namespace WebRepo.DAL.Entities
{
    public class FileBlob : DefaultEntity
    {
        public string FileName { get; set; }

        public byte[]? Data { get; set; }

        public string? PathAPI { get; set; }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        //// Optional properties for tracking who uploaded the file
        //public string UploadedBy { get; set; }
        //public string IPAddress { get; set; }
    }
}
