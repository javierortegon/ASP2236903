using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ASP2236903.Models;

namespace ASP2236903.Controllers
{
    public class ProductoImagenController : Controller
    {
        // GET: ProductoImagen
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargarImagen()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarImagen(int producto, HttpPostedFileBase imagen)
        {
            try {
                //string para guardar la ruta
                string filePath = string.Empty;
                string nameFile = "";

                //condicion para saber si el archivo llego
                if (imagen != null)
                {
                    //ruta de la carpeta que guardara el archivo
                    string path = Server.MapPath("~/Uploads/Imagenes/");

                    //condicion para saber si la carpeta uploads existe
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    nameFile = Path.GetFileName(imagen.FileName);

                    //obtener el nombre del archivo
                    filePath = path + Path.GetFileName(imagen.FileName);

                    //obtener la extension del archivo
                    string extension = Path.GetExtension(imagen.FileName);

                    //guardar el archivo
                    imagen.SaveAs(filePath);
                }

                using (var db = new inventario2021Entities())
                {
                    var imagenProducto = new producto_imagen();
                    imagenProducto.id_producto = producto;
                    imagenProducto.imagen = "/Uploads/Imagenes/" + nameFile;
                    db.producto_imagen.Add(imagenProducto);
                    db.SaveChanges();

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