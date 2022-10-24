using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
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
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Groups", AppGlobals.LookupContext.Groups));
            Categories.Add(category);

            category = new RightCategory("Miscellaneous");
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Advanced Finds", AppGlobals.LookupContext.AdvancedFinds));
            Categories.Add(category);

            Initialize();
        }

        private void Initialize()
        {
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

        public void Reset()
        {
            foreach (var right in Rights)
            {
                right.AllowDelete = true;
            }
        }

        public Right GetRight(TableDefinitionBase tableDefinition)
        {
            var right = Rights.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            return right;
        }

        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            if (tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition != null)
            {
                tableDefinition = tableDefinition.PrimaryKeyFields[0].ParentJoinForeignKeyDefinition.PrimaryTable;
            }
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

        public string GetRightsString()
        {
            var result = string.Empty;

            foreach (var right in Rights)
            {
                var rightBit = "0";
                if (right.AllowDelete)
                {
                    rightBit = "1";
                }
                result += rightBit;

                rightBit = "0";
                if (right.AllowAdd)
                {
                    rightBit = "1";
                }
                result += rightBit;

                rightBit = "0";
                if (right.AllowEdit)
                {
                    rightBit = "1";
                }
                result += rightBit;

                rightBit = "0";
                if (right.AllowView)
                {
                    rightBit = "1";
                }
                result += rightBit;

            }

            return result;
        }

        public void LoadRights(string rightsString)
        {
            Reset();
            if (rightsString.IsNullOrEmpty())
            {
                return;
            }
            foreach (var right in Rights)
            {
                var rightIndex = Rights.IndexOf(right);
                var rightStringIndex = 0;
                if (rightIndex > 0)
                {
                    rightStringIndex = rightIndex * 4;
                }

                if (rightStringIndex > rightsString.Length - 1)
                {
                    return;
                }
                var counter = 0;
                while (counter < 4)
                {
                    var rightBit = rightsString[rightStringIndex].ToString();
                    if (counter == 0)
                    {
                        right.AllowDelete = rightBit.ToBool();
                    }
                    else if (counter == 1)
                    {
                        right.AllowAdd = rightBit.ToBool();
                    }
                    else if (counter == 2)
                    {
                        right.AllowEdit = rightBit.ToBool();
                    }
                    else if (counter == 3)
                    {
                        right.AllowView = rightBit.ToBool();
                    }

                    counter++;
                    rightStringIndex++;
                }
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
