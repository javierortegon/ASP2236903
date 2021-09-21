using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP2236903.Models;
using Rotativa;
using System.IO;
using ASP2236903.Filtros;

namespace ASP2236903.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto.ToList());
            }
                
        }

        public static string NombreProveedor(int idProveedor)
        {
            using (var db = new inventario2021Entities())
            {
                return db.proveedor.Find(idProveedor).nombre;
            }
        }

        public ActionResult ListarProveedores()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(producto producto)
        {
            if (!ModelState.IsValid)
                return View();

            try {
                using (var db = new inventario2021Entities())
                {
                    db.producto.Add(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                var producto = db.producto.Find(id);
                var imagen = db.producto_imagen.Where(e => e.id_producto == producto.id).FirstOrDefault();
                ViewBag.imagen = imagen.imagen;
                return View(producto);
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventario2021Entities())
            {
                producto productoEdit = db.producto.Where(a => a.id == id).FirstOrDefault();
                return View(productoEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(producto productoEdit)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var oldProduct = db.producto.Find(productoEdit.id);
                    oldProduct.nombre = productoEdit.nombre;
                    oldProduct.cantidad = productoEdit.cantidad;
                    oldProduct.descripcion = productoEdit.descripcion;
                    oldProduct.percio_unitario = productoEdit.percio_unitario;
                    oldProduct.id_proveedor = productoEdit.id_proveedor;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto producto = db.producto.Find(id);
                    db.producto.Remove(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Reporte()
        {
            try
            {
                var db = new inventario2021Entities();
                var query = from tabProveedor in db.proveedor
                            join tabProducto in db.producto on tabProveedor.id equals tabProducto.id_proveedor
                            select new Reporte
                            {
                                nombreProveedor = tabProveedor.nombre,
                                telefonoProveedor = tabProveedor.telefono,
                                direccionProveedor = tabProveedor.direccion,
                                nombreProducto = tabProducto.nombre,
                                PrecioProducto = tabProducto.percio_unitario
                            };
                return View(query);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult PdfReporte()
        {
            return new ActionAsPdf("Index") { FileName = "reporte.pdf" };
        }

        [AuthorizeUser(idRol: 1)]
        public ActionResult cargarImagen()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult cargarImagen(int producto, HttpPostedFileBase fileForm)
        {
            //string para guardar la ruta
            string filePath = string.Empty;
            string name = "";

            //condicion para saber si el archivo llego
            if (fileForm != null)
            {
                //ruta de la carpeta que guardara el archivo
                string path = Server.MapPath("~/Uploads/Imagenes/");

                //condicion para saber si la carpeta uploads existe
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                name = Path.GetFileName(fileForm.FileName);
                //obtener el nombre del archivo
                filePath = path + Path.GetFileName(fileForm.FileName);

                //obtener la extension del archivo
                string extension = Path.GetExtension(fileForm.FileName);

                //guardar el archivo
                fileForm.SaveAs(filePath);

                
            }


            using (var db = new inventario2021Entities())
            {
                var imagenProducto = new producto_imagen();
                imagenProducto.id_producto = producto;
                imagenProducto.imagen = "/Uploads/Imagenes/" + name;
                db.producto_imagen.Add(imagenProducto);
                db.SaveChanges();
            }

                return View();
        }

    }
}