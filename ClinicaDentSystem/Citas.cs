using DAO;
using Microsoft.Data.SqlClient;
using MODELOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaDentSystem
{
    public partial class Citas : Form
    {

        private int citaID;

        public int CitaID { get => citaID; set => citaID = value; }

        public Citas()
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
        }

        public Citas(bool esEdicion, int CitaID = 0)
        {
            InitializeComponent();
            ResponsiveLayout.Configure(this);
            this.CitaID = CitaID;
            dtp_FechaCita.MinDate = DateTime.Today;

            if (esEdicion)
            {
                btn_GuardarCita.Visible = false;
                btn_EditarCita.Visible = true;
                guna2ImageButton2.Visible = false;
            }
            else
            {
                btn_GuardarCita.Visible = true;
                btn_EditarCita.Visible = false;
                guna2ImageButton1.Visible = false;
            }
        }

        private void Citas_Load(object sender, EventArgs e)
        {
            ConfigurarFechasIniciales();
            CargarPacientes();
            CargarDentistas();
            CargarEstados();


            if (CitaID > 0)
            {
                string pError = string.Empty;
                CitasDAO dao = new CitasDAO();
                Citass cita = dao.ObtenerPorId(CitaID, out pError);

                if (!string.IsNullOrEmpty(pError))
                {
                    MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime fechaCita = cita.Fechayhora.Date;

                if (fechaCita < dtp_FechaCita.MinDate)
                {
                    dtp_FechaCita.MinDate = fechaCita;
                }

                cbox_Paciente.SelectedValue = cita.PacienteId;
                cbox_Dentista.SelectedValue = cita.DentistaID;
                dtp_FechaCita.Value = fechaCita;
                dtp_HoraCita.Value = cita.Fechayhora;
                txt_MotivoCita.Text = cita.Motivo;
                cbox_EstadoCita.SelectedValue = cita.EstadoID;
                txt_NotasCita.Text = cita.Notasdelacita;
            }
        }

        private void ConfigurarFechasIniciales()
        {
            DateTime hoy = DateTime.Today;

            dtp_FechaCita.Value = hoy;
            dtp_FechaCita.MinDate = hoy;
            dtp_HoraCita.Value = DateTime.Now;
        }
        private void CargarPacientes()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);
                SqlCommand cmd = new SqlCommand("SELECT PacienteId, Nombre + ' ' + Apellido AS NombreCompleto FROM CLINICO.PACIENTE", conn.conn);
                SqlDataReader dr = cmd.ExecuteReader();
                cbox_Paciente.DisplayMember = "NombreCompleto";
                cbox_Paciente.ValueMember = "PacienteId";
                cbox_Paciente.DataSource = null;
                DataTable dt = new DataTable();
                dt.Load(dr);
                cbox_Paciente.DataSource = dt;
                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CargarDentistas()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);
                SqlCommand cmd = new SqlCommand("SELECT DentistaID, Nombre + ' ' + Apellido AS NombreCompleto FROM CLINICO.DENTISTA", conn.conn);
                SqlDataReader dr = cmd.ExecuteReader();
                cbox_Dentista.DisplayMember = "NombreCompleto";
                cbox_Dentista.ValueMember = "DentistaID";
                cbox_Dentista.DataSource = null;
                DataTable dt = new DataTable();
                dt.Load(dr);
                cbox_Dentista.DataSource = dt;
                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarEstados()
        {
            string pError = string.Empty;
            Conexion conn = new Conexion();
            try
            {
                conn.AbrirConexion(out pError);
                SqlCommand cmd = new SqlCommand("SELECT EstadoId, Estado FROM CLINICO.ESTADOCITA", conn.conn);
                SqlDataReader dr = cmd.ExecuteReader();
                cbox_EstadoCita.DisplayMember = "Estado";
                cbox_EstadoCita.ValueMember = "EstadoId";
                cbox_EstadoCita.DataSource = null;
                DataTable dt = new DataTable();
                dt.Load(dr);
                cbox_EstadoCita.DataSource = dt;
                conn.CerrarConexion(out pError);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_GuardarCita_Click(object sender, EventArgs e)
        {
            GuardarCita();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            GuardarCita();
        }

        private void GuardarCita()
        {
            string pError = string.Empty;

            if (dtp_FechaCita.Value.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "No se puede crear una cita con una fecha anterior a hoy.",
                    "Fecha invalida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DateTime fechaHora = dtp_FechaCita.Value.Date + dtp_HoraCita.Value.TimeOfDay;

            Citass cita = new Citass();
            cita.PacienteId = Convert.ToInt32(cbox_Paciente.SelectedValue);
            cita.DentistaID = Convert.ToInt32(cbox_Dentista.SelectedValue);
            cita.Fechayhora = fechaHora;
            cita.Motivo = txt_MotivoCita.Text;
            cita.EstadoID = Convert.ToInt32(cbox_EstadoCita.SelectedValue);
            cita.Notasdelacita = txt_NotasCita.Text;
            cita.Fecha = DateTime.Now;

            CitasDAO dao = new CitasDAO();
            dao.GuardarRegistro(cita, out pError);

            if (pError == "Cita creada correctamente")
            {
                MessageBox.Show(pError, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_EditarCita_Click(object sender, EventArgs e)
        {
            string pError = string.Empty;
            if (dtp_FechaCita.Value.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "No se puede actualizar una cita con una fecha anterior a hoy.",
                    "Fecha invalida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DateTime fechaHora = dtp_FechaCita.Value.Date + dtp_HoraCita.Value.TimeOfDay;

            Citass cita = new Citass();
            cita.Numerodecita = CitaID;
            cita.PacienteId = Convert.ToInt32(cbox_Paciente.SelectedValue);
            cita.DentistaID = Convert.ToInt32(cbox_Dentista.SelectedValue);
            cita.Fechayhora = fechaHora;
            cita.Motivo = txt_MotivoCita.Text;
            cita.EstadoID = Convert.ToInt32(cbox_EstadoCita.SelectedValue);
            cita.Notasdelacita = txt_NotasCita.Text;
            cita.Fecha = dtp_FechaCita.Value.Date;

            CitasDAO dao = new CitasDAO();
            dao.ActualizarRegistro(cita, out pError);

            if (pError == "Cita actualizada correctamente.")
            {
                MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            string pError = string.Empty;
            if (dtp_FechaCita.Value.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "No se puede actualizar una cita con una fecha anterior a hoy.",
                    "Fecha invalida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DateTime fechaHora = dtp_FechaCita.Value.Date + dtp_HoraCita.Value.TimeOfDay;

            Citass cita = new Citass();
            cita.Numerodecita = CitaID;
            cita.PacienteId = Convert.ToInt32(cbox_Paciente.SelectedValue);
            cita.DentistaID = Convert.ToInt32(cbox_Dentista.SelectedValue);
            cita.Fechayhora = fechaHora;
            cita.Motivo = txt_MotivoCita.Text;
            cita.EstadoID = Convert.ToInt32(cbox_EstadoCita.SelectedValue);
            cita.Notasdelacita = txt_NotasCita.Text;
            cita.Fecha = dtp_FechaCita.Value.Date;

            CitasDAO dao = new CitasDAO();
            dao.ActualizarRegistro(cita, out pError);

            if (pError == "Cita actualizada correctamente.")
            {
                MessageBox.Show(pError, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(pError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
