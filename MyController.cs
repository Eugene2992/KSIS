using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApplication1.Controllers
{
    [Controller]
    [Route("")]
    public class MyController : Controller
    {
        private string root = @"D:/Files";

        [HttpGet()]
        [HttpGet("{*filename}")]
        public ActionResult GetFile(string filename)
        {
            string fullpath = root + @"/" + filename;
            if (isFile(filename))
            {
                if (!System.IO.File.Exists(fullpath))
                    return NotFound();
                try
                {
                    FileStream fs = new FileStream(fullpath, FileMode.Open);
                    return File(fs, "application/unknown", filename);
                }
                catch { return BadRequest(); }
            }
            else
            {
                try
                {
                    IReadOnlyCollection<string> FilesCollection = Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(fullpath);
                    IReadOnlyCollection<string> DirectoryCollection = Microsoft.VisualBasic.FileIO.FileSystem.GetDirectories(fullpath);
                    List<string> allFiles = new List<string>(DirectoryCollection);
                    allFiles.AddRange(FilesCollection);
                    return Ok(allFiles);
                }
                catch { return BadRequest(); }

            }
        }

        [HttpHead("{*filename}")]
        public ActionResult GetFileInfo(string filename)
        {
            string fullpath = root + @"/" + filename;
            if (!System.IO.File.Exists(fullpath))
                return NotFound();
            try
            {
                FileInfo fileInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(fullpath);
                Response.Headers.Append("Full name",fileInfo.FullName);
                Response.Headers.Append("Length", fileInfo.Length.ToString());
                return Ok();
            }
            catch { return NotFound(); }
        }

        [HttpPut("{*filename}")]
        public ActionResult Put(IFormFileCollection inputFile, string filename)
        {
            string fullpath = root + @"/" + filename;
            try
            {
                using (var fileStream = new FileStream(root + @"/" + filename, FileMode.Create))
                {
                    inputFile[0].CopyTo(fileStream);
                }
                return Ok("Has been written");
            }
            catch { return NotFound(); }
        }

        [HttpDelete("{*filename}")]
        public ActionResult DeleteFile(string filename)
        {
            string fullpath = root + @"/" + filename;
            if (!System.IO.File.Exists(fullpath))
                return NotFound();
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fullpath);
                return Ok("Has been deleted");
            }
            catch { return NotFound(); }
        }

        [HttpPatch("{*filename}")]
        public ActionResult CopyFile(string filename)
        {
            string[] FilePathAndDirPath = filename.Split("/CopyTo");
            string FilePath = root + @"/" + FilePathAndDirPath[0];
            string DirPath = root + @"" + FilePathAndDirPath[1];
            FilePath = FilePath.Replace("/",@"\");
            DirPath = DirPath.Replace("/", @"\");
            FilePathAndDirPath[0] = FilePath;
            FilePathAndDirPath[1] = DirPath;
            //return Ok(FilePathAndDirPath);
            if (isFile(FilePath))
            {
                if (!System.IO.File.Exists(FilePath))
                    return NotFound();
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(FilePathAndDirPath[0], FilePathAndDirPath[1]);
                    return Ok("Has been copied");
                }
                catch { return Ok("Wrong file path or destination path"); }
            }
            else
            {
                return Ok("No existing file");
            }
        }
        private bool isFile(string str)
        {
            try
            {
                if (str == null)
                    return false;
                int index = str.LastIndexOf(@"\") + 1;
                string substr = str.Substring(index, str.Length - index);
                if ((substr.Contains(".")) && (substr.IndexOf(".") == substr.LastIndexOf(".")))
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

    }
}