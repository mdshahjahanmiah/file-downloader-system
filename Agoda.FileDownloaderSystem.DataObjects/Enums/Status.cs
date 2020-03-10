using Agoda.FileDownloaderSystem.Utilities.EnumAbstration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Enums
{
    public sealed class Status : StringEnum
    {
        public Status(string value) : base(value)
        {
        }
        public const string CompletedConst = "Completed";
        public const string FailedConst = "Failed";
        public static readonly Status Completed = new Status(CompletedConst);
        public static readonly Status Failed = new Status(FailedConst);

        public static int GetKey(string value) 
        {
            switch (value)
            {
                case Status.CompletedConst:
                    return 1;
                case Status.FailedConst:
                    return 2;
                    
            }
            return 0;
        }
    }
}
