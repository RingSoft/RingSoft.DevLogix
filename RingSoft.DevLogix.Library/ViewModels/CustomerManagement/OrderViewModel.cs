﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.Printing.Interop;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class OrderViewModel : DevLogixDbMaintenanceViewModel<Order>
    {
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _customerAutoFillSetup;

        public AutoFillSetup CustomerAutoFillSetup
        {
            get => _customerAutoFillSetup;
            set
            {
                if (_customerAutoFillSetup == value)
                    return;

                _customerAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _customerAutoFillValue;

        public AutoFillValue CustomerAutoFillValue
        {
            get => _customerAutoFillValue;
            set
            {
                if (_customerAutoFillValue == value)
                    return;

                _customerAutoFillValue = value;
                LoadCustomer();
                OnPropertyChanged();
            }
        }

        private DateTime _orderDate;

        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                if (_orderDate == value)
                {
                    return;
                }
                _orderDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _shippedDate;

        public DateTime? ShippedDate
        {
            get => _shippedDate;
            set
            {
                if (_shippedDate == value)
                {
                    return;
                }
                _shippedDate = value;
                OnPropertyChanged();
            }
        }

        private string _companyName;

        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName == value)
                    return;

                _companyName = value;
                OnPropertyChanged();
            }
        }

        private string? _contactName;

        public string? ContactName
        {
            get => _contactName;
            set
            {
                if (_contactName == value)
                    return;

                _contactName = value;
                OnPropertyChanged();
            }
        }

        private string? _contactTitle;

        public string? ContactTitle
        {
            get => _contactTitle;
            set
            {
                if (_contactTitle == value)
                    return;

                _contactTitle = value;
                OnPropertyChanged();
            }
        }

        private string? _address;

        public string? Address
        {
            get => _address;
            set
            {
                if (_address == value)
                {
                    return;
                }
                _address = value;
                OnPropertyChanged();
            }
        }

        private string? _city;

        public string? City
        {
            get => _city;
            set
            {
                if (_city == value)
                    return;

                _city = value;
                OnPropertyChanged();
            }
        }

        private string? _region;

        public string? Region
        {
            get => _region;
            set
            {
                if (_region == value)
                    return;

                _region = value;
                OnPropertyChanged();
            }
        }

        private string? _postalCode;

        public string? PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode == value)
                    return;

                _postalCode = value;
                OnPropertyChanged();
            }
        }

        private string? _country;

        public string? Country
        {
            get => _country;
            set
            {
                if (_country == value)
                    return;

                _country = value;
                OnPropertyChanged();
            }
        }

        private OrderDetailsManager _detailsManager;

        public OrderDetailsManager DetailsManager
        {
            get => _detailsManager;
            set
            {
                if (_detailsManager == value)
                    return;

                _detailsManager = value;
                OnPropertyChanged();
            }
        }


        private double _subTotal;

        public double SubTotal
        {
            get => _subTotal;
            set
            {
                if (_subTotal == value)
                    return;

                _subTotal = value;
                OnPropertyChanged();
            }
        }

        private double _freight;

        public double Freight
        {
            get => _freight;
            set
            {
                if (_freight == value)
                    return;

                _freight = value;
                if (!_loading)
                {
                    DetailsManager.CalculateTotals();
                }
                OnPropertyChanged();
            }
        }

        private double _totalDiscount;

        public double TotalDiscount
        {
            get => _totalDiscount;
            set
            {
                if (_totalDiscount == value)
                    return;

                _totalDiscount = value;
                OnPropertyChanged();
            }
        }

        private double _total;

        public double Total
        {
            get => _total;
            set
            {
                if (_total == value)
                    return;

                _total = value;
                OnPropertyChanged();
            }
        }

        public AutoFillValue DefaultCustomerAutoFillValue { get; private set; }

        private bool _loading;

        public OrderViewModel()
        {
            CustomerAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.CustomerId));

            TablesToDelete.Add(AppGlobals.LookupContext.OrderDetail);
            DetailsManager = new OrderDetailsManager(this);
            PrintProcessingHeader += OrderViewModel_PrintProcessingHeader;
        }

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Customer)
                {
                    var customer =
                        AppGlobals.LookupContext.Customer.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);

                    var context = AppGlobals.DataRepository.GetDataContext();
                    var table = context.GetTable<Customer>();
                    customer = table.FirstOrDefault(p => p.Id == customer.Id);
                    DefaultCustomerAutoFillValue = customer.GetAutoFillValue();
                }
            }

            base.Initialize();
        }

        protected override Order PopulatePrimaryKeyControls(Order newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var order = GetOrder(newEntity.Id);

            Id = newEntity.Id;
            KeyAutoFillValue = order.GetAutoFillValue();

            return order;
        }

        private static Order? GetOrder(int orderId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Order>();

            var order = table
                .Include(p => p.Customer)
                .Include(p => p.Details)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(p => p.Id == orderId);
            return order;
        }

        protected override void LoadFromEntity(Order entity)
        {
            _loading = true;
            CustomerAutoFillValue = entity.Customer.GetAutoFillValue();
            OrderDate = entity.OrderDate.ToLocalTime();
            if (entity.ShippedDate != null)
            {
                ShippedDate = entity.ShippedDate.GetValueOrDefault().ToLocalTime();
            }
            else
            {
                ShippedDate = null;
            }

            CompanyName = entity.CompanyName;
            ContactName = entity.ContactName;
            ContactTitle = entity.ContactTitle;
            Address = entity.Address;
            City = entity.City;
            Region = entity.Region;
            PostalCode = entity.PostalCode;
            Country = entity.Country;
            SubTotal = entity.SubTotal.GetValueOrDefault();
            Freight = entity.Freight.GetValueOrDefault();
            TotalDiscount = entity.TotalDiscount.GetValueOrDefault();
            Total = entity.Total.GetValueOrDefault();
            DetailsManager.LoadGrid(entity.Details);
            _loading = false;
        }

        protected override Order GetEntityData()
        {
            var result = new Order
            {
                Id = Id,
                CustomerId = CustomerAutoFillValue.GetEntity<Customer>().Id,
                OrderDate = OrderDate.ToUniversalTime(),
                ShippedDate = ShippedDate.GetValueOrDefault().ToUniversalTime(),
                CompanyName = CompanyName,
                ContactName = ContactName,
                ContactTitle = ContactTitle,
                Address = Address,
                City = City,
                Region = Region,
                PostalCode = PostalCode,
                Country = Country,
                SubTotal = SubTotal,
                Freight = Freight,
                TotalDiscount = TotalDiscount,
                Total = Total,
            };
            if (ShippedDate == null)
            {
                result.ShippedDate = null;
            }

            if (KeyAutoFillValue == null || !KeyAutoFillValue.IsValid())
            {
                if (result.Id == 0)
                {
                    result.OrderId = Guid.NewGuid().ToString();
                    KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(TableDefinition), result.OrderId);
                }
            }
            else
            {
                result.OrderId = KeyAutoFillValue.Text;
            }

            return result;
        }

        protected override bool ValidateEntity(Order entity)
        {
            var result = base.ValidateEntity(entity);
            if (result && !DetailsManager.ValidateGrid())
            {
                result = false;
            }

            if (!result && MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                KeyAutoFillValue = null;
            }
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            OrderDate = DateTime.Now;
            ShippedDate = DateTime.Now;
            CompanyName = string.Empty;
            ContactName = string.Empty;
            ContactTitle = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            Region = string.Empty;
            PostalCode = string.Empty;
            Country = string.Empty;
            SubTotal = 0;
            Freight = 0;
            TotalDiscount = 0;
            Total = 0;
            DetailsManager.SetupForNewRecord();
            CustomerAutoFillValue = DefaultCustomerAutoFillValue;
        }

        protected override bool SaveEntity(Order entity)
        {
            var makeOrderId = entity.Id == 0;
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Order");
            if (result && makeOrderId)
            {
                entity.OrderId = $"O-{entity.Id}";
                result = context.SaveEntity(entity, "Updating Order ID");
            }

            if (result)
            {
                var newDetails = DetailsManager.GetEntityList();
                foreach (var orderDetail in newDetails)
                {
                    orderDetail.OrderId = entity.Id;
                }

                var detailsTable = context.GetTable<OrderDetail>();
                var existingDetails = detailsTable
                    .Where(p => p.OrderId == Id).ToList();
                context.RemoveRange(existingDetails);
                context.AddRange(newDetails);
                result = context.Commit("Updating Details");
            }
            return result;
        }

        protected override bool DeleteEntity()
        {
            var result = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Order>();
            var order = table.FirstOrDefault(p => p.Id == Id);
            if (order != null)
            {
                var detailsTable = context.GetTable<OrderDetail>();
                var existingDetails = detailsTable
                    .Where(p => p.OrderId == Id).ToList();
                context.RemoveRange(existingDetails);

                result = context.DeleteEntity(order, "Deleting Order");
            }
            return result;
        }

        void LoadCustomer()
        {
            if (_loading)
            {
                return;
            }
            var customer = CustomerAutoFillValue.GetEntity<Customer>();
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            customer = table.FirstOrDefault(p => p.Id == customer.Id);
            if (customer != null)
            {
                CompanyName = customer.CompanyName;
                ContactName = customer.ContactName;
                ContactTitle = customer.ContactTitle;
                Address = customer.Address;
                City = customer.City;
                Region = customer.Region;
                PostalCode = customer.PostalCode;
                Country = customer.Country;
            }
        }

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, int numericFieldIndex = 1,
            int memoFieldIndex = 1)
        {
            printerSetupArgs.PrintingProperties.ReportType = ReportTypes.Custom;
            printerSetupArgs.PrintingProperties.CustomReportPathFileName =
                $"{RingSoftAppGlobals.AssemblyDirectory}\\CustomerManagement\\Invoice.rpt";

            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
            printerSetupArgs.CodeDescription = "Invoice";
            printerSetupArgs.PrintingProperties.ReportTitle = "Invoice";
        }

        public override void ProcessPrintOutputData(PrinterSetupArgs printerSetupArgs)
        {
            base.ProcessPrintOutputData(printerSetupArgs);
            var customProperties = new List<PrintingCustomProperty>();
            customProperties.Add(new PrintingCustomProperty
            {
                Name = "lblOrderDate",
                Value = "Order Date",
            });

            var dateSetup = new DateEditControlSetup
            {
                DateFormatType = DateFormatTypes.DateTime,
            };
            customProperties.Add(new PrintingCustomProperty
            {
                Name = "strDate",
                Value = dateSetup.FormatValueForDisplay(DateTime.Now),
            });

            customProperties.Add(new PrintingCustomProperty
            {
                Name = "lblInvoiceDate",
                Value = "Invoice Date",
            });

            customProperties.Add(new PrintingCustomProperty
            {
                Name = "lblCompany",
                Value = AppGlobals.LoggedInOrganization.Name,
            });

            customProperties.Add(new PrintingCustomProperty
            {
                Name = "lblTo",
                Value = "To",
            });

            customProperties.Add(new PrintingCustomProperty
            {
                Name = "lblSubTotal",
                Value = "Sub Total",
            });

            customProperties.Add(new PrintingCustomProperty
            {
                Name = "lblTotal",
                Value = "Total",
            });

            PrintingInteropGlobals.PropertiesProcessor.CustomProperties = customProperties;
        }

        private void OrderViewModel_PrintProcessingHeader(object? sender, PrinterDataProcessedEventArgs e)
        {
            var dateSetup = new DateEditControlSetup
            {
                DateFormatType = DateFormatTypes.DateTime,
            };

            var qtySetup = new DecimalEditControlSetup()
            {
                FormatType = DecimalEditFormatTypes.Number,
            };

            var currencySetup = new DecimalEditControlSetup()
            {
                FormatType = DecimalEditFormatTypes.Currency,
            };

            var primaryKey = new PrimaryKeyValue(TableDefinition);
            primaryKey.PopulateFromDataRow(e.OutputRow);

            var order = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKey);
            order = GetOrder(order.Id);
            if (order != null)
            {
                e.HeaderRow.StringField01 = dateSetup.FormatValueForDisplay(order.OrderDate.ToLocalTime());
                e.HeaderRow.StringField02 = order.CompanyName;
                e.HeaderRow.StringField03 = order.Address;
                e.HeaderRow.StringField04 = order.City;
                e.HeaderRow.StringField05 = order.Region;
                e.HeaderRow.StringField06 = order.PostalCode;
                e.HeaderRow.StringField07 = order.Country;
                e.HeaderRow.NumberField01 = currencySetup.FormatValue((decimal)order.SubTotal);
                e.HeaderRow.NumberField02 = currencySetup.FormatValue((decimal)order.Total);
                var detailsChunk = new List<PrintingInputDetailsRow>();
                foreach (var detail in order.Details)
                {
                    var detailRow = new PrintingInputDetailsRow();
                    detailRow.HeaderRowKey = e.HeaderRow.RowKey;
                    detailRow.TablelId = 1;
                    detailRow.StringField01 = detail.Product.Description;
                    detailRow.NumberField01 = qtySetup.FormatValue((decimal)detail.Quantity);
                    detailRow.NumberField02 = currencySetup.FormatValue((decimal)detail.ExtendedPrice);
                    detailsChunk.Add(detailRow);
                }

                PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);

                detailsChunk.Clear();
            }
        }
    }
}
