using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.DataLibrary;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class SuperAdminController : AdminBaseController
    {
        // GET: SuperAdmin
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SchoolList()
        {
            return View();
        }
        public IActionResult PaymentList()
        {
            return View();
        }
        public PartialViewResult GetPaymentGatewayListBySchool(string id)
        {
            string[] data = id.Split('~');
            long schoolId = Convert.ToInt64(data[0]);
            int year = Convert.ToInt16(data[2]);
            int opr = 0;
            int monthInDigit = 0;
            string month = data[1];
            if (month != "0")
            {
                monthInDigit = DateTime.ParseExact(data[1], "MMMM", CultureInfo.InvariantCulture).Month;
            }
            SchoolModel model = new SchoolModel();
            model.schoolId = schoolId;
            model.month = monthInDigit;
            model.year = year;

            if ((schoolId == 0) && (monthInDigit == 0))
                opr = 1;
            else if ((schoolId != 0) && (monthInDigit == 0))
                opr = 2;
            else if ((schoolId == 0) && (monthInDigit != 0))
                opr = 3;
            else if ((schoolId != 0) && (monthInDigit != 0))
                opr = 4;

            model.opr = opr;
            return PartialView("~/Views/SuperAdmin/_pv_PaymentList_Grid.cshtml", model);
        }
        public object StatusPaymentGateway(string id)
        {
            string msg = "Failed";
            bool status = false;

            string[] data = id.Split('~');

            long schoolId = Convert.ToInt64(data[0]);
            int paymentStatus = Convert.ToInt16(data[1]);

            var school = _Entities.tb_School.Where(z => z.IsActive && z.SchoolId == schoolId).FirstOrDefault();
            if (school != null)
            {
                if (paymentStatus == 1)
                    school.PaymentOption = false;
                else
                    school.PaymentOption = true;

                status = _Entities.SaveChanges() > 0;
                msg = status ? " Activated" : "No changes made";
            }

            return Json(new { msg = msg, status = status }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult RefreshSchoolGrid()
        {
            return PartialView("~/Views/SuperAdmin/_pv_School_list.cshtml");
        }

        public IActionResult StudentExcelUpload()
        {
            return View();
        }
        [HttpPost]
        public object UploadStudentExcel()
        {
            bool status = false;
            string msg = "failed";
            string message = "";
            //String FileExt = Path.GetExtension(files.FileName).ToUpper();

            if (Request.Files.Count > 0)
            {
                var httpPostedFile = Request.Files[0];
                string folderPath = Server.MapPath("~/Media/Excel/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string pdfName = httpPostedFile.FileName;
                string fileExtension = Path.GetExtension(httpPostedFile.FileName);

                var pdfFilePath = Server.MapPath("~/Media/Excel/" + pdfName);
                var fileSave = "/Media/Excel/" + pdfName;
                httpPostedFile.SaveAs(pdfFilePath);
                msg = "Success";
                status = true;

                //---------------------------------------------------------------------------------------------------------
                try
                {
                    if (pdfFilePath != null)
                    {
                        //string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/upload/");
                        //string fileExtension = Path.GetExtension(model.file.FileName);
                        //string fileName = Guid.NewGuid().ToString();
                        //if (!Directory.Exists(folderPath))
                        //    Directory.CreateDirectory(folderPath);
                        //var fileSavePath = Path.Combine(folderPath, fileName + fileExtension);
                        //filePath = fileSavePath;
                        //model.file.SaveAs(fileSavePath);

                        string conString = string.Empty;
                        switch (fileExtension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = System.Configuration.ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = System.Configuration.ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                                break;
                        }

                        DataTable dt = new DataTable();
                        conString = string.Format(conString, pdfFilePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    //cmdExcel.CommandText = "SELECT * From [" + sheetName + "] where ([SerialNumber] is not null and [SerialNumber]!='') and ([PinNumber] is not null and [PinNumber]!='') and ([CardType] is not null and [CardType]!='') and ([Amount] is not null and [Amount]!='')";
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }

                        //DataTable dt1 = new DataTable();
                        //var rows = from row in dt.AsEnumerable()
                        //           where row.Field<double>("SerialNumber") != 0
                        //           //row.Field<string>("PinNumber") != null &&
                        //           //row.Field<string>("CardType") != null &&
                        //           //row.Field<double>("Amount") != 0
                        //           select row;
                        //foreach (DataRow dr in rows)
                        //{
                        //    dt1.Rows.Add(dr);
                        //}


                        var conStrindda = System.Configuration.ConfigurationManager.ConnectionStrings["ForExcelSchoolMan"];
                        var conString32 = System.Configuration.ConfigurationManager.ConnectionStrings["ForExcelSchoolMan"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(conString32))
                        {
                            //29-08-2018 premjith
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tb_Student";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("SchoolId", "SchoolId");
                                sqlBulkCopy.ColumnMappings.Add("StudentSpecialId", "StudentSpecialId");
                                sqlBulkCopy.ColumnMappings.Add("StundentName", "StundentName");
                                sqlBulkCopy.ColumnMappings.Add("ParentName", "ParentName");
                                sqlBulkCopy.ColumnMappings.Add("Address", "Address");
                                sqlBulkCopy.ColumnMappings.Add("City", "City");
                                sqlBulkCopy.ColumnMappings.Add("ContactNumber", "ContactNumber");
                                sqlBulkCopy.ColumnMappings.Add("ClasssNumber", "ClasssNumber");
                                sqlBulkCopy.ColumnMappings.Add("ClassId", "ClassId");
                                sqlBulkCopy.ColumnMappings.Add("DivisionId", "DivisionId");
                                sqlBulkCopy.ColumnMappings.Add("BusId", "BusId");
                                sqlBulkCopy.ColumnMappings.Add("TripNo", "TripNo");
                                sqlBulkCopy.ColumnMappings.Add("FilePath", "FilePath");
                                sqlBulkCopy.ColumnMappings.Add("TimeStamp", "TimeStamp");
                                sqlBulkCopy.ColumnMappings.Add("StudentGuid", "StudentGuid");
                                sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                                sqlBulkCopy.ColumnMappings.Add("ParentId", "ParentId");
                                sqlBulkCopy.ColumnMappings.Add("State", "State");
                                sqlBulkCopy.ColumnMappings.Add("Gender", "Gender");
                                sqlBulkCopy.ColumnMappings.Add("BloodGroup", "BloodGroup");
                                sqlBulkCopy.ColumnMappings.Add("ParentEmail", "ParentEmail");
                                sqlBulkCopy.ColumnMappings.Add("PostalCode", "PostalCode");
                                sqlBulkCopy.ColumnMappings.Add("DOB", "DOB");
                                sqlBulkCopy.ColumnMappings.Add("Aadhaar", "Aadhaar");
                                sqlBulkCopy.ColumnMappings.Add("BioNumber", "BioNumber");
                                con.Open();
                                //sqlBulkCopy.WriteToServer(dt1);
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }


                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    try
                            //    {
                            //        string SerialNumber = Convert.ToString(row.Field<double?>("SerialNumber"));
                            //        string PinNumber = Convert.ToString(row.Field<string>("PinNumber"));
                            //        string CardType = Convert.ToString(row.Field<string>("CardType"));
                            //        string Amount = Convert.ToString(row.Field<double?>("Amount"));

                            //        if (SerialNumber != null && SerialNumber != string.Empty &&
                            //            PinNumber != null && PinNumber != string.Empty &&
                            //            CardType != null && CardType != string.Empty &&
                            //            Amount != null && Amount != string.Empty)
                            //        {
                            //            try
                            //            {
                            //                var returenValue = _context.sp_add_importedcards(SerialNumber, PinNumber, CardType, Amount, "AED");
                            //            }
                            //            catch (Exception ex1)
                            //            {

                            //            }
                            //        }

                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        status = false;
                            //        message = ex.Message;
                            //    }
                            //}


                        }
                        status = true;
                        message = "Success";
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
                //-------------------------------------------------------------------------------------------------------
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }


        public IActionResult SmsAllHistory()
        {
            FeeModel model = new FeeModel();
            model.StartDate = CurrentTime;
            return View(model);
        }
        public PartialViewResult SmsAllHistoryByDate(string id)
        {
            FeeModel model = new FeeModel();

            string[] splitData = id.Split('~');


            DateTime FDate = Convert.ToDateTime(splitData[0]);
            DateTime LDate = Convert.ToDateTime(splitData[1]);

            //string[] splitFDate = FDate.Split('-');
            //string startDate = splitFDate[1] + '/' + splitFDate[0] + '/' + splitFDate[2];

            //string[] splitLDate = LDate.Split('-');
            //string endDate = splitLDate[1] + '/' + splitLDate[0] + '/' + splitLDate[2] + ' ' + "11:59:00 PM";

            string endDate = LDate.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            model.StartDate = Convert.ToDateTime(FDate);
            model.EndDate = Convert.ToDateTime(endDate);
            return PartialView("~/Views/SuperAdmin/_SmsAllHistory_Grid.cshtml", model);
        }

        public IActionResult PackageList(string id)
        {

            SmsPackageModels model = new SmsPackageModels();
            model.SchoolId = Convert.ToInt64(id);
            model.SmsStatus = _Entities.tb_School.Where(z => z.SchoolId == model.SchoolId).FirstOrDefault().SmsActive;
            model.FromDate = CurrentTime.ToString(); ;
            return View(model);
        }

        public PartialViewResult AddPackageView(string id)
        {
            SmsPackageModels model = new SmsPackageModels();
            model.SchoolId = Convert.ToInt64(id);
            return PartialView("~/Views/SuperAdmin/_pv_AddPackage.cshtml", model);
        }
        [HttpPost]
        public object AddSmsPackage(SmsPackageModels model)
        {
            var message = "failed";
            var status = false;
            var smsPackage = new tb_SmsPackage();
            smsPackage.SchoolId = model.SchoolId;

            if (model.ToDate != null)
            {
                string[] splitData1 = model.ToDate.Split('-');
                var dd1 = splitData1[0];
                var mm1 = splitData1[1];
                var yyyy1 = splitData1[2];
                var dat1 = mm1 + '-' + dd1 + '-' + yyyy1;
                smsPackage.ToDate = Convert.ToDateTime(dat1);
            }
            if (model.FromDate != null)
            {
                string[] splitData12 = model.FromDate.Split('-');
                var dd12 = splitData12[0];
                var mm12 = splitData12[1];
                var yyyy12 = splitData12[2];
                var dat12 = mm12 + '-' + dd12 + '-' + yyyy12;
                smsPackage.FromDate = Convert.ToDateTime(dat12);
            }
            smsPackage.SmsRate = model.SmsRate;
            smsPackage.AllowedSms = model.AllowedSms;
            smsPackage.IsActive = true;
            smsPackage.IsDisabled = true;
            smsPackage.TimeStamp = CurrentTime;
            _Entities.tb_SmsPackage.Add(smsPackage);
            if (_Entities.SaveChanges() > 0)
            {
                message = "success";
                status = true;
            }
            return Json(new { Status = status, Message = message, }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult RefreshSmsPackageGrid(string id)
        {
            SmsPackageModels model = new SmsPackageModels();
            model.SchoolId = Convert.ToInt64(id);
            return PartialView("~/Views/SuperAdmin/_pv_Package_list.cshtml", model);
        }

        public object SmsActivate(string id)
        {
            string msg = "Failed";
            bool status = false;

            string[] data = id.Split('~');

            long schoolId = Convert.ToInt64(data[0]);
            int smsStatus = Convert.ToInt16(data[1]);

            var school = _Entities.tb_School.Where(z => z.IsActive && z.SchoolId == schoolId).FirstOrDefault();
            if (school != null)
            {
                if (smsStatus == 1)
                {
                    school.SmsActive = true;
                    msg = " Activated";
                }
                else
                {
                    school.SmsActive = false;
                    msg = " Deactivated";

                }

                status = _Entities.SaveChanges() > 0;
                msg = status ? msg : "No changes made";
            }

            return Json(new { msg = msg, status = status }, JsonRequestBehavior.AllowGet);
        }

        public object SmsPackageActivate(string id)
        {
            string msg = "Failed";
            bool status = false;

            string[] data = id.Split('~');

            long schoolId = Convert.ToInt64(data[0]);
            int packageId = Convert.ToInt16(data[1]);
            int smsStatus = Convert.ToInt16(data[2]);

            var package = _Entities.tb_SmsPackage.Where(z => z.IsActive && z.SchoolId == schoolId).ToList();
            if (package.Count > 0)
            {
                if (smsStatus == 0)
                {
                    foreach (var item in package)
                    {
                        if (item.PackageId == packageId)
                        {
                            item.IsDisabled = false;
                            _Entities.SaveChanges();
                        }
                        else
                        {
                            item.IsDisabled = true;
                            _Entities.SaveChanges();
                        }
                    }
                    msg = " Activated";
                    status = true;
                }
                else
                {
                    foreach (var item in package)
                    {
                        item.IsDisabled = true;
                        _Entities.SaveChanges();
                    }
                    msg = " Deactivated";
                    status = true;
                }

                msg = status ? msg : "No changes made";
            }

            return Json(new { msg = msg, status = status }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult SchoolTypeDefineHome()
        {
            SchoolModuleModel model = new SchoolModuleModel();
            // model.SchoolId = _user.SchoolId;
            model.mainList = new List<SchoolMainModuleList>();
            var list = _Entities.tb_SchoolSubModule.Where(x => x.IsActive).ToList();
            var mainList = list.Select(x => x.tb_SchoolModuleHome.MainModule).Distinct().ToList();
            foreach (var item in mainList)
            {
                string subIdString = "";
                string main = Convert.ToString(item);
                var sub = list.Where(x => x.tb_SchoolModuleHome.MainModule == main).ToList();
                SchoolMainModuleList one = new SchoolMainModuleList();
                one.Id = sub.FirstOrDefault().Id;
                one.ModuleName = main;
                one.subList = new List<SchoolSubModuleList>();
                foreach (var a in sub)
                {
                    SchoolSubModuleList b = new SchoolSubModuleList();
                    b.MainId = one.Id;
                    b.Id = a.Id;
                    b.SubMosule = a.SchoolSubModule;
                    one.subList.Add(b);
                    if (subIdString == "")
                        subIdString = a.Id.ToString();
                    else
                        subIdString = subIdString + "," + a.Id.ToString();
                }
                one.subIdListString = subIdString;
                model.mainList.Add(one);
            }
            //model.IsAdmin = false;
            return View(model);
        }



    }
}