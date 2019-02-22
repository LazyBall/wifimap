using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WCFService
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // Дополнительные сведения по управлению изменениями конфигурации
            // см. статью MSDN по адресу https://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
