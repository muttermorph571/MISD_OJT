using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Intranet.Models
{
    public class SystemList
    {
        public List<SystemDetails> Systems { get; set; }

        public bool Access_Edit { get; set; }

        public SystemList()
        {
            Systems = new List<SystemDetails>();
        }
    }
    public class SystemDetails
    {
        public int Code { get; set; }
        public string MainSystem { get; set; }
        public string Initials { get; set; }
        public string Description { get; set; }
        public string DeptAccess { get; set; }
        public string DateImplemented { get; set; }
        public DateTime dtDateImplemented { get; set; }
        public string LinkedSystem { get; set; }
    }
    public class ModuleList
    {
        public string MainSystemCode { get; set; }
        public List<SelectListItem> MainSystemSLI { get; set; }
        public List<ModuleDetails> Modules { get; set; }

        public bool Access_Edit { get; set; }

        public ModuleList()
        {
            MainSystemSLI = new List<SelectListItem>();
            Modules = new List<ModuleDetails>();
        }
    }
    public class ModuleDetails
    {
        public int Code { get; set; }
        public string MainSystem { get; set; }
        public string Menu { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public string UserDept { get; set; }
        public string DateImplemented { get; set; }
        public DateTime dtDateImplemented { get; set; }
    }
    public class SoftwareDetails
    {
        public string Code { get; set; }
        public string ControlNo { get; set; }
        public string Description { get; set; }
        public string ProductKey { get; set; }
        public string Remarks { get; set; }
    }
    public class SoftwareInventory
    {
        public class MainForm
        {
            public Details SoftwareDetails { get; set; }
            public List<Details> SoftwareList { get; set; }
            public MainForm()
            {
                SoftwareDetails = new Details();
                SoftwareList = new List<Details>();
            }
        }
        public class Details
        {
            public string Code { get; set; }
            public string ControlNo { get; set; }
            public string Description { get; set; }
            public string ProductKey { get; set; }
            public string Remarks { get; set; }
        }
    }
    public class ITInventory
    {
        public class MainForm
        {
            public Details ITDetails { get; set; }
            public List<Details> ITList { get; set; }
            public MainForm()
            {
                ITDetails = new Details();
                ITList = new List<Details>();
            }
        }
        public class Details
        {
            public string ControlNo { get; set; }
            public bool IsLaptop { get; set; }
            public string CPU { get; set; }
            public string Ram_Description { get; set; }
            public int Ram_Size { get; set; }
            public bool IfStaticIP { get; set; }
            public string IPAddress { get; set; }
            public string LANMAC { get; set; }
            public string WLANMAC { get; set; }
            public bool WithSpiceworks { get; set; }
        }
    }
    public class ITDetails
    {
        public string PCUser { get; set; }
        public string Hostname { get; set; }
        public string ControlNo { get; set; }
        public bool IsLaptop { get; set; }
        public string CPU { get; set; }
        public string Ram_Description { get; set; }
        public int Ram_Size { get; set; }
        public bool IfStaticIP { get; set; }
        public string IPAddress { get; set; }
        public string LANMAC { get; set; }
        public string WLANMAC { get; set; }
        public bool WithSpiceworks { get; set; }
    }
    public class Software_IT_Inventory
    {
        public class MainForm
        {
            public masterITDetails ITDetails { get; set; }
            public masterSoftwareDetails SoftwareDetails { get; set; }
            public masterITLogsDetails ITLogsDetails { get; set; }
            public masterControlNoDetails ControlNoDetails { get; set; }
            public masterPeripheralsDetails PeripheralsDetails { get; set; }
            public List<masterSoftwareDetails> SoftwareList { get; set; }
            public List<masterITDetails> ITList { get; set; }
            public List<masterITLogsDetails> ITLogsList { get; set; }
            public List<masterControlNoDetails> ControlNoList { get; set; }
            public List<masterPeripheralsDetails> PeripheralsList { get; set; }
            public List<SelectListItem>SubCategories{get;set;}
            public MainForm()
            {
                ITDetails = new masterITDetails();
                ITList = new List<masterITDetails>();
                SoftwareDetails = new masterSoftwareDetails();
                SoftwareList = new List<masterSoftwareDetails>();
                ITLogsDetails = new masterITLogsDetails();
                ITLogsList = new List<masterITLogsDetails>();
                ControlNoDetails = new masterControlNoDetails();
                ControlNoList = new List<masterControlNoDetails>();
                PeripheralsDetails = new masterPeripheralsDetails();
                PeripheralsList = new List<masterPeripheralsDetails>();
                SubCategories = new List<SelectListItem>();
            }
        }
        public class masterITDetails
        {
            public string PCUser { get; set; }
            public string Hostname { get; set; }
            public string ControlNo { get; set; }
            public bool IsLaptop { get; set; }
            public string Type { get; set; }
            public string CPU { get; set; }
            public string Ram_Description { get; set; }
            public int Ram_Size { get; set; }
            public string Storage_Description { get; set; }
            public int Storage_Size { get; set; }
            public bool IfStaticIP { get; set; }
            public string IPAddress { get; set; }
            public string LANMAC { get; set; }
            public string WLANMAC { get; set; }
            public bool WithSpiceworks { get; set; }
            public string SubCategoryID { get; set; }
            public decimal AcquisitionCost{ get; set; }
        }
        public class masterSoftwareDetails
        {
            public string Code { get; set; }
            public string ControlNo { get; set; }
            public string Description { get; set; }
            public string ProductKey { get; set; }
            public string Remarks { get; set; }
            public bool checkbox { get; set; }
            public masterSoftwareDetails()
            {
                checkbox = false;
            }
        }
        public class masterITLogsDetails
        {
            public int Ndx { get; set; }
            public string ControlNo { get; set; }
            public string Date { get; set; }
            public string DoneBy { get; set; }
            public string Description { get; set; }
            public string Remarks { get; set; }
            public bool logsCheckbox { get; set; }
            public masterITLogsDetails()
            {
                logsCheckbox = false;
            }
        }
        public class masterControlNoDetails
        {
            public string ControlNo { get; set; }
            public string Description { get; set; }
            public bool IsLaptop { get; set; }
            public string Remarks { get; set; }
            public string Location { get; set; }
            public decimal AcquisitionCost { get; set; }
        }
        public class masterPeripheralsDetails
        {
            public string PCControlNo { get; set; }
            public string ControlNo { get; set; }
            public string Description { get; set; }
            public string Remarks { get; set; }
            public string Location { get; set; }
            public bool checkbox { get; set; }
        }           
    }
}