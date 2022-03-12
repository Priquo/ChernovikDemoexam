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
using ЧерновикDemoexam.FunctionalClasses;
using ЧерновикDemoexam.UIclasses;
using ЧерновикDemoexam.windows;

namespace ЧерновикDemoexam.pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialsList.xaml
    /// </summary>
    public partial class MaterialsList : Page
    {
        List<Material> materials = BaseConnecter.BaseConnect.Material.ToList();
        List<Material> materialsForFilter = BaseConnecter.BaseConnect.Material.ToList();
        List<MaterialType> materialTypes = BaseConnecter.BaseConnect.MaterialType.ToList();
        List<string> sort = new List<string> { "По умолчанию", "Наименование", "Остаток на складе", "Стоимость" };
        List<string> filter = new List<string> { "Все типы" };
        PageChanger pageChanger = new PageChanger();
        public MaterialsList()
        {
            InitializeComponent();
            listBoxMaterials.ItemsSource = materials;

            comboBoxSorting.ItemsSource = sort;
            comboBoxSorting.SelectedIndex = 0;
            comboBoxFilter.ItemsSource = filter;
            comboBoxFilter.SelectedIndex = 0;
            foreach (var type in materialTypes)
            {
                filter.Add(type.Title);
            }

            DataContext = pageChanger;
            pageChanger.CountAllRows = materials.Count;
            pageChanger.CountPage = 15;

            listBoxMaterials.ItemsSource = materialsForFilter.Skip(pageChanger.CountPage * pageChanger.CurrentPage - pageChanger.CountPage).Take(pageChanger.CountPage).ToList();
                        
            textBlockCountPagesOnList.Text = listBoxMaterials.Items.Count + " из " + materialsForFilter.Count.ToString();
        }

        private void Filter(object sender, RoutedEventArgs e)
        {

            //фильтр по типу материала
            if (comboBoxFilter.SelectedValue != null && comboBoxFilter.SelectedIndex != 0)
            {
                materialsForFilter = materialsForFilter.Where(x => x.MaterialTypeText == comboBoxFilter.SelectedValue.ToString()).ToList();
            }
            else
            {
                materialsForFilter = materials;
            }

            //поиск по наименованию
            if (textBoxSearch.Text != "")
            {
                materialsForFilter = materialsForFilter.Where(x => x.Title.Contains(textBoxSearch.Text)).ToList();
            }
            else if (textBoxSearch.Text == "" && comboBoxFilter.SelectedIndex == 0)
            {
                materialsForFilter = materials;
            }

            //сортировка по наименованию, остатку на складе и стоимости материала
            if (comboBoxSorting.SelectedValue != null && comboBoxSorting.SelectedIndex != 0)
            {
                switch (comboBoxSorting.SelectedValue.ToString())
                {
                    case "Наименование":
                        materialsForFilter = materialsForFilter.OrderBy(x => x.Title).ToList();
                        break;
                    case "Остаток на складе":
                        materialsForFilter = materialsForFilter.OrderBy(x => x.CountInStock).ToList();
                        break;
                    case "Стоимость":
                        materialsForFilter = materialsForFilter.OrderBy(x => x.Cost).ToList();
                        break;
                    case "По умолчанию":
                        materialsForFilter = materialsForFilter.OrderBy(x => x.ID).ToList();
                        break;
                }
            }            
            listBoxMaterials.ItemsSource = materialsForFilter;
            pageChanger.CountAllRows = materialsForFilter.Count;
            textBlockCountPagesOnList.Text = listBoxMaterials.Items.Count + " из " + materialsForFilter.Count.ToString();
        }

        private void buttBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = (TextBlock)sender;
            switch (block.Name)
            {
                case "buttNext":
                    pageChanger.CurrentPage++;
                    break;
                case "buttBack":
                    pageChanger.CurrentPage--;
                    break;
                default:
                    pageChanger.CurrentPage = Convert.ToInt32(block.Text);
                    break;                
            }

            listBoxMaterials.ItemsSource = materialsForFilter.Skip(pageChanger.CountPage * pageChanger.CurrentPage - pageChanger.CountPage).Take(pageChanger.CountPage).ToList();
            textBlockCountPagesOnList.Text = listBoxMaterials.Items.Count + " из " + materialsForFilter.Count.ToString();
        }

        private void buttChangeMinCount_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxMaterials.SelectedItems.Count != 0)
            {
                List<Material> selectedMaterials = new List<Material>();
                foreach (var item in listBoxMaterials.SelectedItems)
                {
                    selectedMaterials.Add((Material)item);
                }
                WindowChangeMinCounInPack window = new WindowChangeMinCounInPack(selectedMaterials);
                window.ShowDialog();
                listBoxMaterials.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите элементы для редактирования");
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (listBoxMaterials.SelectedItems.Count > 1)
            {
                MessageBox.Show("Выберите один материал для редактирования");
            }
            else if (e.ClickCount >= 1 && Application.Current.Windows.Count < 3 && listBoxMaterials.SelectedItem != null)
            {
                WindowEditMaterial windowEdit = new WindowEditMaterial((Material)listBoxMaterials.SelectedItem);
                windowEdit.Show();
                listBoxMaterials.Items.Refresh();
            }
            else if (Application.Current.Windows.Count > 2)
                MessageBox.Show("Закройте другие окна редактирования");
            
        }
    }
}
