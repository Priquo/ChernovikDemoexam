using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЧерновикDemoexam.FunctionalClasses;

namespace ЧерновикDemoexam.UIclasses
{
    public class PageChanger : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int[] PageNumber { get; set; } = new int[3];
        public string[] Bold { get; set; } = new string[3];
        int countPage;
        public int CountPage // количество записей на странице по текущему списку материалов
        {
            get => countPage;
            set
            {
                countPage = value;          
                if (countAllRows % value == 0)
                    CountPages = countAllRows / value;
                else
                    CountPages = countAllRows / value + 1;
            }
        }
        int countPages;
        public int CountPages // количество страниц из общего числа записей на количество записей на странице
        {
            get => countPages;
            set => countPages = value;
        }

        int countAllRows;
        public int CountAllRows // количество записей по текущему списку материалов
        {
            get => countAllRows;
            set {
                countAllRows = value;
                if (value % 15 == 0)
                    CountPages = value / 15;
                else
                    CountPages = value / 15 + 1;
            }
        }

        int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                if (currentPage < 1)
                    currentPage = 1;
                if (currentPage > CountPages)
                    currentPage = CountPages;
                for (int i = 0; i < 3; i++)
                {
                    if (CountPages < 3 || currentPage < (1 + 3/2))
                        PageNumber[i] = i + 1;
                    else if (currentPage > CountPages - (1 + 3/2))
                        PageNumber[i] = CountPages - 2 + i;
                    else
                        PageNumber[i] = currentPage + i - 3/2;
                }
                for (int i = 0; i<3; i++)
                {
                    if (PageNumber[i] == currentPage)
                        Bold[i] = "ExtraBold";
                    else
                        Bold[i] = "Reqular";
                }
                PropertyChanged(this, new PropertyChangedEventArgs("CountPagesText"));
                PropertyChanged(this, new PropertyChangedEventArgs("PageNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("Bold"));
            }
        }
        public PageChanger()
        {
            for (int i = 0; i < 3; i++)
            {
                PageNumber[i] = i + 1;
                Bold[i] = "Regular";
            }
            Bold[0] = "ExtraBold";
            currentPage = 1;
            countPage = 1;
            countAllRows = 1;
        }

    }
}
