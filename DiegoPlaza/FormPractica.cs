using DiegoPlaza.Context;
using DiegoPlaza.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoADO
{
    public partial class FormPractica : Form
    {
        ClientesContext context;
        String selected = "";
        String pedidoSelect = "";
        public FormPractica()
        {
            InitializeComponent();
             this.context = new ClientesContext();
        }

        private void FormPractica_Load(object sender, EventArgs e)
        {
            List<Cliente> clientes = this.context.getAllClientes();

            foreach(Cliente c in clientes)
            {
                this.cmbclientes.Items.Add(c.Empresa);
            }


        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            String name = this.cmbclientes.SelectedItem.ToString();
            this.populateCliente(this.context.getDataFromClient(name));
            this.populatePedidos((this.context.getPedidosFromCliente(name)));
            this.selected = name;
      
        }

        private void populateCliente(Cliente c)
        {
            this.txtcargo.Text = c.Cargo;
            this.txtciudad.Text = c.Ciudad;
            this.txtcontacto.Text = c.Contacto;
            this.txtempresa.Text = c.Empresa;
            this.txttelefono.Text = c.Telefono;
        }

        private void populatePedidos(List<Pedido> pedidos)
        {
            this.lstpedidos.Items.Clear();
            if (pedidos.Count == 0)
            {
                MessageBox.Show("El cliente seleccionado no tiene pedidos");
            }
            foreach(Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodigoPedido);
            }
            
            
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedCod = this.lstpedidos.SelectedItem.ToString();
            this.populatePedido(this.context.getInfoFromPedido(selectedCod));
            this.pedidoSelect = selectedCod;
        }

        private void populatePedido(Pedido p)
        {
            this.txtcodigopedido.Text = p.CodigoPedido;
            this.txtfechaentrega.Text = p.FechaEntrega;
            this.txtformaenvio.Text = p.FormaEnvio;
            this.txtimporte.Text = p.importe;
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            String id = this.context.getIdFromName(this.cmbclientes.SelectedItem.ToString());
            this.context.insertPedidio(this.txtfechaentrega.Text, this.txtformaenvio.Text, this.txtimporte.Text, id);
            this.populateCliente(this.context.getDataFromClient(this.selected));
            this.populatePedidos((this.context.getPedidosFromCliente(this.selected)));

        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            this.context.eliminarPedido(this.pedidoSelect);
            this.populateCliente(this.context.getDataFromClient(this.selected));
            this.populatePedidos((this.context.getPedidosFromCliente(this.selected)));
        }
    }
}
