using ObjectExplorer.DocConverters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ObjectExplorer
{
    internal class ObjectPropertyController:DocBase
    {
        private object currentObject;
        ObjectViewModelHierarchy objectHierarchy;
        private RelayCommand _treeViewItemRigthClickCommand;
        private List<MenuItem> contextOperations;
        IComExpose baseCom;

        public ObjectPropertyController(object obj,IComExpose baseCom):base(obj.GetType().ToString())
        {
            this.currentObject = obj;
            this.baseCom = baseCom;
            objectHierarchy = new ObjectViewModelHierarchy(obj);
        }

        private void LoadContextMenu()
        {
            contextOperations = new List<MenuItem>();
            MenuItem newView = new MenuItem();
            newView.Header = "SendToOtherView";
            newView.Click += NewView_Click;
            contextOperations.Add(newView);
        }

        private void NewView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ObjectViewModel.SelectedObject !=null)
            {
                baseCom.NewCommand.Execute(ObjectViewModel.SelectedObject);
            }
        }



        public object CurrentObject { get { return currentObject; }  }
        public ObjectViewModelHierarchy ObjectHierarchy { get { return objectHierarchy; } }
        public ICommand TreeViewItemRigthClickCommand
        {
            get
            {
                if (_treeViewItemRigthClickCommand == null)
                {
                    _treeViewItemRigthClickCommand = new RelayCommand((p) => TreeViewItemRigthClick(p));
                }
                return _treeViewItemRigthClickCommand;
            }
        }

        public List<MenuItem> ContextOperations { get { return contextOperations; } }

        public void TreeViewItemRigthClick(object obj)
        {
            LoadContextMenu();
        }
    }

    internal class ObjectViewModel : NotifyUI
    {
        List<ObjectViewModel> _children;
        readonly ObjectViewModel _parent;
        readonly object _object;
        readonly MemberInfo _info;
        readonly Type _type;
        
        bool _isExpanded;
        bool _isSelected;

        public ObjectViewModel(object obj)
            : this(obj, null, null)
        {
        }

        ObjectViewModel(object obj, MemberInfo info, ObjectViewModel parent)
        {
            _object = obj;
            _info = info;
            if (_object != null)
            {
                _type = obj.GetType();
                if (!IsPrintableType(_type))
                {
                    // load the _children object with an empty collection to allow the + expander to be shown
                    _children = new List<ObjectViewModel>(new ObjectViewModel[] { new ObjectViewModel(null) });
                }
            }
            _parent = parent;
        }

        private object GetValue(PropertyInfo p, object obj)
        {
            object v = null;
            try
            {
                v = p.GetValue(_object, null);
            }
            catch (Exception err)
            {


            }
            return v;
        }

        private object GetValue(MemberInfo m, object obj)
        {
            object v = null;


            if (m.MemberType == MemberTypes.Property)
            {
                PropertyInfo property = (PropertyInfo)m;
                return GetValue(property, obj);
            }
            else if (m.MemberType == MemberTypes.Field)
            {
                FieldInfo fInfo = (FieldInfo)m;
                try
                {
                    v = fInfo.GetValue(obj);
                }
                catch (Exception err)
                {

                }
            }

            return v;
        }

        private void LoadChildrenAsync()
        {
            Task.Run(() => LoadChildren());
        }
        public void LoadChildren()
        {
            if (_object != null)
            {
                // exclude value types and strings from listing child members
                if (!IsPrintableType(_type))
                {
                    var propLoad =Task.Run(()=> GetPropsandFiels());
                    // if this is a collection type, add the contained items to the children
                    var colLoad = Task.Run(() => CollectionLoad());
                    _children = new List<ObjectViewModel>();
                    _children.AddRange(propLoad.Result);
                    CurrentDispatcher.Invoke(() => NotifyPropertyChanged("Children"));
                    _children.AddRange(colLoad.Result);
                    CurrentDispatcher.Invoke(() => NotifyPropertyChanged("Children"));
                    _children = _children.OrderBy(x => x.Name).ToList();
                    CurrentDispatcher.Invoke(() => NotifyPropertyChanged("Children"));
                }
            }
        }

        private IEnumerable<ObjectViewModel> GetPropsandFiels()
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var children = _type.GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(_type.GetProperties(bindingFlags)).ToArray() // exclude indexed parameters for now
                .Select(p =>
                    new ObjectViewModel(GetValue(p, _object), p, this)
                );

            foreach (var item in children)
            {
                yield return item;
            }
        }

        private IEnumerable<ObjectViewModel> CollectionLoad()
        {

            var collection = _object as IEnumerable;
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    yield return new ObjectViewModel(item, null, this);//// todo: add something to view the index value
                    
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if the object graph can display this type without enumerating its children
        /// </summary>
        static bool IsPrintableType(Type type)
        {
            return type != null && (
                type.IsPrimitive ||
                type.IsAssignableFrom(typeof(string)) ||
                type.IsEnum);
        }

        public ObjectViewModel Parent
        {
            get { return _parent; }
        }

        public MemberInfo Info
        {
            get { return _info; }
        }

        public List<ObjectViewModel> Children
        {
            get { return _children; }
        }

        public string Type
        {
            get
            {
                var type = string.Empty;
                if (_object != null)
                {
                    type = string.Format("({0})", _type.Name);
                }
                else
                {
                    if (_info != null)
                    {
                        type = string.Format("({0})", _info.Name);
                    }
                }
                return type;
            }
        }

        public string Name
        {
            get
            {
                var name = string.Empty;
                if (_info != null)
                {
                    name = _info.Name;
                }
                return name;
            }
        }

        public string Value
        {
            get
            {
                var value = string.Empty;
                if (_object != null)
                {
                    if (IsPrintableType(_type))
                    {
                        value = _object.ToString();
                    }
                }
                else
                {
                    value = "<null>";
                }
                return value;
            }
        }

        public static object SelectedObject { get; private set; }
        #region Presentation Members

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    if (_isExpanded)
                    {
                        LoadChildren();
                    }
                    NotifyPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                {
                    _parent.IsExpanded = true;
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    SelectedObject = _object;
                   NotifyPropertyChanged("IsSelected");
                }
            }
        }

        public bool NameContains(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(Name))
            {
                return false;
            }

            return Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public bool ValueContains(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(Value))
            {
                return false;
            }

            return Value.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        #endregion

    }

    internal class ObjectViewModelHierarchy
    {
        readonly ReadOnlyCollection<ObjectViewModel> _firstGeneration;
        readonly ObjectViewModel _rootObject;

        public ObjectViewModelHierarchy(object rootObject)
        {
            _rootObject = new ObjectViewModel(rootObject);
            _firstGeneration = new ReadOnlyCollection<ObjectViewModel>(new ObjectViewModel[] { _rootObject });
        }

        public ReadOnlyCollection<ObjectViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
        }
    }
}
