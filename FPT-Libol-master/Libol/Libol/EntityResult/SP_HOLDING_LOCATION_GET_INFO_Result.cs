using Libol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class SP_HOLDING_LOCATION_GET_INFO_Result
    {
        public SP_HOLDING_LOCATION_GET_INFO_Result()
        {

        }

        public static List<HOLDING_LOCATION> ConvertToHoldingLocation(List<SP_HOLDING_LOCATION_GET_INFO_Result> list)
        {
            List<HOLDING_LOCATION> locs = new List<HOLDING_LOCATION>();
            foreach (var item in list)
            {
                locs.Add(new HOLDING_LOCATION()
                {

                   ID = item.ID,
                   CodeLoc = item.CodeLoc,
                   LibID = item.LibID,
                   MaxNumber = item.MaxNumber,
                   Status = item.Status,
                   Symbol = item.Symbol

                });
            }
            return locs;
        }
        public int ID { get; set; }
        public Nullable<int> LibID { get; set; }
        public string Symbol { get; set; }
        public bool Status { get; set; }
        public Nullable<int> MaxNumber { get; set; }
        public string CodeLoc { get; set; }
        public string CodeSymbol { get; set; }


    }
}