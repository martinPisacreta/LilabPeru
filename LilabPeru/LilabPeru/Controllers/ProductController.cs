using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LilabPeru.Entities;
using LilabPeru.Entities.Custom;

namespace LilabPeru.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private readonly ProyectoContext _context;


        public ProductController(ProyectoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Producto>> Index()
        {
            var productos = _context.Producto
                           .Select(p => new Producto
                           {
                               IdProducto = p.IdProducto,
                               Nombre = p.Nombre,
                               Precio = p.Precio,
                               Stock = p.Stock
                           }).ToList();
            return productos;
        }

        [HttpPost("agregar-producto")]
        public ActionResult<Producto> Create(ProductRequestAlta model)
        {
            Producto producto = null;
          
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int stock = 0;
                    producto = new Producto();
                    producto.Nombre = model.nombre;
                    producto.Precio = model.precio;
                    producto.Stock = stock;
                    _context.Producto.Add(producto);
                    _context.SaveChanges();

                    int id_producto = producto.IdProducto;

                    alta_historial(_context, id_producto, stock);
                

                    transaction.Commit();

                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest();
                }
            }
        }

        [HttpPost("editar-producto")]
        public ActionResult Edit(ProductRequestModificacion model)
        {
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Producto producto_db = _context.Producto.Where(p => p.IdProducto == model.id_producto).FirstOrDefault();
                    producto_db.Stock = model.stock;
                    _context.Producto.Update(producto_db);
                    _context.SaveChanges();

                  

                    alta_historial(_context, model.id_producto, model.stock);
                 
                    transaction.Commit();

                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest();
                }
            }
        }

        private void alta_historial(ProyectoContext _context,int id_producto,int stock)
        {
            Historial historial = null;
            try
            {
                historial = new Historial();
                historial.IdProducto = id_producto;
                historial.Fecha = DateTime.Now;
                historial.Stock = stock;
                _context.Historial.Add(historial);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
