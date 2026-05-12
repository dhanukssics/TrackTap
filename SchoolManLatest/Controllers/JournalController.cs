using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.ClassLibrary;
using TrackTap.Data;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class JournalController : BaseController
    {
        // GET: Journal
        public IActionResult AccountSettings()
        {
            var model = new AddAccountHeadModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object SubmitAddAccountHead(AddAccountHeadModel model)
        {
            string msg = "Failed";
            bool status = false;
            var head = _Entities.tb_AccountHead.Create();
            var account = _Entities.tb_AccountHead.Where(x => x.AccountId == model.AccountHeadId).FirstOrDefault();
            try
            {
                //basheer on 29/01/2018 touppercase
                if (account == null)
                {

                    head.AccHeadName = model.AccountHeadName.ToUpper();
                    head.SchoolId = _user.SchoolId;
                    head.IsActive = true;
                    head.TimeStamp = CurrentTime;
                    _Entities.tb_AccountHead.Add(head);
                    status = _Entities.SaveChanges() > 0;
                    msg = "Success";
                    if (status)
                    {
                        if (_Entities.tb_SubLedgerData.Any(x => x.SubLedgerName == model.AccountHeadName.ToUpper() && x.IsActive))
                        {
                            msg = "failed";
                        }
                        else
                        {
                            var sub = _Entities.tb_SubLedgerData.Create();
                            sub.SubLedgerName = "--";
                            sub.AccHeadId = head.AccountId;
                            sub.IsActive = true;
                            sub.TimeStamp = CurrentTime;
                            _Entities.tb_SubLedgerData.Add(sub);
                            status = _Entities.SaveChanges() > 0;
                            msg = "Success";
                        }
                    }
                    else
                    {
                        account.AccHeadName = model.AccountHeadName;
                        account.TimeStamp = DateTime.Now;
                        status = _Entities.SaveChanges() > 0;
                        if(status)
                        {
                            msg = "Success";
                        }
                        else
                        {
                            msg = "failed";
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = status ? "Account Head added successfully !" : "Failed to add Account Head !" }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetSubLedgerListList()
        {
            var model = new AddAccountHeadModel();
            model.SchoolId = _user.SchoolId;
            var input = _Entities.tb_AccountHead.Where(x => x.IsActive && x.SchoolId == _user.SchoolId).OrderBy(x => x.AccHeadName).ToList();
            ViewBag.store = input.Select(x => new SelectListItem { Text = x.AccHeadName, Value = x.AccountId.ToString() }).ToList();
            return PartialView("~/Views/Journal/_pv_Add_SubLedger.cshtml", model);
        }
        public object SubmitAddSubLedger(AddAccountHeadModel model)
        {
            string msg = "Failed";
            bool status = false;
            var sub = _Entities.tb_SubLedgerData.Create();
            try
            {
                sub.SubLedgerName = model.SubLedger;
                sub.AccHeadId = model.AccountHeadId;
                sub.IsActive = true;
                sub.TimeStamp = CurrentTime;
                _Entities.tb_SubLedgerData.Add(sub);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = status ? "Sub Ledger added successfully !" : "Failed to add Sub Ledger !" }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetAccountHeadDataList()
        {
            var model = new AddAccountHeadModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_AcchountHeadList.cshtml", model);
        }
        //Code added by Gayathri A (30/10/2023) Edit operation in Accountsetting
        public object EditSubLedger(string id)
        {
            bool status = false;
            string msg = "False";
            long subId = Convert.ToInt64(id);
            var head = _Entities.tb_AccountHead.Where(x => x.AccountId == subId && x.IsActive).FirstOrDefault();
            msg = head.AccHeadName;

            //var sub = _Entities.tb_SubLedgerData.FirstOrDefault(x => x.LedgerId == subId && x.IsActive);
            //if (sub != null)
            //{
            //    int count = _Entities.tb_SubLedgerData.Where(x => x.AccHeadId == sub.AccHeadId && x.IsActive).Count();
            //    if (count == 1)
            //    {
            //        var head = _Entities.tb_AccountHead.Where(x => x.AccountId == sub.AccHeadId && x.IsActive).FirstOrDefault();
            //        msg = head.AccHeadName;

            //    }

            //}

            return Json(new { msg = msg,subId= subId }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteSubLedger(string id)
        {
            bool status = false;
            string msg = "False";
            long subId = Convert.ToInt64(id);
            var sub = _Entities.tb_SubLedgerData.FirstOrDefault(x => x.LedgerId == subId && x.IsActive);
            if (sub != null)
            {
                int count = _Entities.tb_SubLedgerData.Where(x => x.AccHeadId == sub.AccHeadId && x.IsActive).Count();
                if (count == 1)
                {
                    var head = _Entities.tb_AccountHead.Where(x => x.AccountId == sub.AccHeadId && x.IsActive).FirstOrDefault();
                    if (head != null)
                    {
                        head.IsActive = false;
                        status = _Entities.SaveChanges() > 0;
                    }
                }
                sub.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------Day Book Module Starts----------------------
        public IActionResult DayBook()
        {
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object GetVoucherNo(string id)
        {
            string voucherNumber = "";
            bool status = true;
            string msg = "Failed";
            int typeId = Convert.ToInt32(id);
            var data = _Entities.tb_DayBookId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (data != null)
            {
                if (typeId == 0)
                    voucherNumber = Convert.ToString(data.ExpenseId);
                else
                    voucherNumber = Convert.ToString(data.IncomeId);
                msg = "Success";
            }
            else
            {
                var dayBookId = _Entities.tb_DayBookId.Create();
                dayBookId.SchoolId = _user.SchoolId;
                dayBookId.IncomeId = 1;
                dayBookId.ExpenseId = 1;
                _Entities.tb_DayBookId.Add(dayBookId);
                status = _Entities.SaveChanges() > 0;
                if (typeId == 0)
                    voucherNumber = Convert.ToString(dayBookId.ExpenseId);
                else
                    voucherNumber = Convert.ToString(dayBookId.IncomeId);
                msg = "Success";
            }
            return Json(new { status = status, result = voucherNumber }, JsonRequestBehavior.AllowGet);
        }
        public object AddDayBook(AddDayBookModel model)
        {
            string msg = "Failed";
            bool status = false;
            int typeId = Convert.ToInt32(model.TypeId);
            if (_Entities.tb_DayBookData.Any(x => x.TypeId == typeId && x.VoucherNo == model.VoucherNo && x.IsActive && x.SchoolId == _user.SchoolId))
            {
                msg = "Please refresh !";
                var check = _user.tb_DayBookData.Where(x => x.TypeId == typeId && x.SchoolId == _user.SchoolId && x.IsActive).OrderByDescending(x => x.DayBookId).First();
                var idCheck = _Entities.tb_DayBookId.Where(x => x.SchoolId == _user.SchoolId).First();
                if (typeId == 0)
                {
                    if (idCheck.ExpenseId == Convert.ToInt32(check.VoucherNo))
                    {
                        var vou = _Entities.tb_DayBookId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                        vou.ExpenseId = vou.ExpenseId + 1;
                        _Entities.SaveChanges();
                        model.VoucherNo = vou.ExpenseId.ToString();
                    }
                }
                else
                {
                    if (idCheck.IncomeId == Convert.ToInt32(check.VoucherNo))
                    {
                        var vou = _Entities.tb_DayBookId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                        vou.IncomeId = vou.IncomeId + 1;
                        _Entities.SaveChanges();
                        model.VoucherNo = vou.ExpenseId.ToString();
                    }
                }

            }
            //else
            //{
            try
            {
                var dayBook = _Entities.tb_DayBookData.Create();
                dayBook.TypeId = typeId;
                try
                {
                    if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                    {
                        string[] splitData = model.EntryDateString.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var date = mm + '-' + dd + '-' + yyyy;
                        dayBook.EntryDate = Convert.ToDateTime(date);
                    }
                }
                catch
                {

                }
                //dayBook.EntryDate = model.EntryDate;
                dayBook.VoucherNo = model.VoucherNo;
                dayBook.HeadId = model.HeadId;
                dayBook.SubLedgerId = model.SubLedgerId;
                dayBook.Amount = model.Amount;
                if (model.Narration == null)
                    dayBook.Narration = " ";
                else
                    dayBook.Narration = model.Narration;
                dayBook.SchoolId = _user.SchoolId;
                dayBook.UserId = _user.UserId;
                dayBook.IsActive = true;
                dayBook.TimeStamp = CurrentTime;
                _Entities.tb_DayBookData.Add(dayBook);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
                //----- Increase Voucher nummber
                var vou = _Entities.tb_DayBookId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                if (vou != null)
                {
                    if (dayBook.TypeId == 0)
                        vou.ExpenseId = vou.ExpenseId + 1;
                    else
                        vou.IncomeId = vou.IncomeId + 1;
                    _Entities.SaveChanges();
                }
                //----- Increase Voucher nummber
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            //}
            return Json(new { status = status, msg = status ? "DayBook added successfully !" : "Failed to add DayBook !" }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SearchVoucherNo()
        {
            var model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_SearchVoucherNo.cshtml", model);
        }
        public PartialViewResult EditDayBookView()
        {
            var model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_Edit_DayBook.cshtml", model);
        }
        public PartialViewResult SearchVoucher(AddDayBookModel model)
        {
            string msg = "Failed";
            //bool status = false;
            int typeId = Convert.ToInt32(model.TypeId);
            var data = _Entities.tb_DayBookData.Where(x => x.TypeId == typeId && x.SchoolId == _user.SchoolId && x.IsActive && x.VoucherNo == model.SearchVoucherNo).FirstOrDefault();
            model.SchoolId = _user.SchoolId;
            if (data != null)
            {
                model.EntryDateString = data.EntryDate.ToString("dd-MM-yyyy");
                model.EntryDate = data.EntryDate;
                model.HeadId = data.HeadId;
                model.SubLedgerId = data.SubLedgerId;
                model.Amount = data.Amount;
                model.Narration = data.Narration;
                model.VoucherNo = model.SearchVoucherNo;
                if (typeId == 0)
                    model.TypeId = AccountType.Expense;
                else
                    model.TypeId = AccountType.Income;

                model.DayBookId = data.DayBookId;
                return PartialView("~/Views/Journal/_pv_Edit_DayBook.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/Journal/_pv_Add_DayBook.cshtml", model);
            }
        }

        public PartialViewResult AddDayBookReaload()
        {
            var model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_Add_DayBook.cshtml", model);
        }

        public object EditDayBook(AddDayBookModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var data = _Entities.tb_DayBookData.Where(x => x.DayBookId == model.DayBookId && x.IsActive && x.SchoolId == _user.SchoolId).FirstOrDefault();
                if (data != null)
                {
                    //data.TypeId = Convert.ToInt32(data.TypeId);
                    //data.EntryDate = model.EntryDate;
                    try
                    {
                        if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                        {
                            string[] splitData = model.EntryDateString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var date = mm + '-' + dd + '-' + yyyy;
                            data.EntryDate = Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {

                    }
                    data.VoucherNo = model.VoucherNo;
                    data.HeadId = model.HeadId;
                    data.SubLedgerId = model.SubLedgerId;
                    data.Amount = model.Amount;
                    if (model.Narration != null)
                        data.Narration = model.Narration;
                    else
                        data.Narration = " ";
                    status = _Entities.SaveChanges() > 0;
                    if (status == true)
                        msg = "Successful";
                    else
                        msg = "No Changes";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public object CheckVoucherNumber(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (text != string.Empty && text != null)
            {
                try
                {
                    string[] data = text.Split('~');
                    var voucherNo = data[0];
                    var typeId = Convert.ToInt32(data[1]);
                    if (_Entities.tb_DayBookData.Any(x => x.VoucherNo == voucherNo && x.TypeId == typeId && x.IsActive && x.SchoolId == _user.SchoolId))
                    {

                    }
                    else
                    {
                        Status = true;
                    }
                }
                catch
                {

                }
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult StatusRange()
        {
            var model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_ProfitOrLoss.cshtml", model);
        }
        public PartialViewResult DayBookPrint(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            try
            {
                if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                {
                    string[] splitData = model.EntryDateString.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var date = mm + '-' + dd + '-' + yyyy;
                    model.EntryDate = Convert.ToDateTime(date);
                }
            }
            catch
            {

            }
            var head = new TrackTap.Data.SubLedgerData(model.SubLedgerId);
            model.HeadName = head.AccountHeadName;
            model.SubLedger = head.SubLedgerName;
            int type = Convert.ToInt32(model.TypeId);
            if (type == 0)
                model.TypeData = "Expense";
            else
                model.TypeData = "Income";
            //return PartialView("~/Views/Journal/_pv_DayBookPrint.cshtml", model);
            return PartialView("~/Views/Journal/_pv_Print_DayBook.cshtml", model);
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
        public object SearchCashBookData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            model.startDate = start;
            model.endDate = end;
            return PartialView("~/Views/Journal/_pv_CashBookList.cshtml", model);
        }
        public object SearchCashBookDailyData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            model.startDate = start;
            model.endDate = end;
            return PartialView("~/Views/Journal/_pv_CashBookDailyReport.cshtml", model);
        }

        public object DayBookStatusData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1] + " 11:59:59 PM");
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            model.startDate = start;
            model.endDate = end;
            return PartialView("~/Views/Journal/_pv_CashBookSearchDate.cshtml", model);
        }

        public IActionResult CashBookSummary(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        //public object SearchCashBookData(AddDayBookModel model)
        public PartialViewResult SearchCashBookCashBookSummary(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            model.startDate = start;
            model.endDate = end;
            return PartialView("~/Views/Journal/_pv_CashBookListSummary.cshtml", model);
        }
        public IActionResult CashBookDailyReport(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        public object SearchCashBookDailyReportData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            model.startDate = start;
            model.endDate = end;
            return PartialView("~/Views/Journal/_pv_CashBookDailyReport.cshtml", model);
        }
        //---------------------------------------------------------------Ledger Module Starts----------------------

        public IActionResult Ledger(AddDayBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        public object SearchLedgerData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            var filterData = splitDate[2];
            AddDayBookModel model = new AddDayBookModel();
            model.SchoolId = _user.SchoolId;
            model.startDate = start;
            model.endDate = end;
            if (filterData != "0")
            {
                string[] filter = filterData.Split('!');
                try
                {
                    model.HeadId = Convert.ToInt64(filter[0]);
                    model.FilterTypeId = Convert.ToInt32(filter[1]);
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
            return PartialView("~/Views/Journal/_pv_LedgerList.cshtml", model);
        }

        //---------------------------------------------------------------Bank Details Module Starts----------------------
        public IActionResult BankDetails(BankDetailsModel model)
        {
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object SubmitAddBankName(BankDetailsModel model)
        {
            string msg = "Failed";
            bool status = false;
            var bank_one= _Entities.tb_Banks.Where(x=>x.BankId==model.BankId).FirstOrDefault();
            if(bank_one==null)
            {
                var bank = _Entities.tb_Banks.Create();
              
                    bank.BankName = model.BankName;
                    bank.SchoolId = _user.SchoolId;
                    bank.IsActive = true;
                    bank.TimeStamp = CurrentTime;
                    _Entities.tb_Banks.Add(bank);
                    status = _Entities.SaveChanges() > 0;
                    msg = "Success";
               
            }
            else
            {
                bank_one.BankName = model.BankName;
                bank_one.SchoolId = _user.SchoolId;
                bank_one.IsActive = true;
                bank_one.TimeStamp = CurrentTime;
                    //_Entities.tb_Banks.Add(bank_one);
                    status = _Entities.SaveChanges() > 0;
                    msg = "Success";

             }
           
         
            return Json(new { status = status, message = status ? "Bank Details added successfully !" : "Failed to add Bank Details !" }, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult GetBankDetailsList()
        {
            var model = new BankDetailsModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_BankDetailsList.cshtml", model);
        }
        public object DeleteBank(string id)
        {
            bool status = false;
            string msg = "False";
            long bankId = Convert.ToInt64(id);
            var bank = _Entities.tb_Banks.FirstOrDefault(x => x.BankId == bankId && x.IsActive);
            if (bank != null)
            {
                bank.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //Created by gayathri(2/11/2023)For bank edit option
        public object EditBank(string id)
        {
            bool status = false;
            string msg = "False";
           
            long bankId = Convert.ToInt64(id);
            var bank = _Entities.tb_Banks.FirstOrDefault(x => x.BankId == bankId && x.IsActive);
            if (bank != null)
            {
                //bank.IsActive = false;
                //status = _Entities.SaveChanges() > 0;
                msg = bank.BankName;
                
            }
            //msg = status ? "Deleted" : "Failed";
            return Json(new {msg = msg, id = id }, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------Bank Book Module Starts----------------------
        public IActionResult BankBook(BankBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object GetBankVoucherNo(string id)
        {
            string voucherNumber = "";
            bool status = true;
            string msg = "Failed";
            int typeId = Convert.ToInt32(id);
            var data = _Entities.tb_BankBookId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (data != null)
            {
                if (typeId == 0)
                    voucherNumber = Convert.ToString(data.DepositId);
                else
                    voucherNumber = Convert.ToString(data.WithdrawId);
                msg = "Success";
            }
            else
            {
                var dayBookId = _Entities.tb_BankBookId.Create();
                dayBookId.SchoolId = _user.SchoolId;
                dayBookId.DepositId = 1;
                dayBookId.WithdrawId = 1;
                _Entities.tb_BankBookId.Add(dayBookId);
                status = _Entities.SaveChanges() > 0;
                if (typeId == 0)
                    voucherNumber = Convert.ToString(dayBookId.DepositId);
                else
                    voucherNumber = Convert.ToString(dayBookId.WithdrawId);
                msg = "Success";
            }
            return Json(new { status = status, result = voucherNumber }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddBankBookReaload()
        {
            var model = new BankBookModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_Add_BankBook.cshtml", model);
        }
        public PartialViewResult SearchVoucherNoForBank()
        {
            var model = new BankBookModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_SearchVoucherNo_Bank.cshtml", model);
        }
        public PartialViewResult SearchVoucherForBank(BankBookModel model)
        {
            string msg = "Failed";
            bool status = false;
            int typeId = Convert.ToInt32(model.TypeId);
            var data = _Entities.tb_BankBookData.Where(x => x.TypeId == typeId && x.SchoolId == _user.SchoolId && x.IsActive && x.VoucherNumber == model.SearchVoucherNo).FirstOrDefault();
            model.SchoolId = _user.SchoolId;
            if (data != null)
            {
                model.EntryDateString = data.EntryDate.ToString("dd-MM-yyyy");
                model.EntryDate = data.EntryDate;
                model.HeadId = data.HeadId;
                model.SubLedgerId = data.SubledgerId;
                model.Amount = data.Amount;
                model.Narration = data.Narration;
                model.VoucherNo = model.SearchVoucherNo;
                if (typeId == 0)
                    model.TypeId = BankType.Deposit;
                else
                    model.TypeId = BankType.Withdraw;
                model.BankBookId = data.Id;
                model.BankId = data.BankId;
                model.ChequeNo = data.ChequeNo;
                if (data.ChequeDate != null)
                {
                    model.ChequeDate = data.ChequeDate ?? CurrentTime;
                    model.ChequeDateString = model.ChequeDate.ToString("dd-MM-yyyy");
                }
                return PartialView("~/Views/Journal/_pv_Edit_BankBook.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/Journal/_pv_Add_BankDetails.cshtml", model);
            }
        }
        public object CheckVoucherNumberForBank(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (text != string.Empty && text != null)
            {
                try
                {
                    string[] data = text.Split('~');
                    var voucherNo = data[0];
                    var typeId = Convert.ToInt32(data[1]);
                    if (_Entities.tb_BankBookData.Any(x => x.VoucherNumber == voucherNo && x.TypeId == typeId && x.IsActive && x.SchoolId == _user.SchoolId))
                    {

                    }
                    else
                    {
                        Status = true;
                    }
                }
                catch
                {

                }
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public object AddBankBook(BankBookModel model)
        {
            string msg = "Failed";
            bool status = false;
            var bankBook = _Entities.tb_BankBookData.Create();
            try
            {
                bankBook.TypeId = Convert.ToInt32(model.TypeId);
                try
                {
                    if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                    {
                        string[] splitData = model.EntryDateString.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var date = mm + '-' + dd + '-' + yyyy;
                        bankBook.EntryDate = Convert.ToDateTime(date);
                    }
                }
                catch
                {

                }
                try
                {
                    if (model.ChequeDateString != string.Empty && model.ChequeDateString != null)
                    {
                        string[] splitData = model.ChequeDateString.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var date = mm + '-' + dd + '-' + yyyy;
                        bankBook.ChequeDate = Convert.ToDateTime(date);
                    }
                }
                catch
                {

                }
                bankBook.VoucherNumber = model.VoucherNo;
                bankBook.HeadId = model.HeadId;
                bankBook.SubledgerId = model.SubLedgerId;
                bankBook.Amount = model.Amount;
                if (model.Narration == null)
                    bankBook.Narration = " ";
                else
                    bankBook.Narration = model.Narration;
                bankBook.SchoolId = _user.SchoolId;
                bankBook.UserId = _user.UserId;
                bankBook.IsActive = true;
                bankBook.TimeStamp = CurrentTime;
                bankBook.ChequeNo = model.ChequeNo;
                bankBook.BankId = model.BankId;
                if (bankBook.TypeId != 0)
                {
                    if (model.iswithdraw != true)
                        bankBook.IsWithdraw = false;
                    else
                        bankBook.IsWithdraw = true;

                }
                _Entities.tb_BankBookData.Add(bankBook);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
                var vou = _Entities.tb_BankBookId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                if (vou != null)
                {
                    if (bankBook.TypeId == 0)
                        vou.DepositId = vou.DepositId + 1;
                    else
                        vou.WithdrawId = vou.WithdrawId + 1;
                    _Entities.SaveChanges();
                }

                ///////////////////////////////////
                if (bankBook.TypeId != 0)
                {
                    var xx = _Entities.tb_DayBookData.Where(x => x.SchoolId == _user.SchoolId && x.HeadId == model.HeadId && x.SubLedgerId == model.SubLedgerId && x.Amount == model.Amount && x.Narration == model.Narration).FirstOrDefault();
                    if (xx == null)
                    {

                        if (model.iswithdraw != true)
                        {

                            long Vouchr = 1;
                            var vouch1 = _Entities.tb_DayBookIdBank.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                            if (vouch1 != null)
                            {

                                Vouchr = vouch1.ExpenseId;
                            }


                            var dayBook = _Entities.tb_DayBookData.Create();
                            dayBook.TypeId = 0;  //Expense
                            try
                            {
                                //if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                                //{
                                //    string[] splitData = model.EntryDateString.Split('-');
                                //    var dd = splitData[0];
                                //    var mm = splitData[1];
                                //    var yyyy = splitData[2];
                                //    var date = mm + '-' + dd + '-' + yyyy;
                                //    dayBook.EntryDate = Convert.ToDateTime(date);
                                //}
                            }
                            catch
                            {

                            }

                            dayBook.EntryDate = bankBook.EntryDate;
                            dayBook.VoucherNo = "BK" + Vouchr;
                            dayBook.HeadId = model.HeadId;
                            dayBook.SubLedgerId = model.SubLedgerId;
                            dayBook.Amount = model.Amount;
                            if (model.Narration == null)
                                dayBook.Narration = " ";
                            else
                                dayBook.Narration = model.Narration;
                            dayBook.SchoolId = _user.SchoolId;
                            dayBook.UserId = _user.UserId;
                            dayBook.IsActive = true;
                            dayBook.TimeStamp = CurrentTime;
                            dayBook.IsWithdraw = false;
                            _Entities.tb_DayBookData.Add(dayBook);
                            status = _Entities.SaveChanges() > 0;
                            msg = "Success";
                            //----- Increase Voucher nummber
                            var vouch = _Entities.tb_DayBookIdBank.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                            if (vouch != null)
                            {

                                vouch.ExpenseId = vouch.ExpenseId + 1;
                                _Entities.SaveChanges();
                            }
                            else
                            {
                                var voucherTable = _Entities.tb_DayBookIdBank.Create();
                                voucherTable.SchoolId = _user.SchoolId;
                                voucherTable.ExpenseId = 2;
                                voucherTable.IncomeId = 1;
                                _Entities.tb_DayBookIdBank.Add(voucherTable);
                                _Entities.SaveChanges();

                            }
                            //----- Increase Voucher nummber
                        }
                    }
                    ///////////////////////////////////
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = status ? "BankBook added successfully !" : "Failed to add BankBook !" }, JsonRequestBehavior.AllowGet);
        }
        public object EditBankBook(BankBookModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var data = _Entities.tb_BankBookData.Where(x => x.Id == model.BankBookId && x.IsActive && x.SchoolId == _user.SchoolId).FirstOrDefault();
                if (data != null)
                {
                    try
                    {
                        if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                        {
                            string[] splitData = model.EntryDateString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var date = mm + '-' + dd + '-' + yyyy;
                            data.EntryDate = Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {

                    }
                    data.VoucherNumber = model.VoucherNo;
                    data.HeadId = model.HeadId;
                    data.SubledgerId = model.SubLedgerId;
                    data.Amount = model.Amount;
                    if (model.Narration != null)
                        data.Narration = model.Narration;
                    else
                        data.Narration = " ";
                    if (model.ChequeNo != null)
                        data.ChequeNo = model.ChequeNo;
                    else
                        data.ChequeNo = " ";
                    data.BankId = model.BankId;
                    try
                    {
                        if (model.ChequeDateString != string.Empty && model.ChequeDateString != null)
                        {
                            string[] splitData = model.ChequeDateString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var date = mm + '-' + dd + '-' + yyyy;
                            data.ChequeDate = Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {

                    }
                    status = _Entities.SaveChanges() > 0;
                    if (status == true)
                        msg = "Successful";
                    else
                        msg = "No Changes";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult BankBookPrint(BankBookModel model)
        {
            model.SchoolId = _user.SchoolId;
            try
            {
                if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                {
                    string[] splitData = model.EntryDateString.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var date = mm + '-' + dd + '-' + yyyy;
                    model.EntryDate = Convert.ToDateTime(date);
                }
            }
            catch
            {

            }
            var head = new TrackTap.Data.SubLedgerData(model.SubLedgerId);
            model.HeadName = head.AccountHeadName;
            model.SubLedger = head.SubLedgerName;
            model.BankName = _Entities.tb_Banks.Where(x => x.BankId == model.BankId && x.IsActive).Select(x => x.BankName).FirstOrDefault();
            try
            {
                if (model.ChequeDateString != string.Empty && model.ChequeDateString != null)
                {
                    string[] splitData = model.ChequeDateString.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var date = mm + '-' + dd + '-' + yyyy;
                    model.ChequeDate = Convert.ToDateTime(date);
                }
            }
            catch
            {

            }
            int type = Convert.ToInt32(model.TypeId);
            if (type == 0)
                model.TypeData = "Deposit";
            else
                model.TypeData = "Withdraw";
            return PartialView("~/Views/Journal/_pv_BankBookPrint.cshtml", model);
        }
        public PartialViewResult CurrentBankBalance(string id)
        {
            var model = new BankBookModel();
            model.SchoolId = _user.SchoolId;
            model.BankId = Convert.ToInt64(id);
            return PartialView("~/Views/Journal/_pv_BankBalance.cshtml", model);
        }
        public object CheckWithdrawAmount(string text)// From Add
        {
            bool Status = false;
            string Message = "Failed";
            try
            {
                string[] data = text.Split('~');
                var bankId = Convert.ToInt64(data[1]);
                decimal amountWithdraw = Convert.ToDecimal(data[0]);
                var amount = new TrackTap.Data.School(_user.SchoolId).GetBankCurrentBalance(bankId);
                if (amountWithdraw <= amount)
                {
                }
                else
                {
                    Status = true;
                    Message = "Dont have this much amount !";
                }
            }
            catch
            {

            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public object CheckWithdrawAmountEdit(string text)// From Edit
        {
            string[] data = text.Split('~');
            bool Status = false;
            string Message = "Failed";
            try
            {
                decimal amountWithdraw = Convert.ToDecimal(data[0]);
                long bankId = Convert.ToInt64(data[1]);
                var accountbankId = Convert.ToInt64(data[2]);
                var bankdata = _Entities.tb_BankBookData.Where(x => x.Id == bankId).FirstOrDefault();
                var amount = new TrackTap.Data.School(_user.SchoolId).GetBankCurrentBalance(accountbankId);
                amount = amount + bankdata.Amount;
                if (amountWithdraw <= amount)
                {
                }
                else
                {
                    Status = true;
                    Message = "Dont have this much amount !";
                }
            }
            catch
            {

            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------Assets / Liabilities Module Starts-----------

        public IActionResult Assets()
        {
            var model = new AssetsLiabilityModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object GetAssestsInvoiceNumber(string id)
        {
            string invoiceNumber = "";
            bool status = true;
            string msg = "Failed";
            int typeId = Convert.ToInt32(id);
            var data = _Entities.tb_AssetsLiabilityId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (data != null)
            {
                if (typeId == 0)
                    invoiceNumber = "A" + Convert.ToString(data.AssetsId);
                else
                    invoiceNumber = "L" + Convert.ToString(data.LiabilityId);
                msg = "Success";
            }
            else
            {
                var assetsId = _Entities.tb_AssetsLiabilityId.Create();
                assetsId.SchoolId = _user.SchoolId;
                assetsId.AssetsId = 1;
                assetsId.LiabilityId = 1;
                _Entities.tb_AssetsLiabilityId.Add(assetsId);
                status = _Entities.SaveChanges() > 0;
                if (typeId == 0)
                    invoiceNumber = "A" + Convert.ToString(assetsId.AssetsId);
                else
                    invoiceNumber = "L" + Convert.ToString(assetsId.LiabilityId);
                msg = "Success";
            }
            return Json(new { status = status, result = invoiceNumber }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult SearchVoucherNoForAssets()
        {
            var model = new AssetsLiabilityModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_SearchVoucherNo_Assets.cshtml", model);
        }
        public PartialViewResult AddAssetsReaload()
        {
            var model = new AssetsLiabilityModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_Add_Assests.cshtml", model);
        }

        public PartialViewResult AssestsPrint(AssetsLiabilityModel model)
        {
            model.SchoolId = _user.SchoolId;
            try
            {
                if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                {
                    string[] splitData = model.EntryDateString.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var date = mm + '-' + dd + '-' + yyyy;
                    model.EntryDate = Convert.ToDateTime(date);
                }
            }
            catch
            {

            }
            var head = new TrackTap.Data.AccountHead(model.HeadId);
            model.HeadName = head.AccHeadName;
            int type = Convert.ToInt32(model.TypeId);
            if (type == 0)
                model.TypeData = "Assets";
            else
                model.TypeData = "Liability";
            if (model.AddStatus == true)
                model.AddStatusString = "Yes";
            else
                model.AddStatusString = "No";
            return PartialView("~/Views/Journal/_pv_AssetsPrint.cshtml", model);
        }

        public object CheckInvoiceNumberForAssets(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (text != string.Empty && text != null)
            {
                try
                {
                    string[] data = text.Split('~');
                    var invoiceNo = data[0];
                    var typeId = Convert.ToInt32(data[1]);
                    if (_Entities.tb_AssetsLiabilityData.Any(x => x.InviceNumber == invoiceNo && x.TypeId == typeId && x.IsActive && x.SchoolId == _user.SchoolId))
                    {

                    }
                    else
                    {
                        Status = true;
                    }
                }
                catch
                {

                }
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SearchInvoiceForAssets(AssetsLiabilityModel model)
        {
            string msg = "Failed";
            bool status = false;
            int typeId = Convert.ToInt32(model.TypeId);
            var data = _Entities.tb_AssetsLiabilityData.Where(x => x.TypeId == typeId && x.SchoolId == _user.SchoolId && x.IsActive && x.InviceNumber == model.SearchInviceNumber).FirstOrDefault();
            model.SchoolId = _user.SchoolId;
            if (data != null)
            {
                model.EntryDateString = data.EntryDate.ToString("dd-MM-yyyy");
                model.HeadId = data.HeadId;
                model.Amount = data.Amount;
                model.AddStatus = data.AddStatus;
                model.Narration = data.Narration;
                model.InviceNumber = data.InviceNumber;
                if (typeId == 0)
                    model.TypeId = AssetsLiabilityType.Assets;
                else
                    model.TypeId = AssetsLiabilityType.Liability;
                model.Id = data.Id;
                return PartialView("~/Views/Journal/_pv_Edit_Assets.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/Journal/_pv_Add_Assests.cshtml", model);
            }
        }

        public object AddAssests(AssetsLiabilityModel model)
        {
            string msg = "Failed";
            bool status = false;
            var assets = _Entities.tb_AssetsLiabilityData.Create();
            try
            {
                assets.TypeId = Convert.ToInt32(model.TypeId);
                try
                {
                    if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                    {
                        string[] splitData = model.EntryDateString.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var date = mm + '-' + dd + '-' + yyyy;
                        assets.EntryDate = Convert.ToDateTime(date);
                    }
                }
                catch
                {

                }
                assets.InviceNumber = model.InviceNumber;
                assets.HeadId = model.HeadId;
                assets.Amount = model.Amount;
                if (model.Narration == null)
                    assets.Narration = " ";
                else
                    assets.Narration = model.Narration;
                assets.SchoolId = _user.SchoolId;
                assets.UserId = _user.UserId;
                assets.IsActive = true;
                assets.TimeStamp = CurrentTime;
                assets.AddStatus = model.AddStatus;
                _Entities.tb_AssetsLiabilityData.Add(assets);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
                var invoice = _Entities.tb_AssetsLiabilityId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                if (invoice != null)
                {
                    if (assets.TypeId == 0)
                        invoice.AssetsId = invoice.AssetsId + 1;
                    else
                        invoice.LiabilityId = invoice.LiabilityId + 1;
                    _Entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = status ? "Successfully !" : "Failed !" }, JsonRequestBehavior.AllowGet);
        }

        public object EditAssets(AssetsLiabilityModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var data = _Entities.tb_AssetsLiabilityData.Where(x => x.Id == model.Id && x.IsActive && x.SchoolId == _user.SchoolId).FirstOrDefault();
                if (data != null)
                {

                    try
                    {
                        if (model.EntryDateString != string.Empty && model.EntryDateString != null)
                        {
                            string[] splitData = model.EntryDateString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var date = mm + '-' + dd + '-' + yyyy;
                            data.EntryDate = Convert.ToDateTime(date);
                        }
                    }
                    catch
                    {

                    }
                    data.InviceNumber = model.InviceNumber;
                    data.AddStatus = model.AddStatus;
                    data.HeadId = model.HeadId;
                    data.Amount = model.Amount;
                    if (model.Narration != null)
                        data.Narration = model.Narration;
                    else
                        data.Narration = " ";
                    status = _Entities.SaveChanges() > 0;
                    if (status == true)
                        msg = "Successful";
                    else
                        msg = "No Changes";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
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
        public object SearchTrialBalanceData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            TrialBalanceModel model = new TrialBalanceModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = start;
            model.Today = end;
            return PartialView("~/Views/Journal/_pv_TrialBalanceList.cshtml", model);
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
        public object SearchBankBalanceData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            var checkId = splitDate[2];
            long bankId = 0;
            try
            {
                bankId = Convert.ToInt64(checkId);
            }
            catch
            {
                bankId = 0;
            }

            BankStatementModel model = new BankStatementModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = start;
            model.EndDate = end;
            model.BankId = bankId;
            return PartialView("~/Views/Journal/_pv_BankStatementList.cshtml", model);
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
        public object SearchReceiptPaymentData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            ReceiptPaymentModel model = new ReceiptPaymentModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = start;
            model.EndDate = end;
            return PartialView("~/Views/Journal/_pv_ReceiptPaymentList.cshtml", model);
        }
        public object SearchReceiptPaymentBankData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            ReceiptPaymentModel model = new ReceiptPaymentModel();
            model.StartDate = start;
            model.EndDate = end;
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Journal/_pv_ReceiptPaymentBankList.cshtml", model);
        }
        public object ReceiptPaymentAmount(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            ReceiptPaymentModel model = new ReceiptPaymentModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = start;
            model.EndDate = end;
            return PartialView("~/Views/Journal/_pv_ReceiptPaymentAmount.cshtml", model);
        }
        public object Checkaccounthead(string id)
        {

            bool status = false;
            string msg = "";
            var accheadcount = _Entities.tb_AccountHead.Where(x => x.AccHeadName.Trim().ToUpper() == id.Trim().ToUpper() && x.IsActive == true && x.SchoolId == _user.SchoolId).ToList();
            if (accheadcount != null && accheadcount.Count > 0)
            {
                status = true;
                msg = "Account Head already Exists";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public object UpdateAccountHead(string id)
        {

            bool status = false;
            string msg = "";
            string[] splitData = id.Split('~');
            string  accname = Convert.ToString(splitData[0]);
            Int64 accid = Convert.ToInt64(splitData[1]);
            var accheadcount = _Entities.tb_AccountHead.Where(x => x.AccountId== accid).FirstOrDefault();
            if (accheadcount != null)
            {
                accheadcount.AccHeadName = accname;
                accheadcount.TimeStamp = DateTime.Now;
                if (_Entities.SaveChanges() > 0)
                {
                    msg = "success";
                    status = true;
                }
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

    }
}






