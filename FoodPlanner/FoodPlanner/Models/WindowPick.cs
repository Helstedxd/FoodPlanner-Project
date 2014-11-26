using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class WindowPick
    {
        #region Fields
        private string _name;
        private Uri _viewPath;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public Uri ViewPath
        {
            get
            {
                return _viewPath;
            }
        }
        #endregion

        public WindowPick(Uri viewPath, string name)
        {
            _viewPath = viewPath;
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
