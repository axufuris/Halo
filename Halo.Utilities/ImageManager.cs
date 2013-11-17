using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Halo.Utilities
{
    public class ImageManager
    {
        /// <summary>
        /// Validates the file is image.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateFileIsImage(string contentType)
        {
            bool isImage = false;

            switch (contentType.ToLower())
            {
                case "image/gif":
                    isImage = true;
                    break;
                case "image/jpeg":
                    isImage = true;
                    break;
                case "image/pjpeg":
                    isImage = true;
                    break;
                case "image/png":
                    isImage = true;
                    break;
                case "image/tiff":
                    isImage = true;
                    break;
                case "image/x-icon":
                    isImage = true;
                    break;
                case "image/vnd.microsoft.icon":
                    isImage = true;
                    break;
                default:
                    isImage = false;
                    break;
            }

            return isImage;
        }

        /// <summary>
        /// Validates the size of the user image.
        /// </summary>
        /// <param name="maxFileSize">Size of the max file.</param>
        /// <param name="contentLength">Length of the content.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateUserImageSize(int maxFileSize, int contentLength)
        {
            bool validFileSize = false;
            int defaultMaxFileSize = 524288;

            if (maxFileSize > 0)
            {
                defaultMaxFileSize = maxFileSize;
            }

            if (defaultMaxFileSize > contentLength)
            {
                return validFileSize = false;
            }

            return validFileSize;
        }

        /// <summary>
        /// This will validate if the image is really an image from it's header type. 
        /// This method is harder to cheat.
        /// </summary>
        /// <param name="fileUpload">The file upload.</param>
        /// <returns>True/False</returns>
        public static bool ValidateImageHeader(FileUpload fileUpload)
        {
            return ValidateImageHeader(fileUpload.FileName, fileUpload.FileContent);
        }

        /// <summary>
        /// Validates the image header.
        /// </summary>
        /// <param name="postedFile">The posted file.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateImageHeader(HttpPostedFile postedFile)
        {
            return ValidateImageHeader(postedFile.FileName, postedFile.InputStream);
        }

        /// <summary>
        /// Validates the image header.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateImageHeader(string fileName, Stream stream)
        {
            Dictionary<string, byte[]> imageHeader = new Dictionary<string, byte[]>();
            byte[] header;

            imageHeader.Add("JPG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            imageHeader.Add("JPGXIF", new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 });
            imageHeader.Add("JPEG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            imageHeader.Add("PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 });
            imageHeader.Add("TIF", new byte[] { 0x49, 0x49, 0x2A, 0x00 });
            imageHeader.Add("TIFF", new byte[] { 0x49, 0x49, 0x2A, 0x00 });
            imageHeader.Add("GIF", new byte[] { 0x47, 0x49, 0x46, 0x38 });
            imageHeader.Add("BMP", new byte[] { 0x42, 0x4D });
            imageHeader.Add("ICO", new byte[] { 0x00, 0x00, 0x01, 0x00 });

            try
            {
                string fileExt = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToUpper();
                var tempList = imageHeader.Where(n => n.Key.Contains(fileExt)).ToList();

                foreach (var temp in tempList)
                {
                    header = new byte[temp.Value.Length];

                    stream.Read(header, 0, header.Length);

                    if (CompareArray(temp.Value, header))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        return true;
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="fileuploadImage">The fileupload image.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static System.Drawing.Image ResizeImage(FileUpload fileuploadImage, int maxHeight, int maxWidth)
        {
            System.Drawing.Image imageToBeResized = System.Drawing.Image.FromStream(fileuploadImage.PostedFile.InputStream);

            return ResizeImage(imageToBeResized, maxHeight, maxWidth);
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="postedFile">The posted file.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static System.Drawing.Image ResizeImage(HttpPostedFile postedFile, int maxHeight, int maxWidth)
        {
            System.Drawing.Image imageToBeResized = System.Drawing.Image.FromStream(postedFile.InputStream);

            return ResizeImage(imageToBeResized, maxHeight, maxWidth);
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="postedImage">The posted image.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static System.Drawing.Image ResizeImage(System.Drawing.Image postedImage, int maxHeight, int maxWidth)
        {
            var ratioX = (double)maxWidth / postedImage.Width;
            var ratioY = (double)maxHeight / postedImage.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(postedImage.Width * ratio);
            var newHeight = (int)(postedImage.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(postedImage, 0, 0, newWidth, newHeight);

            return newImage;
        }

        /// <summary>
        /// Validates the size.
        /// </summary>
        /// <param name="fileUpload">The file upload.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateHeightWidth(FileUpload fileUpload, int maxHeight, int maxWidth)
        {
            var image = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream);

            if (image.Height <= maxHeight && image.Width <= maxWidth)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates the size.
        /// </summary>
        /// <param name="postedFile">The posted file.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateHeightWidth(HttpPostedFile postedFile, int maxHeight, int maxWidth)
        {
            var image = System.Drawing.Image.FromStream(postedFile.InputStream);

            if (image.Height <= maxHeight && image.Width <= maxWidth)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates the size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool ValidateHeightWidth(System.Drawing.Image image, int maxHeight, int maxWidth)
        {
            if (image.Height <= maxHeight && image.Width <= maxWidth)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares the array.
        /// </summary>
        /// <param name="a1">The a1.</param>
        /// <param name="a2">The a2.</param>
        /// <returns>True/False</returns>
        /// <author>
        /// Andy Xufuris
        /// </author>
        private static bool CompareArray(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }

            return true;
        }
    }  /// End of Class
}
