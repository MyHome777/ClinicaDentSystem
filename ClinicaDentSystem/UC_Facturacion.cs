using DAO;
using MODELOS;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using UI;

namespace ClinicaDentSystem
{
    public partial class UC_Facturacion : UserControl
    {
        private readonly FacturacionDAO _facturacionDAO = new FacturacionDAO();
        private readonly CorreoFacturaService _correoFacturaService = new CorreoFacturaService();
        private readonly BindingList<FacturaDetalle> _detalles = new BindingList<FacturaDetalle>();
        private decimal _porcentajeDescuento;
        private int? _descuentoServicioID;
        private int? _descuentoProductoID;

        public UC_Facturacion()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            ConfigurarGridDetalles();
            ConfigurarEventos();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void UC_Facturacion_Load(object sender, EventArgs e)
        {
            CargarCombos();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object? sender, EventArgs e)
        {
            using Descuento descuento = new Descuento();
            descuento.ShowDialog();
            CargarDescuentos();
        }

        private void guna2Button4_Click(object? sender, EventArgs e)
        {
            using facEmitidas frm = new facEmitidas();
            frm.ShowDialog();
        }

        private void ConfigurarEventos()
        {
            guna2Button2.Click += btnAgregarDetalle_Click;
            guna2ImageButton2.Click += btnAgregarDetalle_Click;
            guna2Button3.Click += btnQuitarDetalle_Click;
            guna2ImageButton1.Click += btnQuitarDetalle_Click;
            guna2Button1.Click += btnEmitirFactura_Click;
            guna2ImageButton5.Click += btnEmitirFactura_Click;
            guna2Button4.Click += guna2Button4_Click;
            guna2ImageButton3.Click += guna2Button4_Click;
            guna2ImageButton4.Click += guna2Button5_Click;
            guna2ComboBox5.SelectedIndexChanged += cmbTipo_SelectedIndexChanged;
            guna2ComboBox6.SelectedIndexChanged += cmbItem_SelectedIndexChanged;
            guna2ComboBox4.SelectedIndexChanged += cmbDescuento_SelectedIndexChanged;
        }

