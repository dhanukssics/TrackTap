
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackTap.Data;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class JournalController : BaseController
    {
        private readonly SchoolDbContext _Entities;
        public JournalController(SchoolDbContext Entities, SchoolRepository schoolRepository, ParentRepository parentRepository, TeacherRepository teacherRepository) : base(Entities, schoolRepository, parentRepository, teacherRepository)
        {
            _Entities = Entities;
        }

        // GET: Journal
        public IActionResult AccountSettings()
        {
            var model = new AddAccountHeadModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAddAccountHead(AddAccountHeadModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var account = await _Entities.TbAccountHeads
                    .FirstOrDefaultAsync(x =>
                        x.AccountId == model.AccountHeadId);

                // ADD NEW
                if (account == null)
                {
                    var head = new TbAccountHead
                    {
                        AccHeadName =
                            model.AccountHeadName.ToUpper(),

                        SchoolId = _user.SchoolId,

                        IsActive = true,

                        TimeStamp = CurrentTime
                    };

                    await _Entities.TbAccountHeads
                        .AddAsync(head);

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        bool subLedgerExists =
                            await _Entities.TbSubLedgerData
                            .AnyAsync(x =>
                                x.SubLedgerName ==
                                model.AccountHeadName
                                    .ToUpper()
                                && x.IsActive);

                        if (!subLedgerExists)
                        {
                            var sub = new TbSubLedgerDatum
                            {
                                SubLedgerName = "--",

                                AccHeadId = head.AccountId,

                                IsActive = true,

                                TimeStamp = CurrentTime
                            };

                            await _Entities.TbSubLedgerData
                                .AddAsync(sub);

                            status =
                                await _Entities
                                    .SaveChangesAsync() > 0;
                        }

                        msg = status
                            ? "Success"
                            : "Failed";
                    }
                }

                // UPDATE
                else
                {
                    account.AccHeadName =
                        model.AccountHeadName.ToUpper();

                    account.TimeStamp =
                        CurrentTime;

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    msg = status
                        ? "Success"
                        : "Failed";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                message = status
                    ? "Account Head added successfully!"
                    : "Failed to add Account Head!"
            });
        }
        public async Task<IActionResult> GetSubLedgerListList()
        {
            var model = new AddAccountHeadModel
            {
                SchoolId = _user.SchoolId
            };

            var input = await _Entities.TbAccountHeads
                .Where(x =>
                    x.IsActive &&
                    x.SchoolId == _user.SchoolId)
                .OrderBy(x => x.AccHeadName)
                .ToListAsync();

            ViewBag.store = input
                .Select(x => new SelectListItem
                {
                    Text = x.AccHeadName,

                    Value = x.AccountId.ToString()
                })
                .ToList();

            return PartialView(
                "~/Views/Journal/_pv_Add_SubLedger.cshtml",
                model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAddSubLedger(AddAccountHeadModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var sub = new TbSubLedgerDatum
                {
                    SubLedgerName = model.SubLedger,

                    AccHeadId = model.AccountHeadId,

                    IsActive = true,

                    TimeStamp = CurrentTime
                };

                await _Entities.TbSubLedgerData
                    .AddAsync(sub);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                message = status
                    ? "Sub Ledger added successfully!"
                    : "Failed to add Sub Ledger!"
            });
        }
        public IActionResult GetAccountHeadDataList()
        {
            var model = new AddAccountHeadModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_AcchountHeadList.cshtml",
                model);
        }
        //Code added by Gayathri A (30/10/2023) Edit operation in Accountsetting
        public async Task<IActionResult> EditSubLedger(string id)
        {
            bool status = false;

            string msg = "False";

            long subId = Convert.ToInt64(id);

            var head = await _Entities.TbAccountHeads
                .FirstOrDefaultAsync(x =>
                    x.AccountId == subId
                    && x.IsActive);

            if (head != null)
            {
                msg = head.AccHeadName;

                status = true;
            }

            return Json(new
            {
                status = status,

                msg = msg,

                subId = subId
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSubLedger(long id)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var sub = await _Entities.TbSubLedgerData
                    .FirstOrDefaultAsync(x =>
                        x.LedgerId == id
                        && x.IsActive);

                if (sub != null)
                {
                    int count = await _Entities.TbSubLedgerData
                        .CountAsync(x =>
                            x.AccHeadId == sub.AccHeadId
                            && x.IsActive);

                    // If last subledger, deactivate account head also
                    if (count == 1)
                    {
                        var head = await _Entities.TbAccountHeads
                            .FirstOrDefaultAsync(x =>
                                x.AccountId == sub.AccHeadId
                                && x.IsActive);

                        if (head != null)
                        {
                            head.IsActive = false;
                        }
                    }

                    sub.IsActive = false;

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
        //---------------------------------------------------------------Day Book Module Starts----------------------
        public IActionResult DayBook()
        {
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public async Task<IActionResult> GetVoucherNo(long id)
        {
            string voucherNumber = "";

            bool status = true;

            string msg = "Failed";

            int typeId = Convert.ToInt32(id);

            var data = await _Entities.TbDayBookIds
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == _user.SchoolId);

            if (data != null)
            {
                voucherNumber = typeId == 0
                    ? data.ExpenseId.ToString()
                    : data.IncomeId.ToString();

                msg = "Success";
            }
            else
            {
                var dayBookId = new TbDayBookId
                {
                    SchoolId = _user.SchoolId,

                    IncomeId = 1,

                    ExpenseId = 1
                };

                await _Entities.TbDayBookIds
                    .AddAsync(dayBookId);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                voucherNumber = typeId == 0
                    ? dayBookId.ExpenseId.ToString()
                    : dayBookId.IncomeId.ToString();

                msg = "Success";
            }

            return Json(new
            {
                status = status,

                result = voucherNumber,

                message = msg
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddDayBook(AddDayBookModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                int typeId =
                    Convert.ToInt32(model.TypeId);

                bool voucherExists =
                    await _Entities.TbDayBookData
                    .AnyAsync(x =>
                        x.TypeId == typeId
                        && x.VoucherNo == model.VoucherNo
                        && x.IsActive
                        && x.SchoolId == _user.SchoolId);

                if (voucherExists)
                {
                    msg = "Please refresh!";

                    var check = await _Entities
                        .TbDayBookData
                        .Where(x =>
                            x.TypeId == typeId
                            && x.SchoolId ==
                                _user.SchoolId
                            && x.IsActive)
                        .OrderByDescending(x =>
                            x.DayBookId)
                        .FirstOrDefaultAsync();

                    var idCheck = await _Entities
                        .TbDayBookIds
                        .FirstOrDefaultAsync(x =>
                            x.SchoolId ==
                            _user.SchoolId);

                    if (check != null &&
                        idCheck != null)
                    {
                        if (typeId == 0)
                        {
                            if (idCheck.ExpenseId ==
                                Convert.ToInt32(
                                    check.VoucherNo))
                            {
                                idCheck.ExpenseId += 1;

                                await _Entities
                                    .SaveChangesAsync();

                                model.VoucherNo =
                                    idCheck.ExpenseId
                                        .ToString();
                            }
                        }
                        else
                        {
                            if (idCheck.IncomeId ==
                                Convert.ToInt32(
                                    check.VoucherNo))
                            {
                                idCheck.IncomeId += 1;

                                await _Entities
                                    .SaveChangesAsync();

                                model.VoucherNo =
                                    idCheck.IncomeId
                                        .ToString();
                            }
                        }
                    }
                }

                var dayBook =
                    new TbDayBookDatum();

                dayBook.TypeId = typeId;

                try
                {
                    if (!string.IsNullOrEmpty(
                        model.EntryDateString))
                    {
                        string[] splitData =
                            model.EntryDateString
                                .Split('-');

                        var dd = splitData[0];

                        var mm = splitData[1];

                        var yyyy = splitData[2];

                        var date =
                            $"{mm}-{dd}-{yyyy}";

                        dayBook.EntryDate =
                            Convert.ToDateTime(date);
                    }
                }
                catch
                {
                }

                dayBook.VoucherNo =
                    model.VoucherNo;

                dayBook.HeadId =
                    model.HeadId;

                dayBook.SubLedgerId =
                    model.SubLedgerId;

                dayBook.Amount =
                    model.Amount;

                dayBook.Narration =
                    string.IsNullOrWhiteSpace(
                        model.Narration)
                    ? " "
                    : model.Narration;

                dayBook.SchoolId =
                    _user.SchoolId;

                dayBook.UserId =
                    _user.UserId;

                dayBook.IsActive = true;

                dayBook.TimeStamp =
                    CurrentTime;

                await _Entities.TbDayBookData
                    .AddAsync(dayBook);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                if (status)
                {
                    msg = "Success";

                    // Increase voucher number
                    var vou = await _Entities
                        .TbDayBookIds
                        .FirstOrDefaultAsync(x =>
                            x.SchoolId ==
                            _user.SchoolId);

                    if (vou != null)
                    {
                        if (dayBook.TypeId == 0)
                        {
                            vou.ExpenseId += 1;
                        }
                        else
                        {
                            vou.IncomeId += 1;
                        }

                        await _Entities
                            .SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = status
                    ? "DayBook added successfully!"
                    : "Failed to add DayBook!"
            });
        }
        public IActionResult SearchVoucherNo()
        {
            var model = new AddDayBookModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_SearchVoucherNo.cshtml",
                model);
        }
        public IActionResult EditDayBookView()
        {
            var model = new AddDayBookModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_Edit_DayBook.cshtml",
                model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchVoucher(
    AddDayBookModel model)
        {
            int typeId =
                Convert.ToInt32(model.TypeId);

            var data = await _Entities.TbDayBookData
                .FirstOrDefaultAsync(x =>
                    x.TypeId == typeId
                    && x.SchoolId == _user.SchoolId
                    && x.IsActive
                    && x.VoucherNo ==
                        model.SearchVoucherNo);

            model.SchoolId = _user.SchoolId;

            if (data != null)
            {
                model.EntryDateString =
                    data.EntryDate
                        .ToString("dd-MM-yyyy");

                model.EntryDate =
                    data.EntryDate;

                model.HeadId =
                    data.HeadId;

                model.SubLedgerId =
                    data.SubLedgerId;

                model.Amount =
                    data.Amount;

                model.Narration =
                    data.Narration;

                model.VoucherNo =
                    model.SearchVoucherNo;

                model.TypeId =
                    typeId == 0
                    ? AccountType.Expense
                    : AccountType.Income;

                model.DayBookId =
                    data.DayBookId;

                return PartialView(
                    "~/Views/Journal/_pv_Edit_DayBook.cshtml",
                    model);
            }

            return PartialView(
                "~/Views/Journal/_pv_Add_DayBook.cshtml",
                model);
        }
        public IActionResult AddDayBookReaload()
        {
            var model = new AddDayBookModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_Add_DayBook.cshtml",
                model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDayBook(AddDayBookModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var data = await _Entities.TbDayBookData
                    .FirstOrDefaultAsync(x =>
                        x.DayBookId == model.DayBookId
                        && x.IsActive
                        && x.SchoolId == _user.SchoolId);

                if (data != null)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(
                            model.EntryDateString))
                        {
                            string[] splitData =
                                model.EntryDateString
                                    .Split('-');

                            var dd = splitData[0];

                            var mm = splitData[1];

                            var yyyy = splitData[2];

                            var date =
                                $"{mm}-{dd}-{yyyy}";

                            data.EntryDate =
                                Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {
                    }

                    data.VoucherNo =
                        model.VoucherNo;

                    data.HeadId =
                        model.HeadId;

                    data.SubLedgerId =
                        model.SubLedgerId;

                    data.Amount =
                        model.Amount;

                    data.Narration =
                        !string.IsNullOrWhiteSpace(
                            model.Narration)
                        ? model.Narration
                        : " ";

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    msg = status
                        ? "Successful"
                        : "No Changes";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                message = msg
            });
        }

        public async Task<IActionResult> CheckVoucherNumber(string text)
        {
            bool status = false;

            string message = "Failed";

            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    string[] data =
                        text.Split('~');

                    string voucherNo =
                        data[0];

                    int typeId =
                        Convert.ToInt32(data[1]);

                    bool exists =
                        await _Entities.TbDayBookData
                        .AnyAsync(x =>
                            x.VoucherNo == voucherNo
                            && x.TypeId == typeId
                            && x.IsActive
                            && x.SchoolId ==
                                _user.SchoolId);

                    if (!exists)
                    {
                        status = true;

                        message = "Available";
                    }
                    else
                    {
                        message =
                            "Voucher already exists";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            return Json(new
            {
                Status = status,

                Message = message
            });
        }
        public IActionResult StatusRange()
        {
            var model = new AddDayBookModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_ProfitOrLoss.cshtml",
                model);
        }
        public IActionResult DayBookPrint(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;

            try
            {
                if (!string.IsNullOrWhiteSpace(
                    model.EntryDateString))
                {
                    string[] splitData =
                        model.EntryDateString
                            .Split('-');

                    var dd = splitData[0];

                    var mm = splitData[1];

                    var yyyy = splitData[2];

                    var date =
                        $"{mm}-{dd}-{yyyy}";

                    model.EntryDate =
                        Convert.ToDateTime(date);
                }
            }
            catch
            {
            }

            var head =
                new TrackTap.Data.SubLedgerData(
                    model.SubLedgerId);

            model.HeadName =
                head.AccountHeadName;

            model.SubLedger =
                head.SubLedgerName;

            int type =
                Convert.ToInt32(model.TypeId);

            model.TypeData =
                type == 0
                ? "Expense"
                : "Income";

            return PartialView(
                "~/Views/Journal/_pv_Print_DayBook.cshtml",
                model);
        }

        //---------------------------------------------------------------Cash Book Module Starts----------------------
        public IActionResult CashBook(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        //public object SearchCashBookData(AddDayBookModel model)
        public IActionResult SearchCashBookData(string id)
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
                new AddDayBookModel
                {
                    SchoolId = _user.SchoolId,

                    startDate = start,

                    endDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_CashBookList.cshtml",
                model);
        }
        public IActionResult SearchCashBookDailyData(string id)
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
                new AddDayBookModel
                {
                    SchoolId = _user.SchoolId,

                    startDate = start,

                    endDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_CashBookDailyReport.cshtml",
                model);
        }

        public IActionResult DayBookStatusData(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1] +
                    " 11:59:59 PM");

            var model =
                new AddDayBookModel
                {
                    SchoolId = _user.SchoolId,

                    startDate = start,

                    endDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_CashBookSearchDate.cshtml",
                model);
        }
        public IActionResult CashBookSummary(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        //public object SearchCashBookData(AddDayBookModel model)
        public IActionResult SearchCashBookCashBookSummary(string id)
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
                new AddDayBookModel
                {
                    SchoolId = _user.SchoolId,

                    startDate = start,

                    endDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_CashBookListSummary.cshtml",
                model);
        }
        public IActionResult CashBookDailyReport(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        public IActionResult SearchCashBookDailyReportData(string id)
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
                new AddDayBookModel
                {
                    SchoolId = _user.SchoolId,

                    startDate = start,

                    endDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_CashBookDailyReport.cshtml",
                model);
        }
        //---------------------------------------------------------------Ledger Module Starts----------------------

        public IActionResult Ledger(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        public IActionResult SearchLedgerData(string id)
        {
            string[] splitDate =
                id.Split('~');

            DateTime start =
                Convert.ToDateTime(
                    splitDate[0]);

            DateTime end =
                Convert.ToDateTime(
                    splitDate[1]);

            string filterData =
                splitDate[2];

            var model =
                new AddDayBookModel
                {
                    SchoolId = _user.SchoolId,

                    startDate = start,

                    endDate = end
                };

            if (filterData != "0")
            {
                string[] filter =
                    filterData.Split('!');

                try
                {
                    model.HeadId =
                        Convert.ToInt64(
                            filter[0]);

                    model.FilterTypeId =
                        Convert.ToInt32(
                            filter[1]);
                }
                catch
                {
                    model.HeadId = 0;

                    model.FilterTypeId = 0;
                }
            }
            else
            {
                model.HeadId = 0;

                model.FilterTypeId = 0;
            }

            return PartialView(
                "~/Views/Journal/_pv_LedgerList.cshtml",
                model);
        }
        //---------------------------------------------------------------Bank Details Module Starts----------------------
        public IActionResult BankDetails(BankDetailsModel model)
        {
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAddBankName(BankDetailsModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var bank = await _Entities.TbBanks
                    .FirstOrDefaultAsync(x =>
                        x.BankId == model.BankId);

                // ADD
                if (bank == null)
                {
                    bank = new TbBank
                    {
                        BankName = model.BankName,

                        SchoolId = _user.SchoolId,

                        IsActive = true,

                        TimeStamp = CurrentTime
                    };

                    await _Entities.TbBanks
                        .AddAsync(bank);
                }
                // UPDATE
                else
                {
                    bank.BankName =
                        model.BankName;

                    bank.SchoolId =
                        _user.SchoolId;

                    bank.IsActive = true;

                    bank.TimeStamp =
                        CurrentTime;
                }

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                msg = status
                    ? "Success"
                    : "Failed";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                message = status
                    ? "Bank Details added successfully!"
                    : "Failed to add Bank Details!"
            });
        }
        public IActionResult GetBankDetailsList()
        {
            var model = new BankDetailsModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_BankDetailsList.cshtml",
                model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBank(long id)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var bank = await _Entities.TbBanks
                    .FirstOrDefaultAsync(x =>
                        x.BankId == id
                        && x.IsActive);

                if (bank != null)
                {
                    bank.IsActive = false;

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
        //Created by gayathri(2/11/2023)For bank edit option
        public async Task<IActionResult> EditBank(long id)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var bank = await _Entities.TbBanks
                    .FirstOrDefaultAsync(x =>
                        x.BankId == id
                        && x.IsActive);

                if (bank != null)
                {
                    status = true;

                    msg = bank.BankName;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = msg,

                id = id
            });
        }
        //---------------------------------------------------------------Bank Book Module Starts----------------------
        public IActionResult BankBook(BankBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public async Task<IActionResult> GetBankVoucherNo(long id)
        {
            string voucherNumber = "";

            bool status = true;

            string msg = "Failed";

            int typeId = Convert.ToInt32(id);

            var data = await _Entities.TbBankBookIds
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == _user.SchoolId);

            if (data != null)
            {
                voucherNumber = typeId == 0
                    ? data.DepositId.ToString()
                    : data.WithdrawId.ToString();

                msg = "Success";
            }
            else
            {
                var bankBookId =
                    new TbBankBookId
                    {
                        SchoolId = _user.SchoolId,

                        DepositId = 1,

                        WithdrawId = 1
                    };

                await _Entities.TbBankBookIds
                    .AddAsync(bankBookId);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                voucherNumber = typeId == 0
                    ? bankBookId.DepositId.ToString()
                    : bankBookId.WithdrawId.ToString();

                msg = "Success";
            }

            return Json(new
            {
                status = status,

                result = voucherNumber,

                message = msg
            });
        }
        public IActionResult AddBankBookReaload()
        {
            var model = new BankBookModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_Add_BankBook.cshtml",
                model);
        }
        public IActionResult SearchVoucherNoForBank()
        {
            var model = new BankBookModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_SearchVoucherNo_Bank.cshtml",
                model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchVoucherForBank(BankBookModel model)
        {
            int typeId =
                Convert.ToInt32(model.TypeId);

            var data = await _Entities.TbBankBookData
                .FirstOrDefaultAsync(x =>
                    x.TypeId == typeId
                    && x.SchoolId == _user.SchoolId
                    && x.IsActive
                    && x.VoucherNumber ==
                        model.SearchVoucherNo);

            model.SchoolId = _user.SchoolId;

            if (data != null)
            {
                model.EntryDateString =
                    data.EntryDate
                        .ToString("dd-MM-yyyy");

                model.EntryDate =
                    data.EntryDate;

                model.HeadId =
                    data.HeadId;

                model.SubLedgerId =
                    data.SubledgerId;

                model.Amount =
                    data.Amount;

                model.Narration =
                    data.Narration;

                model.VoucherNo =
                    model.SearchVoucherNo;

                model.TypeId =
                    typeId == 0
                    ? BankType.Deposit
                    : BankType.Withdraw;

                model.BankBookId =
                    data.Id;

                model.BankId =
                    data.BankId;

                model.ChequeNo =
                    data.ChequeNo;

                if (data.ChequeDate != null)
                {
                    model.ChequeDate =
                        data.ChequeDate
                        ?? CurrentTime;

                    model.ChequeDateString =
                        model.ChequeDate
                            .ToString("dd-MM-yyyy");
                }

                return PartialView(
                    "~/Views/Journal/_pv_Edit_BankBook.cshtml",
                    model);
            }

            return PartialView(
                "~/Views/Journal/_pv_Add_BankDetails.cshtml",
                model);
        }
        public async Task<IActionResult> CheckVoucherNumberForBank(string text)
        {
            bool status = false;

            string message = "Failed";

            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    string[] data =
                        text.Split('~');

                    string voucherNo =
                        data[0];

                    int typeId =
                        Convert.ToInt32(data[1]);

                    bool exists =
                        await _Entities.TbBankBookData
                        .AnyAsync(x =>
                            x.VoucherNumber == voucherNo
                            && x.TypeId == typeId
                            && x.IsActive
                            && x.SchoolId ==
                                _user.SchoolId);

                    if (!exists)
                    {
                        status = true;

                        message = "Available";
                    }
                    else
                    {
                        message =
                            "Voucher already exists";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            return Json(new
            {
                Status = status,

                Message = message
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddBankBook(BankBookModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var bankBook = new TbBankBookDatum();

                bankBook.TypeId =
                    Convert.ToInt32(model.TypeId);

                // Entry Date
                try
                {
                    if (!string.IsNullOrWhiteSpace(
                        model.EntryDateString))
                    {
                        string[] splitData =
                            model.EntryDateString
                                .Split('-');

                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];

                        var date =
                            $"{mm}-{dd}-{yyyy}";

                        bankBook.EntryDate =
                            Convert.ToDateTime(date);
                    }
                }
                catch
                {
                }

                // Cheque Date
                try
                {
                    if (!string.IsNullOrWhiteSpace(
                        model.ChequeDateString))
                    {
                        string[] splitData =
                            model.ChequeDateString
                                .Split('-');

                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];

                        var date =
                            $"{mm}-{dd}-{yyyy}";

                        bankBook.ChequeDate =
                            Convert.ToDateTime(date);
                    }
                }
                catch
                {
                }

                bankBook.VoucherNumber =
                    model.VoucherNo;

                bankBook.HeadId =
                    model.HeadId;

                bankBook.SubledgerId =
                    model.SubLedgerId;

                bankBook.Amount =
                    model.Amount;

                bankBook.Narration =
                    string.IsNullOrWhiteSpace(
                        model.Narration)
                    ? " "
                    : model.Narration;

                bankBook.SchoolId =
                    _user.SchoolId;

                bankBook.UserId =
                    _user.UserId;

                bankBook.IsActive = true;

                bankBook.TimeStamp =
                    CurrentTime;

                bankBook.ChequeNo =
                    model.ChequeNo;

                bankBook.BankId =
                    model.BankId;

                // Withdraw
                if (bankBook.TypeId != 0)
                {
                    bankBook.IsWithdraw =
                        model.iswithdraw == true;
                }

                await _Entities.TbBankBookData
                    .AddAsync(bankBook);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                msg = "Success";

                // Update Voucher Number
                var vou = await _Entities
                    .TbBankBookIds
                    .FirstOrDefaultAsync(x =>
                        x.SchoolId ==
                        _user.SchoolId);

                if (vou != null)
                {
                    if (bankBook.TypeId == 0)
                    {
                        vou.DepositId += 1;
                    }
                    else
                    {
                        vou.WithdrawId += 1;
                    }

                    await _Entities
                        .SaveChangesAsync();
                }

                ///////////////////////////////////
                // Create DayBook Entry
                ///////////////////////////////////

                if (bankBook.TypeId != 0)
                {
                    var existingDayBook =
                        await _Entities.TbDayBookData
                        .FirstOrDefaultAsync(x =>
                            x.SchoolId ==
                                _user.SchoolId
                            && x.HeadId ==
                                model.HeadId
                            && x.SubLedgerId ==
                                model.SubLedgerId
                            && x.Amount ==
                                model.Amount
                            && x.Narration ==
                                model.Narration);

                    if (existingDayBook == null)
                    {
                        if (model.iswithdraw != true)
                        {
                            long voucher = 1;

                            var voucherData =
                                await _Entities
                                    .TbDayBookIdBanks
                                    .FirstOrDefaultAsync(x =>
                                        x.SchoolId ==
                                        _user.SchoolId);

                            if (voucherData != null)
                            {
                                voucher =
                                    voucherData.ExpenseId;
                            }

                            var dayBook =
                                new TbDayBookDatum();

                            dayBook.TypeId = 0;

                            dayBook.EntryDate =
                                bankBook.EntryDate;

                            dayBook.VoucherNo =
                                "BK" + voucher;

                            dayBook.HeadId =
                                model.HeadId;

                            dayBook.SubLedgerId =
                                model.SubLedgerId;

                            dayBook.Amount =
                                model.Amount;

                            dayBook.Narration =
                                string.IsNullOrWhiteSpace(
                                    model.Narration)
                                ? " "
                                : model.Narration;

                            dayBook.SchoolId =
                                _user.SchoolId;

                            dayBook.UserId =
                                _user.UserId;

                            dayBook.IsActive = true;

                            dayBook.TimeStamp =
                                CurrentTime;

                            dayBook.IsWithdraw = false;

                            await _Entities.TbDayBookData
                                .AddAsync(dayBook);

                            status =
                                await _Entities
                                    .SaveChangesAsync() > 0;

                            msg = "Success";

                            // Increase Voucher
                            var vouch =
                                await _Entities
                                    .TbDayBookIdBanks
                                    .FirstOrDefaultAsync(x =>
                                        x.SchoolId ==
                                        _user.SchoolId);

                            if (vouch != null)
                            {
                                vouch.ExpenseId += 1;

                                await _Entities
                                    .SaveChangesAsync();
                            }
                            else
                            {
                                var voucherTable =
                                    new TbDayBookIdBank
                                    {
                                        SchoolId =
                                            _user.SchoolId,

                                        ExpenseId = 2,

                                        IncomeId = 1
                                    };

                                await _Entities
                                    .TbDayBookIdBanks
                                    .AddAsync(voucherTable);

                                await _Entities
                                    .SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = status
                    ? "BankBook added successfully!"
                    : "Failed to add BankBook!"
            });
        }
        [HttpPost]
        public async Task<IActionResult> EditBankBook(BankBookModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var data = await _Entities.TbBankBookData
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.BankBookId
                        && x.IsActive
                        && x.SchoolId == _user.SchoolId);

                if (data != null)
                {
                    // Entry Date
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(
                            model.EntryDateString))
                        {
                            string[] splitData =
                                model.EntryDateString
                                    .Split('-');

                            var dd = splitData[0];

                            var mm = splitData[1];

                            var yyyy = splitData[2];

                            var date =
                                $"{mm}-{dd}-{yyyy}";

                            data.EntryDate =
                                Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {
                    }

                    data.VoucherNumber =
                        model.VoucherNo;

                    data.HeadId =
                        model.HeadId;

                    data.SubledgerId =
                        model.SubLedgerId;

                    data.Amount =
                        model.Amount;

                    data.Narration =
                        !string.IsNullOrWhiteSpace(
                            model.Narration)
                        ? model.Narration
                        : " ";

                    data.ChequeNo =
                        !string.IsNullOrWhiteSpace(
                            model.ChequeNo)
                        ? model.ChequeNo
                        : " ";

                    data.BankId =
                        model.BankId;

                    // Cheque Date
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(
                            model.ChequeDateString))
                        {
                            string[] splitData =
                                model.ChequeDateString
                                    .Split('-');

                            var dd = splitData[0];

                            var mm = splitData[1];

                            var yyyy = splitData[2];

                            var date =
                                $"{mm}-{dd}-{yyyy}";

                            data.ChequeDate =
                                Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {
                    }

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    msg = status
                        ? "Successful"
                        : "No Changes";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                message = msg
            });
        }
        public async Task<IActionResult> BankBookPrint(BankBookModel model)
        {
            model.SchoolId = _user.SchoolId;

            // Entry Date
            try
            {
                if (!string.IsNullOrWhiteSpace(
                    model.EntryDateString))
                {
                    string[] splitData =
                        model.EntryDateString
                            .Split('-');

                    var dd = splitData[0];

                    var mm = splitData[1];

                    var yyyy = splitData[2];

                    var date =
                        $"{mm}-{dd}-{yyyy}";

                    model.EntryDate =
                        Convert.ToDateTime(date);
                }
            }
            catch
            {
            }

            var head =
                new TrackTap.Data.SubLedgerData(
                    model.SubLedgerId);

            model.HeadName =
                head.AccountHeadName;

            model.SubLedger =
                head.SubLedgerName;

            model.BankName =
                await _Entities.TbBanks
                    .Where(x =>
                        x.BankId == model.BankId
                        && x.IsActive)
                    .Select(x => x.BankName)
                    .FirstOrDefaultAsync();

            // Cheque Date
            try
            {
                if (!string.IsNullOrWhiteSpace(
                    model.ChequeDateString))
                {
                    string[] splitData =
                        model.ChequeDateString
                            .Split('-');

                    var dd = splitData[0];

                    var mm = splitData[1];

                    var yyyy = splitData[2];

                    var date =
                        $"{mm}-{dd}-{yyyy}";

                    model.ChequeDate =
                        Convert.ToDateTime(date);
                }
            }
            catch
            {
            }

            int type =
                Convert.ToInt32(model.TypeId);

            model.TypeData =
                type == 0
                ? "Deposit"
                : "Withdraw";

            return PartialView(
                "~/Views/Journal/_pv_BankBookPrint.cshtml",
                model);
        }
        public IActionResult CurrentBankBalance(long id)
        {
            var model = new BankBookModel
            {
                SchoolId = _user.SchoolId,

                BankId = id
            };

            return PartialView(
                "~/Views/Journal/_pv_BankBalance.cshtml",
                model);
        }
        public IActionResult CheckWithdrawAmount(string text)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                string[] data =
                    text.Split('~');

                decimal amountWithdraw =
                    Convert.ToDecimal(data[0]);

                long bankId =
                    Convert.ToInt64(data[1]);

                decimal amount =
                    new TrackTap.Data.School(
                        _user.SchoolId)
                    .GetBankCurrentBalance(
                        bankId);

                if (amountWithdraw > amount)
                {
                    status = true;

                    message =
                        "Don't have this much amount!";
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
        public async Task<IActionResult> CheckWithdrawAmountEdit(string text)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                string[] data =
                    text.Split('~');

                decimal amountWithdraw =
                    Convert.ToDecimal(data[0]);

                long bankBookId =
                    Convert.ToInt64(data[1]);

                long accountBankId =
                    Convert.ToInt64(data[2]);

                var bankData = await _Entities
                    .TbBankBookData
                    .FirstOrDefaultAsync(x =>
                        x.Id == bankBookId);

                decimal amount =
                    new TrackTap.Data.School(
                        _user.SchoolId)
                    .GetBankCurrentBalance(
                        accountBankId);

                if (bankData != null)
                {
                    amount =
                        amount + bankData.Amount;
                }

                if (amountWithdraw > amount)
                {
                    status = true;

                    message =
                        "Don't have this much amount!";
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
        //---------------------------------------------------------------Assets / Liabilities Module Starts-----------

        public IActionResult Assets()
        {
            var model = new AssetsLiabilityModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public async Task<IActionResult> GetAssestsInvoiceNumber(long id)
        {
            string invoiceNumber = "";

            bool status = true;

            string msg = "Failed";

            int typeId = Convert.ToInt32(id);

            var data = await _Entities
                .TbAssetsLiabilityIds
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == _user.SchoolId);

            if (data != null)
            {
                invoiceNumber = typeId == 0
                    ? "A" + data.AssetsId
                    : "L" + data.LiabilityId;

                msg = "Success";
            }
            else
            {
                var assetsId =
                    new TbAssetsLiabilityId
                    {
                        SchoolId = _user.SchoolId,

                        AssetsId = 1,

                        LiabilityId = 1
                    };

                await _Entities
                    .TbAssetsLiabilityIds
                    .AddAsync(assetsId);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                invoiceNumber = typeId == 0
                    ? "A" + assetsId.AssetsId
                    : "L" + assetsId.LiabilityId;

                msg = "Success";
            }

            return Json(new
            {
                status = status,

                result = invoiceNumber,

                message = msg
            });
        }
        public IActionResult SearchVoucherNoForAssets()
        {
            var model = new AssetsLiabilityModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_SearchVoucherNo_Assets.cshtml",
                model);
        }
        public IActionResult AddAssetsReaload()
        {
            var model = new AssetsLiabilityModel
            {
                SchoolId = _user.SchoolId
            };

            return PartialView(
                "~/Views/Journal/_pv_Add_Assests.cshtml",
                model);
        }
        public IActionResult AssestsPrint(AssetsLiabilityModel model)
        {
            model.SchoolId = _user.SchoolId;

            try
            {
                if (!string.IsNullOrWhiteSpace(
                    model.EntryDateString))
                {
                    string[] splitData =
                        model.EntryDateString
                            .Split('-');

                    var dd = splitData[0];

                    var mm = splitData[1];

                    var yyyy = splitData[2];

                    var date =
                        $"{mm}-{dd}-{yyyy}";

                    model.EntryDate =
                        Convert.ToDateTime(date);
                }
            }
            catch
            {
            }

            var head =
                new TrackTap.Data.AccountHead(
                    model.HeadId);

            model.HeadName =
                head.AccHeadName;

            int type =
                Convert.ToInt32(model.TypeId);

            model.TypeData =
                type == 0
                ? "Assets"
                : "Liability";

            model.AddStatusString =
                model.AddStatus
                ? "Yes"
                : "No";

            return PartialView(
                "~/Views/Journal/_pv_AssetsPrint.cshtml",
                model);
        }
        public async Task<IActionResult> CheckInvoiceNumberForAssets(string text)
        {
            bool status = false;

            string message = "Failed";

            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    string[] data =
                        text.Split('~');

                    string invoiceNo =
                        data[0];

                    int typeId =
                        Convert.ToInt32(data[1]);

                    bool exists =
                        await _Entities
                            .TbAssetsLiabilityData
                            .AnyAsync(x =>
                                x.InviceNumber == invoiceNo
                                && x.TypeId == typeId
                                && x.IsActive
                                && x.SchoolId ==
                                    _user.SchoolId);

                    if (!exists)
                    {
                        status = true;

                        message = "Available";
                    }
                    else
                    {
                        message =
                            "Invoice number already exists";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            return Json(new
            {
                Status = status,

                Message = message
            });
        }
        [HttpPost]
        public async Task<IActionResult> SearchInvoiceForAssets(AssetsLiabilityModel model)
        {
            int typeId =
                Convert.ToInt32(model.TypeId);

            var data = await _Entities
                .TbAssetsLiabilityData
                .FirstOrDefaultAsync(x =>
                    x.TypeId == typeId
                    && x.SchoolId == _user.SchoolId
                    && x.IsActive
                    && x.InviceNumber ==
                        model.SearchInviceNumber);

            model.SchoolId = _user.SchoolId;

            if (data != null)
            {
                model.EntryDateString =
                    data.EntryDate
                        .ToString("dd-MM-yyyy");

                model.HeadId =
                    data.HeadId;

                model.Amount =
                    data.Amount;

                model.AddStatus =
                    data.AddStatus;

                model.Narration =
                    data.Narration;

                model.InviceNumber =
                    data.InviceNumber;

                model.TypeId =
                    typeId == 0
                    ? AssetsLiabilityType.Assets
                    : AssetsLiabilityType.Liability;

                model.Id =
                    data.Id;

                return PartialView(
                    "~/Views/Journal/_pv_Edit_Assets.cshtml",
                    model);
            }

            return PartialView(
                "~/Views/Journal/_pv_Add_Assests.cshtml",
                model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssests(
    AssetsLiabilityModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var assets =
                    new TbAssetsLiabilityDatum();

                assets.TypeId =
                    Convert.ToInt32(
                        model.TypeId);

                // Entry Date
                try
                {
                    if (!string.IsNullOrWhiteSpace(
                        model.EntryDateString))
                    {
                        string[] splitData =
                            model.EntryDateString
                                .Split('-');

                        var dd = splitData[0];

                        var mm = splitData[1];

                        var yyyy = splitData[2];

                        var date =
                            $"{mm}-{dd}-{yyyy}";

                        assets.EntryDate =
                            Convert.ToDateTime(date);
                    }
                }
                catch
                {
                }

                assets.InviceNumber =
                    model.InviceNumber;

                assets.HeadId =
                    model.HeadId;

                assets.Amount =
                    model.Amount;

                assets.Narration =
                    string.IsNullOrWhiteSpace(
                        model.Narration)
                    ? " "
                    : model.Narration;

                assets.SchoolId =
                    _user.SchoolId;

                assets.UserId =
                    _user.UserId;

                assets.IsActive = true;

                assets.TimeStamp =
                    CurrentTime;

                assets.AddStatus =
                    model.AddStatus;

                await _Entities
                    .TbAssetsLiabilityData
                    .AddAsync(assets);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                msg = "Success";

                // Update Invoice Number
                var invoice = await _Entities
                    .TbAssetsLiabilityIds
                    .FirstOrDefaultAsync(x =>
                        x.SchoolId ==
                        _user.SchoolId);

                if (invoice != null)
                {
                    if (assets.TypeId == 0)
                    {
                        invoice.AssetsId += 1;
                    }
                    else
                    {
                        invoice.LiabilityId += 1;
                    }

                    await _Entities
                        .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                msg = status
                    ? "Successfully!"
                    : "Failed!"
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditAssets(AssetsLiabilityModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                var data = await _Entities
                    .TbAssetsLiabilityData
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.Id
                        && x.IsActive
                        && x.SchoolId == _user.SchoolId);

                if (data != null)
                {
                    // Entry Date
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(
                            model.EntryDateString))
                        {
                            string[] splitData =
                                model.EntryDateString
                                    .Split('-');

                            var dd = splitData[0];

                            var mm = splitData[1];

                            var yyyy = splitData[2];

                            var date =
                                $"{mm}-{dd}-{yyyy}";

                            data.EntryDate =
                                Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {
                    }

                    data.InviceNumber =
                        model.InviceNumber;

                    data.AddStatus =
                        model.AddStatus;

                    data.HeadId =
                        model.HeadId;

                    data.Amount =
                        model.Amount;

                    data.Narration =
                        !string.IsNullOrWhiteSpace(
                            model.Narration)
                        ? model.Narration
                        : " ";

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    msg = status
                        ? "Successful"
                        : "No Changes";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,

                message = msg
            });
        }
        //---------------------------------------------------------------Trial Balance--------------------------------

        public IActionResult TrialBalance()
        {
            TrialBalanceModel model = new TrialBalanceModel();
            model.Today = CurrentTime;
            model.StartDate = CurrentTime;
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult SearchTrialBalanceData(string id)
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
                new TrialBalanceModel
                {
                    SchoolId = _user.SchoolId,

                    StartDate = start,

                    Today = end
                };

            return PartialView(
                "~/Views/Journal/_pv_TrialBalanceList.cshtml",
                model);
        }
        //---------------------------------------------------------------Balance Sheet--------------------------------
        public IActionResult BalanceSheet()
        {
            TrialBalanceModel model = new TrialBalanceModel();
            model.Today = CurrentTime;
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        //---------------------------------------------------------------Bank Statement--------------------------------
        public IActionResult BankStatement()
        {
            BankStatementModel model = new BankStatementModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = new DateTime(CurrentTime.Year, CurrentTime.Month, 1);
            model.EndDate = model.StartDate.AddMonths(1).AddDays(-1);
            model.BankId = 0;
            return View(model);
        }
        public IActionResult SearchBankBalanceData(string id)
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
                    Convert.ToInt64(checkId);
            }
            catch
            {
                bankId = 0;
            }

            var model =
                new BankStatementModel
                {
                    SchoolId = _user.SchoolId,

                    StartDate = start,

                    EndDate = end,

                    BankId = bankId
                };

            return PartialView(
                "~/Views/Journal/_pv_BankStatementList.cshtml",
                model);
        }
        //---------------------------------------------------------------Receipt Payment--------------------------------
        public IActionResult ReceiptPayment()
        {
            ReceiptPaymentModel model = new ReceiptPaymentModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = CurrentTime;
            model.EndDate = CurrentTime;
            return View(model);
        }
        public IActionResult SearchReceiptPaymentData(string id)
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
                new ReceiptPaymentModel
                {
                    SchoolId = _user.SchoolId,

                    StartDate = start,

                    EndDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_ReceiptPaymentList.cshtml",
                model);
        }
        public IActionResult SearchReceiptPaymentBankData(string id)
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
                new ReceiptPaymentModel
                {
                    StartDate = start,

                    EndDate = end,

                    SchoolId = _user.SchoolId
                };

            return PartialView(
                "~/Views/Journal/_pv_ReceiptPaymentBankList.cshtml",
                model);
        }
        public IActionResult ReceiptPaymentAmount(string id)
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
                new ReceiptPaymentModel
                {
                    SchoolId = _user.SchoolId,

                    StartDate = start,

                    EndDate = end
                };

            return PartialView(
                "~/Views/Journal/_pv_ReceiptPaymentAmount.cshtml",
                model);
        }
        public async Task<IActionResult> Checkaccounthead(string id)
        {
            bool status = false;

            string msg = "";

            bool exists = await _Entities
                .TbAccountHeads
                .AnyAsync(x =>
                    x.AccHeadName.Trim().ToUpper() ==
                        id.Trim().ToUpper()
                    && x.IsActive
                    && x.SchoolId ==
                        _user.SchoolId);

            if (exists)
            {
                status = true;

                msg = "Account Head already Exists";
            }

            return Json(new
            {
                status = status,

                msg = msg
            });
        }

    }
}






