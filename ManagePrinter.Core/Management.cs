using System;
using System.Management;

namespace ManagePrinter.Core
{
    public static class PrintersManagement
    {
        private static ManagementScope oManagementScope = null;

        public static void GetAllPrinters()
        {
            try
            {
                string query = "SELECT * from Win32_Printer";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                using (ManagementObjectCollection coll = searcher.Get())
                {
                    try
                    {
                        foreach (ManagementObject printer in coll)
                        {
                            foreach (PropertyData property in printer.Properties)
                            {
                                Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                            }
                            Console.WriteLine("**********************************************************");
                        }
                    }
                    catch (ManagementException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Adds the Printer
        public static bool AddPrinter(string sPrinterName)
        {
            try
            {
                oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
                oManagementScope.Connect();

                ManagementClass oPrinterClass = new ManagementClass(new ManagementPath("Win32_Printer"), null);
                ManagementBaseObject oInputParameters = oPrinterClass.GetMethodParameters("AddPrinterConnection");

                oInputParameters.SetPropertyValue("Name", sPrinterName);

                oPrinterClass.InvokeMethod("AddPrinterConnection", oInputParameters, null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Deletes the printer
        public static bool DeletePrinter(string sPrinterName)
        {
            oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
            oManagementScope.Connect();

            SelectQuery oSelectQuery = new SelectQuery();
            oSelectQuery.QueryString = @"SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";


            ManagementObjectSearcher oObjectSearcher = new ManagementObjectSearcher(oManagementScope, oSelectQuery);
            ManagementObjectCollection oObjectCollection = oObjectSearcher.Get();

            if (oObjectCollection.Count != 0)
            {
                foreach (ManagementObject oItem in oObjectCollection)
                {
                    oItem.Delete();
                    return true;
                }
            }
            return false;
        }
        //Renames the printer
        public static void RenamePrinter(string sPrinterName, string newName)
        {
            oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
            oManagementScope.Connect();

            SelectQuery oSelectQuery = new SelectQuery();
            oSelectQuery.QueryString = @"SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";


            ManagementObjectSearcher oObjectSearcher = new ManagementObjectSearcher(oManagementScope, oSelectQuery);
            ManagementObjectCollection oObjectCollection = oObjectSearcher.Get();

            if (oObjectCollection.Count != 0)
            {
                foreach (ManagementObject oItem in oObjectCollection)
                {
                    oItem.InvokeMethod("RenamePrinter", new object[] { newName });
                    return;
                }
            }

        }
        //Sets the printer as Default
        public static void SetDefaultPrinter(string sPrinterName)
        {
            oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
            oManagementScope.Connect();

            SelectQuery oSelectQuery = new SelectQuery();
            oSelectQuery.QueryString = @"SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";


            ManagementObjectSearcher oObjectSearcher = new ManagementObjectSearcher(oManagementScope, oSelectQuery);
            ManagementObjectCollection oObjectCollection = oObjectSearcher.Get();

            if (oObjectCollection.Count != 0)
            {
                foreach (ManagementObject oItem in oObjectCollection)
                {
                    oItem.InvokeMethod("SetDefaultPrinter", new object[] { sPrinterName });
                    return;

                }
            }
        }
        //Gets the printer information
        public static void GetPrinterInfo(string sPrinterName)
        {
            oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
            oManagementScope.Connect();

            SelectQuery oSelectQuery = new SelectQuery();
            oSelectQuery.QueryString = @"SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";


            ManagementObjectSearcher oObjectSearcher = new ManagementObjectSearcher(oManagementScope, @oSelectQuery);
            ManagementObjectCollection oObjectCollection = oObjectSearcher.Get();

            foreach (ManagementObject oItem in oObjectCollection)
            {
                Console.WriteLine("Name : " + oItem["Name"].ToString());
                Console.WriteLine("PortName : " + oItem["PortName"].ToString());
                Console.WriteLine("DriverName : " + oItem["DriverName"].ToString());
                Console.WriteLine("DeviceID : " + oItem["DeviceID"].ToString());
                Console.WriteLine("Shared : " + oItem["Shared"].ToString());
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
        //Checks whether a printer is installed
        public static bool IsPrinterInstalled(string sPrinterName)
        {
            oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
            oManagementScope.Connect();

            SelectQuery oSelectQuery = new SelectQuery();
            oSelectQuery.QueryString = @"SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";


            ManagementObjectSearcher oObjectSearcher = new ManagementObjectSearcher(oManagementScope, oSelectQuery);
            ManagementObjectCollection oObjectCollection = oObjectSearcher.Get();

            return oObjectCollection.Count > 0;
        }
    }
}
