using System;
using System.Collections.Generic;

namespace LilabPeru.Entities
{
    public partial class Producto
    {
        public Producto()
        {
            Historial = new HashSet<Historial>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public virtual ICollection<Historial> Historial { get; set; }
    }
}
