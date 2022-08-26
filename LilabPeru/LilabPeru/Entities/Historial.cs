using System;
using System.Collections.Generic;

namespace LilabPeru.Entities
{
    public partial class Historial
    {
        public int IdHistorial { get; set; }
        public int IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public int Stock { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
    }
}
