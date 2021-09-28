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

        Task SaveFIRDocumentFile(Applications applications, IFormFile file);

        Task SaveBlendFile(BlendRequests blend, IFormFile file);

        FileStream DownloadCertificateFile(Applications applications, string link);

        FileStream DownloadFIRDocumentFile(Applications applications, string link);

        FileStream DownloadBlendFile(BlendDocuments blendDoc, int certYear);

        Task SaveSeedDocument(Seeds sid, string docType, IFormFile file);

        FileStream DownloadSeedFile(SeedDocuments doc, int certYear);
        FileStream DownloadTagFile(TagDocuments doc, int certYear);

        Task SaveTagDocument(int tagId, int certYear, IFormFile file);
       
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

        public async Task SaveSeedDocument(Seeds sid, string docType, IFormFile file)
        {
           var localFolder = $"{GetRoot()}/certyear{sid.CertYear}/sid{sid.Id}/{docType}/";
           await SaveFile(localFolder, file);
        }

        public async Task SaveBlendFile(BlendRequests blend, IFormFile file)
        {
           var localFolder = $"{GetRoot()}/certyear{blend.CertYear}/blend{blend.Id}/";
           await SaveFile(localFolder, file);
        }
        

        public async Task SaveTagDocument(int tagId, int certYear, IFormFile file)
        {
            var localFolder = $"{GetRoot()}/certyear{certYear}/tagid{tagId}/";
            await SaveFile(localFolder, file);
        }

        public async Task SaveCertificateFile(Applications app, IFormFile file)
        {            
           var localFolder = $"{GetRoot()}/certyear{app.CertYear}/appId{app.Id}/cert_tags/";
           await SaveFile(localFolder, file);
        }

        public async Task SaveFIRDocumentFile(Applications app, IFormFile file)
        {            
           var localFolder = $"{GetRoot()}/certyear{app.CertYear}/appId{app.Id}/fir/";
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

        public FileStream DownloadCertificateFile(Applications app, string link)
        {   
            var localFolder = $"{GetRoot()}/certyear{app.CertYear}/appId{app.Id}/cert_tags/";
            var filePath =  Path.Combine(localFolder, link);
            return GetFile(filePath);
        }

        public FileStream DownloadFIRDocumentFile(Applications app, string link)
        {   
            var localFolder = $"{GetRoot()}/certyear{app.CertYear}/appId{app.Id}/fir/";
            var filePath =  Path.Combine(localFolder, link);
            return GetFile(filePath);
        }

        public FileStream DownloadSeedFile(SeedDocuments doc, int certYear)
        {
            var localFolder =  $"{GetRoot()}/certyear{certYear}/sid{doc.SeedsId}/{doc.DocumentType.Folder}/";
            var filePath = Path.Combine(localFolder, doc.Link);   
            return GetFile(filePath);
        }

        public FileStream DownloadBlendFile(BlendDocuments doc, int certYear)
        {
            var localFolder =  $"{GetRoot()}/certyear{certYear}/blend{doc.BlendId}/";
            var filePath = Path.Combine(localFolder, doc.Link);   
            return GetFile(filePath);
        }

        public FileStream DownloadTagFile(TagDocuments doc, int certYear)
        {
            var localFolder =  $"{GetRoot()}/certyear{certYear}/tagid{doc.TagId}/";
            var filePath = Path.Combine(localFolder, doc.Link);   
            return GetFile(filePath);
        }

        private FileStream GetFile(string filePath)
        {
            var content = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
            return content;
        }

        private string GetRoot()
        {
            return _configuration["FilePath"];
        }
       
    }
}