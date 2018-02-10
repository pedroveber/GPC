using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for WSArquivo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSArquivo : System.Web.Services.WebService
    {
               
        [WebMethod]
        public bool SaveDocument(Byte[] docbinaryarray,string key,int tipo)
        {
            try
            {
                if (key == "741852963")
                {
                    string strdocPath;
                    //TODO: Colocar parâmetro para a Pasta
                    strdocPath = @"C:\ArquivosProxy\Pendente\" + DateTime.Now.ToString("yyyyMMddhhmmss") + (tipo ==1 ? "GVG":(tipo==2? "SIEGE":"DEF")) + ".txt";
                    FileStream objfilestream = new FileStream(strdocPath, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(docbinaryarray, 0, docbinaryarray.Length);
                    objfilestream.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            
        }


    }
}
