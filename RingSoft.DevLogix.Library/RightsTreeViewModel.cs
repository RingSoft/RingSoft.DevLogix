using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.DevLogix.Library
{
    public class TreeViewItem : INotifyPropertyChanged
    {
        public TreeViewItem Parent { get; set; }

        public string Text { get; set; }

        public bool SettingCheck { get; private set; }

        private bool? _isChecked;

        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                {
                    return;
                }
                ThreeState = value == null;
                _isChecked = value;
                OnPropertyChanged();
                CheckChanged?.Invoke(this, EventArgs.Empty);
                if (_isChecked != null)
                {
                    if (Items.Any())
                    {
                        SettingCheck = true;
                        foreach (var treeViewItem in Items)
                        {
                            treeViewItem.IsChecked = _isChecked;
                        }

                        if (Parent != null)
                        {
                            CheckParent();
                        }
                        SettingCheck = false;
                    }
                    else
                    {
                        if (Parent != null && !Parent.SettingCheck)
                        {
                            CheckParent();
                        }
                    }
                }
                else
                {
                    if (Parent != null)
                    {
                        Parent.IsChecked = _isChecked;
                    }
                }
            }
        }

        private bool _threeState;

        public bool ThreeState
        {
            get => _threeState;
            set
            {
                if (_threeState == value)
                {
                    return;
                }
                _threeState = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler CheckChanged;

        public TreeViewItem(string text, bool? isChecked, TreeViewItem parent)
        {
            Parent = parent;
            Text = text;
            IsChecked = isChecked;
        }

        public void CheckParent()
        {
            var checkedItems = Parent.Items.Where(p => p.IsChecked == true);
            var anyChecked = checkedItems.Any();
            if (anyChecked)
            {
                if (checkedItems.Count() == Parent.Items.Count)
                {
                    Parent.IsChecked = true;
                }
                else
                {
                    Parent.IsChecked = null;
                }
            }
            else
            {
                Parent.IsChecked = false;
            }
        }

        public ObservableCollection<TreeViewItem> Items { get; set; } = new ObservableCollection<TreeViewItem>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RightsTreeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> _treeRoot;

        public ObservableCollection<TreeViewItem> TreeRoot
        {
            get => _treeRoot;
            set
            {
                if (_treeRoot == value)
                {
                    return;
                }

                _treeRoot = value;
                OnPropertyChanged();
            }
        }

        private ItemRights _rights = new ItemRights();

        public void Initialize()
        {
            TreeRoot = new ObservableCollection<TreeViewItem>();
            foreach (var right in _rights.Rights)
            {
                var item = new TreeViewItem(right.TableDefinition.Description, right.AllowView, null);
                TreeRoot.Add(item);

                var viewItem = new TreeViewItem("Allow View", right.AllowView, item);
                viewItem.CheckChanged += (sender, args) => right.AllowView = viewItem.IsChecked.Value;
                right.AllowViewChanged += (sender, args) => viewItem.IsChecked = right.AllowView;
                item.Items.Add(viewItem);

                var editItem = new TreeViewItem("Allow Edit", right.AllowEdit, item);
                editItem.CheckChanged += (sender, args) => right.AllowEdit = editItem.IsChecked.Value;
                right.AllowEditChanged += (sender, args) => editItem.IsChecked = right.AllowEdit;
                item.Items.Add(editItem);

                var addItem = new TreeViewItem("Allow Add", right.AllowAdd, item);
                addItem.CheckChanged += (sender, args) => right.AllowAdd = addItem.IsChecked.Value;
                right.AllowAddChanged += (sender, args) => addItem.IsChecked = right.AllowAdd;
                item.Items.Add(addItem);

                var deleteItem = new TreeViewItem("Allow Delete", right.AllowDelete, item);
                deleteItem.CheckChanged += (sender, args) => right.AllowDelete = deleteItem.IsChecked.Value;
                right.AllowDeleteChanged += (sender, args) => deleteItem.IsChecked = right.AllowDelete;
                item.Items.Add(deleteItem);
            }
            //var item = new TreeViewItem
            //{
            //    Text = "First",
            //    IsChecked = false
            //};
            //item.Items = new ObservableCollection<TreeViewItem>();
            //item.Items.Add(new TreeViewItem(text: "Testing 1", isChecked: false, parent: item));
            //var newItem = new TreeViewItem(text: "Testing 2", isChecked: true, parent: item);
            //item.Items.Add(newItem);
            //newItem.CheckParent();

            //newItem.Items.Add(new TreeViewItem(text: "Testing 1", isChecked: false, parent: newItem));
            //var secondNew = new TreeViewItem(text: "Testing 1", isChecked: true, parent: newItem);
            //newItem.Items.Add(secondNew);
            //secondNew.CheckParent();
            
            //TreeRoot.Add(item);
            //TreeRoot.Add(new TreeViewItem
            //{
            //    Text = "Second",
            //    IsChecked = false
            //});
            //TreeRoot.Add(new TreeViewItem
            //{
            //    Text = "Third",
            //    IsChecked = true
            //});
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
