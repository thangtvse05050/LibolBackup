using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class FPT_SP_GET_ITEM_Result
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }
        public string tacGia { get; set; }
        public string noiXuatBan { get; set; }
        public string nhaXuatBan { get; set; }
        public int namXuatBan { get; set; }
        public int soluong { get; set; }
        public string DKCB { get; set; }
        public int soluongmuon { get; set; }
        public int soluongton { get; set; }

        public FPT_SP_GET_ITEM_Result()
        {
        }

        public FPT_SP_GET_ITEM_Result(int iD, string content, string code, string tacGia, string noiXuatBan, string nhaXuatBan, int namXuatBan, int soluong, string dKCB, int soluongmuon, int soluongton)
        {
            ID = iD;
            Content = content;
            Code = code;
            this.tacGia = tacGia;
            this.noiXuatBan = noiXuatBan;
            this.nhaXuatBan = nhaXuatBan;
            this.namXuatBan = namXuatBan;
            this.soluong = soluong;
            DKCB = dKCB;
            this.soluongmuon = soluongmuon;
            this.soluongton = soluongton;
        }
    }
}