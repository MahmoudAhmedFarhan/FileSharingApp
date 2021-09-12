using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Data
{
    public class Uploads
    {
        public Uploads()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }
        public string Id { set; get; }

        public string OriginalFileName { get; set; }
        public string FileName { set; get; }
        public string ContentType { set; get; }
        public decimal Size { set; get; }
        public string UserId { set; get; }

        public DateTime UploadDate { get; set; }

        public ApplicationUser User { get; set; }

        public long DownloadCount { get; set; }
    }
}
