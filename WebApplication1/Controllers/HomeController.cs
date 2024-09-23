using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        string dir = @"C:\Program Files\Maple 2022\bin.X86_64_WINDOWS";





        public ActionResult Index()
        {
            string debai = " a:=3: b:=4: c:=a+b: fprintf(h,\"tong = %a \", c): ";
            ViewData["KETQUA"] = Run(debai);

            return View();
        }
        public string RandomNameFile()
        {
            string path = System.IO.Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }
        public string Run(string input)
        {
            try
            {
                string filename = RandomNameFile();
                string fresult = Server.MapPath("\\Data\\" + filename + "_ketqua.txt").Replace(@"\", @"/");
                string finput = Server.MapPath("\\Data\\" + filename + "_debai.txt").Replace(@"\", @"/");
                string fbat = Server.MapPath("\\Data\\" + filename + "_bat.bat").Replace(@"\", @"/");               
                string fileresult = "h := fopen(\"" + fresult + "\", WRITE, TEXT): ";


                GoToConnectToMaple(input, fresult, finput, fbat, fileresult);
                
                string output = System.IO.File.ReadAllText(fresult);
                return output;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void CreateFileData(string pathFileData, string dauvao, string fileresult)
        {
            FileStream fs = new FileStream(pathFileData, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);

            sw.WriteLine("restart:");
            sw.WriteLine(fileresult);
            sw.WriteLine(dauvao);
            sw.WriteLine("fclose(h):");
            sw.Close();
            fs.Close();
        }
        public static void CreateFileBat(string pathFileBat, string pathFolderMaple, string pathFileData)
        {
            FileStream fs = new FileStream(pathFileBat, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
            //sw.WriteLine("cd " + pathFolderMaple);
            sw.WriteLine("cmaple.exe \"" + pathFileData + " \"");
            sw.Close();
            fs.Close();
        }
        public void GoToConnectToMaple(string input, string fresult, string finput, string fbat, string fileresult)
        {
            string pathFolderMaple = dir;
            CreateFileData(finput, input, fileresult);
            /// System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
            CreateFileBat(fbat, pathFolderMaple, finput);
            // System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = true;
            proc.StartInfo.FileName = fbat;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            proc.Start();
            proc.WaitForExit();
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}