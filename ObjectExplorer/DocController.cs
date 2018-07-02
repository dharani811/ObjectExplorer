using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ObjectExplorer
{
    internal class DocController:NotifyUI,IComExpose
    {
        private ObservableCollection<ObjectPropertyController> documents;
        private ObjectPropertyController activeDocument;
        private ICommand newCommand;

        public DocController(object objectToLoad)
        {
            documents = new ObservableCollection<ObjectPropertyController>();
            activeDocument = new ObjectPropertyController(objectToLoad,this);
            documents.Add(activeDocument);
            activeDocument = new ObjectPropertyController(objectToLoad, this);
            documents.Add(activeDocument);
            newCommand = new RelayCommand((p) => NewDoc(p));
        }

        private void NewDoc(object p)
        {
            ActiveDocument = new ObjectPropertyController(p, this);
            documents.Add(activeDocument);
        }

        public ObservableCollection<ObjectPropertyController> Documents { get { return documents; } }
        public ObjectPropertyController ActiveDocument { get { return activeDocument; } set { activeDocument = value;
                NotifyPropertyChanged("ActiveDocument");
            } }


        ICommand IComExpose.NewCommand { get { return newCommand; } }
    }
}
