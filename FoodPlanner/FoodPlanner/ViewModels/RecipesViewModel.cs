using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.ViewModels
{
    class RecipesViewModel
    {
        private string _searchDefine;

        public RecipesViewModel()
        {
            this.listOstrings = new ObservableCollection<string>();
        }
        
        public string SearchDefine
        {
            get
            {
                return _searchDefine;
            }
            set
            {
                _searchDefine = value;
                //listOstrings.Add(value);
            }
        }

        public ObservableCollection<string> listOstrings { get; private set; }

    }
}
