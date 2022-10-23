using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library
{
    public enum RightTypes
    {
        AllowView = 0,
        AllowAdd = 1,
        AllowEdit = 2,
        AllowDelete = 3,
    }

    public enum ItemRightTypes
    {
        User = 0,
        Group = 1,
    }

    public class Right : INotifyPropertyChanged
    {
        public TableDefinitionBase TableDefinition { get; set; }

        private bool _allowDelete;

        public bool AllowDelete
        {
            get => _allowDelete;
            set
            {
                if (_allowDelete == value)
                    return;

                _allowDelete = value;
                OnPropertyChanged();
                AllowDeleteChanged?.Invoke(this, EventArgs.Empty);
                if (AllowDelete)
                {
                    AllowAdd = AllowDelete;
                }
            }
        }

        private bool _allowAdd;

        public bool AllowAdd
        {
            get => _allowAdd;
            set
            {
                if (_allowAdd == value)
                {
                    return;
                }
                _allowAdd = value;
                OnPropertyChanged();
                AllowAddChanged?.Invoke(this, EventArgs.Empty);
                if (AllowAdd)
                {
                    AllowEdit = AllowAdd;
                }
                else
                {
                    AllowDelete = AllowAdd;
                }
            }
        }
        private bool _allowEdit;

        public bool AllowEdit
        {
            get => _allowEdit;
            set
            {
                if (AllowEdit == value)
                {
                    return;
                }
                _allowEdit = value;
                OnPropertyChanged();
                AllowEditChanged?.Invoke(this, EventArgs.Empty);
                if (AllowEdit)
                {
                    AllowView = AllowEdit;
                }
                else
                {
                    AllowAdd = AllowEdit;
                }
            }
        }

        private bool _allowView;

        public bool AllowView
        {
            get => _allowView;
            set
            {
                if (_allowView == value)
                {
                    return;
                }
                _allowView = value;
                OnPropertyChanged();
                AllowViewChanged?.Invoke(this, EventArgs.Empty);
                if (!AllowView)
                {
                    AllowEdit = AllowView;
                }
            }
        }

        public event EventHandler AllowViewChanged;
        public event EventHandler AllowEditChanged;
        public event EventHandler AllowAddChanged;
        public event EventHandler AllowDeleteChanged;

        public Right()
        {
            AllowDelete = true;
        }
        public override string ToString()
        {
            return TableDefinition.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RightCategory
    {
        public string Category { get; set; }

        public List<RightCategoryItem> Items { get; private set; } = new List<RightCategoryItem>();

        public RightCategory(string name)
        {
            Category = name;
        }
    }

    public class RightCategoryItem
    {
        public RightCategoryItem(string item, TableDefinitionBase tableDefinition)
        {
            Description = item;
            TableDefinition = tableDefinition;
        }

        public string Description { get; set; }

        public TableDefinitionBase TableDefinition { get; set; }
    }

    public class ItemRights
    {
        public ObservableCollection<Right> Rights { get; set; }

        public List<RightCategory> Categories { get; set; } = new List<RightCategory>();

        public ItemRights()
        {
            Categories = new List<RightCategory>();

            var category = new RightCategory("User Management");
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Users", AppGlobals.LookupContext.Users));
            Categories.Add(category);

            category = new RightCategory("Miscellaneous");
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Advanced Finds", AppGlobals.LookupContext.AdvancedFinds));
            Categories.Add(category);

            Rights = new ObservableCollection<Right>();
            var tables = AppGlobals.LookupContext.TableDefinitions
                .OrderByDescending(p => p.IsAdvancedFind).ToList();
            foreach (var tableDefinition in tables)
            {
                if (tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition == null)
                {
                    Rights.Add(new Right
                    {
                        TableDefinition = tableDefinition
                    });
                }
            }
        }

        public Right GetRight(TableDefinitionBase tableDefinition)
        {
            var right = Rights.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return right;
        }

        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            var right = GetRight(tableDefinition);
            switch (rightType)
            {
                case RightTypes.AllowView:
                    return right.AllowView;
                case RightTypes.AllowAdd:
                    return right.AllowAdd;
                case RightTypes.AllowEdit:
                    return right.AllowEdit;
                case RightTypes.AllowDelete:
                    return right.AllowDelete;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rightType), rightType, null);
            }
        }
    }

    public class AppRights
    {
        public ItemRights UserRights { get; set; }

        public List<ItemRights> GroupRights { get; private set; }

        public AppRights()
        {
            UserRights = new ItemRights();

            GroupRights = new List<ItemRights>();
        }

        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return UserRights.HasRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasRight(tableDefinition, rightType));
        }
    }
}
