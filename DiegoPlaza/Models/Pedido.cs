using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiegoPlaza.Models
{
    class Pedido
    {
        public String CodigoPedido { get; set; }
        public String CodigoCliente { get; set; }
        public String FechaEntrega { get; set; }
        public String FormaEnvio { get; set; }
        public String importe { get; set; }

    }
}
