using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.ClassLibrary;
using TrackTap.DataLibrary;
using TrackTap.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using TrackTap.MapModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

using IronBarCode;
using System.Drawing;
using System.Web.UI;
using System.Collections;
using System.Drawing.Imaging;
using System.Globalization;

using TrackTap.Service.Helper;
using System.Web.Script.Serialization;
using TrackTap.PostModel;
using TrackTap.Helper;

using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using TrackTap.ClassLibrary.Utility;
using TrackTap.Data;
using System.Threading;
using System.IO;




namespace TrackTap.Controllers
{
    public class LaboratoryController : BaseController
    {
        // GET: Laboratory
        public IActionResult StockUpdate()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public object GetAllSupplierName()
        {
            long SchoolId = _user.SchoolId;
            var result = new TrackTap.Data.WebsiteService().GetAllSupplierList(SchoolId);
            return Json(new { Status = true, Message = "", result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object SubmitStockUpdate(StockUpdateModel model)
        {
            var message = "failed";
            var status = false;
            var stock = new tb_StockUpdate();
            var pstock = new tb_Purchase();
            //var stockExist = _Entities.tb_StockUpdate.Where(z => z.Item == model.Item && z.StockQuantity != null).FirstOrDefault();
            var stockExist = _Entities.tb_StockUpdate.Where(z => z.Item == model.Item ).FirstOrDefault();


            if (stockExist != null)
            {
               // var stockQuantity = stockExist.StockQuantity;
                var stockQuantity = stockExist.StockId;

                if (_Entities.SaveChanges() > 0)
                {
                    message = "success";
                    status = true;
                }
                return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);

            }





            else
            {
                stock.CategoryId = model.CategoryId;
                stock.Item = model.Item;
                stock.Price = model.Price;
                stock.PurchaseId = model.PurchaseId;
                stock.SupplierName = model.SupplirName;
                stock.StockStatus = (int)model.Status;
                stock.SchoolId = _user.SchoolId;
                stock.UserId = _user.UserId;
                //stock.StockQuantity = model.Quantity;

                stock.IsActive = true;
                stock.TimeStamp = CurrentTime;

                //purchase

                pstock.CategoryId = model.CategoryId;
                pstock.Item = model.Item;
                pstock.Price = model.Price;
                pstock.PurchaseId = model.PurchaseId;
                pstock.SupplierName = model.SupplirName;
                pstock.StockStatus = (int)model.Status;
                pstock.SchoolId = _user.SchoolId;
                pstock.UserId = _user.UserId;
                pstock.StockQuantity = model.Quantity;

                pstock.IsActive = true;
                pstock.TimeStamp = CurrentTime;

                //purchase



                _Entities.tb_StockUpdate.Add(stock);
                _Entities.tb_Purchase.Add(pstock);

                if (_Entities.SaveChanges() > 0)
                {
                    message = "success";
                    status = true;
                }
                return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
            }



            // var stock = new tb_StockUpdate();


        }
        public object DeleteStock(string id)
        {
            bool status = false;
            string message = "Failed";
            long stockId = Convert.ToInt64(id);
            var stock = _Entities.tb_StockUpdate.Where(z => z.StockId == stockId).FirstOrDefault();
            stock.IsActive = false;
            //   _Entities.SP_DeleteStockByStockId(Convert.ToInt32(stockId));
            status = _Entities.SaveChanges() > 0;
            message = status ? " Stock deleted successfully" : "Stock to delete Category";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditStock(string id)
        {
            long stockId = Convert.ToInt64(id);
            var data = _Entities.tb_StockUpdate.Where(x => x.SchoolId == _user.SchoolId && x.StockId == stockId && x.IsActive).FirstOrDefault();
            var model = new StockUpdateModel();
            model.CategoryId = data.CategoryId;
            model.Item = data.Item;
            model.Price = data.Price;
            model.PurchaseId = data.PurchaseId;
            model.SupplirName = data.SupplierName;
            var status = (StockStatus)System.Enum.Parse(typeof(StockStatus), data.StockStatus.ToString());
            model.Status = status;
            model.SchoolId = _user.SchoolId;
            model.StockId = data.StockId;
            return PartialView("~/Views/Laboratory/_pv_Edit_StockUpdate.cshtml", model);
        }
        public object SubmitEditStockUpdate(StockUpdateModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var data = _Entities.tb_StockUpdate.Where(x => x.StockId == model.StockId && x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
                if (data != null)
                {
                    data.CategoryId = model.CategoryId;
                    data.Item = model.Item;
                    data.Price = model.Price;
                    data.PurchaseId = model.PurchaseId;
                    data.SupplierName = model.SupplirName;
                    data.StockStatus = (int)model.Status;
                    status = _Entities.SaveChanges() > 0;
                    msg = "Successful";
                }
                else
                {
                    msg = "No such data!";
                }
            }
            catch
            {
                msg = "No chages made";
            }
            if (status == false)
                msg = "No chages made";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult LabInventoryReport()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult DatatableLabInventoryReport(string id)
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            model.CategoryId = Convert.ToInt64(id);
            return PartialView("~/Views/Laboratory/_pv_LabInventoryReportList.cshtml", model);
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
        public object Deleteitem(string id)
        {
            bool status = false;
            string msg = "False";
            long bankId = Convert.ToInt64(id);
            var bank = _Entities.tb_AddCategory.FirstOrDefault(x => x.Id == bankId);
            if (bank != null)
            {
                //bank.IsActive = false;
                _Entities.tb_AddCategory.Remove(bank);
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //Created by gayathri(08/11/2023)For bank edit option
        public object EditItem(string id)
        {
            bool status = false;
            string msg = "False";
            Int64 Category = 0;
            string item = "";
            string unit = "";
            long bankId = Convert.ToInt64(id);
            var bank = _Entities.tb_AddCategory.FirstOrDefault(x => x.Id == bankId);
            if (bank != null)
            {
                var categoryname = _Entities.tb_LaboratoryCategory.Where(x => x.CategoryId == bank.CategoryId).FirstOrDefault();
                //Category = categoryname.LaboratoryName;
                Category = Convert.ToInt64(bank.CategoryId);
                item = bank.Item;
                unit = bank.Unit;

                //bank.IsActive = false;
                //status = _Entities.SaveChanges() > 0;
                //msg = bank.Item;


            }
            //msg = status ? "Deleted" : "Failed";
            return Json(new { Category= Category, item= item, unit= unit ,bankId= bankId }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult SubmitStockAddNew(StockUpdateModel model)
        {

            var message = "failed";
            var status = false;
            var stock = new tb_AddCategory();
            stock.CategoryId = model.CategoryId;
            stock.Item = model.Item;

            stock.Unit = model.Unit;


            stock.SchoolId = _user.SchoolId;
            stock.UserId = _user.UserId;

            stock.TimeStamp = CurrentTime;
            var additem = _Entities.tb_AddCategory.Where(x => x.Id == model.CatId).FirstOrDefault();
            if(additem==null)
            {
                _Entities.tb_AddCategory.Add(stock);
                if (_Entities.SaveChanges() > 0)
                {
                    message = "success";
                    status = true;
                }
            }
            else
            {
                additem.CategoryId = model.CategoryId;
                additem.Item = model.Item;

                additem.Unit = model.Unit;

                additem.TimeStamp = DateTime.Now;
                if (_Entities.SaveChanges() > 0)
                {
                    message = "success";
                    status = true;
                }
            }
          


            return Json(new { Status = status, message = status ? "Successsfully Added" :"failed" }, JsonRequestBehavior.AllowGet);






        }

        public object EditItemById(string id)
        {
            var message = "failed";
            var status = false;
            var stock = new tb_AddCategory();
            string[] splitDate = id.Split('~');
            Int64 Id = Convert.ToInt64(splitDate[0]);
            string Item = Convert.ToString(splitDate[1]);
            string Unit = Convert.ToString(splitDate[2]);
            Int64 CategoryId = Convert.ToInt64(splitDate[3]);
            //stock.Item = model.Item;

            //stock.Unit = model.Unit;

            //Int64 Catid = Convert.ToInt64(id);
            stock.SchoolId = _user.SchoolId;
            stock.UserId = _user.UserId;

            stock.TimeStamp = CurrentTime;
            var additem = _Entities.tb_AddCategory.Where(x => x.Id == Id).FirstOrDefault();
           
                additem.CategoryId = CategoryId;
                additem.Item = Item;

                additem.Unit = Unit;

                additem.TimeStamp = DateTime.Now;
                if (_Entities.SaveChanges() > 0)
                {
                    message = "success";
                    status = true;
                }
            



            return Json(new { Status = status, message = status ? "Successsfully Added" : "failed" }, JsonRequestBehavior.AllowGet);




        }

        public PartialViewResult PurchaseReportByDate(string id)
        {
            StockUpdateModel model = new StockUpdateModel();

            string[] splitData = id.Split('~');


            DateTime FDate = Convert.ToDateTime(splitData[0]);
            DateTime LDate = Convert.ToDateTime(splitData[1]);


            string endDate = LDate.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            model.StartDate = Convert.ToDateTime(FDate);
            model.EndDate = Convert.ToDateTime(endDate);
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Laboratory/_pv_PurchaseReport.cshtml", model);
        }



        public PartialViewResult SalesReportByDate(string id)
        {
            StockUpdateModel model = new StockUpdateModel();

            string[] splitData = id.Split('~');


            DateTime FDate = Convert.ToDateTime(splitData[0]);
            DateTime LDate = Convert.ToDateTime(splitData[1]);


            string endDate = LDate.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            model.StartDate = Convert.ToDateTime(FDate);
            model.EndDate = Convert.ToDateTime(endDate);
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Laboratory/_pv_SalesReport.cshtml", model);
        }






        public IActionResult AddCategory()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;

            return PartialView("~/Views/Laboratory/_pv_AddCategory_Model.cshtml", model);




        }



        public IActionResult SubmitCategory(StockUpdateModel model)
        {
            var stock = new tb_LaboratoryCategory();

            stock.LaboratoryName = model.LaboratoryName;
            stock.SchoolId = _user.SchoolId;
            stock.IsActive = true;
            _Entities.tb_LaboratoryCategory.Add(stock);
            _Entities.SaveChanges();

            return RedirectToAction("StoreList",model);
        }



        public PartialViewResult DatatableLabPurchaseReport(string id)
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            model.CategoryId = Convert.ToInt64(id);
            return PartialView("~/Views/Laboratory/_pv_LabPurchaseReportList.cshtml", model);
        }



        public PartialViewResult DatatableLabSalesReport(string id)
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            model.CategoryId = Convert.ToInt64(id);
            return PartialView("~/Views/Laboratory/_pv_LabSalesReportList.cshtml", model);
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




        public PartialViewResult GetUserListByAdmissionNumberBilling(string id)
        {
            string[] splitData = id.Split('~');
            SchoolModel model = new SchoolModel();
            model.StudentSpecialId = Convert.ToString(splitData[0]);

            //  model.divisionId = Convert.ToInt64(splitData[1]);
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/Laboratory/_pv_StoreBilling_UserByAdmissionNumber_Grid.cshtml", model);

        }






        private void AllUpdatesInBalance(DateTime billDate, int sourceId, long BankId, decimal amount)
        {
            if (sourceId == Convert.ToInt32(DataFromStatus.Cash))
            {
                var balance = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.IsActive == true && x.BankId == 0 && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) > billDate.Date).ToList();
                if (balance != null && balance.Count > 0)
                {
                    foreach (var item in balance)
                    {
                        item.Opening = item.Opening + amount;
                        item.Closing = item.Closing + amount;
                        _Entities.SaveChanges();
                    }
                }
            }
            else
            {
                var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && x.SourceId == sourceId && x.BankId == BankId && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) > billDate.Date).ToList();
                if (balance != null && balance.Count > 0)
                {
                    foreach (var item in balance)
                    {
                        item.Opening = item.Opening + amount;
                        item.Closing = item.Closing + amount;
                        _Entities.SaveChanges();
                    }
                }
            }
        }



        [HttpPost]
        public object StudentMainBillPay(FeeModel model)
        {
            string salesid = model.salesid;
            
            // saving in sales table
            List<string> feeDetails1 = model.FeeDetails.Split(',').ToList();
         
            var stock = new tb_SalesStock();
       
        
            foreach (var fee in feeDetails1)
            {
                string[] splitData = fee.Split('^');

                stock.CategoryId = Convert.ToInt32(splitData[3]);
                stock.Item = Convert.ToString(splitData[7]);
                stock.StockQuantity = Convert.ToInt32(splitData[6]);
                stock.Price = Convert.ToDecimal(splitData[5]);
                stock.SalesId = splitData[9];
                stock.StockId = Convert.ToInt32(splitData[8]);
                
                stock.IsActive = true;
                string Accountnumber= splitData[10];
                stock.StudentId= Convert.ToInt32(splitData[2]);
                stock.SchoolId = _user.SchoolId;
                stock.UserId = _user.UserId;

                stock.TimeStamp = CurrentTime;

                var Mainstock = new tb_StockUpdate();
                var pstock = new tb_Purchase();
                //var stockExist = _Entities.tb_StockUpdate.Where(z => z.Item == stock.Item && z.StockQuantity != null).FirstOrDefault();

                var stockExist = _Entities.tb_StockUpdate.Where(z => z.Item == stock.Item ).FirstOrDefault();


                //if (stockExist.StockQuantity< stock.StockQuantity)
                //{
                //    return Json(new { msg = "ITEM OUT OF STOCK "  }, JsonRequestBehavior.AllowGet);
                //    //   ViewBag.Javascript = "<script language='javascript' type='text/javascript'>alert('Data Already Exists');</script>";
                //   // return RedirectToAction("Billing");
                //}

                _Entities.tb_SalesStock.Add(stock);
                _Entities.SaveChanges();

             
         
                if (stockExist != null)
                {

                    //var stockQuantity = stockExist.StockQuantity;
                    //stock.StockQuantity = stockQuantity - model.Quantity;
                    //_Entities.SP_UpdateStockQuantity(stock.StockQuantity, model.Item);
                    //_Entities.SaveChanges();




                }
            }

            //individual saving
            //stock.CategoryId = model.CategoryId;
            //stock.Item = model.Item;
            //stock.StockQuantity = model.Quantity;
            //stock.Price = model.Price;
            //stock.SalesId = "s100";
            //stock.StockId = model.StockId ;

            //stock.IsActive = true;


            //stock.SchoolId = _user.SchoolId;
            //stock.UserId = _user.UserId;

            //stock.TimeStamp = CurrentTime;
            //_Entities.tb_SalesStock.Add(stock);
            //_Entities.SaveChanges();


            //saving in sales table





            decimal sumAmt = 0;
            bool status = false;
            string message = "Failed";
            List<string> feeDetails = model.FeeDetails.Split(',').ToList();
            long SchoolId = model.SchoolId;
            long ClassId = model.ClassId;
            long StudentId = model.StudentId;
            DateTime BillDate = model.TimeStamp;
            if (model.TimeStamp.ToString("MM-dd-YYYY") == CurrentTime.ToString("MM-dd-YYYY"))
            {
                BillDate = CurrentTime;
            }
            decimal TotalAmountPaid = 0;
            if (model.PaidAmount != 0)
            {
                TotalAmountPaid = Convert.ToDecimal(model.PaidAmount);
            }
            Guid PaymentGuid = Guid.NewGuid(); // To find same bill elements
            long BillNo = 1;
             var payment = new tb_Payment();
            var payment1 = new tb_StockPayment();
            var billNo = _Entities.tb_StockPaymentBillNo.Where(z => z.SchoolId == SchoolId).FirstOrDefault();
            if (billNo != null)
            {
                BillNo = billNo.BillNo + 1;
            }
            else
            {
                var slNoTable = new tb_StockPaymentBillNo();
                slNoTable.SchoolId = SchoolId;
                slNoTable.BillNo = 1;
                _Entities.tb_StockPaymentBillNo.Add(slNoTable);
                status = _Entities.SaveChanges() > 0 ? true : false;
            }
            var studDetails = _Entities.tb_Student.Where(z => z.StudentId == StudentId ).FirstOrDefault();
            long thisBillVoucherNumber = 0;
            var vouchrTbl = _Entities.tb_StockVoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (vouchrTbl == null)
            {
                var voucherNumber = new tb_StockVoucherNumber();
                voucherNumber.SchoolId = _user.SchoolId;
                voucherNumber.PaymentVoucher = 1;
                voucherNumber.ReceiptVoucher = 1;
                voucherNumber.ContraVoucher = 1;
                voucherNumber.IsActive = true;
                voucherNumber.TimeStamp = CurrentTime;
                _Entities.tb_StockVoucherNumber.Add(voucherNumber);
                _Entities.SaveChanges();
                thisBillVoucherNumber = voucherNumber.ReceiptVoucher;
            }
            else
            {
                thisBillVoucherNumber = vouchrTbl.ReceiptVoucher;
            }
            long headIdBill = 0;
            var accountHead = _Entities.tb_StockAccountHead.Where(x => x.SchoolId == _user.SchoolId && x.ForBill == true).FirstOrDefault();
            if (accountHead == null)
            {
                var AccountHead = new tb_StockAccountHead();
                AccountHead.AccHeadName = "Stock Income";
                AccountHead.ForBill = true;
                AccountHead.SchoolId = _user.SchoolId;
                AccountHead.IsActive = true;
                AccountHead.TimeStamp = CurrentTime;
                _Entities.tb_StockAccountHead.Add(AccountHead);
                _Entities.SaveChanges();
                headIdBill = AccountHead.AccountId;
            }
            else
                headIdBill = accountHead.AccountId;

            var previousBalanceAmount = _Entities.tb_StockStudentBalance.Where(z => z.StudentId == StudentId && z.IsActive).FirstOrDefault();//Archana 30-11-2018
            decimal prevAdv = 0;
            if (previousBalanceAmount != null)
                prevAdv = previousBalanceAmount.Amount;
            bool UpdateVoucher = false;
            foreach (var fee in feeDetails)
            {
                string[] splitData = fee.Split('^');
                payment1.StockId = Convert.ToInt32(splitData[8]);
                payment1.PaymentGuid = PaymentGuid;
                payment1.PaymentType = 1;
                payment1.SchoolId = _user.SchoolId;
                payment1.TimeStamp= CurrentTime;
                payment1.StudentId= Convert.ToInt32(splitData[2]);
                payment1.Amount= Convert.ToDecimal(splitData[5]);
                var IsBillExist = _Entities.tb_StockPayment.Where(x => x.SalesId == model.salesid).FirstOrDefault();
                if(IsBillExist==null)
                {
                    payment1.BillNo = Convert.ToInt32(BillNo);
                }
                
                payment1.ClassId= Convert.ToInt32(splitData[1]);
                payment1.IsActive = true;
                payment1.IsPaid = true;
             

                //  stock.CategoryId = Convert.ToInt32(splitData[3]);
                // stock.Item = Convert.ToString(splitData[7]);
                //stock.StockQuantity = Convert.ToInt32(splitData[6]);
                //   stock.Price = Convert.ToDecimal(splitData[5]);
                //  stock.SalesId = "s100";
                //stock.StockId = Convert.ToInt32(splitData[8]);

                // stock.IsActive = true;


                //stock.SchoolId = _user.SchoolId;
                // stock.UserId = _user.UserId;

                //stock.TimeStamp = CurrentTime;

                //decimal paymentAmount = Convert.ToDecimal(splitData[0]);
                //long feeId = Convert.ToInt32(splitData[1]);
                //payment.FeeId = feeId;
                //payment.FeeGuid = new Guid(splitData[2]);
                //decimal maxAmount = Convert.ToDecimal(splitData[3]);
                //decimal discount = Convert.ToDecimal(splitData[4]);
                //payment.MaxAmount = maxAmount;
                //payment.Discount = discount;
                //payment.BillType = Convert.ToInt32(splitData[6]);
                //int isAmountEdit = Convert.ToInt16(splitData[5]);

                //if (stock.Price!=0   /*isAmountEdit != 0*/)
                //{
                //    var paymentList = new TrackTap.Data.Student(StudentId).GetStudentPaymentFees().OrderBy(z => z.DueDate).ToList();
                //    var dueFee = paymentList.Where(z => z.FeeGuid == payment.FeeGuid).FirstOrDefault();
                //    if (dueFee != null)
                //    {
                //        if (dueFee.Amount != paymentAmount)
                //        {
                //            var due = new tb_FeeDues();
                //            due.FeeId = payment.FeeId;
                //            decimal amtAfterDiscount = maxAmount - discount;
                //            due.Amount = amtAfterDiscount - paymentAmount;
                //            due.FeeDuesGuid = Guid.NewGuid();
                //            due.StudentId = StudentId;
                //            due.IsActive = true;
                //            due.DueDate = dueFee.DueDate;
                //            due.TimeStamp = BillDate;
                //            due.ParentGuid = payment.FeeGuid;
                //            due.BillNo = BillNo;
                //            _Entities.tb_FeeDues.Add(due);
                //            // status = _Entities.SaveChanges() > 0 ? true : false;
                //        }
                //    }
                //}

                payment1.Amount = stock.Price;
                sumAmt = sumAmt + payment.Amount;
                payment1.PaymentMode = model.PaymentType;
                if (model.PaymentType == 2)
                {
                    payment1.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                    payment1.BankId = model.BankId;
                    payment1.ChequeNumber = model.ChequeNumber;
                }
                else
                {
                    payment.BankId = 0;
                }
                payment.BillNo = BillNo;
                payment.IsPaid = true;
                payment.PaymentType = 1;
                payment.PaymentGuid = PaymentGuid;
                payment.StudentId = StudentId;
                payment.ClassId = ClassId;
                payment.SchoolId = SchoolId;
                payment.TimeStamp = BillDate;
                payment.IsActive = true;
                
                // Archana (12-12-2018)
                // Checking the bill have a partial paid balance payment, then the bill wants to save the first bill no 
                try
                {
                    long previousBillno = 0; 
                    payment1.PartialPaidParentBillNo = previousBillno;
                }
                catch
                {
                    payment1.PartialPaidParentBillNo = 0;
                }
                payment1.SalesId = model.salesid;
              payment1.IssuedPerson = _user.UserId;
                _Entities.tb_StockPayment.Add(payment1);
                status = _Entities.SaveChanges() > 0 ? true : false;

                try
                {
                    var d = BillDate.ToString("MM-dd-yyyy");
                    DateTime todayDate = Convert.ToDateTime(d);
                    var incDetail = _Entities.tb_StockIncome.Where(z => z.AccountHead == "Fee Collected" && z.Date == todayDate && z.SchoolId == _user.SchoolId && z.IsActive).FirstOrDefault();
                    if (incDetail != null)
                    {
                        double? payAmt = Convert.ToDouble(stock.Price);
                        double? amt = incDetail.Amount;
                        payAmt = payAmt + amt;
                        incDetail.Amount = Convert.ToDouble(payAmt);
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                    else
                    {
                        var income = new tb_StockIncome();
                        income.AccountHead = "Fee Collected";
                        income.Amount = Convert.ToDouble(stock.Price);
                        income.Particular = "Stock Income";
                        income.SchoolId = _user.SchoolId;
                        income.IsActive = true;
                        income.Date = todayDate;
                        _Entities.tb_StockIncome.Add(income);
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                }
                catch (Exception ex)
                {

                }

                #region Account Sction

                if (prevAdv < Convert.ToDecimal(stock.Price))
                {
                    var studDetails1 = _Entities.tb_Student.Where(z => z.StudentId == StudentId && z.IsActive == true).FirstOrDefault();
                    decimal currentPaiedAmountPerItem = Convert.ToDecimal(stock.Price) - prevAdv;
                    #region The Payment mode is Cash
                    if (model.PaymentType == 1)// Cash
                    {
                        UpdateVoucher = true;
                       // var cashEntry = new tb_StockCashEntry();
                        var cashEntry1 = new tb_StockCashEntry();
                        if (vouchrTbl != null)
                            cashEntry1.VoucherNumber = thisBillVoucherNumber.ToString();
                        else
                        cashEntry1.VoucherNumber = "1";
                        cashEntry1.BillNo = BillNo.ToString();
                        cashEntry1.VoucherType = "RV";
                        cashEntry1.TransactionType = "R";
                        cashEntry1.Amount = currentPaiedAmountPerItem;
                        cashEntry1.HeadId = headIdBill;
                        cashEntry1.StockId = stock.StockId;
                        cashEntry1.Narration = "Cash Paid " + studDetails.StundentName;
                        cashEntry1.EnterDate = BillDate;
                        cashEntry1.UserId = _user.UserId;
                        cashEntry1.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                        cashEntry1.CancelStatus = false;
                        cashEntry1.SchoolId = _user.SchoolId;
                        cashEntry1.Migration = false;
                        cashEntry1.IsActive = true;
                        cashEntry1.TimeStamp = CurrentTime;
                        if (cashEntry1.EnterDate.Date == CurrentTime.Date)
                            cashEntry1.EditStatus = "N";
                        else if (cashEntry1.EnterDate.Date < CurrentTime.Date)
                            cashEntry1.EditStatus = "P";
                        else
                            cashEntry1.EditStatus = "F";
                        cashEntry1.ReverseStatus = false;
                        cashEntry1.AdvanceStatus = false;
                        _Entities.tb_StockCashEntry.Add(cashEntry1);
                        _Entities.SaveChanges();

                        #region Data added to Balance table for Account
                        int sourceId = Convert.ToInt32(DataFromStatus.Cash);
                        var balance = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == 0).FirstOrDefault();
                        if (balance != null)
                        {
                            balance.Closing = balance.Closing + currentPaiedAmountPerItem;
                            balance.TimeStamp = CurrentTime;
                            _Entities.SaveChanges();
                        }
                        else
                        {
                            try
                            {
                                var balanceEntry = new tb_StockBalance();
                                balanceEntry.SchoolId = _user.SchoolId;
                                balanceEntry.CurrentDate = BillDate;
                                balanceEntry.SourceId = sourceId;
                                DateTime yesterday = _Entities.tb_StockBalance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                if (yesterday.Year != 0001)
                                    balanceEntry.Opening = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == 0).ToList().Sum(x => x.Closing);
                                else
                                    balanceEntry.Opening = 0;
                                balanceEntry.Closing = balanceEntry.Opening + currentPaiedAmountPerItem;
                                balanceEntry.IsActive = true;
                                balanceEntry.BankId = 0;
                                balanceEntry.TimeStamp = CurrentTime;
                                _Entities.tb_StockBalance.Add(balanceEntry);
                                _Entities.SaveChanges();

                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        AllUpdatesInBalance(BillDate, sourceId, 0, currentPaiedAmountPerItem);

                        #endregion Data added to Balance table for Account
                    }
                    #endregion The Payment mode is Cash
                    #region The Payment mode is Bank
                    else// Bank
                    {
                        UpdateVoucher = true;
                        var bankEntry = new tb_StockBankEntry();
                        if (vouchrTbl != null)
                            bankEntry.VoucherNumber = thisBillVoucherNumber.ToString();
                        else
                        bankEntry.VoucherNumber = "1";
                        bankEntry.VoucherType = "RV";
                        bankEntry.BillNo = BillNo.ToString();
                        bankEntry.TransactionType = "R";
                        bankEntry.Amount = currentPaiedAmountPerItem;
                        bankEntry.ModeType = model.PaymentType;
                        if (model.PaymentType == 2)
                        {
                            bankEntry.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                            bankEntry.ChequeNumber = model.ChequeNumber;
                        }

                        bankEntry.HeadId = headIdBill;
                        bankEntry.SubId = stock.StockId;
                        bankEntry.BankId = model.BankId;
                        bankEntry.Narration = "Fee Paid " + studDetails.StundentName;
                        bankEntry.EnterDate = BillDate;
                        bankEntry.UserId = _user.UserId;
                        bankEntry.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                        bankEntry.CancelStatus = false;
                        bankEntry.SchoolId = _user.SchoolId;
                        bankEntry.Migration = false;
                        bankEntry.IsActive = true;
                        bankEntry.TimeStamp = CurrentTime;
                        if (bankEntry.EnterDate.Date == CurrentTime.Date)
                            bankEntry.EditStatus = "N";
                        else if (bankEntry.EnterDate.Date < CurrentTime.Date)
                            bankEntry.EditStatus = "P";
                        else
                            bankEntry.EditStatus = "F";
                        _Entities.tb_StockBankEntry.Add(bankEntry);
                        _Entities.SaveChanges();


                        #region Data added to Balance table for Account
                        int sourceId = Convert.ToInt32(DataFromStatus.Bank);
                        var balance = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == model.BankId).FirstOrDefault();
                        if (balance != null)
                        {
                            balance.Closing = balance.Closing + currentPaiedAmountPerItem;
                            balance.TimeStamp = CurrentTime;
                            _Entities.SaveChanges();
                        }
                        else
                        {
                            var balanceEntry = new tb_StockBalance();
                            balanceEntry.SchoolId = _user.SchoolId;
                            balanceEntry.CurrentDate = BillDate;
                            balanceEntry.SourceId = sourceId;
                            DateTime yesterday = _Entities.tb_StockBalance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == model.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                            if (yesterday.Year != 0001)
                                balanceEntry.Opening = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                            else
                                balanceEntry.Opening = 0;
                            balanceEntry.Closing = balanceEntry.Opening + currentPaiedAmountPerItem;
                            balanceEntry.IsActive = true;
                            balanceEntry.BankId = model.BankId;
                            balanceEntry.TimeStamp = CurrentTime;
                            _Entities.tb_StockBalance.Add(balanceEntry);
                            _Entities.SaveChanges();
                        }

                        AllUpdatesInBalance(BillDate, sourceId, model.BankId, currentPaiedAmountPerItem);
                        #endregion Data added to Balance table for Account
                    }
                    #endregion The Payment mode is Bank
                    prevAdv = 0;//Here clear the all previous amount ,becuse it will reduced the current fee amount
                }
                else
                {
                    prevAdv = prevAdv - Convert.ToDecimal(stock.Price);
                }
                #endregion Account Sction

            }
            if (UpdateVoucher == true)
            {
                var vouchrTbl2 = _Entities.tb_StockVoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                vouchrTbl2.ReceiptVoucher = vouchrTbl2.ReceiptVoucher + 1;
                _Entities.SaveChanges();
            }

            var billNo1 = _Entities.tb_StockPaymentBillNo.Where(z => z.SchoolId == SchoolId).FirstOrDefault();
            if (billNo1 != null)
            {
                billNo.BillNo = BillNo;
                status = _Entities.SaveChanges() > 0 ? true : false;
            }
            try

            {
                decimal payableAmount = 0;
                decimal bal = 0;
                decimal prevBal = 0;
                //bool balAndCash = false;
                bool ispayable = false;

                decimal tempSumTotal = 0;
                tempSumTotal = sumAmt;
                if (sumAmt == 0)
                {
                    sumAmt = payment.Amount;
                }
                var balance = _Entities.tb_StockStudentBalance.Where(z => z.StudentId == StudentId && z.IsActive).FirstOrDefault();
                if (balance != null)
                {
                    prevBal = balance.Amount;
                    bal = balance.Amount;
                    //sumAmt = (bal - sumAmt);
                    if ((prevBal < tempSumTotal) && (prevBal != 0))
                    {
                        ispayable = true;
                        payableAmount = tempSumTotal - prevBal;

                    }
                    
                    if (TotalAmountPaid != 0)
                    {
                        var tempBal = TotalAmountPaid - sumAmt;
                        bal = tempBal + prevBal;
                    }
                    else
                    {
                        if (ispayable)
                        {
                            bal = 0;
                        }
                        else
                        {
                            bal = balance.Amount - sumAmt;
                        }
                    }
                    if (bal < 0) //if no balance available (balance.Amount - sumAmt) gets -ve
                    {
                        bal = 0;
                    }
                }
                else
                {
                    if (TotalAmountPaid != 0)
                    {
                        bal = TotalAmountPaid - sumAmt;
                    }
                    else
                    {
                        //bal = Math.Abs(bal - sumAmt);
                        bal = bal - sumAmt;
                    }
                    if (bal < 0)
                    {
                        bal = 0;
                    }
                }
                if (balance != null)
                {
                    try
                    {
                        balance.Amount = bal;
                        _Entities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var messageex = ex.Message;
                    }
                }
                else
                {
                    if (bal != 0)
                    {
                        var studentBalance = new tb_StockStudentBalance();
                        studentBalance.StudentId = StudentId;
                        studentBalance.Amount = bal;
                        studentBalance.IsActive = true;
                        _Entities.tb_StockStudentBalance.Add(studentBalance);
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                }
                if ((TotalAmountPaid != 0) || (bal != 0))
                {
                    var studentPaidsAmount = new tb_StockStudentPaidAmount();
                    studentPaidsAmount.StudentId = StudentId;
                    studentPaidsAmount.PaidAmount = TotalAmountPaid;
                    studentPaidsAmount.PreviousBalance = prevBal;
                    studentPaidsAmount.BalanceAmount = bal;
                    studentPaidsAmount.BillNo = BillNo;
                    studentPaidsAmount.AddAccountStatus = false;
                    studentPaidsAmount.IsActive = true;
                    _Entities.tb_StockStudentPaidAmount.Add(studentPaidsAmount);
                    status = _Entities.SaveChanges() > 0 ? true : false;
                }
               
                if (ispayable)
                {
                    var studentPaidsAmount = new tb_StockStudentPaidAmount();
                    studentPaidsAmount.StudentId = StudentId;
                    studentPaidsAmount.PaidAmount = payableAmount;
                    studentPaidsAmount.PreviousBalance = prevBal;
                    studentPaidsAmount.BalanceAmount = bal;
                    studentPaidsAmount.BillNo = BillNo;
                    studentPaidsAmount.IsActive = true;
                    studentPaidsAmount.AddAccountStatus = false;
                    _Entities.tb_StockStudentPaidAmount.Add(studentPaidsAmount);
                    status = _Entities.SaveChanges() > 0 ? true : false;
                }
                #region Archana : Is the student pay more amount than he payable ?
                decimal currentPayableAmt = tempSumTotal - prevBal; // Fee Item Total - Previous Amount
                if (currentPayableAmt > 0)// The Student wnats to pay now
                {
                    decimal advance = TotalAmountPaid - currentPayableAmt;// The actual bill amount after the previous advance
                    long subId = 0;
                    if (advance > 0)
                    {
                        var head = _Entities.tb_StockAccountHead.Where(x => x.SchoolId == _user.SchoolId && x.IsActive && x.ForBill == true).FirstOrDefault();
                        if (head != null)
                        {
                            var sub = _Entities.tb_StockSubLedgerData.Where(x => x.AccHeadId == head.AccountId && x.IsActive).FirstOrDefault();
                            if (sub == null)
                            {
                                var subAdd = new tb_StockSubLedgerData();
                                subAdd.SubLedgerName = "Advance Amount";
                                subAdd.AccHeadId = head.AccountId;
                                subAdd.IsActive = true;
                                subAdd.TimeStamp = CurrentTime;
                                _Entities.tb_StockSubLedgerData.Add(subAdd);
                                _Entities.SaveChanges();
                                subId = subAdd.LedgerId;
                            }
                            else
                            {
                                subId = sub.LedgerId;
                            }

                            if (model.PaymentType == 1)// Cash
                            {
                                var advCash = new tb_StockCashEntry();
                                advCash.VoucherNumber = thisBillVoucherNumber.ToString();
                                advCash.VoucherType = "RV";
                                advCash.BillNo = "";
                                advCash.TransactionType = "R";
                                advCash.Amount = advance;
                                advCash.HeadId = head.AccountId;
                                advCash.StockId= subId;
                                advCash.Narration = "Advance Paid " + studDetails.StundentName;
                                advCash.EnterDate = BillDate;
                                advCash.UserId = _user.UserId;
                                advCash.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advCash.CancelStatus = false;
                                advCash.SchoolId = _user.SchoolId;
                                advCash.Migration = false;
                                advCash.IsActive = true;
                                advCash.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advCash.EditStatus = "P";
                                else
                                    advCash.EditStatus = "N";
                                advCash.ReverseStatus = false;
                                advCash.AdvanceStatus = false;
                                _Entities.tb_StockCashEntry.Add(advCash);
                                _Entities.SaveChanges();




                                int sourceId = Convert.ToInt32(DataFromStatus.Cash);
                                var balanceNow = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == 0).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + advance;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_StockBalance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_StockBalance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + advance;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_StockBalance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, advance);

                            }
                            else
                            {
                                var advBank = new tb_StockBankEntry();
                                advBank.VoucherNumber = thisBillVoucherNumber.ToString();
                                advBank.VoucherType = "RV";
                                advBank.BillNo = "";
                                advBank.TransactionType = "R";
                                advBank.Amount = advance;
                                advBank.ModeType = model.PaymentType;
                                if (model.PaymentType == 2)
                                {
                                    advBank.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                                    advBank.ChequeNumber = model.ChequeNumber;
                                }
                                advBank.HeadId = head.AccountId;
                                advBank.SubId = subId;
                                advBank.BankId = model.BankId;
                                advBank.Narration = "Advance Paid " + studDetails.StundentName;
                                advBank.EnterDate = BillDate;
                                advBank.UserId = _user.UserId;
                                advBank.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advBank.CancelStatus = false;
                                advBank.SchoolId = _user.SchoolId;
                                advBank.Migration = false;
                                advBank.IsActive = true;
                                advBank.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advBank.EditStatus = "P";
                                else
                                    advBank.EditStatus = "N";
                                _Entities.tb_StockBankEntry.Add(advBank);
                                _Entities.SaveChanges();


                                int sourceId = Convert.ToInt32(DataFromStatus.Bank);
                                var balanceNow = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == model.BankId).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + advance;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_StockBalance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_StockBalance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == model.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + advance;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_StockBalance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, advance);

                            }
                            if (UpdateVoucher == false)
                            {
                                var vouchrTbl2 = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                                vouchrTbl2.ReceiptVoucher = vouchrTbl2.ReceiptVoucher + 1;
                                _Entities.SaveChanges();
                            }
                        }
                    }
                }
                else // The student does not want to pay , the all bill amount will satisfies the previous advance amount
                {
                    if (TotalAmountPaid > 0)//The student paid amount ,when their is no need to paid
                    {
                        long subId = 0;
                        var head = _Entities.tb_StockAccountHead.Where(x => x.SchoolId == _user.SchoolId && x.IsActive && x.ForBill == true).FirstOrDefault();
                        if (head != null)
                        {
                            var sub = _Entities.tb_StockSubLedgerData.Where(x => x.AccHeadId == head.AccountId && x.IsActive).FirstOrDefault();
                            if (sub == null)
                            {
                                var subAdd = new tb_StockSubLedgerData();
                                subAdd.SubLedgerName = "Advance Amount";
                                subAdd.AccHeadId = head.AccountId;
                                subAdd.IsActive = true;
                                subAdd.TimeStamp = CurrentTime;
                                _Entities.tb_StockSubLedgerData.Add(subAdd);
                                _Entities.SaveChanges();
                                subId = subAdd.LedgerId;
                            }
                            else
                            {
                                subId = sub.LedgerId;
                            }
                            if (model.PaymentType == 1)// Cash
                            {
                                var advCash = new tb_StockCashEntry();
                                advCash.VoucherNumber = thisBillVoucherNumber.ToString();
                                advCash.VoucherType = "RV";
                                advCash.BillNo = "";
                                advCash.TransactionType = "R";
                                advCash.Amount = TotalAmountPaid;
                                advCash.HeadId = head.AccountId;
                                advCash.StockId = subId;
                                advCash.Narration = "Advance Paid " + studDetails.StundentName;
                                advCash.EnterDate = BillDate;
                                advCash.UserId = _user.UserId;
                                advCash.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advCash.CancelStatus = false;
                                advCash.SchoolId = _user.SchoolId;
                                advCash.Migration = false;
                                advCash.IsActive = true;
                                advCash.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advCash.EditStatus = "P";
                                else
                                    advCash.EditStatus = "N";
                                advCash.ReverseStatus = false;
                                advCash.AdvanceStatus = false;
                                _Entities.tb_StockCashEntry.Add(advCash);
                                _Entities.SaveChanges();


                                int sourceId = Convert.ToInt32(DataFromStatus.Cash);
                                var balanceNow = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == 0).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + TotalAmountPaid;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_StockBalance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + TotalAmountPaid;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_StockBalance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, TotalAmountPaid);
                            }
                            else
                            {
                                var advBank = new tb_StockBankEntry();
                                advBank.VoucherNumber = thisBillVoucherNumber.ToString();
                                advBank.VoucherType = "RV";
                                advBank.BillNo = " ";
                                advBank.TransactionType = "R";
                                advBank.Amount = TotalAmountPaid;
                                advBank.ModeType = model.PaymentType;
                                if (model.PaymentType == 2)
                                {
                                    advBank.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                                    advBank.ChequeNumber = model.ChequeNumber;
                                }
                                advBank.HeadId = head.AccountId;
                                advBank.SubId = subId;
                                advBank.BankId = model.BankId;
                                advBank.Narration = "Advance Paid " + studDetails.StundentName;
                                advBank.EnterDate = BillDate;
                                advBank.UserId = _user.UserId;
                                advBank.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advBank.CancelStatus = false;
                                advBank.SchoolId = _user.SchoolId;
                                advBank.Migration = false;
                                advBank.IsActive = true;
                                advBank.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advBank.EditStatus = "P";
                                else
                                    advBank.EditStatus = "N";
                                _Entities.tb_StockBankEntry.Add(advBank);
                                _Entities.SaveChanges();


                                int sourceId = Convert.ToInt32(DataFromStatus.Bank);
                                var balanceNow = _Entities.tb_StockBalance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == model.BankId).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + TotalAmountPaid;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_StockBalance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == model.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + TotalAmountPaid;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_StockBalance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, TotalAmountPaid);

                            }
                            if (UpdateVoucher == false)
                            {
                                var vouchrTbl2 = _Entities.tb_StockVoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                                vouchrTbl2.ReceiptVoucher = vouchrTbl2.ReceiptVoucher + 1;
                                _Entities.SaveChanges();
                            }
                        }
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {


            }