        private void ConfigurarGridDetalles()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tipo",
                DataPropertyName = nameof(FacturaDetalle.Tipo),
                Width = 110
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Servicio / Producto",
                DataPropertyName = nameof(FacturaDetalle.Descripcion),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad",
                DataPropertyName = nameof(FacturaDetalle.Cantidad),
                Width = 90
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio",
                DataPropertyName = nameof(FacturaDetalle.PrecioAplicado),
                DefaultCellStyle = { Format = "C2" },
                Width = 110
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Subtotal",
                DataPropertyName = nameof(FacturaDetalle.Subtotal),
                DefaultCellStyle = { Format = "C2" },
                Width = 120
            });

            dataGridView1.DataSource = _detalles;
        }

        private void CargarCombos()
        {
            guna2DateTimePicker1.MinDate = DateTime.Today;
            guna2DateTimePicker1.Value = DateTime.Now;

            CargarCitas();
            CargarEstadosFactura();
            CargarUsuarios();
            CargarDescuentos();
            CargarTiposDetalle();
            CargarCantidades();
            ActualizarTotales();
        }

        private void CargarCitas()
        {
            DataTable citas = _facturacionDAO.ObtenerCitas(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            guna2ComboBox1.DataSource = citas;
            guna2ComboBox1.DisplayMember = "Descripcion";
            guna2ComboBox1.ValueMember = "CitaID";
            guna2ComboBox1.SelectedIndex = citas.Rows.Count > 0 ? 0 : -1;
        }

        private void CargarEstadosFactura()
        {
            DataTable estados = _facturacionDAO.ObtenerEstadosFactura(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            guna2ComboBox2.DataSource = estados;
            guna2ComboBox2.DisplayMember = "Estado";
            guna2ComboBox2.ValueMember = "EstadoId";

            if (estados.Rows.Count == 0)
            {
                MessageBox.Show("No hay estados de factura. Ejecuta el bloque nuevo de facturacion en Nuevos SP antes de probar esta pantalla.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            DataRow[] emitida = estados.Select("Estado = 'EMITIDA'");
            if (emitida.Length > 0)
                guna2ComboBox2.SelectedValue = Convert.ToInt32(emitida[0]["EstadoId"]);
        }

        private void CargarUsuarios()
        {
            DataTable usuarios = _facturacionDAO.ObtenerUsuarios(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            guna2ComboBox3.DataSource = usuarios;
            guna2ComboBox3.DisplayMember = "Usuario";
            guna2ComboBox3.ValueMember = "UsuarioId";
            guna2ComboBox3.Enabled = false;

            int usuarioActivoId = Program.UsuarioActivo?.UsuarioId ?? 0;
            if (usuarioActivoId > 0)
                guna2ComboBox3.SelectedValue = usuarioActivoId;
        }

        private void CargarDescuentos()
        {
            DataTable descuentos = _facturacionDAO.ObtenerDescuentos(out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            guna2ComboBox4.DataSource = descuentos;
            guna2ComboBox4.DisplayMember = "Descripcion";
            guna2ComboBox4.ValueMember = "DescuentoID";
            guna2ComboBox4.SelectedIndex = descuentos.Rows.Count > 0 ? 0 : -1;
            ActualizarPorcentajeDescuento();
        }

        private void CargarTiposDetalle()
        {
            guna2ComboBox5.Items.Clear();
            guna2ComboBox5.Items.Add("Producto");
            guna2ComboBox5.Items.Add("Servicio");
            guna2ComboBox5.SelectedIndex = 0;
        }

        private void CargarCantidades()
        {
            guna2ComboBox7.Items.Clear();
            for (int i = 1; i <= 20; i++)
                guna2ComboBox7.Items.Add(i);

            guna2ComboBox7.SelectedIndex = 0;
        }

        private void CargarItemsFacturables()
        {
            string tipo = guna2ComboBox5.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(tipo))
                return;

            DataTable items = _facturacionDAO.ObtenerItemsFacturables(tipo, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            guna2ComboBox6.DataSource = items;
            guna2ComboBox6.DisplayMember = "Descripcion";
            guna2ComboBox6.ValueMember = "ItemID";
            guna2ComboBox6.SelectedIndex = items.Rows.Count > 0 ? 0 : -1;
            ActualizarPrecioSeleccionado();
        }

        private void ActualizarPrecioSeleccionado()
        {
            guna2ComboBox8.Items.Clear();

            DataRowView? fila = guna2ComboBox6.SelectedItem as DataRowView;
            if (fila == null)
                return;

            decimal precio = Convert.ToDecimal(fila["Precio"], CultureInfo.InvariantCulture);
            guna2ComboBox8.Items.Add(FormatearMoneda(precio));
            guna2ComboBox8.SelectedIndex = 0;
        }

        private void cmbTipo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CargarItemsFacturables();
        }

        private void cmbItem_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarPrecioSeleccionado();
        }

        private void cmbDescuento_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarPorcentajeDescuento();
        }

        private void btnAgregarDetalle_Click(object? sender, EventArgs e)
        {
            AgregarDetalle();
        }

        private void btnQuitarDetalle_Click(object? sender, EventArgs e)
        {
            QuitarDetalle();
        }

        private void btnEmitirFactura_Click(object? sender, EventArgs e)
        {
            EmitirFactura();
        }

        private void AgregarDetalle()
        {
            DataRowView? fila = guna2ComboBox6.SelectedItem as DataRowView;
            if (fila == null)
            {
                MessageBox.Show("Debe seleccionar un servicio o producto.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string tipo = guna2ComboBox5.SelectedItem?.ToString() ?? string.Empty;
            int itemID = Convert.ToInt32(fila["ItemID"]);
            int cantidad = Convert.ToInt32(guna2ComboBox7.SelectedItem);
            decimal precio = Convert.ToDecimal(fila["Precio"], CultureInfo.InvariantCulture);
            string descripcion = fila["Descripcion"].ToString() ?? string.Empty;

            if (tipo == "Producto")
            {
                int stock = Convert.ToInt32(fila["Stock"]);
                int cantidadYaAgregada = _detalles.Where(d => d.ProductoID == itemID).Sum(d => d.Cantidad);

                if (cantidadYaAgregada + cantidad > stock)
                {
                    MessageBox.Show("La cantidad seleccionada supera el stock disponible.",
                                    "Facturacion",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }

            FacturaDetalle detalle = new FacturaDetalle
            {
                Tipo = tipo,
                ItemID = itemID,
                ServicioID = tipo == "Servicio" ? itemID : null,
                ProductoID = tipo == "Producto" ? itemID : null,
                Descripcion = descripcion,
                Cantidad = cantidad,
                PrecioAplicado = precio
            };

            _detalles.Add(detalle);
            ActualizarTotales();
        }

        private void QuitarDetalle()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is not FacturaDetalle detalle)
            {
                MessageBox.Show("Debe seleccionar un detalle para quitar.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            _detalles.Remove(detalle);
            ActualizarTotales();
        }

        private void EmitirFactura()
        {
            if (!TryGetSelectedInt(guna2ComboBox1.SelectedValue, out int citaID))
            {
                MessageBox.Show("Debe seleccionar una cita.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (!TryGetSelectedInt(guna2ComboBox2.SelectedValue, out int estadoID))
            {
                MessageBox.Show("Debe seleccionar un estado de factura.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (!TryGetSelectedInt(guna2ComboBox3.SelectedValue, out int usuarioID))
            {
                MessageBox.Show("Debe seleccionar el usuario facturador.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un detalle a la factura.",
                                "Facturacion",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int? descuentoID = null;
            if (TryGetSelectedInt(guna2ComboBox4.SelectedValue, out int descuentoSeleccionado) && descuentoSeleccionado > 0)
                descuentoID = descuentoSeleccionado;

            foreach (FacturaDetalle detalle in _detalles)
            {
                bool aplicaDescuento = DescuentoAplicaAlDetalle(detalle);
                detalle.DescuentoID = aplicaDescuento ? descuentoID : null;
                detalle.PorcentajeDescuento = aplicaDescuento ? _porcentajeDescuento : 0m;
            }

            if (!_detalles.Any(d => d.DescuentoID.HasValue))
                descuentoID = null;

            FacturaEmision factura = new FacturaEmision
            {
                UsuarioID = usuarioID,
                CitaID = citaID,
                FechaEmision = guna2DateTimePicker1.Value,
                EstadoId = estadoID,
                DescuentoID = descuentoID,
                PorcentajeDescuento = _porcentajeDescuento,
                Detalles = _detalles.ToList()
            };

            bool emitida = _facturacionDAO.EmitirFactura(factura, out int facturaID, out string error);
            if (!emitida)
            {
                MessageBox.Show(error,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            bool correoEnviado = _correoFacturaService.EnviarFactura(facturaID, out string mensajeCorreo);
            string mensajeFinal = correoEnviado
                ? $"Factura #{facturaID} emitida correctamente.\n{mensajeCorreo}"
                : $"Factura #{facturaID} emitida correctamente, pero no se pudo enviar por correo.\n{mensajeCorreo}";

            MessageBox.Show(mensajeFinal,
                            "Facturacion",
                            MessageBoxButtons.OK,
                            correoEnviado ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            LimpiarFactura();
            CargarCitas();
            CargarItemsFacturables();
        }

        private void ActualizarPorcentajeDescuento()
        {
            if (guna2ComboBox4.SelectedItem is DataRowView fila)
            {
                _porcentajeDescuento = Convert.ToDecimal(fila["Porcentaje"], CultureInfo.InvariantCulture);
                _descuentoServicioID = ObtenerEnteroNullable(fila, "ServicioID");
                _descuentoProductoID = ObtenerEnteroNullable(fila, "ProductoID");
            }
            else
            {
                _porcentajeDescuento = 0m;
                _descuentoServicioID = null;
                _descuentoProductoID = null;
            }

            ActualizarTotales();
        }

        private void ActualizarTotales()
        {
            decimal subtotal = _detalles.Sum(d => d.Subtotal);
            decimal descuento = _detalles.Sum(CalcularDescuentoDetalle);
            decimal total = subtotal - descuento;

            label18.Text = FormatearMoneda(subtotal);
            label19.Text = FormatearMoneda(descuento);
            label21.Text = FormatearMoneda(total);
        }

        private decimal CalcularDescuentoDetalle(FacturaDetalle detalle)
        {
            return DescuentoAplicaAlDetalle(detalle)
                ? detalle.Subtotal * _porcentajeDescuento / 100m
                : 0m;
        }

        private bool DescuentoAplicaAlDetalle(FacturaDetalle detalle)
        {
            if (_porcentajeDescuento <= 0)
                return false;

            if (_descuentoServicioID.HasValue)
                return detalle.ServicioID == _descuentoServicioID.Value;

            if (_descuentoProductoID.HasValue)
                return detalle.ProductoID == _descuentoProductoID.Value;

            return false;
        }

        private void LimpiarFactura()
        {
            _detalles.Clear();
            ActualizarTotales();

            if (guna2ComboBox4.Items.Count > 0)
                guna2ComboBox4.SelectedIndex = 0;

            guna2DateTimePicker1.Value = DateTime.Now;
        }

        private static bool TryGetSelectedInt(object? selectedValue, out int value)
        {
            value = 0;

            if (selectedValue == null || selectedValue is DataRowView)
                return false;

            return int.TryParse(selectedValue.ToString(), out value);
        }

        private static int? ObtenerEnteroNullable(DataRowView fila, string columna)
        {
            if (!fila.Row.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return null;

            return Convert.ToInt32(fila[columna], CultureInfo.InvariantCulture);
        }

        private static string FormatearMoneda(decimal valor)
        {
            return "$" + valor.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static void MostrarError(string error)
        {
            MessageBox.Show(error,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            ReporteIngresos reporteingresos = new ReporteIngresos();
            reporteingresos.ShowDialog();
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ReporteIngresos reporteingresos = new ReporteIngresos();
            reporteingresos.ShowDialog();
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
