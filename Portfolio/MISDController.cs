using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Intranet.Filters;
using Intranet.Models;
using Constant;
using System.Data.SqlClient;
using System.Net;
using System.Globalization;
using static Intranet.Models.Software_IT_Inventory;

namespace Intranet.Controllers
{
    [ValidateUserSession]
    public class MISDController : Controller
    {
        public SQLActions sql;
        public SqlDataReader rsData;
        // GET: MISD
        public ActionResult Index()
        {
            return View();
        }
        [CheckAuthorization]
        public ActionResult ARMISD()
        {
            Buyer.BuyerInfo bi = new Buyer.BuyerInfo();
            if (TempData["MISDForm"] != null)
            {
                bi = (Buyer.BuyerInfo)TempData["MISDForm"];
            }
            bi.AccountName = "test name";
            return View(bi);
        }
        [CheckAuthorization]
        public ActionResult SaveMISD(Buyer.BuyerInfo bi)
        {
            TempData["Message"] = bi.AccountName;
            TempData["MISDForm"] = bi;
            return RedirectToAction("ARMISD");
        }
        //For IT Inventory
        [CheckAuthorization]
        public ActionResult ITInventory()
        {
            MainForm mainForm = new MainForm();
            if (TempData["tmpMainForm"] != null)
            {
                mainForm = (MainForm)TempData["tmpMainForm"];
                mainForm.ITList = GetITInventoryMasterlist();
                TempData["tmpMainForm"] = null;
                return View(mainForm);
            }
            mainForm.ITList = GetITInventoryMasterlist();
            return View(mainForm);
        }
        [CheckAuthorization]
        public List<masterITDetails> GetITInventoryMasterlist()
        {
            List<masterITDetails> masterlist = new List<masterITDetails>();
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITList");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new masterITDetails
                            {
                                ControlNo = rsData["ControlNo"].ToString(),
                                IsLaptop = (bool)rsData["IsLaptop"],
                                Type = rsData["Type"].ToString(),
                                Hostname = rsData["Hostname"].ToString(),
                                PCUser = rsData["PCUser"].ToString(),
                                CPU = rsData["CPU"].ToString(),
                                Ram_Description = rsData["Ram_Description"].ToString(),
                                Ram_Size = Convert.ToInt32(rsData["Ram_Size"]),
                                Storage_Description = rsData["Storage_Description"].ToString(),
                                Storage_Size = Convert.ToInt32(rsData["Storage_Size"]),
                                IfStaticIP = (bool)rsData["IfStaticIP"],
                                IPAddress = rsData["IPAddress"].ToString(),
                                LANMAC = rsData["LANMAC"].ToString(),
                                WLANMAC = rsData["WLANMAC"].ToString(),
                                WithSpiceworks = (bool)rsData["WithSpiceworks"]
                            };
                            masterlist.Add(software);
                        }
                    }
                }
            }
            return masterlist;
        }

        [CheckAuthorization]
        public ActionResult GetITDetails(string itLoadCode)
        {
            MainForm mainForm = new MainForm();
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITDetails");
                sql.AddParametersWithValue("@ControlNo", itLoadCode);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            mainForm.ITDetails.ControlNo = rsData["ControlNo"].ToString();
                            mainForm.ITDetails.PCUser = rsData["PCUser"].ToString();
                            mainForm.ITDetails.Hostname = rsData["Hostname"].ToString();
                            mainForm.ITDetails.IsLaptop = (bool)rsData["IsLaptop"];
                            mainForm.ITDetails.CPU = rsData["CPU"].ToString();
                            mainForm.ITDetails.Ram_Description = rsData["Ram_Description"].ToString();
                            mainForm.ITDetails.Ram_Size = Convert.ToInt32(rsData["Ram_Size"]);
                            mainForm.ITDetails.AcquisitionCost = Convert.ToDecimal(rsData["AcquisitionCost"]);
                            mainForm.ITDetails.Storage_Description = rsData["Storage_Description"].ToString();
                            mainForm.ITDetails.Storage_Size = Convert.ToInt32(rsData["Storage_Size"]);
                            mainForm.ITDetails.IfStaticIP = (bool)rsData["IfStaticIP"];
                            mainForm.ITDetails.IPAddress = rsData["IPAddress"].ToString();
                            mainForm.ITDetails.LANMAC = rsData["LANMAC"].ToString();
                            mainForm.ITDetails.WLANMAC = rsData["WLANMAC"].ToString();
                            mainForm.ITDetails.WithSpiceworks = (bool)rsData["WithSpiceworks"];
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetSoftwareListIT");
                sql.AddParametersWithValue("@controlNo", itLoadCode);
                if (itLoadCode != "")
                {
                    rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                    using (rsData)
                    {
                        if (rsData.HasRows)
                        {
                            while (rsData.Read())
                            {
                                var software = new Software_IT_Inventory.masterSoftwareDetails
                                {
                                    Code = rsData["Code"].ToString(),
                                    ControlNo = rsData["ControlNo"].ToString(),
                                    Description = rsData["Description"].ToString(),
                                    ProductKey = rsData["ProductKey"].ToString(),
                                    Remarks = rsData["Remarks"].ToString()
                                };
                                mainForm.SoftwareList.Add(software);
                            }
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetPeripheralsListIT");
                sql.AddParametersWithValue("@ControlNo", itLoadCode);
                if (itLoadCode != "")
                {
                    rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                    using (rsData)
                    {
                        if (rsData.HasRows)
                        {
                            while (rsData.Read())
                            {
                                var peripherals = new Software_IT_Inventory.masterPeripheralsDetails
                                {
                                    PCControlNo = rsData["PCControlNo"].ToString(),
                                    ControlNo = rsData["ControlNo"].ToString(),
                                    Description = rsData["Description"].ToString(),
                                    Remarks = rsData["Remarks"].ToString(),
                                    Location = rsData["LocDesc"].ToString(),
                                };
                                mainForm.PeripheralsList.Add(peripherals);
                            }
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITLogsList");
                sql.AddParametersWithValue("@ControlNo", itLoadCode);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new masterITLogsDetails
                            {
                                Ndx = Convert.ToInt32(rsData["Ndx"]),
                                Date = rsData["Date"].ToString(),
                                DoneBy = rsData["DoneBy"].ToString(),
                                Description = rsData["Description"].ToString(),
                                Remarks = rsData["Remarks"].ToString(),
                            };
                            mainForm.ITLogsList.Add(software);
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ITInventory_Retrieve";
                sql.AddParametersWithValue("@mode", "SubCategory");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var SubCategories = new SelectListItem
                            {
                                Value = rsData["SubCategoryID"].ToString(),
                                Text = rsData["Description"].ToString(),
                            };
                            mainForm.SubCategories.Add(SubCategories);
                        }
                    }
                }
            }
            TempData["tmpMainForm"] = mainForm;
            return RedirectToAction("ITInventory");
        }
        [CheckAuthorization]
        public ActionResult LoadPeripheralsList(Software_IT_Inventory.MainForm mainForm, string subCategoryID, string PCControlNo)
        {
            mainForm.PeripheralsList = new List<Software_IT_Inventory.masterPeripheralsDetails>();
            mainForm.ITDetails.ControlNo = PCControlNo;
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ITInventory_Retrieve";
                sql.AddParametersWithValue("@mode", "PeripheralsMF");
                sql.AddParametersWithValue("@subCategoryID", subCategoryID);
                sql.AddParametersWithValue("@PCControlNo", PCControlNo);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var Peripherals = new masterPeripheralsDetails
                            {
                                PCControlNo = rsData["PCControlNo"].ToString(),
                                ControlNo = rsData["ControlNo"].ToString(),
                                Description = rsData["Description"].ToString(),
                                Remarks = rsData["Remarks"].ToString(),
                                Location = rsData["Location"].ToString(),
                                checkbox = Convert.ToBoolean(rsData["IsChecked"]),
                            };
                            mainForm.PeripheralsList.Add(Peripherals);
                        }
                    }
                }
            }
            return PartialView("_PeripheralsMasterlist", mainForm);
        }
        [CheckAuthorization]
        public ActionResult LoadControlNoMasterlist(Software_IT_Inventory.MainForm mainForm)
        {
            mainForm.ITList = new List<Software_IT_Inventory.masterITDetails>();
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITList");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new masterITDetails
                            {
                                ControlNo = rsData["ControlNo"].ToString(),
                                IsLaptop = (bool)rsData["IsLaptop"],
                                Type = rsData["ControlNo"].ToString(),
                                Hostname = rsData["Hostname"].ToString(),
                                PCUser = rsData["PCUser"].ToString(),
                                CPU = rsData["CPU"].ToString(),
                                Ram_Description = rsData["Ram_Description"].ToString(),
                                Ram_Size = Convert.ToInt32(rsData["Ram_Size"]),
                                Storage_Description = rsData["Storage_Description"].ToString(),
                                Storage_Size = Convert.ToInt32(rsData["Storage_Size"]),  
                                AcquisitionCost = Convert.ToDecimal(rsData["AcquisitionCost"]),
                                IfStaticIP = (bool)rsData["IfStaticIP"],
                                IPAddress = rsData["IPAddress"].ToString(),
                                LANMAC = rsData["LANMAC"].ToString(),
                                WLANMAC = rsData["WLANMAC"].ToString(),
                                WithSpiceworks = (bool)rsData["WithSpiceworks"]
                            };
                            mainForm.ITList.Add(software);
                        }
                    }
                }
            }
            return PartialView("_ControlNoMasterlist", mainForm);
        }
        [CheckAuthorization]
        public ActionResult LoadControlNoLookup(Software_IT_Inventory.MainForm mainForm)
        {
            mainForm.ControlNoList = new List<Software_IT_Inventory.masterControlNoDetails>();
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ITInventory_Retrieve";
                sql.AddParametersWithValue("@mode", "ListFromAdmin");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var controlNo = new Software_IT_Inventory.masterControlNoDetails
                            {
                                ControlNo = rsData["ControlNo"].ToString(),
                                Description = rsData["Description"].ToString(),
                                IsLaptop = Convert.ToBoolean(rsData["IsLaptop"]),
                                Remarks = rsData["Remarks"].ToString(),                             
                                Location = rsData["Location"].ToString(),
                            };
                            string acquisitionCostString = rsData["AcquisitionCost"].ToString();
                            CultureInfo phCulture = new CultureInfo("en-PH");
                            controlNo.AcquisitionCost = decimal.Parse(acquisitionCostString, NumberStyles.Currency, phCulture);
                            controlNo.AcquisitionCost = decimal.Parse(controlNo.AcquisitionCost.ToString("N2"), phCulture);

                            mainForm.ControlNoList.Add(controlNo);
                        }
                    }
                }
            }
            return PartialView("_ControlNoLookup", mainForm);
        }
        [CheckAuthorization]
        public ActionResult LoadAddSoftwareList(Software_IT_Inventory.MainForm mainForm, string PCControlNo)
        {
            mainForm.SoftwareList = new List<Software_IT_Inventory.masterSoftwareDetails>();
            mainForm.ITDetails.ControlNo = PCControlNo;
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsADD_LINK";
                sql.AddParametersWithValue("@mode", "AddSoftwareList");
                sql.AddParametersWithValue("@controlNo", PCControlNo);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new Software_IT_Inventory.masterSoftwareDetails
                            {
                                Code = rsData["Code"].ToString(),
                                ControlNo = rsData["ControlNo"].ToString(),
                                Description = rsData["Description"].ToString(),
                                ProductKey = rsData["ProductKey"].ToString(),
                                Remarks = rsData["Remarks"].ToString(),
                                checkbox = Convert.ToBoolean(rsData["IsChecked"])
                            };
                            mainForm.SoftwareList.Add(software);
                        }
                    }
                }
            }
            return PartialView("_AddSoftwareList", mainForm);
        }
        //Responsible for Loading the Masterlist (first page)
        [CheckAuthorization]
        public ActionResult ITInventoryMasterlist(Software_IT_Inventory.MainForm mainForm,string ITBtnName)
        {
            mainForm.ITDetails = new Software_IT_Inventory.masterITDetails();
            mainForm.ITList = new List<Software_IT_Inventory.masterITDetails>();
            if (ITBtnName == "BackToMasterlist")
            {
                sql = new SQLActions();
                using (sql)
                {
                    sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                    sql.AddParametersWithValue("@mode", "GetITList");
                    rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                    using (rsData)
                    {
                        if (rsData.HasRows)
                        {
                            while (rsData.Read())
                            {
                                var software = new Software_IT_Inventory.masterITDetails
                                {
                                    ControlNo = rsData["ControlNo"].ToString(),
                                    IsLaptop = (bool)rsData["IsLaptop"],
                                    Type = rsData["Type"].ToString(),
                                    Hostname = rsData["Hostname"].ToString(),
                                    PCUser = rsData["PCUser"].ToString(),
                                    CPU = rsData["CPU"].ToString(),
                                    Ram_Description = rsData["Ram_Description"].ToString(),
                                    Ram_Size = Convert.ToInt32(rsData["Ram_Size"]),
                                    Storage_Description = rsData["Storage_Description"].ToString(),
                                    Storage_Size = Convert.ToInt32(rsData["Storage_Size"]),
                                    IfStaticIP = (bool)rsData["IfStaticIP"],
                                    IPAddress = rsData["IPAddress"].ToString(),
                                    LANMAC = rsData["LANMAC"].ToString(),
                                    WLANMAC = rsData["WLANMAC"].ToString(),
                                    WithSpiceworks = (bool)rsData["WithSpiceworks"]
                                };
                                mainForm.ITList.Add(software);
                            }
                        }
                    }
                    return View("ITInventoryMasterlist", mainForm);
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITList");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new Software_IT_Inventory.masterITDetails
                            {
                                ControlNo = rsData["ControlNo"].ToString(),
                                IsLaptop = (bool)rsData["IsLaptop"],
                                Type = rsData["Type"].ToString(),
                                Hostname = rsData["Hostname"].ToString(),
                                PCUser = rsData["PCUser"].ToString(),
                                CPU = rsData["CPU"].ToString(),
                                Ram_Description = rsData["Ram_Description"].ToString(),
                                Ram_Size = Convert.ToInt32(rsData["Ram_Size"]),
                                Storage_Description = rsData["Storage_Description"].ToString(),
                                Storage_Size = Convert.ToInt32(rsData["Storage_Size"]),
                                AcquisitionCost = Convert.ToDecimal(rsData["AcquisitionCost"]),
                                IfStaticIP = (bool)rsData["IfStaticIP"],
                                IPAddress = rsData["IPAddress"].ToString(),
                                LANMAC = rsData["LANMAC"].ToString(),
                                WLANMAC = rsData["WLANMAC"].ToString(),
                                WithSpiceworks = (bool)rsData["WithSpiceworks"]
                            };
                            mainForm.ITList.Add(software);
                        }
                    }
                }
                return View("ITInventoryMasterlist",mainForm);
            }           
        } 
        [CheckAuthorization]
        public ActionResult LoadITInventory(string itLoadCode, string subCategoryID, string PCControlNo)
        {
            Software_IT_Inventory.MainForm mainForm = new Software_IT_Inventory.MainForm();
            mainForm.ITDetails = new Software_IT_Inventory.masterITDetails();
            mainForm.ITList = new List<Software_IT_Inventory.masterITDetails>();
            mainForm.PeripheralsList = new List<Software_IT_Inventory.masterPeripheralsDetails>();
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITList");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new Software_IT_Inventory.masterITDetails
                            {
                                ControlNo = rsData["ControlNo"].ToString(),
                                IsLaptop = (bool)rsData["IsLaptop"],
                                Hostname = rsData["Hostname"].ToString(),
                                PCUser = rsData["PCUser"].ToString(),
                                CPU = rsData["CPU"].ToString(),
                                Ram_Description = rsData["Ram_Description"].ToString(),
                                Ram_Size = Convert.ToInt32(rsData["Ram_Size"]),
                                Storage_Description = rsData["Storage_Description"].ToString(),
                                Storage_Size = Convert.ToInt32(rsData["Storage_Size"]),
                                IfStaticIP = (bool)rsData["IfStaticIP"],
                                IPAddress = rsData["IPAddress"].ToString(),
                                LANMAC = rsData["LANMAC"].ToString(),
                                WLANMAC = rsData["WLANMAC"].ToString(),
                                WithSpiceworks = (bool)rsData["WithSpiceworks"]
                            };
                            mainForm.ITList.Add(software);
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITDetails");
                sql.AddParametersWithValue("@ControlNo", itLoadCode);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            //To read from sql database
                            mainForm.ITDetails.ControlNo = rsData["ControlNo"].ToString();
                            mainForm.ITDetails.PCUser = rsData["PCUser"].ToString();
                            mainForm.ITDetails.Hostname = rsData["Hostname"].ToString();
                            mainForm.ITDetails.IsLaptop = (bool)rsData["IsLaptop"];
                            mainForm.ITDetails.CPU = rsData["CPU"].ToString();
                            mainForm.ITDetails.Ram_Description = rsData["Ram_Description"].ToString();
                            mainForm.ITDetails.Ram_Size = Convert.ToInt32(rsData["Ram_Size"]);
                            mainForm.ITDetails.Storage_Description = rsData["Storage_Description"].ToString();
                            mainForm.ITDetails.Storage_Size = Convert.ToInt32(rsData["Storage_Size"]);
                            mainForm.ITDetails.AcquisitionCost = Convert.ToInt32(rsData["AcquisitionCost"]);
                            mainForm.ITDetails.IfStaticIP = (bool)rsData["IfStaticIP"];
                            mainForm.ITDetails.IPAddress = rsData["IPAddress"].ToString();
                            mainForm.ITDetails.LANMAC = rsData["LANMAC"].ToString();
                            mainForm.ITDetails.WLANMAC = rsData["WLANMAC"].ToString();
                            mainForm.ITDetails.WithSpiceworks = (bool)rsData["WithSpiceworks"];
                        }
                    }
                }
            }            
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetSoftwareListIT");
                sql.AddParametersWithValue("@ControlNo", itLoadCode);
                if(itLoadCode != "")
                {
                    rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                    using (rsData)
                    {
                        if (rsData.HasRows)
                        {
                            while (rsData.Read())
                            {
                                var software = new Software_IT_Inventory.masterSoftwareDetails
                                {
                                    Code = rsData["Code"].ToString(),
                                    ControlNo = rsData["ControlNo"].ToString(),
                                    Description = rsData["Description"].ToString(),
                                    ProductKey = rsData["ProductKey"].ToString(),
                                    Remarks = rsData["Remarks"].ToString()
                                };
                                mainForm.SoftwareList.Add(software);
                            }
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetPeripheralsListIT");
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                if (itLoadCode != "")
                {
                    rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                    using (rsData)
                    {
                        if (rsData.HasRows)
                        {
                            while (rsData.Read())
                            {
                                var peripherals = new Software_IT_Inventory.masterPeripheralsDetails
                                {                                    
                                    ControlNo = rsData["ControlNo"].ToString(),
                                    PCControlNo = rsData["PCControlNo"].ToString(),
                                    Description = rsData["Description"].ToString(),
                                    Remarks = rsData["Remarks"].ToString(),
                                    Location = rsData["LocDesc"].ToString()
                                };
                                mainForm.PeripheralsList.Add(peripherals);
                            }
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ITInventory_Retrieve";
                sql.AddParametersWithValue("@mode", "PeripheralsMF");
                sql.AddParametersWithValue("@subCategoryID", subCategoryID);
                sql.AddParametersWithValue("@PCControlNo", mainForm.ITDetails.ControlNo);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var Peripherals = new masterPeripheralsDetails
                            {
                                PCControlNo = rsData["PCControlNo"].ToString(),
                                ControlNo = rsData["ControlNo"].ToString(),
                                Description = rsData["Description"].ToString(),
                                Remarks = rsData["Remarks"].ToString(),
                                Location = rsData["Location"].ToString(),
                                checkbox = Convert.ToBoolean(rsData["IsChecked"]),
                            };
                            mainForm.PeripheralsList.Add(Peripherals);
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ITInventory_Retrieve";
                sql.AddParametersWithValue("@mode", "SubCategory");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var SubCategories = new SelectListItem
                            {
                                Value = rsData["SubCategoryID"].ToString(),
                                Text = rsData["Description"].ToString(),
                            };
                            mainForm.SubCategories.Add(SubCategories);
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsGET_LIST";
                sql.AddParametersWithValue("@mode", "GetITLogsList");
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new Software_IT_Inventory.masterITLogsDetails
                            {
                                Ndx = Convert.ToInt32(rsData["Ndx"]),
                                Date = rsData["Date"].ToString(),
                                DoneBy = rsData["DoneBy"].ToString(),
                                Description = rsData["Description"].ToString(),
                                Remarks = rsData["Remarks"].ToString()
                            };
                            mainForm.ITLogsList.Add(software);
                        }
                    }
                }
            }
            TempData["tmpMainForm"] = mainForm;
            return RedirectToAction("ITInventory");
        }
        //IT Inventory button functions 'CRUD'
        [HttpPost]
        [CheckAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyITDetails(Software_IT_Inventory.MainForm mainForm, string ITBtnName, string ITLogsBtnName, 
            string AddSoftwareBtnName, string PeripheralsBtnName, string LinkBtnName)
        {
            if (ITBtnName == "Save")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsADD_LINK";
                sql.AddParametersWithValue("@mode", "AddITList");
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                sql.AddParametersWithValue("@IsLaptop", mainForm.ITDetails.IsLaptop);
                sql.AddParametersWithValue("@PCUser", mainForm.ITDetails.PCUser);
                sql.AddParametersWithValue("@Hostname", mainForm.ITDetails.Hostname);
                sql.AddParametersWithValue("@CPU", mainForm.ITDetails.CPU);
                sql.AddParametersWithValue("@Ram_Description", mainForm.ITDetails.Ram_Description);
                sql.AddParametersWithValue("@Ram_Size", mainForm.ITDetails.Ram_Size);
                sql.AddParametersWithValue("@Storage_Description", mainForm.ITDetails.Storage_Description);
                sql.AddParametersWithValue("@Storage_Size", mainForm.ITDetails.Storage_Size);
                sql.AddParametersWithValue("@IfStaticIP", mainForm.ITDetails.IfStaticIP);
                sql.AddParametersWithValue("@IPAddress", mainForm.ITDetails.IPAddress);
                sql.AddParametersWithValue("@LANMAC", mainForm.ITDetails.LANMAC);
                sql.AddParametersWithValue("@WLANMAC", mainForm.ITDetails.WLANMAC);
                sql.AddParametersWithValue("@WithSpiceworks", mainForm.ITDetails.WithSpiceworks);
                if (sql.DoAction())
                {
                    TempData["Message"] = "Successfully saved.";
                    sql.CommitTransaction();
                    sql.Dispose();
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to Save.";
                    sql.Dispose();
                }
                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            else if (ITBtnName == "Update")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_UpdateITList";
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                sql.AddParametersWithValue("@IsLaptop", mainForm.ITDetails.IsLaptop);
                sql.AddParametersWithValue("@PCUser", mainForm.ITDetails.PCUser);
                sql.AddParametersWithValue("@Hostname", mainForm.ITDetails.Hostname);
                sql.AddParametersWithValue("@CPU", mainForm.ITDetails.CPU);
                sql.AddParametersWithValue("@Ram_Description", mainForm.ITDetails.Ram_Description);
                sql.AddParametersWithValue("@Ram_Size", mainForm.ITDetails.Ram_Size);
                sql.AddParametersWithValue("@Storage_Description", mainForm.ITDetails.Storage_Description);
                sql.AddParametersWithValue("@Storage_Size", mainForm.ITDetails.Storage_Size);
                sql.AddParametersWithValue("@IfStaticIP", mainForm.ITDetails.IfStaticIP);
                sql.AddParametersWithValue("@IPAddress", mainForm.ITDetails.IPAddress);
                sql.AddParametersWithValue("@LANMAC", mainForm.ITDetails.LANMAC);
                sql.AddParametersWithValue("@WLANMAC", mainForm.ITDetails.WLANMAC);
                sql.AddParametersWithValue("@WithSpiceworks", mainForm.ITDetails.WithSpiceworks);
                bool isSuccess = sql.DoAction();
                if (isSuccess)
                {
                    TempData["Message"] = "Successfully updated.";
                    sql.CommitTransaction();
                    sql.Dispose();
                    return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to update.";
                    sql.Dispose();
                    return RedirectToAction("LoadITInventory");
                }
            }
            else if (ITLogsBtnName == "DeleteLogs")
            {
                bool blnWithError = false;
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                foreach (var item in mainForm.ITLogsList)
                {
                    if (item.logsCheckbox)
                    {
                        sql.StoredProcedureName = "spOPMAS_ModifyITDetailsCLEAR_DELETE";
                        sql.AddParametersWithValue("@mode", "DelLogsCheckbox");
                        sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                        sql.AddParametersWithValue("@Ndx", item.Ndx);
                        if (!sql.DoAction())
                        {
                            blnWithError = true;
                            break;
                        }
                    }
                }
                if (blnWithError)
                {
                    TempData["Message"] = sql.ErrorMessage;
                    sql.RollBack();
                }
                else
                {
                    sql.CommitTransaction();
                    TempData["Message"] = "Logs Deleted.";
                }

                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            else if (PeripheralsBtnName == "DeletePeripherals")
            {
                bool blnWithError = false;
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                foreach (var item in mainForm.PeripheralsList)
                {
                    if (item.checkbox)
                    {
                        sql.StoredProcedureName = "spOPMAS_ModifyITDetailsCLEAR_DELETE";
                        sql.AddParametersWithValue("@mode", "DelPeripheralsCheckbox");
                        sql.AddParametersWithValue("@controlNo", item.ControlNo);
                        if (!sql.DoAction())
                        {
                            blnWithError = true;
                            break;
                        }
                    }
                }
                if (blnWithError)
                {
                    TempData["Message"] = sql.ErrorMessage;
                    sql.RollBack();
                }
                else
                {
                    sql.CommitTransaction();
                    TempData["Message"] = "Peripherals Deleted.";
                }

                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            else if (ITBtnName == "Delete")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsCLEAR_DELETE";
                sql.AddParametersWithValue("@mode", "DelITAll");
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                bool isSuccess = sql.DoAction();
                if (isSuccess)
                {
                    TempData["Message"] = "Successfully deleted.";
                    sql.CommitTransaction();
                    sql.Dispose();
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to delete.";
                    sql.Dispose();
                }
                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            if (AddSoftwareBtnName == "Save")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsADD_LINK";
                sql.AddParametersWithValue("@mode", "AddSoftwareList");
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                sql.AddParametersWithValue("@IsLaptop", mainForm.ITDetails.IsLaptop);
                sql.AddParametersWithValue("@PCUser", mainForm.ITDetails.PCUser);
                sql.AddParametersWithValue("@Hostname", mainForm.ITDetails.Hostname);
                sql.AddParametersWithValue("@CPU", mainForm.ITDetails.CPU);
                sql.AddParametersWithValue("@Ram_Description", mainForm.ITDetails.Ram_Description);
                sql.AddParametersWithValue("@Ram_Size", mainForm.ITDetails.Ram_Size);
                sql.AddParametersWithValue("@Storage_Description", mainForm.ITDetails.Storage_Description);
                sql.AddParametersWithValue("@Storage_Size", mainForm.ITDetails.Storage_Size);
                sql.AddParametersWithValue("@IfStaticIP", mainForm.ITDetails.IfStaticIP);
                sql.AddParametersWithValue("@IPAddress", mainForm.ITDetails.IPAddress);
                sql.AddParametersWithValue("@LANMAC", mainForm.ITDetails.LANMAC);
                sql.AddParametersWithValue("@WLANMAC", mainForm.ITDetails.WLANMAC);
                sql.AddParametersWithValue("@WithSpiceworks", mainForm.ITDetails.WithSpiceworks);
                if (sql.DoAction())
                {
                    TempData["Message"] = "Successfully saved.";
                    sql.CommitTransaction();
                    sql.Dispose();
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to Save.";
                    sql.Dispose();
                }
                return RedirectToAction("GetITDetails", new { addSoftwareListCode = mainForm.ITDetails.ControlNo });
            }
            else if (ITLogsBtnName == "SaveLogs")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsADD_LINK";
                sql.AddParametersWithValue("@mode", "AddITLogsList");
                sql.AddParametersWithValue("@ControlNo", mainForm.ITDetails.ControlNo);
                sql.AddParametersWithValue("@Date", mainForm.ITLogsDetails.Date);
                sql.AddParametersWithValue("@DoneBy", mainForm.ITLogsDetails.DoneBy);
                sql.AddParametersWithValue("@Description", mainForm.ITLogsDetails.Description);
                sql.AddParametersWithValue("@Remarks", mainForm.ITLogsDetails.Remarks);             
                if (sql.DoAction())
                {
                    TempData["Message"] = "Successfully saved.";
                    sql.CommitTransaction();
                    sql.Dispose();
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to Save.";
                    sql.Dispose();
                }
                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            else if (ITLogsBtnName == "UpdateLogs")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_UpdateITLogsList";
                sql.AddParametersWithValue("@ControlNo", mainForm.ITLogsDetails.ControlNo);
                sql.AddParametersWithValue("@Date", mainForm.ITLogsDetails.Date);
                sql.AddParametersWithValue("@DoneBy", mainForm.ITLogsDetails.DoneBy);
                sql.AddParametersWithValue("@Description", mainForm.ITLogsDetails.Description);
                sql.AddParametersWithValue("@Remarks", mainForm.ITLogsDetails.Remarks);
                bool isSuccess = sql.DoAction();
                if (isSuccess)
                {
                    TempData["Message"] = "Successfully updated.";
                    sql.CommitTransaction();
                    sql.Dispose();
                    return RedirectToAction("LoadITInventory", new { itLogsCode = mainForm.ITLogsDetails.ControlNo });
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to update.";
                    sql.Dispose();
                    return RedirectToAction("LoadITInventory");
                }
            }
            //Linking Buttons
            else if (LinkBtnName == "LinkSoftware")
            {
                bool blnWithError = false;
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_ModifyITDetailsCLEAR_DELETE";
                sql.AddParametersWithValue("@mode", "ClearITSoftwareList");
                sql.AddParametersWithValue("@controlNo", mainForm.ITDetails.ControlNo);
                if (!sql.DoAction())
                {
                    blnWithError = true;
                }
                if (blnWithError == false)
                {
                    foreach (var item in mainForm.SoftwareList)
                    {
                        if (item.checkbox)
                        {
                            sql.StoredProcedureName = "spOPMAS_ModifyITDetailsADD_LINK";
                            sql.AddParametersWithValue("@mode", "LinkSoftwareList");
                            sql.AddParametersWithValue("@pcControlNo", mainForm.ITDetails.ControlNo);
                            sql.AddParametersWithValue("@code", item.Code);
                            if (!sql.DoAction())
                            {
                                blnWithError = true;
                                break;
                            }
                        }
                    }
                }
                if (blnWithError)
                {
                    TempData["Message"] = sql.ErrorMessage;
                    sql.RollBack();
                }
                else
                {
                    sql.CommitTransaction();
                    TempData["Message"] = "Software succesfully updated.";
                }

                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            else if (LinkBtnName == "LinkPeripherals")
            {
                bool blnWithError = false;
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                if (blnWithError == false)
                {
                    foreach (var item in mainForm.PeripheralsList)
                    {
                        if (item.checkbox)
                        {
                            sql.StoredProcedureName = "spOPMAS_ModifyITDetailsADD_LINK";
                            sql.AddParametersWithValue("@mode", "LinkPeripheralsList");
                            sql.AddParametersWithValue("@pcControlNo", mainForm.ITDetails.ControlNo);
                            sql.AddParametersWithValue("@ControlNo", item.ControlNo);
                            if (!sql.DoAction())
                            {
                                blnWithError = true;
                                break;
                            }
                        }
                    }
                }
                if (blnWithError)
                {
                    TempData["Message"] = sql.ErrorMessage;
                    sql.RollBack();
                }
                else
                {
                    sql.CommitTransaction();
                    TempData["Message"] = "Peripherals succesfully updated.";
                }

                return RedirectToAction("GetITDetails", new { itLoadCode = mainForm.ITDetails.ControlNo });
            }
            else if (ITBtnName == "Delete" && !HttpContext.Request.IsAjaxRequest())
            {

                return View("ConfirmDelete", mainForm. ITDetails);
            }
            else
            {
                TempData["Message"] = "Something Went Wrong.";
                return View("ITInventory");
            }
        }
        //For Software Inventory
        [CheckAuthorization]
        public ActionResult LoadSoftwareInventory(string prmCode = "")
        {
            Software_IT_Inventory.MainForm mainForm = new Software_IT_Inventory.MainForm();
            mainForm.SoftwareDetails = new Software_IT_Inventory.masterSoftwareDetails();
            mainForm.SoftwareList = new List<Software_IT_Inventory.masterSoftwareDetails>();
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_SoftwareInventory";
                sql.AddParametersWithValue("@mode", "GetSoftwareDetails");
                sql.AddParametersWithValue("@code", prmCode);
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            //To read from sql database
                            mainForm.SoftwareDetails.Code = (rsData["Code"]).ToString();
                            mainForm.SoftwareDetails.ControlNo = rsData["ControlNo"].ToString();
                            mainForm.SoftwareDetails.Description = rsData["Description"].ToString();
                            mainForm.SoftwareDetails.ProductKey = rsData["ProductKey"].ToString();
                            mainForm.SoftwareDetails.Remarks = rsData["Remarks"].ToString();
                        }
                    }
                }
            }
            sql = new SQLActions();
            using (sql)
            {
                sql.StoredProcedureName = "spOPMAS_SoftwareInventory";
                sql.AddParametersWithValue("@mode", "GetSoftwareList");
                rsData = sql.GetData(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                using (rsData)
                {
                    if (rsData.HasRows)
                    {
                        while (rsData.Read())
                        {
                            var software = new Software_IT_Inventory.masterSoftwareDetails
                            {
                                Code = rsData["Code"].ToString(),
                                ControlNo = rsData["ControlNo"].ToString(),
                                Description = rsData["Description"].ToString(),
                                ProductKey = rsData["ProductKey"].ToString(),
                                Remarks = rsData["Remarks"].ToString()
                            };
                            mainForm.SoftwareList.Add(software);
                        }
                    }
                }
            }
            return View("SoftwareInventory", mainForm);
        }
        [HttpPost]
        [CheckAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyDetails(SoftwareInventory.MainForm mainForm, string BtnName)
        {
            if (BtnName == "Save")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_SoftwareInventory";
                sql.AddParametersWithValue("@mode", "AddSoftwareInventoryList");
        sql.AddParametersWithValue("@controlNo", mainForm.SoftwareDetails.ControlNo);
                sql.AddParametersWithValue("@description", mainForm.SoftwareDetails.Description);
                sql.AddParametersWithValue("@productKey", mainForm.SoftwareDetails.ProductKey);
                sql.AddParametersWithValue("@remarks", mainForm.SoftwareDetails.Remarks);
                if (sql.DoAction())
                {
                    TempData["Message"] = "Successfully saved.";
                    sql.CommitTransaction();
                    sql.Dispose();
                    return RedirectToAction("LoadSoftwareInventory");
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to Save.";
                    sql.Dispose();
                    return RedirectToAction("LoadSoftwareInventory");
                }
            }
            else if (BtnName == "Update")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_SoftwareInventory";
                sql.AddParametersWithValue("@mode", "UpdateSoftwareList");
                sql.AddParametersWithValue("@code", mainForm.SoftwareDetails.Code);
                sql.AddParametersWithValue("@controlNo", mainForm.SoftwareDetails.ControlNo);
                sql.AddParametersWithValue("@description", mainForm.SoftwareDetails.Description);
                sql.AddParametersWithValue("@productKey", mainForm.SoftwareDetails.ProductKey);
                sql.AddParametersWithValue("@remarks", mainForm.SoftwareDetails.Remarks);
                bool isSuccess = sql.DoAction();
                if (isSuccess)
                {
                    TempData["Message"] = "Successfully updated.";
                    sql.CommitTransaction();
                    sql.Dispose();
                    return RedirectToAction("LoadSoftwareInventory", new { prmCode = mainForm.SoftwareDetails.Code });
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to update.";
                    sql.Dispose();
                    return RedirectToAction("LoadSoftwareInventory");
                }
            }
            else if (BtnName == "Delete")
            {
                sql = new SQLActions();
                sql.BeginTransaction(GlobalVariable.SPCServer, GlobalVariable.PRSDatabase, "", "");
                sql.StoredProcedureName = "spOPMAS_SoftwareInventory";
                sql.AddParametersWithValue("@mode", "DelSoftwareList");
                sql.AddParametersWithValue("@code", mainForm.SoftwareDetails.Code);
                bool isSuccess = sql.DoAction();
                if (isSuccess)
                {
                    TempData["Message"] = "Successfully deleted.";
                    sql.CommitTransaction();
                    sql.Dispose();
                    return RedirectToAction("LoadSoftwareInventory");
                }
                else
                {
                    sql.RollBack();
                    TempData["Message"] = "Failed to delete.";
                    sql.Dispose();
                    return RedirectToAction("LoadSoftwareInventory");
                }
            }
            else if (BtnName == "Delete" && !HttpContext.Request.IsAjaxRequest())
            {

                return View("ConfirmDelete", mainForm.SoftwareDetails);
            }
            else
            {
                TempData["Message"] = "Something Went Wrong.";
                return View("SoftwareInventory");
            }
        }    
    }
}