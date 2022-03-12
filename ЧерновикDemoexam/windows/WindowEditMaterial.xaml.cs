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
        Material material;
        List<string> materialTypesText = new List<string>();
        List<string> suppliersText = new List<string>();
        List<MaterialType> materialTypes = BaseConnecter.BaseConnect.MaterialType.ToList();
        List<Supplier> suppliers = BaseConnecter.BaseConnect.Supplier.ToList();
        public WindowEditMaterial(Material material)
        {
            InitializeComponent();
            this.Title = "Редактирование материала";
            this.material = material;
            comboBoxMaterialType.ItemsSource = materialTypesText;
            foreach (var type in materialTypes)
            {
                materialTypesText.Add(type.Title);
            }
            comboBoxMaterialType.SelectedIndex = comboBoxMaterialType.Items.IndexOf(material.MaterialTypeText);
            comboBoxSuppliersList.ItemsSource = suppliersText;
            foreach (var supp in suppliers)
            {
                suppliersText.Add(supp.Title);
            }
            textBoxTitle.Text = material.Title;
            textBoxCountInPack.Text = material.CountInPack.ToString();
            textBoxCoustInStock.Text = material.CountInStock.ToString();
            textBoxMinCount.Text = material.MinCount.ToString();
            textBoxDiscription.Text = material.Description.ToString();
            textBoxUnit.Text = material.Unit.ToString();
            textBoxCost.Text = material.Cost.ToString();
            textBlockImagePath.Text = material.Image == "" ? textBlockImagePath.Text : material.Image;
            textBlockSuppliers.Text = material.SuppliersList == "" ? textBlockSuppliers.Text : material.SuppliersList;
        }

        private void buttSave_Click(object sender, RoutedEventArgs e)
        {
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Cost = Convert.ToDecimal(textBoxCost.Text);
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).CountInPack = Convert.ToInt32(textBoxCost.Text);
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).CountInStock = Convert.ToInt32(textBoxCoustInStock.Text);
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Description = textBoxDiscription.Text;
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Image = textBlockImagePath.Text;
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).MaterialTypeID = BaseConnecter.BaseConnect.MaterialType.FirstOrDefault(x => x.Material == comboBoxMaterialType.SelectedValue).ID;
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).Title = textBoxTitle.Text;
            BaseConnecter.BaseConnect.Material.FirstOrDefault(x => x.ID == material.ID).MinCount = Convert.ToInt32(textBoxMinCount.Text);

        }

        private void buttDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить данный материал?", "Внимание!!!", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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
                        if (!material.Supplier.Contains(suppliers.FirstOrDefault(x => x.Title == comboBoxSuppliersList.SelectedValue.ToString())))
                        {
                            textBlockSuppliers.Text = textBlockSuppliers.Text.Contains("Список поставщиков") ? "" : textBlockSuppliers.Text;
                            if (textBlockSuppliers.Text == "")
                                textBlockSuppliers.Text += comboBoxSuppliersList.SelectedValue;
                            else
                                textBlockSuppliers.Text += ", " + comboBoxSuppliersList.SelectedValue;
                            material.Supplier.Add(suppliers.FirstOrDefault(x => x.Title == comboBoxSuppliersList.SelectedValue.ToString()));
                        }
                        else
                        {
                            MessageBox.Show("Этот поставщик уже есть");
                        }
                        break;
                    case "buttRemoveSuppliers":
                        if (material.Supplier.Contains(suppliers.FirstOrDefault(x => x.Title == comboBoxSuppliersList.SelectedValue.ToString())))
                        {
                            if (material.Supplier.Count > 1)
                                textBlockSuppliers.Text = textBlockSuppliers.Text.Replace(", " + comboBoxSuppliersList.SelectedValue, "");
                            else
                                textBlockSuppliers.Text = textBlockSuppliers.Text.Replace(comboBoxSuppliersList.SelectedValue.ToString(), "");
                            material.Supplier.Remove(suppliers.FirstOrDefault(x => x.Title == comboBoxSuppliersList.SelectedValue.ToString()));
                        }
                        else
                        {
                            MessageBox.Show("Этого поставщика нет в данных материала");
                        }
                        break;
                }
            }           
            
        }

        private void buttSortSuppliersListByName_Click(object sender, RoutedEventArgs e)
        {
            suppliersText.Sort();
            comboBoxSuppliersList.Items.Refresh();
        }
    }
}
