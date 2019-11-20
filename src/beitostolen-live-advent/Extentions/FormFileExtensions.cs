using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace beitostolen_live_api.Extentions
{
    public static class FormFileExtensions
    {
        public static string GetBase64HtmlString(this IFormFile file)
		{
            var stream = file.OpenReadStream();

            using (var reader = new BinaryReader(stream))
            {
                var array = reader.ReadBytes((int)stream.Length);
                return $"data:{file.ContentType};base64,{Convert.ToBase64String(array)}";
            }
		}
    }
}
