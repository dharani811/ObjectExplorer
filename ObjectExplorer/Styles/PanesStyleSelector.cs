using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ObjectExplorer.Styles
{
    internal class PanesStyleSelector : StyleSelector
    {
        #region Public Property

        public Style DocumentStyle
        {
            get;
            set;
        }
       

        #endregion

        #region Constructor

        public PanesStyleSelector()
        {
        }

        #endregion

        #region Public Methods

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is ObjectPropertyController)
                return DocumentStyle;
            return base.SelectStyle(item, container);
        }

        #endregion
    }
}
