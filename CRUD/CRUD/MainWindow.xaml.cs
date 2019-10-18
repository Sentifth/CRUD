using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace CRUD
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\McLarenCRUD.mdb";
            MostrarDatos();
        }
        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from ventas";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtSerie.Text = "";
            txtCliente.Text = "";
            cbModelo.SelectedIndex = 0;
            txtAño.Text = "";
            txtPrecio.Text = "";
            btnNuevo.Content = "Nuevo";
            txtSerie.IsEnabled = true;
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtSerie.Text != "")
            {
                if (txtSerie.IsEnabled == true)
                {
                    if (cbModelo.Text != "Selecciona el Modelo")
                    {
                        cmd.CommandText = "insert into ventas(Serie,Cliente,Modelo,Año,Precio) " +
                            "Values(" + txtSerie.Text + ",'" + txtCliente.Text + "','" + cbModelo.Text + "'," + txtAño.Text + ",'" + txtPrecio.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Venta de auto agregada correctamente...");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar el Modelo....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Progra set Cliente='" + txtCliente.Text + "',Modelo='" + cbModelo.Text + "',Año=" + txtAño.Text
                        + ",Precio='" + txtPrecio.Text + "' where Serie=" + txtSerie.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos de la venta actualizados...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner la serie del un automovil.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtSerie.Text = row["Serie"].ToString();
                txtCliente.Text = row["Cliente"].ToString();
                cbModelo.Text = row["Modelo"].ToString();
                txtAño.Text = row["Año"].ToString();
                txtPrecio.Text = row["Precio"].ToString();
                txtSerie.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar una venta...");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from ventas where Serie=" + row["Serie"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Venta eliminada correctamente...");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar una venta...");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
