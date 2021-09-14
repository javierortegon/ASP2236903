using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP2236903.Models;

namespace ASP2236903.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult uploadCSV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uploadCSV(HttpPostedFileBase fileForm)
        {
            try {

                //string para guardar la ruta
                string filePath = string.Empty;

                //condicion para saber si el archivo llego
                if(fileForm != null)
                {
                    //ruta de la carpeta que guardara el archivo
                    string path = Server.MapPath("~/Uploads/");

                    //condicion para saber si la carpeta uploads existe
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //obtener el nombre del archivo
                    filePath = path + Path.GetFileName(fileForm.FileName);

                    //obtener la extension del archivo
                    string extension = Path.GetExtension(fileForm.FileName);

                    //guardar el archivo
                    fileForm.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach(string row in csvData.Split('\n')) 
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var newProveedor = new proveedor
                            {
                                nombre = row.Split(';')[0],
                                telefono = row.Split(';')[2],
                                direccion = row.Split(';')[1],
                                nombre_contacto = row.Split(';')[3]
                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.proveedor.Add(newProveedor);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                return View();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

    }
}