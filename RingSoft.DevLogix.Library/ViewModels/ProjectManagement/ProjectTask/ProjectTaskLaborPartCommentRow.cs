using System;
using System.Collections.Generic;
using System.Linq;
using MySqlX.XDevAPI.Common;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskLaborPartCommentRow : ProjectTaskLaborPartRow
    {
        public override LaborPartLineTypes LaborPartLineType => LaborPartLineTypes.Comment;
        public override double GetExtendedMinutesCost()
        {
            return 0;
        }

        public override bool AllowUserDelete => Value != null;

        public string Comment { get; private set; }

        public bool CommentCrLf { get; private set; }

        public DataEntryGridMemoValue Value { get; private set; }

        public const int MaxCharactersPerLine = 40;

        public ProjectTaskLaborPartCommentRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
            DisplayStyleId = ProjectTaskLaborPartsManager.CommentRowDisplayStyleId;
        }

        public void SetComment(string comment)
        {
            SetValue(comment);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            DataEntryGridCellProps result = null;

            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    if (Value == null)
                    {
                        result = new DataEntryGridTextCellProps(this, columnId);
                    }
                    break;
                case ProjectTaskLaborPartColumns.LaborPart:
                    if (Value == null)
                        result = new DataEntryGridTextCellProps(this, columnId, Comment);
                    else
                        result = new DataEntryGridButtonCellProps(this, columnId) { Text = Comment };
                    break;
                default:
                    result = new DataEntryGridTextCellProps(this, columnId);
                    break;
            }
            if (result != null)
                return result;

            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;

            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    if (Value == null)
                        return new DataEntryGridControlCellStyle()
                        {
                            IsVisible = false,
                            State = DataEntryGridCellStates.Disabled
                        };
                    break;
                case ProjectTaskLaborPartColumns.LaborPart:
                    if (Value == null)
                    {
                        return new DataEntryGridCellStyle()
                        {
                            ColumnHeader = "Comment",
                            State = DataEntryGridCellStates.ReadOnly
                        };
                    }
                    else
                    {
                        return new DataEntryGridButtonCellStyle
                        {
                            ColumnHeader = "Comment",
                            Content = "Edit Comment..."
                        };
                    }
                    break;
                case ProjectTaskLaborPartColumns.Complete:
                    return new DataEntryGridControlCellStyle
                    {
                        State = DataEntryGridCellStates.ReadOnly,
                        IsVisible = false,
                    };
                default:
                    return new DataEntryGridCellStyle() { State = DataEntryGridCellStates.ReadOnly };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectTaskLaborPartColumns)value.ColumnId;

            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    if (Manager.ViewModel.View.ShowCommentEditor(Value))
                    {
                        UpdateFromValue();
                    }
                    else
                    {
                        value.OverrideCellMovement = true;
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public void SetValue(string text)
        {
            if (Value == null)
                Value = new DataEntryGridMemoValue(MaxCharactersPerLine);

            Value.Text = text;

            UpdateFromValue();
        }

        public void SetValue(DataEntryGridMemoValue value)
        {
            Value = value;
            UpdateFromValue();
        }

        private void UpdateFromValue(bool loading = false)
        {
            DeleteDescendants();
            var firstLine = true;
            foreach (var gridMemoValueLine in Value.Lines)
            {
                if (firstLine)
                {
                    Comment = gridMemoValueLine.Text;
                    CommentCrLf = gridMemoValueLine.CrLf;
                    Manager.Grid?.UpdateRow(this);
                    firstLine = false;
                }
                else
                {
                    var childCommentRow = new ProjectTaskLaborPartCommentRow(Manager)
                    {
                        Comment = gridMemoValueLine.Text,
                        CommentCrLf = gridMemoValueLine.CrLf
                    };
                    AddChildRow(childCommentRow);
                }
            }
        }

        public override void AddContextMenuItems(List<DataEntryGridContextMenuItem> contextMenuItems, int columnId)
        {
            contextMenuItems.Add(new DataEntryGridContextMenuItem("_Edit Comment",
                    new RelayCommand<int>(h => EditComment(columnId)))
                { CommandParameter = columnId });
            base.AddContextMenuItems(contextMenuItems, columnId);
        }

        private void EditComment(int columnId)
        {
            var parentRow = this;
            if (!string.IsNullOrEmpty(ParentRowId))
            {
                var gridRow = GetParentRow();
                if (gridRow is ProjectTaskLaborPartCommentRow parentCommentRow)
                    parentRow = parentCommentRow;
            }

            if (parentRow == this)
            {
                if (Manager.ViewModel.View.ShowCommentEditor(Value))
                {
                    UpdateFromValue();
                    Manager.Grid?.GotoCell(this, columnId);
                }
            }
            else
            {
                parentRow.EditComment(columnId);
            }
        }



        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            if (entity.ParentRowId.IsNullOrEmpty())
            {
                LoadChildren(entity);
            }
            base.LoadFromEntity(entity);
        }

        public override void LoadChildren(ProjectTaskLaborPart entity)
        {
            var gridMemoValue = new DataEntryGridMemoValue(MaxCharactersPerLine);
            gridMemoValue.AddLine(entity.Description, entity.CommentCrLf.Value);

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                gridMemoValue.AddLine(child.Description, child.CommentCrLf.Value);
            }

            SetValue(gridMemoValue);

            base.LoadChildren(entity);
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            entity.Description = Comment;
            entity.CommentCrLf = CommentCrLf;

            base.SaveToEntity(entity, rowIndex);
        }
    }
}
