using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ang7.PDFCode
{
    public static class ImageHelper
    {
        public static string CopyImageToAppData()
        {
            // اسم الصورة زي ما هو في Resources/Images
            string fileName = "angah.png";

            // المسار النهائي بعد النسخ
            string targetPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            // لو الصورة لسه متنسختش
            if (!File.Exists(targetPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync(fileName)
                                             .GetAwaiter()
                                             .GetResult();
                using var fileStream = File.Create(targetPath);
                stream.CopyTo(fileStream);
            }

            return targetPath;
        }
    }
}
