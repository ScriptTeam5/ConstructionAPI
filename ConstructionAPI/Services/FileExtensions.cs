namespace ConstructionAPI.Services
{
    public static class FileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // Extension control
            string fileExtension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(fileExtension) ||
                !fileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".webp", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (file.Length > 5 * 1024 * 1024) // 5 MB
            {
                return false;
            }
            // MIME control
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
        //public static bool IsImage(this IFormFile file)
        //{
        //    return file.ContentType == "image/jpg" ||
        //        file.ContentType == "image/jpeg" ||
        //        file.ContentType == "image/png" ||
        //        file.ContentType == "image/webp" ||
        //        file.ContentType == "image/gif";
        //}

        public static bool IsCV(this IFormFile CVfile)
        {
            return CVfile.ContentType == "application/pdf" ||
             CVfile.ContentType == "application/doc" ||
             CVfile.ContentType == "application/docx" ||
             CVfile.ContentType == "application/pptx";

        }
        public async static Task<string> SaveAsync(this IFormFile file, string root)
        {
            string path = Path.Combine(@"Uploads", root);

            string fileName = Path.Combine(Guid.NewGuid().ToString() + Path.GetFileName(file.FileName));

            string resultPath = Path.Combine(path, fileName);

            using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
