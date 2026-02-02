using articulomodelo.Backend.Modelo;
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
using System.Windows.Shapes;

namespace articulomodelo.Frontend.Cosas_Scrapped
{
    /// <summary>
    /// Interaction logic for Eliminar_Articulo.xaml
    /// </summary>
    public partial class Eliminar_Articulo : Window
    {
        public Eliminar_Articulo()
        {
            InitializeComponent();
            CargarArticulos();
        }
    
       //Cargar Articulos
        private void CargarArticulos()
        {
            using (var db = new DiinventarioexamenContext())
            {
                var articulos = db.Modeloarticulos.OrderBy(p => p.Nombre).ToList();
                eliminar_articulo_item.ItemsSource = articulos;
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Eliminar_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }



}
