
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Portal.Models
{
    public class MailRequestModel
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
