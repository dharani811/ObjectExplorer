using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace ObjectExplorer.Styles
{
    internal class PanesTemplateSelector : DataTemplateSelector
    {
        #region Public Property

        public DataTemplate ContentTemplate
        {
            get;
            set;
        }
     

        #endregion

        #region Constructor

        public PanesTemplateSelector()
        {

        }

        #endregion

        #region Public Methods

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;

            if (item is ObjectPropertyController)
                return ContentTemplate;


            return base.SelectTemplate(item, container);
        }

        #endregion
    }
}
