using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CCIA.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace CCIA.Services
{
    public interface IFileIOService
    {
       
        bool CheckDeniedExtension(string extension);

        Task SaveCertificateFile(Applications applications, IFormFile file);

       MemoryStream DownloadCertificateFile(Applications applications, string link);
       
    }

    public class FileIOService : IFileIOService
    {       

         private readonly IConfiguration _configuration;

        public FileIOService(IConfiguration configuration)
        {           
            _configuration = configuration;
        }
               

        public bool CheckDeniedExtension(string ext)
        {
            var permittedExtensions =  _configuration.GetSection("AllowedFileExtensions").Get<string[]>();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return true;
            }
            return false;
        }

        public async Task SaveCertificateFile(Applications app, IFormFile file)
        {            
           var localFolder = $"{GetRoot()}/certyear{app.CertYear}/appId{app.Id}/cert_tags/";
           await SaveFile(localFolder, file);
        }

        private async Task SaveFile(string localFolder,IFormFile file)
        {
            System.IO.Directory.CreateDirectory(localFolder);
           
           var filePath = Path.Combine(localFolder, file.FileName);
           using (var stream = System.IO.File.Create(filePath))
           {
                await file.CopyToAsync(stream);
           }
        }

        public MemoryStream DownloadCertificateFile(Applications app, string link)
        {   
            var localFolder = $"{GetRoot()}/certyear{app.CertYear}/appId{app.Id}/cert_tags/";
            var filePath =  Path.Combine(localFolder, link);
            var net = new System.Net.WebClient();
            var data = net.DownloadData(filePath);
            var content = new System.IO.MemoryStream(data);
            return content;
            //var contentType = "APPLICATION/octet-stream";
            //return new Microsoft.AspNetCore.Mvc.FileStreamResult(content, contentType);
        }

        private string GetRoot()
        {
            return _configuration["FilePath"];
        }
       
    }
}