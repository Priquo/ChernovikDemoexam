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
using ЧерновикDemoexam.FunctionalClasses;

namespace ЧерновикDemoexam
{
    /// <summary>
    /// Логика взаимодействия для WindowChangeMinCounInPack.xaml
    /// </summary>
    public partial class WindowChangeMinCounInPack : Window
    {
        List<Material> materials;
        public WindowChangeMinCounInPack(List<Material> materials)
        {
            InitializeComponent();
            this.materials = materials;
            this.materials.Sort(
                delegate (Material m1, Material m2)
                { 
                    return m1.MinCount.CompareTo(m2.MinCount); 
                });
            textBoxNewCount.Text = materials.Last().MinCount.ToString();
        }

        private void buttSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var material in materials)
            {
                BaseConnecter.BaseConnect.Material.Where(x => x.ID == material.ID).FirstOrDefault().MinCount = Convert.ToDouble(textBoxNewCount.Text);
            }
            BaseConnecter.BaseConnect.SaveChanges();
            MessageBox.Show("Данные успешно сохранены!");
        }

        private void buttBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
