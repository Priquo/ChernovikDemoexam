using Microsoft.Win32;
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

namespace ЧерновикDemoexam.windows
{
    /// <summary>
    /// Логика взаимодействия для WindowEditMaterial.xaml
    /// </summary>
    public partial class WindowEditMaterial : Window
    {
        bool isNewMaterial = false;
        Material material;
        List<MaterialType> materialTypes = BaseConnecter.BaseConnect.MaterialType.ToList();
        List<Supplier> suppliers = BaseConnecter.BaseConnect.Supplier.ToList();
        List<Supplier> Suppliers { get; set; }        
        public WindowEditMaterial()
        {
            InitializeComponent();
            Title = "Добавление материала";
            material = new Material();

            comboBoxMaterialType.ItemsSource = materialTypes;
            comboBoxMaterialType.DisplayMemberPath = "Title";
            comboBoxMaterialType.SelectedValuePath = "ID";
            
            comboBoxSuppliersList.ItemsSource = suppliers;
            comboBoxSuppliersList.DisplayMemberPath = "Title";
            comboBoxSuppliersList.SelectedValuePath = "ID";

            Suppliers = new List<Supplier>();
            listBoxSuppliers.ItemsSource = Suppliers;
            listBoxSuppliers.DisplayMemberPath = "Title";

            isNewMaterial = true;
        }
        public WindowEditMaterial(Material material)
        {
            InitializeComponent();
            this.Title = "Редактирование материала";
            this.material = material;
            Suppliers = material.Supplier.ToList();

            comboBoxMaterialType.ItemsSource = materialTypes;
            comboBoxMaterialType.DisplayMemberPath = "Title";
            comboBoxMaterialType.SelectedValuePath = "ID";

            comboBoxSuppliersList.ItemsSource = suppliers;
            comboBoxSuppliersList.DisplayMemberPath = "Title";
            comboBoxSuppliersList.SelectedValuePath = "ID";

            listBoxSuppliers.ItemsSource = Suppliers;
            listBoxSuppliers.DisplayMemberPath = "Title";
            
            textBoxTitle.Text = material.Title;
            textBoxCountInPack.Text = material.CountInPack.ToString();
            textBoxCoustInStock.Text = material.CountInStock.ToString();
            textBoxMinCount.Text = material.MinCount.ToString();
            textBoxDiscription.Text = material.Description.ToString();
            textBoxUnit.Text = material.Unit == null ? "" : material.Unit.ToString();
            textBoxCost.Text = material.Cost.ToString();
            textBlockImagePath.Text = material.Image == "" ? textBlockImagePath.Text : material.Image;
        }
        
        private void buttSave_Click(object sender, RoutedEventArgs e)
        {
            switch (isNewMaterial)
            {
                case true:
                    int lastID = BaseConnecter.BaseConnect.Material.ToList().Last().ID + 1;
                    material.ID = lastID;
                    material.Cost = Convert.ToDecimal(textBoxCost.Text);
                    material.CountInPack = Convert.ToInt32(textBoxCountInPack.Text);
                    material.CountInStock = Convert.ToInt32(textBoxCoustInStock.Text);
                    material.MinCount = Convert.ToDouble(textBoxMinCount.Text);
                    material.MaterialTypeID = (int)comboBoxMaterialType.SelectedValue;
                    material.Description = textBoxDiscription.Text;
                    material.Image = textBlockImagePath.Text;
                    material.Title = textBoxTitle.Text;
                    material.Supplier = Suppliers;
                    material.Unit = textBoxUnit.Text;
                    BaseConnecter.BaseConnect.Material.Add(material);
                    break;
                case false:
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Cost = Convert.ToDecimal(textBoxCost.Text);
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).CountInPack = Convert.ToInt32(textBoxCost.Text);
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).CountInStock = Convert.ToInt32(textBoxCoustInStock.Text);
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Description = textBoxDiscription.Text;
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Image = textBlockImagePath.Text;
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).MaterialTypeID = (int)comboBoxMaterialType.SelectedValue;
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Title = textBoxTitle.Text;
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).MinCount = Convert.ToInt32(textBoxMinCount.Text);
                    BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Unit = textBoxUnit.Text;
                    break;
            }
            BaseConnecter.BaseConnect.SaveChanges();
            MessageBox.Show("Сохранение произведено успешно!");
        }

        private void buttDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить данный материал?", "Внимание!!!", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK && material != null)
            {
                if (BaseConnecter.BaseConnect.ProductMaterial.FirstOrDefault(x => x.MaterialID == material.ID) != null)
                {
                    MessageBox.Show("Удаление материала невозможно, так как он используется в производстве продукта", "Операция прервана", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    List<MaterialCountHistory> histories = BaseConnecter.BaseConnect.MaterialCountHistory.Where(x => x.MaterialID == material.ID).ToList();
                    BaseConnecter.BaseConnect.MaterialCountHistory.RemoveRange(histories);
                    List<Supplier> suppliersMaterial = BaseConnecter.BaseConnect.Supplier.ToList();
                    suppliersMaterial = suppliersMaterial.Where(x => x.Material.Contains(material)).ToList();
                    foreach (var supplier in suppliersMaterial)
                    {
                        supplier.Material.Remove(material);
                    }
                    BaseConnecter.BaseConnect.Material.Remove(material);
                    BaseConnecter.BaseConnect.SaveChanges();
                    MessageBox.Show("Удаление произведено успешно!");
                }
            }
        }

        private void buttBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditSuppliersList(object sender, RoutedEventArgs e)
        {
            if (comboBoxSuppliersList.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика");
            }
            else
            {
                Button button = (Button)sender;
                switch (button.Name)
                {
                    case "buttAddSuppliers":
                        if (!material.Supplier.Contains((Supplier)comboBoxSuppliersList.SelectedItem))
                        {
                            material.Supplier.Add((Supplier)comboBoxSuppliersList.SelectedItem);
                            Suppliers.Add((Supplier)comboBoxSuppliersList.SelectedItem);
                        }
                        else
                        {
                            MessageBox.Show("Этот поставщик уже есть");
                        }
                        break;
                    case "buttRemoveSuppliers":
                        if (material.Supplier.Contains((Supplier)comboBoxSuppliersList.SelectedItem))
                        {
                            material.Supplier.Remove((Supplier)comboBoxSuppliersList.SelectedItem);
                            Suppliers.Remove((Supplier)comboBoxSuppliersList.SelectedItem);                            
                        }
                        else
                        {
                            MessageBox.Show("Этого поставщика нет в данных материала");
                        }
                        break;
                }
                listBoxSuppliers.Items.Refresh();
            }           
            
        }

        private void buttSortSuppliersListByName_Click(object sender, RoutedEventArgs e)
        {
            suppliers = suppliers.OrderBy(x => x.Title).ToList();
            comboBoxSuppliersList.Items.Refresh();
        }

        private void buttAddNewImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Файлы рисунков (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == true)
            {
                textBlockImagePath.Text = openFile.FileName;
                material.RealImagePath = openFile.FileName;
            }
        }
    }
}
