﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DevLogix.Library
{
    public interface IRightsTreeControl
    {
        void SetDataChanged();
    }
    public class TreeViewItem : INotifyPropertyChanged
    {
        public TreeViewItem Parent { get; set; }

        public string Text { get; set; }

        public bool SettingCheck { get; private set; }

        public RightTypes RightType { get; set; }

        public TableDefinitionBase TableDefinition { get; set; }

        public SpecialRight SpecialRight { get; set; }

        public override string ToString()
        {
            return Text;
        }

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

                RightsTreeViewModel?.SetDataChanged();
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
                    }
                    if (Parent != null && !Parent.SettingCheck)
                    {
                        CheckParent();
                    }
                    SettingCheck = false;
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

        private bool _allowsEdit;

        public bool AllowsEdit
        {
            get => _allowsEdit;
            set
            {
                if (_allowsEdit == value)
                {
                    return;
                }
                _allowsEdit = value;
                OnPropertyChanged();
            }
        }

        public RightsTreeViewModel RightsTreeViewModel { get; }

        public event EventHandler CheckChanged;

        public TreeViewItem(string text, bool? isChecked, TreeViewItem parent, RightsTreeViewModel rightsTreeViewModel)
        {
            Parent = parent;
            Text = text;
            IsChecked = isChecked;
            RightsTreeViewModel = rightsTreeViewModel;
        }

        public void CheckParent()
        {
            var checkedItems = Parent.Items.Where(p => p.IsChecked == true
                                                       || p.ThreeState == true);

            var threeStateItems = Parent.Items.Where(p => p.ThreeState == true);

            var anyChecked = checkedItems.Any();
            if (anyChecked)
            {
                if (checkedItems.Count() == Parent.Items.Count)
                {
                    if (threeStateItems.Any())
                    {
                        Parent.IsChecked = null;
                    }
                    else
                    {
                        Parent.IsChecked = true;
                    }
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

        public TreeViewItem()
        {
            AllowsEdit = true;
        }

        public void SetReadOnlyMode(bool readOnlyMode)
        {
            AllowsEdit = !readOnlyMode;
            foreach (var treeViewItem in Items)
            {
                treeViewItem.SetReadOnlyMode(readOnlyMode);
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

        private List<TreeViewItem> _rightsItems = new List<TreeViewItem>();
        private bool? _readOnlyMode;
        private bool _initialized;
        private string _loadedRights;
        private bool _rightsLoaded;

        public ItemRights Rights { get; } = new DevLogixRights();

        public IRightsTreeControl Control { get; private set; }

        public void Initialize(IRightsTreeControl control)
        {
            _rightsLoaded = false;
            Control = control;
            TreeRoot = new ObservableCollection<TreeViewItem>();
            foreach (var category in Rights.Categories)
            {
                var categoryItem = new TreeViewItem(category.Category, false, null, this);
                TreeRoot.Add(categoryItem);

                foreach (var rightCategoryItem in category.Items)
                {
                    var right = Rights.GetRight(rightCategoryItem.TableDefinition);
                    var item = new TreeViewItem(rightCategoryItem.Description, false, categoryItem, this);
                    categoryItem.Items.Add(item);

                    var viewItem = new TreeViewItem("Allow View", right.AllowView, item, this)
                        {RightType = RightTypes.AllowView, TableDefinition = right.TableDefinition};
                    viewItem.CheckChanged += (sender, args) => right.AllowView = viewItem.IsChecked.Value;
                    right.AllowViewChanged += (sender, args) => viewItem.IsChecked = right.AllowView;
                    _rightsItems.Add(viewItem);
                    item.Items.Add(viewItem);

                    foreach (var rightSpecialRight in right.SpecialRights)
                    {
                        var specialRightItem = new TreeViewItem(rightSpecialRight.Description,
                            rightSpecialRight.HasRight, item, this);
                        specialRightItem.SpecialRight = rightSpecialRight;
                        right.AllowViewChanged += (sender, args) =>
                        {
                            if (!right.AllowView)
                            {
                                specialRightItem.IsChecked = right.AllowView;
                            }
                        };
                        right.AllowEditChanged += (sender, args) =>
                        {
                            specialRightItem.IsChecked = right.AllowEdit;
                        };
                        specialRightItem.CheckChanged += (sender, args) =>
                        {
                            specialRightItem.SpecialRight.HasRight = specialRightItem.IsChecked.Value;
                            if (specialRightItem.IsChecked.Value)
                            {
                                right.AllowView = true;
                            }
                        };
                        _rightsItems.Add(specialRightItem);
                        item.Items.Add(specialRightItem);
                    }

                    var editItem = new TreeViewItem("Allow Edit", right.AllowEdit, item, this)
                        {RightType = RightTypes.AllowEdit, TableDefinition = right.TableDefinition};
                    editItem.CheckChanged += (sender, args) => right.AllowEdit = editItem.IsChecked.Value;
                    right.AllowEditChanged += (sender, args) => editItem.IsChecked = right.AllowEdit;
                    _rightsItems.Add(editItem);
                    item.Items.Add(editItem);

                    var addItem = new TreeViewItem("Allow Add", right.AllowAdd, item, this) 
                        {RightType = RightTypes.AllowAdd, TableDefinition = right.TableDefinition};
                    addItem.CheckChanged += (sender, args) => right.AllowAdd = addItem.IsChecked.Value;
                    right.AllowAddChanged += (sender, args) => addItem.IsChecked = right.AllowAdd;
                    _rightsItems.Add(addItem);
                    item.Items.Add(addItem);

                    var deleteItem = new TreeViewItem("Allow Delete", right.AllowDelete, item, this)
                        {RightType = RightTypes.AllowDelete, TableDefinition = right.TableDefinition};
                    deleteItem.CheckChanged += (sender, args) => right.AllowDelete = deleteItem.IsChecked.Value;
                    right.AllowDeleteChanged += (sender, args) => deleteItem.IsChecked = right.AllowDelete;
                    _rightsItems.Add(deleteItem);
                    item.Items.Add(deleteItem);
                }
            }
            if (_readOnlyMode != null)
            {
                SetReadOnlyMode(_readOnlyMode.Value);
            }
            _initialized = true;
            if (!_loadedRights.IsNullOrEmpty())
            {
                LoadRights(_loadedRights);
            }

            _rightsLoaded = true;
        }

        public void SetReadOnlyMode(bool readOnlyValue = true)
        {
            if (TreeRoot == null)
            {
                _readOnlyMode = readOnlyValue;
                return;
            }
            foreach (var treeViewItem in TreeRoot)
            {
                treeViewItem.SetReadOnlyMode(readOnlyValue);
            }
        }

        public void Reset()
        {
            Rights.Reset();
            foreach (var rightsRight in Rights.Rights)
            {
                foreach (var specialRight in rightsRight.SpecialRights)
                {
                    var tableSpecialRight = _rightsItems.FirstOrDefault(p =>
                        p.SpecialRight == specialRight);
                    if (tableSpecialRight != null) tableSpecialRight.IsChecked = specialRight.HasRight;
                }
            }
        }

        public void LoadRights(string rightsString)
        {
            if (!_initialized)
            {
                _loadedRights = rightsString;
                return;
            }
            _rightsLoaded = false;
            Rights.LoadRights(rightsString);
            foreach (var rightsRight in Rights.Rights)
            {
                var tableRights = _rightsItems.Where(p => p.TableDefinition == rightsRight.TableDefinition);
                foreach (var tableRight in tableRights)
                {
                    switch (tableRight.RightType)
                    {
                        case RightTypes.AllowView:
                            tableRight.IsChecked = rightsRight.AllowView;
                            break;
                        case RightTypes.AllowAdd:
                            tableRight.IsChecked = rightsRight.AllowAdd;
                            break;
                        case RightTypes.AllowEdit:
                            tableRight.IsChecked = rightsRight.AllowEdit;
                            break;
                        case RightTypes.AllowDelete:
                            tableRight.IsChecked = rightsRight.AllowDelete;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var specialRight in rightsRight.SpecialRights)
                    {
                        var tableSpecialRight = _rightsItems.FirstOrDefault(p =>
                            p.SpecialRight == specialRight);
                        if (tableSpecialRight != null) tableSpecialRight.IsChecked = specialRight.HasRight;
                    }
                }
            }
            _rightsLoaded = true;
        }

        public void SetDataChanged()
        {
            if (_rightsLoaded)
            {
                Control.SetDataChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
