using DiegoPlaza.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiegoPlaza.Context



/*

PROCEDURE INSERT PEDIDO:

CREATE OR ALTER PROCEDURE NUEVOPEDIDO(@FechaEntrega DATETIME, @FORMAENVIO NVARCHAR(100), @IMPORTE NVARCHAR(100), @IDEMPRESA NVARCHAR(100)) AS
INSERT INTO pedidos values(FORMAT(GETDATE(), 'MMMM-dd-yyyy', 'es-es'), @IDEMPRESA, @FechaEntrega, @FORMAENVIO, @IMPORTE)
--FORMAT(GETDATE(), 'MMMM-dd-yyyy', 'es-es') 
GO

PREOCEDURE DELETE PEDIDO:
CREATE OR ALTER PROCEDURE DELETEPEDIDO(@CodigoPedido NVARCHAR(100)) AS
DELETE FROM pedidos WHERE pedidos.CodigoPedido = @CodigoPedido;
GO




 */
{
    class ClientesContext
    {
        public SqlConnection conn;
        public SqlCommand command;
        public SqlDataReader reader;

        public ClientesContext()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("Config.json");
            IConfigurationRoot config = builder.Build();

            this.conn = new SqlConnection();
            this.conn.ConnectionString = config["urlBD"];
            this.command = new SqlCommand();
            this.command.Connection = this.conn;
        }

        public List<Cliente> getAllClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            String sql = "SELECT * FROM clientes";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.Text;
            this.conn.Open();
            this.reader = this.command.ExecuteReader();

            while (reader.Read())
            {
                Cliente c = new Cliente();
                c.CodigoCliente = reader["CodigoCliente"].ToString();
                c.Empresa = reader["Empresa"].ToString();
                c.Contacto = reader["Contacto"].ToString();
                c.Cargo = reader["Cargo"].ToString();
                c.Ciudad = reader["Ciudad"].ToString();
                c.Telefono = reader["Telefono"].ToString();
                clientes.Add(c);
            }

            this.conn.Close();
            reader.Close();

            return clientes;
        }
        public Cliente getDataFromClient(String clientName)
        {
            Cliente c = new Cliente();
            String sql = "SELECT * FROM clientes WHERE clientes.EMPRESA =  @EMPRESA";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.Text;
            this.command.Parameters.AddWithValue("@EMPRESA", clientName);
            this.conn.Open();
            this.reader = command.ExecuteReader();

            while (reader.Read()) {
                c.CodigoCliente = reader["CodigoCliente"].ToString();
                c.Empresa = reader["Empresa"].ToString();
                c.Contacto = reader["Contacto"].ToString();
                c.Cargo = reader["Cargo"].ToString();
                c.Ciudad = reader["Ciudad"].ToString();
                c.Telefono = reader["Telefono"].ToString();
            }

            this.conn.Close();
            this.reader.Close();
            this.command.Parameters.Clear();

            return c;
        }

        public List<Pedido> getPedidosFromCliente(String codCliente) {
            List<Pedido> pedidos = new List<Pedido>();

            String sql = "SELECT * FROM pedidos left join clientes on pedidos.CodigoCliente = clientes.CodigoCliente WHERE clientes.Empresa =  @CLIENTE";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.Text;
            this.command.Parameters.AddWithValue("@CLIENTE", codCliente);
            this.conn.Open();
            this.reader = command.ExecuteReader();

            while (reader.Read())
            {
                Pedido p = new Pedido();
                p.CodigoPedido = reader["CodigoPedido"].ToString();
                p.CodigoCliente = reader["CodigoCliente"].ToString();
                p.FechaEntrega = reader["FechaEntrega"].ToString();
                p.FormaEnvio = reader["FormaEnvio"].ToString();
                p.importe = reader["Importe"].ToString();
                pedidos.Add(p);

            }

            this.conn.Close();
            this.reader.Close();
            this.command.Parameters.Clear();

            return pedidos;
        }

        public Pedido getInfoFromPedido(String codCliente)
        {
            Pedido p = new Pedido();

            String sql = "SELECT * FROM pedidos WHERE pedidos.CodigoPedido = @CLIENTE";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.Text;
            this.command.Parameters.AddWithValue("@CLIENTE", codCliente);
            this.conn.Open();
            this.reader = command.ExecuteReader();

            while (reader.Read())
            {
                p.CodigoPedido = reader["CodigoPedido"].ToString();
                p.CodigoCliente = reader["CodigoCliente"].ToString();
                p.FechaEntrega = reader["FechaEntrega"].ToString();
                p.FormaEnvio = reader["FormaEnvio"].ToString();
                p.importe = reader["Importe"].ToString();
            }

            this.conn.Close();
            this.reader.Close();
            this.command.Parameters.Clear();

            return p;



        }

        public int insertPedidio(String fehaEntrega, String FormaEnvio, String Importe, String idEmpresa)
        {
            int aff = 0;
            String sql = "NUEVOPEDIDO";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.StoredProcedure;
            this.command.Parameters.AddWithValue("@IDEMPRESA", idEmpresa);
            this.command.Parameters.AddWithValue("@IMPORTE", Importe);
            this.command.Parameters.AddWithValue("@FORMAENVIO", FormaEnvio);
            this.command.Parameters.AddWithValue("@FechaEntrega", DateTime.Parse(fehaEntrega));
            this.conn.Open();
            aff = this.command.ExecuteNonQuery();
            this.conn.Close();
            this.command.Parameters.Clear();

            Debug.WriteLine(idEmpresa);
            return aff;

            
        }

        public String getIdFromName(String name)
        {
            String n = "";
            String sql = "SELECT CodigoCliente FROM clientes WHERE Empresa = @CLIENTE";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.Text;
            this.command.Parameters.AddWithValue("@CLIENTE", name);
            this.conn.Open();
            this.reader = command.ExecuteReader();

            while (reader.Read())
            {
                n = reader["CodigoCliente"].ToString();
            }

            this.conn.Close();
            this.reader.Close();
            this.command.Parameters.Clear();

            return n;
        } 

        public int eliminarPedido(String pedido)
        {
            int aff = 0;
            String sql = "DELETEPEDIDO";
            this.command.CommandText = sql;
            this.command.CommandType = System.Data.CommandType.StoredProcedure;
            this.command.Parameters.AddWithValue("@CodigoPedido", pedido);
            this.conn.Open();
            aff = this.command.ExecuteNonQuery();
            this.conn.Close();
            this.command.Parameters.Clear();

            Debug.WriteLine(pedido);
            return aff;
        }
        
    }
}
