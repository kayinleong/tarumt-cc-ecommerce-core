using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class UserFileService
    {
        private readonly CoreContext _context;
        private readonly string _filePath;
        private readonly string _urlFilePath;

        public UserFileService(IWebHostEnvironment env, IConfiguration config, CoreContext context)
        {
            _context = context;
            
            if (env.IsDevelopment())
            {
                _urlFilePath = "/files/";
                _filePath = Path.Combine(Directory.GetCurrentDirectory(), @config["File:Path"]!);
                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }
            }
            else
            {
                _urlFilePath = "";
                _filePath = Path.Combine(@config["File:Path"]!);
                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }
            }
        }

        public async Task<UserFile> Create(IFormFile file, string folderName)
        {
            string filename = file.FileName;
            string[] filenameArray = filename.Split(".");
            string finalFilename = $"{Guid.NewGuid()}_{filenameArray[0]}.{filenameArray[1]}";

            await UploadAsync(file, finalFilename, folderName);

            UserFile userFile = new UserFile()
            {
                FileName = finalFilename,
                Path = _urlFilePath + folderName
            };

            await _context.UserFiles.AddAsync(userFile);
            await _context.SaveChangesAsync();

            userFile.Path = _urlFilePath + folderName + finalFilename;

            return userFile;
        }

        public async Task UploadAsync(IFormFile file, string filename, string folderName = "public/")
        {
            string targetedFilePath = Path.Combine(_filePath, folderName);
            if (!Directory.Exists(targetedFilePath))
            {
                Directory.CreateDirectory(targetedFilePath);
            }

            await using FileStream stream = new FileStream(Path.Combine(targetedFilePath, filename), FileMode.Create);
            file.CopyTo(stream);
        }
    }
}
