using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЧерновикDemoexam.FunctionalClasses;

namespace ЧерновикDemoexam
{
    public partial class Material
    {
        public string SuppliersList
        {
            get
            {
                string suppliers = "";
                if (Supplier.Count != 0)
                {
                    foreach (var supp in Supplier)
                    {
                        suppliers += supp.Title.ToString() + ", ";
                    }
                    suppliers = suppliers.Remove(suppliers.Length - 2, 2);
                }                
                return suppliers;
            }
        }
        public string MaterialTypeText { get => MaterialType.Title.ToString(); }
        public string RealImagePath
        {
            get
            {
                string path = @"\images";
                path +=  Image == "" ? @"\picture.png" : Image.ToString();
                return path;
            }
            set => RealImagePath = value;
        }
        public bool CountLessInStock { get => CountInStock < MinCount; }
        public bool CountMoreInStock { get => CountInStock > MinCount*3; }
    }
}
