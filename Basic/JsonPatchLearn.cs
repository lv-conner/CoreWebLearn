using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace Basic
{
    public class JsonPatchLearn
    {
        public static void LoadJson()
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();
            var filepath = Path.Combine(Directory.GetCurrentDirectory() + "appsettings.json");
            var file = File.Open(filepath,FileMode.OpenOrCreate);
        }
    }
}
