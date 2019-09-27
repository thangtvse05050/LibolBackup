using Libol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class SP_HOLDING_LIBRARY_SELECT_Result
    {
        public SP_HOLDING_LIBRARY_SELECT_Result()
        {

        }

        public static List<HOLDING_LIBRARY> ConvertToHoldingLibrary(List<SP_HOLDING_LIBRARY_SELECT_Result> list)
        {
            List<HOLDING_LIBRARY> libs = new List<HOLDING_LIBRARY>();
            foreach (var item in list)
            {
                libs.Add(new HOLDING_LIBRARY() {

                    ID = item.ID,
                    Name = item.Name,
                    AccessEntry = item.AccessEntry,
                    Code = item.Code,
                    Address = item.Address,
                    LocalLib = item.LocalLib

                });
            }
            return libs;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool LocalLib { get; set; }
        public string Address { get; set; }
        public string AccessEntry { get; set; }
        public string FullName { get; set; }

    }
}