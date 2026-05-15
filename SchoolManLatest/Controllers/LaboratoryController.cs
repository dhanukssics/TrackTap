
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using TrackTap.Helper;
using TrackTap.MapModel;
using TrackTap.Models;
using TrackTap.PostModel;
using TrackTap.Repository;
using TrackTap.Service.Helper;
using TrackTap.Utility;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;
using IMailService = TrackTap.Repository.IMailService;



namespace TrackTap.Controllers
{
    public class LaboratoryController : BaseController
    {
        private readonly SchoolDbContext _Entities;
        private readonly IWebHostEnvironment _environment;
        private readonly HttpClient _httpClient;

        private readonly Repository.IMailService _mailService;
        public LaboratoryController(SchoolDbContext Entities, SchoolRepository schoolRepository, ParentRepository parentRepository, TeacherRepository teacherRepository,IMailService mailService,IWebHostEnvironment environment, HttpClient httpClient) : base(Entities, schoolRepository, parentRepository, teacherRepository)
        {
            _Entities = Entities;
            _mailService = mailService;
            _environment = environment;
            _httpClient = httpClient;
        }

        // GET: Laboratory
        public IActionResult StockUpdate()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public IActionResult GetAllSupplierName()
        {
            long schoolId =
                _user.SchoolId;

            var result =
                new TrackTap.Data.WebsiteService()
                    .GetAllSupplierList(
                        schoolId);

            return Json(new
            {
                Status = true,

                Message = "",

                result = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitStockUpdate(StockUpdateModel model)
        {
            string message = "failed";

            bool status = false;

            try
            {
                var stockExist = await _Entities
                    .TbStockUpdates
                    .FirstOrDefaultAsync(x =>
                        x.Item == model.Item);

                // STOCK EXISTS
                if (stockExist != null)
                {
                    // Your original code was not updating anything.
                    // Keeping same logic for migration.

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        message = "success";
                    }

                    return Json(new
                    {
                        Status = status,

                        Message = message
                    });
                }

                
                var stock =
                    new TbStockUpdate
                    {
                        CategoryId = model.CategoryId,

                        Item = model.Item,

                        Price = model.Price,

                        PurchaseId = model.PurchaseId,

                        SupplierName = model.SupplirName,

                        StockStatus =
                            (int)model.Status,

                        SchoolId =
                            _user.SchoolId,

                        UserId =
                            _user.UserId,

                        IsActive = true,

                        TimeStamp =
                            CurrentTime
                    };

                

                var purchase =
                    new TbPurchase
                    {
                        CategoryId = model.CategoryId,

                        Item = model.Item,

                        Price = model.Price,

                        PurchaseId = model.PurchaseId,

                        SupplierName = model.SupplirName,

                        StockStatus =
                            (int)model.Status,

                        SchoolId =
                            _user.SchoolId,

                        UserId =
                            _user.UserId,

                        StockQuantity =
                            model.Quantity,

                        IsActive = true,

                        TimeStamp =
                            CurrentTime
                    };

                await _Entities.TbStockUpdates
                    .AddAsync(stock);

                await _Entities.TbPurchases
                    .AddAsync(purchase);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                if (status)
                {
                    message = "success";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                Status = status,

                Message = message
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteStock(long id)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                var stock = await _Entities
                    .TbStockUpdates
                    .FirstOrDefaultAsync(x =>
                        x.StockId == id);

                if (stock != null)
                {
                    stock.IsActive = false;

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    message = status
                        ? "Stock deleted successfully"
                        : "Failed to delete stock";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = message
            });
        }
        public async Task<IActionResult> EditStock(long id)
        {
            var data = await _Entities
                .TbStockUpdates
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == _user.SchoolId
                    && x.StockId == id
                    && x.IsActive);

            if (data == null)
            {
                return NotFound();
            }

            var model =
                new StockUpdateModel
                {
                    CategoryId = data.CategoryId,

                    Item = data.Item,

                    Price = data.Price,

                    PurchaseId = data.PurchaseId,

                    SupplirName = data.SupplierName,

                    Status = (StockStatus)data.StockStatus,

                    SchoolId = _user.SchoolId,

                    StockId = data.StockId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_Edit_StockUpdate.cshtml",
                model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitEditStockUpdate(StockUpdateModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var data = await _Entities
                    .TbStockUpdates
                    .FirstOrDefaultAsync(x =>
                        x.StockId == model.StockId
                        && x.SchoolId == _user.SchoolId
                        && x.IsActive);

                if (data != null)
                {
                    data.CategoryId =
                        model.CategoryId;

                    data.Item =
                        model.Item;

                    data.Price =
                        model.Price;

                    data.PurchaseId =
                        model.PurchaseId;

                    data.SupplierName =
                        model.SupplirName;

                    data.StockStatus =
                        (int)model.Status;

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    msg = status
                        ? "Successful"
                        : "No changes made";
                }
                else
                {
                    msg = "No such data!";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = msg
            });
        }
        public IActionResult LabInventoryReport()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult DatatableLabInventoryReport(long id)
        {
            var model =
                new StockUpdateModel
                {
                    SchoolId = _user.SchoolId,

                    CategoryId = id
                };

            return PartialView(
                "~/Views/Laboratory/_pv_LabInventoryReportList.cshtml",
                model);
        }


        //jibin 9/14/2020

        public IActionResult PurchaseReport()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }

        public IActionResult SalesReport()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }



        public IActionResult StoreList()
        {

            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Deleteitem(long id)
        {
            bool status = false;

            string msg = "False";

            try
            {
                var item = await _Entities
                    .TbAddCategories
                    .FirstOrDefaultAsync(x =>
                        x.Id == id);

                if (item != null)
                {
                    _Entities.TbAddCategories
                        .Remove(item);

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;
                }

                msg = status
                    ? "Deleted"
                    : "Failed";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = msg
            });
        }
        //Created by gayathri(08/11/2023)For bank edit option
        public async Task<IActionResult> EditItem(long id)
        {
            long category = 0;

            string item = "";

            string unit = "";

            var categoryItem = await _Entities
                .TbAddCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == id);

            if (categoryItem != null)
            {
                var categoryData = await _Entities
                    .TbLaboratoryCategories
                    .FirstOrDefaultAsync(x =>
                        x.CategoryId ==
                            categoryItem.CategoryId);

                category =
                    Convert.ToInt64(
                        categoryItem.CategoryId);

                item =
                    categoryItem.Item;

                unit =
                    categoryItem.Unit;
            }

            return Json(new
            {
                Category = category,

                item = item,

                unit = unit,

                bankId = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitStockAddNew(StockUpdateModel model)
        {
            bool status = false;

            string message = "failed";

            try
            {
                var addItem = await _Entities
                    .TbAddCategories
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.CatId);

                // ADD NEW ITEM
                if (addItem == null)
                {
                    var stock =
                        new TbAddCategory
                        {
                            CategoryId =
                                model.CategoryId,

                            Item =
                                model.Item,

                            Unit =
                                model.Unit,

                            SchoolId =
                                _user.SchoolId,

                            UserId =
                                _user.UserId,

                            TimeStamp =
                                CurrentTime
                        };

                    await _Entities
                        .TbAddCategories
                        .AddAsync(stock);
                }

                // UPDATE EXISTING ITEM
                else
                {
                    addItem.CategoryId =
                        model.CategoryId;

                    addItem.Item =
                        model.Item;

                    addItem.Unit =
                        model.Unit;

                    addItem.TimeStamp =
                        DateTime.Now;
                }

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                if (status)
                {
                    message = "success";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                Status = status,

                message = status
                    ? "Successfully Added"
                    : "Failed"
            });
        }
        [HttpPost]
        public async Task<IActionResult> EditItemById(string id)
        {
            bool status = false;

            string message = "failed";

            try
            {
                string[] splitData =
                    id.Split('~');

                long itemId =
                    Convert.ToInt64(
                        splitData[0]);

                string item =
                    Convert.ToString(
                        splitData[1]);

                string unit =
                    Convert.ToString(
                        splitData[2]);

                long categoryId =
                    Convert.ToInt64(
                        splitData[3]);

                var addItem = await _Entities
                    .TbAddCategories
                    .FirstOrDefaultAsync(x =>
                        x.Id == itemId);

                if (addItem != null)
                {
                    addItem.CategoryId =
                        categoryId;

                    addItem.Item =
                        item;

                    addItem.Unit =
                        unit;

                    addItem.TimeStamp =
                        DateTime.Now;

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        message = "success";
                    }
                }
                else
                {
                    message = "No such item found";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                Status = status,

                message = status
                    ? "Successfully Added"
                    : message
            });
        }
        public IActionResult PurchaseReportByDate(string id)
        {
            string[] splitData =
                id.Split('~');

            DateTime fromDate =
                Convert.ToDateTime(
                    splitData[0]);

            DateTime toDate =
                Convert.ToDateTime(
                    splitData[1]);

            string endDate =
                toDate.ToString("MM-dd-yyyy")
                + " 11:59:00 PM";

            var model =
                new StockUpdateModel
                {
                    StartDate =
                        fromDate,

                    EndDate =
                        Convert.ToDateTime(
                            endDate),

                    SchoolId =
                        _user.SchoolId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_PurchaseReport.cshtml",
                model);
        }

        public IActionResult SalesReportByDate(string id)
        {
            string[] splitData =
                id.Split('~');

            DateTime fromDate =
                Convert.ToDateTime(
                    splitData[0]);

            DateTime toDate =
                Convert.ToDateTime(
                    splitData[1]);

            string endDate =
                toDate.ToString("MM-dd-yyyy")
                + " 11:59:00 PM";

            var model =
                new StockUpdateModel
                {
                    StartDate =
                        fromDate,

                    EndDate =
                        Convert.ToDateTime(
                            endDate),

                    SchoolId =
                        _user.SchoolId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_SalesReport.cshtml",
                model);
        }

        public IActionResult AddCategory()
        {
            var model =
                new StockUpdateModel
                {
                    SchoolId = _user.SchoolId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_AddCategory_Model.cshtml",
                model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCategory(StockUpdateModel model)
        {
            try
            {
                var stock =
                    new TbLaboratoryCategory
                    {
                        LaboratoryName =
                            model.LaboratoryName,

                        SchoolId =
                            _user.SchoolId,

                        IsActive = true
                    };

                await _Entities
                    .TbLaboratoryCategories
                    .AddAsync(stock);

                await _Entities
                    .SaveChangesAsync();

                return RedirectToAction(
                    "StoreList",
                    model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        public IActionResult DatatableLabPurchaseReport(long id)
        {
            var model =
                new StockUpdateModel
                {
                    SchoolId = _user.SchoolId,

                    CategoryId = id
                };

            return PartialView(
                "~/Views/Laboratory/_pv_LabPurchaseReportList.cshtml",
                model);
        }

        public IActionResult DatatableLabSalesReport(long id)
        {
            var model =
                new StockUpdateModel
                {
                    SchoolId = _user.SchoolId,

                    CategoryId = id
                };

            return PartialView(
                "~/Views/Laboratory/_pv_LabSalesReportList.cshtml",
                model);
        }

        public IActionResult Billing()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }

        public IActionResult StockBillingDetails(string id)
        {
            long studentId = Convert.ToInt32(id);
            var student = new TrackTap.Data.Student(studentId);
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentName = student.StundentName;
            model.SchoolModel.classNumber = student.ClasssNumber; //Archana
            model.SchoolModel.className = student.ClassName;
            model.SchoolModel.division = student.DivisionName;
            model.SchoolModel.classInCharge = student.Teacher == null ? "Not Assigned" : student.Teacher.TeacherName;
            model.SchoolModel.classId = student.ClassId;
            model.SchoolModel.studentId = studentId;
            model.SchoolModel.curredntDateTime = CurrentTime;
            model.SchoolId = _user.SchoolId;
            model.DivisionId = student.DivisionId;
            model.AdmissionNo = student.StudentSpecialId;
            model.AcademicYearId = student.AcademicYearId;
            return View(model);
        }
        public IActionResult GetUserListByAdmissionNumberBilling(string id)
        {
            string[] splitData =
                id.Split('~');

            var model =
                new SchoolModel
                {
                    StudentSpecialId =
                        Convert.ToString(
                            splitData[0]),

                    // divisionId =
                    // Convert.ToInt64(splitData[1]),

                    schoolId =
                        _user.SchoolId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_StoreBilling_UserByAdmissionNumber_Grid.cshtml",
                model);
        }
        private async Task AllUpdatesInBalance(DateTime billDate,int sourceId,long bankId,decimal amount)
        {
            if (sourceId ==
                Convert.ToInt32(
                    DataFromStatus.Cash))
            {
                var balances = await _Entities
                    .TbStockBalances
                    .Where(x =>
                        x.SchoolId == _user.SchoolId
                        && x.SourceId == sourceId
                        && x.IsActive
                        && x.BankId == 0
                        && x.CurrentDate.Date >
                            billDate.Date)
                    .ToListAsync();

                if (balances.Any())
                {
                    foreach (var item in balances)
                    {
                        item.Opening += amount;

                        item.Closing += amount;
                    }

                    await _Entities
                        .SaveChangesAsync();
                }
            }
            else
            {
                var balances = await _Entities
                    .TbBalances
                    .Where(x =>
                        x.SchoolId == _user.SchoolId
                        && x.IsActive
                        && x.SourceId == sourceId
                        && x.BankId == bankId
                        && x.CurrentDate.Date >
                            billDate.Date)
                    .ToListAsync();

                if (balances.Any())
                {
                    foreach (var item in balances)
                    {
                        item.Opening += amount;

                        item.Closing += amount;
                    }

                    await _Entities
                        .SaveChangesAsync();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> StudentMainBillPay(FeeModel model)
        {
            bool status = false;

            string message = "Failed";

            using var transaction =
                await _Entities.Database
                    .BeginTransactionAsync();

            try
            {
                // STEP 1
                await SaveSalesStock(model);

                // STEP 2
                long billNo =
                    await SavePayment(model);

                // STEP 3
                await UpdateStudentBalance(
                    model,
                    billNo);

                // STEP 4
                if (model.PaymentType == 1)
                {
                    await CreateCashEntry(
                        model,
                        billNo);
                }
                else
                {
                    await CreateBankEntry(
                        model,
                        billNo);
                }

                // STEP 5
                await UpdateBalance(
                    model);

                await transaction.CommitAsync();

                // STEP 6
                await SendEmail(model);

                // STEP 7
                await SendSMS(model);

                status = true;

                message = "Bill Paid Successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                message = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = message
            });
        }

        private async Task SaveSalesStock(FeeModel model)
        {
            List<string> feeDetails =
                model.FeeDetails
                    .Split(',')
                    .ToList();

            foreach (var fee in feeDetails)
            {
                string[] splitData =
                    fee.Split('^');

                var stock =
                    new TbSalesStock
                    {
                        CategoryId =
                            Convert.ToInt32(splitData[3]),

                        Item =
                            splitData[7],

                        StockQuantity =
                            Convert.ToInt32(splitData[6]),

                        Price =
                            Convert.ToDecimal(splitData[5]),

                        SalesId =
                            splitData[9],

                        StockId =
                            Convert.ToInt32(splitData[8]),

                        StudentId =
                            Convert.ToInt32(splitData[2]),

                        SchoolId =
                            _user.SchoolId,

                        UserId =
                            _user.UserId,

                        IsActive = true,

                        TimeStamp = CurrentTime
                    };

                await _Entities
                    .TbSalesStocks
                    .AddAsync(stock);
            }

            await _Entities
                .SaveChangesAsync();
        }

        private async Task<long> SavePayment(FeeModel model)
        {
            long billNo = 1;

            var billNumber = await _Entities
                .TbStockPaymentBillNos
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == model.SchoolId);

            if (billNumber != null)
            {
                billNo = billNumber.BillNo + 1;

                billNumber.BillNo = billNo;
            }
            else
            {
                var newBill =
                    new TbStockPaymentBillNo
                    {
                        SchoolId = model.SchoolId,

                        BillNo = 1
                    };

                await _Entities
                    .TbStockPaymentBillNos
                    .AddAsync(newBill);
            }

            Guid paymentGuid =
                Guid.NewGuid();

            List<string> feeDetails =
                model.FeeDetails
                    .Split(',')
                    .ToList();

            foreach (var fee in feeDetails)
            {
                string[] splitData =
                    fee.Split('^');

                var payment =
                    new TbStockPayment
                    {
                        StockId =
                            Convert.ToInt32(splitData[8]),

                        StudentId =
                            Convert.ToInt32(splitData[2]),

                        ClassId =
                            Convert.ToInt32(splitData[1]),

                        Amount =
                            Convert.ToDecimal(splitData[5]),

                        SalesId =
                            splitData[9],

                        BillNo =
                            Convert.ToInt32(billNo),

                        PaymentGuid =
                            paymentGuid,

                        PaymentType = 1,

                        IsPaid = true,

                        IsActive = true,

                        SchoolId = _user.SchoolId,

                        TimeStamp = CurrentTime
                    };

                await _Entities
                    .TbStockPayments
                    .AddAsync(payment);
            }

            await _Entities
                .SaveChangesAsync();

            return billNo;
        }

        private async Task UpdateStudentBalance(FeeModel model,long billNo)
        {
            decimal totalAmount = 0;

            List<string> feeDetails =
                model.FeeDetails
                    .Split(',')
                    .ToList();

            foreach (var fee in feeDetails)
            {
                string[] splitData =
                    fee.Split('^');

                totalAmount +=
                    Convert.ToDecimal(splitData[5]);
            }

            var studentBalance = await _Entities
                .TbStockStudentBalances
                .FirstOrDefaultAsync(x =>
                    x.StudentId == model.StudentId
                    && x.IsActive);

            if (studentBalance != null)
            {
                studentBalance.Amount =
                    studentBalance.Amount - totalAmount;

                if (studentBalance.Amount < 0)
                {
                    studentBalance.Amount = 0;
                }
            }
            else
            {
                var balance =
                    new TbStockStudentBalance
                    {
                        StudentId = model.StudentId,

                        Amount = 0,

                        IsActive = true
                    };

                await _Entities
                    .TbStockStudentBalances
                    .AddAsync(balance);
            }

            var paidAmount =
                new TbStockStudentPaidAmount
                {
                    StudentId = model.StudentId,

                    PaidAmount = totalAmount,

                    BillNo = billNo,

                    IsActive = true,

                    AddAccountStatus = false
                };

            await _Entities
                .TbStockStudentPaidAmounts
                .AddAsync(paidAmount);

            await _Entities
                .SaveChangesAsync();
        }

        private async Task CreateCashEntry(FeeModel model,long billNo)
        {
            decimal amount = 0;

            List<string> feeDetails =
                model.FeeDetails
                    .Split(',')
                    .ToList();

            foreach (var fee in feeDetails)
            {
                string[] splitData =
                    fee.Split('^');

                amount +=
                    Convert.ToDecimal(splitData[5]);
            }

            var cashEntry =
                new TbStockCashEntry
                {
                    VoucherNumber = billNo.ToString(),

                    BillNo = billNo.ToString(),

                    VoucherType = "RV",

                    TransactionType = "R",

                    Amount = amount,

                    EnterDate = CurrentTime,

                    UserId = _user.UserId,

                    SchoolId = _user.SchoolId,

                    IsActive = true,

                    TimeStamp = CurrentTime
                };

            await _Entities
                .TbStockCashEntries
                .AddAsync(cashEntry);

            await _Entities
                .SaveChangesAsync();
        }

        private async Task CreateBankEntry(FeeModel model,long billNo)
        {
            decimal amount = 0;

            List<string> feeDetails =
                model.FeeDetails
                    .Split(',')
                    .ToList();

            foreach (var fee in feeDetails)
            {
                string[] splitData =
                    fee.Split('^');

                amount +=
                    Convert.ToDecimal(splitData[5]);
            }

            var bankEntry =
                new TbStockBankEntry
                {
                    VoucherNumber = billNo.ToString(),

                    BillNo = billNo.ToString(),

                    VoucherType = "RV",

                    TransactionType = "R",

                    Amount = amount,

                    BankId = model.BankId,

                    ModeType = model.PaymentType,

                    ChequeNumber = model.ChequeNumber,

                    EnterDate = CurrentTime,

                    UserId = _user.UserId,

                    SchoolId = _user.SchoolId,

                    IsActive = true,

                    TimeStamp = CurrentTime
                };

            await _Entities
                .TbStockBankEntries
                .AddAsync(bankEntry);

            await _Entities
                .SaveChangesAsync();
        }

        private async Task UpdateBalance(FeeModel model)
        {
            int sourceId =
                model.PaymentType == 1
                ? 0
                : 1;

            decimal amount = 0;

            List<string> feeDetails =
                model.FeeDetails
                    .Split(',')
                    .ToList();

            foreach (var fee in feeDetails)
            {
                string[] splitData =
                    fee.Split('^');

                amount +=
                    Convert.ToDecimal(splitData[5]);
            }

            var balance = await _Entities
                .TbStockBalances
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == _user.SchoolId
                    && x.SourceId == sourceId
                    && x.BankId == model.BankId
                    && x.CurrentDate.Date == CurrentTime.Date);

            if (balance != null)
            {
                balance.Closing += amount;
            }
            else
            {
                var newBalance =
                    new TbStockBalance
                    {
                        SchoolId = _user.SchoolId,

                        CurrentDate = CurrentTime,

                        SourceId = sourceId,

                        BankId = model.BankId,

                        Opening = 0,

                        Closing = amount,

                        IsActive = true,

                        TimeStamp = CurrentTime
                    };

                await _Entities
                    .TbStockBalances
                    .AddAsync(newBalance);
            }

            await _Entities
                .SaveChangesAsync();
        }

        private async Task SendEmail(FeeModel model)
        {
            try
            {
                var student = await _Entities
                    .TbStudents
                    .Include(x => x.School)
                    .FirstOrDefaultAsync(x =>
                        x.StudentId == model.StudentId);

                if (student == null)
                {
                    return;
                }

                string filePath =
                    Path.Combine(
                        _environment.WebRootPath,
                        "Content/email/FeePayment.html");

                string template =await System.IO.File
        .ReadAllTextAsync(filePath);

                string body = template
                    .Replace("{{schoolname}}",
                        student.School.SchoolName)
                    .Replace("{{parent}}",
                        student.ParentName)
                    .Replace("{{student}}",
                        student.StundentName);

                await _mailService.SendAsync(
                    student.ParentEmail,
                    "School Fee Payment",
                    body);
            }
            catch
            {
            }
        }

        private async Task SendSMS(FeeModel model)
        {
            try
            {
                var student = await _Entities
                    .TbStudents
                    .FirstOrDefaultAsync(x =>
                        x.StudentId == model.StudentId);

                if (student == null)
                {
                    return;
                }

                string message =
                    $"Dear Parent of {student.StundentName}, Fee Paid Successfully";

                using HttpClient client =
                    new HttpClient();

                string url =
                    "http://sms-api-url";

                await client.GetAsync(url);
            }
            catch
            {
            }
        }

        private async Task<string> GetRequest(string url)
        {
            _httpClient.DefaultRequestHeaders
                .UserAgent
                .ParseAdd(
                    "Mozilla/5.0");

            _httpClient.Timeout =
                TimeSpan.FromMinutes(5);

            var response =
                await _httpClient
                    .GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadAsStringAsync();
        }
        private async Task<string> PostRequest(string url,HttpContent content)
        {
            var response =
                await _httpClient
                    .PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadAsStringAsync();
        }

        public IActionResult PrintStockBillData(string id)
        {
            string[] splitData =
                id.Split('~');

            var model =
                new PrintBill
                {
                    studentId =
                        Convert.ToInt64(
                            splitData[0]),

                    billNumber =
                        Convert.ToInt64(
                            splitData[1]),

                    Amount =
                        Convert.ToInt64(
                            splitData[2]),

                    CurrentDate =
                        CurrentTime,

                    SalesId =
                        splitData[3],

                    UserId =
                        _user.UserId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_PrintStockBillData.cshtml",
                model);
        }

        public IActionResult CollectionReport()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }


        public IActionResult DatatableLabCollectionReport(long id)
        {
            var model =
                new StockUpdateModel
                {
                    SchoolId = _user.SchoolId,

                    CategoryId = id
                };

            return PartialView(
                "~/Views/Laboratory/_pv_LabSalesCollectionList.cshtml",
                model);
        }


        public IActionResult CollectionReportByDate(string id)
        {
            string[] splitData =
                id.Split('~');

            DateTime fromDate =
                Convert.ToDateTime(
                    splitData[0]);

            DateTime toDate =
                Convert.ToDateTime(
                    splitData[1]);

            string endDate =
                toDate.ToString("MM-dd-yyyy")
                + " 11:59:00 PM";

            var model =
                new StockUpdateModel
                {
                    StartDate =
                        fromDate,

                    EndDate =
                        Convert.ToDateTime(
                            endDate),

                    SchoolId =
                        _user.SchoolId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_CollectionReport.cshtml",
                model);
        }



        public IActionResult AddStockBillPartialView(long id)
        {
            var model =
                new FeeModel
                {
                    SchoolModel =
                        new SchoolModel
                        {
                            studentId = id
                        }
                };

            return PartialView(
                "~/Views/Laboratory/_pv_History_Billing_StudentStock_Model.cshtml",
                model);
        }

        public async Task<IActionResult> LoadTableForStock(string id)
        {
            string[] splitData =
                id.Split('~');

            long billNumber =
                Convert.ToInt64(
                    splitData[0]);

            long studentId =
                Convert.ToInt64(
                    splitData[1]);

            var stockPayment = await _Entities
                .TbStockPayments
                .FirstOrDefaultAsync(x =>
                    x.BillNo == billNumber
                    && x.IsActive);

            var model =
                new FeeModel
                {
                    SchoolModel =
                        new SchoolModel
                        {
                            studentId = studentId
                        },

                    BillNumber =
                        billNumber,

                    salesid =
                        stockPayment?.SalesId,

                    UserId =
                        _user.UserId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_StockHistory_PopupGrid.cshtml",
                model);
        }

        public IActionResult CashEntryReportStockHome()
        {
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = CurrentTime;
            model.ToDate = CurrentTime;
            return View(model);
        }


        public IActionResult DayBookReportStockHome()
        {
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = CurrentTime;
            model.ToDate = CurrentTime;
            return View(model);
        }
        public IActionResult BankEntryReportStockHome()
        {
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = CurrentTime;
            model.ToDate = CurrentTime;
            return View(model);
        }
        public IActionResult CashBookReportStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1]);

            var model =
                new DayBookReportModelStock
                {
                    SchoolId =
                        _user.SchoolId,

                    FromDate =
                        start,

                    ToDate =
                        end,

                    SchoolName =
                        _user.School
                            .SchoolName,

                    Heading =
                        $"{start} to {end}"
                };

            return PartialView(
                "~/Views/Laboratory/_pv_CashBookReportStock.cshtml",
                model);
        }

        public IActionResult BankBookReportStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1]);