            var dateTime = BillDate.ToString("dd-MMM-yyyy");

            var description = "failed";
            #region Email
            try
            {
                var smtpDetails = _Entities.tb_SMTPDetail.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                var paidAmount = Convert.ToInt32(payment.Amount);
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/email/FeePayment.html");
                var emailTemplate = System.IO.File.ReadAllText(filePath);
                var mailBody = emailTemplate.Replace("{{schoolname}}", studDetails.tb_School.SchoolName)
                   .Replace("{{parent}}", studDetails.ParentName)
                .Replace("{{student}}", studDetails.StundentName)
                .Replace("{{amount}}", string.Format("{0:0.00}", sumAmt))
                .Replace("{{date}}", dateTime);
                Mail.Send("School Fee Payment", mailBody, studDetails.ParentName, smtpDetails.EmailId, smtpDetails.Password, new System.Collections.ArrayList { studDetails.ParentEmail });
                description = "success";
            }
            catch
            {
                description = "Something went wrong";
            }
            #endregion Email
            var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
            if (package != null)
            {
                try
                {
                    if (model.TimeStamp.ToString("MM-dd-YYYY") == CurrentTime.ToString("MM-dd-YYYY"))
                    {

                        #region  SMS 
                        HttpClient client = new HttpClient();
                        var history = new tb_SmsHistory();
                        var numbers = new List<string>();
                        var MsgId = new List<string>();
                        var numb = "";
                        string messagepre = "";
                        var senderName = "MYSCHO";

                        var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true).FirstOrDefault();
                        if (senderData != null)
                            senderName = senderData.SenderId;
                        message = "success";
                        status = true;
                        var smsHead = new tb_SmsHead();
                        smsHead.Head = "BillDate Payment " + studDetails.StundentName;
                        smsHead.SchoolId = _user.SchoolId;
                        smsHead.TimeStamp = CurrentTime;
                        smsHead.IsActive = true;
                        smsHead.SenderType = (int)SMSSendType.Student;
                        _Entities.tb_SmsHead.Add(smsHead);
                        status = _Entities.SaveChanges() > 0;


                        messagepre = "Dear Parent of " + studDetails.StundentName + ", you have paid Rs." + string.Format("{0:0.00}", sumAmt) + " on " + dateTime;

                        var phone = studDetails.ContactNumber.ToString();
                        int length = messagepre.Length;
                        int que = length / 160;
                        int rem = length % 160;
                        if (rem > 0)
                            que++;
                        int smsCount = que;
                        var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + messagepre + "&sender=" + senderName;
                        //  var url = "http://bhashsms.com//api/sendmsg.php?user=srishtitrans&pass=123456&sender=MCHILD&phone=" + phone + "&text=" + item.Description + "&priority=ndnd&stype=normal";

                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest request = this.GetRequest(url);
                        WebResponse webResponse = request.GetResponse();
                        var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                        var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                        alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);
                        if (status)
                        {
                            tb_SmsHistory sms = new tb_SmsHistory();
                            sms.IsActive = true;
                            sms.MessageContent = messagepre;
                            sms.MessageDate = CurrentTime;
                            sms.ScholId = _user.SchoolId;
                            sms.StuentId = StudentId;
                            sms.MobileNumber = phone;
                            sms.HeadId = smsHead.HeadId;
                            sms.SendStatus = Convert.ToString(respList.success);
                            if (respList.data != null)
                            {
                                sms.MessageReturnId = respList.data[0].messageId;
                                sms.DelivaryStatus = "Pending";
                            }
                            sms.SmsSentPerStudent = smsCount;
                            _Entities.tb_SmsHistory.Add(sms);
                            _Entities.SaveChanges();
                        }
                        #endregion  SMS 
                    }
                }
                catch (Exception ex)
                {
                    var x = ex.InnerException;
                }
            }
            return Json(new { status = status, serialNo = BillNo,Salesid= salesid, msg = status ? "Bill Paid Sucessfully" : "Failed To Pay Bill" }, JsonRequestBehavior.AllowGet);
        }



        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

            request.Timeout = Convert.ToInt32(new TimeSpan(0, 5, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
        }


        public PartialViewResult PrintStockBillData(string id)
        {
            string[] splitData = id.Split('~');
            var model = new PrintBill();
            model.studentId = Convert.ToInt64(splitData[0]);
            model.billNumber = Convert.ToInt64(splitData[1]);
            model.Amount = Convert.ToInt64(splitData[2]);
            model.CurrentDate = CurrentTime;
            model.SalesId = splitData[3];
            model.UserId = _user.UserId;
            return PartialView("~/Views/Laboratory/_pv_PrintStockBillData.cshtml", model);
        }

        public IActionResult CollectionReport()
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }


        public PartialViewResult DatatableLabCollectionReport(string id)
        {
            StockUpdateModel model = new StockUpdateModel();
            model.SchoolId = _user.SchoolId;
            model.CategoryId = Convert.ToInt64(id);
            return PartialView("~/Views/Laboratory/_pv_LabSalesCollectionList.cshtml", model);
        }


        public PartialViewResult CollectionReportByDate(string id)
        {
            StockUpdateModel model = new StockUpdateModel();

            string[] splitData = id.Split('~');


            DateTime FDate = Convert.ToDateTime(splitData[0]);
            DateTime LDate = Convert.ToDateTime(splitData[1]);


            string endDate = LDate.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            model.StartDate = Convert.ToDateTime(FDate);
            model.EndDate = Convert.ToDateTime(endDate);
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Laboratory/_pv_CollectionReport.cshtml", model);
        }




        public PartialViewResult AddStockBillPartialView(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id);
            return PartialView("~/Views/Laboratory/_pv_History_Billing_StudentStock_Model.cshtml", model);
        }

        public PartialViewResult LoadTableForStock(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            string[] splitData = id.Split('~');
            model.SchoolModel.studentId = Convert.ToInt64(splitData[1]);
            model.BillNumber = Convert.ToInt64(splitData[0]);
            var Stock_Payment = _Entities.tb_StockPayment.Where(z => z.BillNo == model.BillNumber && z.IsActive ).FirstOrDefault();
            model.salesid = Stock_Payment.SalesId;

            model.UserId = _user.UserId;
            return PartialView("~/Views/Laboratory/_pv_StockHistory_PopupGrid.cshtml", model);
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
        public PartialViewResult CashBookReportStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
            model.ToDate = end;
            model.SchoolName = _user.tb_School.SchoolName;
            model.Heading = start + " to " + end;
            return PartialView("~/Views/Laboratory/_pv_CashBookReportStock.cshtml", model);
        }

        public PartialViewResult BankBookReportStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
            model.ToDate = end;
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
            model.BankId = bankId;
            model.SchoolName = _user.tb_School.SchoolName;
            model.Heading = start + " to " + end;
            return PartialView("~/Views/Laboratory/_pv_BankBookReportStock.cshtml", model);
        }

        public PartialViewResult DayBookReportStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolName = _user.tb_School.SchoolName;
            model.Heading = start + " to " + end;
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
            model.ToDate = end;
            return PartialView("~/Views/Laboratory/_pv_DayBookReportStock.cshtml", model);
        }


        public PartialViewResult GetOpenBallanceByCashStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
            return PartialView("~/Views/Laboratory/_pv_StockCashEntryOpeningBalance.cshtml", model);
        }
        public PartialViewResult GetClosingBallanceByCashStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime end = Convert.ToDateTime(splitDate[1]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.ToDate = end;
            return PartialView("~/Views/Laboratory/_pv_StockCashEntryClosingBalance.cshtml", model);
        }

        public PartialViewResult GetOpenBallanceByBankStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
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
            model.BankId = bankId;
            return PartialView("~/Views/Laboratory/_pv_StockBankEntryOpeningBalance.cshtml", model);
        }
        public PartialViewResult GetClosingBallanceByBankStock(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime end = Convert.ToDateTime(splitDate[1]);
            DayBookReportModelStock model = new DayBookReportModelStock();
            model.SchoolId = _user.SchoolId;
            model.ToDate = end;
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
            model.BankId = bankId;
            return PartialView("~/Views/Laboratory/_pv_StockBankEntryClosingBalance.cshtml", model);
        }



    }
}
                     
                    
          

//jibin 9/14/2020