using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class IntelligentProblemSolvingController : Controller
    {
        // GET: IntelligentProblemSolving
        public ActionResult Index()
        {
            ViewData["KETQUAXULY"] = "";
            ViewData["GIATHUYET_H"] = "";
            ViewData["KETLUAN_G"] = "";
            return View();
        }
        public static void CreateFileData(string KnowledgePath, string pathFileData, string H, string G, string ResultsPath)
        {
            FileStream fs = new FileStream(pathFileData, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
            sw.WriteLine("restart: ");
            sw.WriteLine("libname := libname,\"" + KnowledgePath + "\": ");
            sw.WriteLine("with(problemsolving): ");
            sw.WriteLine("H:={" + H + "}: ");
            sw.WriteLine("G:={" + G + "}: ");
            sw.WriteLine("Init(): ");
            sw.WriteLine("Sol:=FindSolution(H, G): ");
            sw.WriteLine("duongdan:=\"" + ResultsPath + "\": ");
            sw.WriteLine("OutPutSolution(Sol, duongdan): ");
            sw.Close();
            fs.Close();
        }
        public static void CreateFileBat(string pathFileBat, string InputPath)
        {
            FileStream fs = new FileStream(pathFileBat, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
            //sw.WriteLine("cd " + pathFolderMaple);
            sw.WriteLine("cmaple.exe \"" + InputPath + "\"");
            sw.Close();
            fs.Close();
        }
        public void GoToConnectToMaple(string BatPath)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = true;
            proc.StartInfo.FileName = BatPath;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            proc.Start();
            proc.WaitForExit();
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
        }
        public string RandomNameFile()
        {
            string path = System.IO.Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }
        [HttpPost]
        public ActionResult Index(FormCollection form) 
        {
            string H = form["GIATHIET_H"].ToString();
            string G = form["KETLUAN_G"].ToString();

            string filename = RandomNameFile();

            string KnowledgePath = Server.MapPath("\\KB-IE\\").Replace(@"\", @"/");
            string ResultPath = Server.MapPath("\\Solved\\" + filename + "_ketqua.html").Replace(@"\", @"/");
            string DataPath = Server.MapPath("\\Data\\" + filename + "_debai.txt").Replace(@"\", @"/");
            string BatPath = Server.MapPath("\\BatFiles\\" + filename + "_bat.bat").Replace(@"\", @"/");

            CreateFileData(KnowledgePath, DataPath, H, G, ResultPath);
            CreateFileBat(BatPath, DataPath);
            GoToConnectToMaple(BatPath);
            
            if (System.IO.File.Exists(ResultPath))
            {
                string output = System.IO.File.ReadAllText(ResultPath);
                ViewData["KETQUAXULY"] = output;
            }
            else
            {
                ViewData["KETQUAXULY"] = "Kết quả không tồn tại, Vui lòng kiểm tra lại";
            }
            return View();
        }
    }
}