            string checkId =
                splitDate[2];

            long bankId = 0;

            try
            {
                bankId =
                    Convert.ToInt64(
                        checkId);
            }
            catch
            {
                bankId = 0;
            }

            var model =
                new DayBookReportModelStock
                {
                    SchoolId =
                        _user.SchoolId,

                    FromDate =
                        start,

                    ToDate =
                        end,

                    BankId =
                        bankId,

                    SchoolName =
                        _user.School
                            .SchoolName,

                    Heading =
                        $"{start:dd-MM-yyyy} to {end:dd-MM-yyyy}"
                };

            return PartialView(
                "~/Views/Laboratory/_pv_BankBookReportStock.cshtml",
                model);
        }
        public IActionResult DayBookReportStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1]);

            var model =
                new DayBookReportModelStock
                {
                    SchoolName =
                        _user.School
                            .SchoolName,

                    Heading =
                        $"{start:dd-MM-yyyy} to {end:dd-MM-yyyy}",

                    SchoolId =
                        _user.SchoolId,

                    FromDate =
                        start,

                    ToDate =
                        end
                };

            return PartialView(
                "~/Views/Laboratory/_pv_DayBookReportStock.cshtml",
                model);
        }

        public IActionResult GetOpenBallanceByCashStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            var model =
                new DayBookReportModelStock
                {
                    SchoolId =
                        _user.SchoolId,

                    FromDate =
                        start
                };

            return PartialView(
                "~/Views/Laboratory/_pv_StockCashEntryOpeningBalance.cshtml",
                model);
        }
        public IActionResult GetClosingBallanceByCashStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1]);

            var model =
                new DayBookReportModelStock
                {
                    SchoolId =
                        _user.SchoolId,

                    ToDate =
                        end
                };

            return PartialView(
                "~/Views/Laboratory/_pv_StockCashEntryClosingBalance.cshtml",
                model);
        }

        public IActionResult GetOpenBallanceByBankStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            long.TryParse(
                splitDate[2],
                out long bankId);

            var model =
                new DayBookReportModelStock
                {
                    SchoolId =
                        _user.SchoolId,

                    FromDate =
                        start,

                    BankId =
                        bankId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_StockBankEntryOpeningBalance.cshtml",
                model);
        }
        public IActionResult GetClosingBallanceByBankStock(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1]);

            long.TryParse(
                splitDate[2],
                out long bankId);

            var model =
                new DayBookReportModelStock
                {
                    SchoolId =
                        _user.SchoolId,

                    ToDate =
                        end,

                    BankId =
                        bankId
                };

            return PartialView(
                "~/Views/Laboratory/_pv_StockBankEntryClosingBalance.cshtml",
                model);
        }



    }
}
                     
                    
          

//jibin 9/14/2020