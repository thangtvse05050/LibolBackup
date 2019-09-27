using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class Temper
    {
        public int? UseCount { get; set; }
        public string ReId { get; set; }
        public int? POID { get; set; }
        public string SoChungTu { get; set; }
        public string NhanDe { get; set; }
        public string ISBN { get; set; }
        public string NgayChungTu { get; set; }
        public string DKCB { get; set; }
        public string NgayBoSung { get; set; }
        public string NhaXuatBan { get; set; }
        public string NamXuatBan { get; set; }
        public float DonGia { get; set; }
        public string DonViTienTe { get; set; }
        public string TinhTrangSach { get; set; }
        public int ItemID { get; set; }
        public int SLN { get; set; }
        public double ThanhTien { get; set; }


        public Temper()
        {
        }

        public Temper(string dKCB, int itemID)
        {
            DKCB = dKCB;
            ItemID = itemID;
        }

        public Temper(string soChungTu, string nhanDe, string iSBN, string ngayChungTu, string dKCB, string ngayBoSung, string nhaXuatBan, string namXuatBan, float donGia, string donViTienTe, string tinhTrangSach, int itemID, int sLN, double thanhTien)
        {
            SoChungTu = soChungTu;
            NhanDe = nhanDe;
            ISBN = iSBN;
            NgayChungTu = ngayChungTu;
            DKCB = dKCB;
            NgayBoSung = ngayBoSung;
            NhaXuatBan = nhaXuatBan;
            NamXuatBan = namXuatBan;
            DonGia = donGia;
            DonViTienTe = donViTienTe;
            TinhTrangSach = tinhTrangSach;
            ItemID = itemID;
            SLN = sLN;
            ThanhTien = thanhTien;
        }

        public Temper(int? pOID, string soChungTu, string nhanDe, string iSBN, string ngayChungTu, string dKCB, string ngayBoSung, string nhaXuatBan, string namXuatBan, float donGia, string donViTienTe, string tinhTrangSach, int itemID, int sLN, double thanhTien)
        {
            POID = pOID;
            SoChungTu = soChungTu;
            NhanDe = nhanDe;
            ISBN = iSBN;
            NgayChungTu = ngayChungTu;
            DKCB = dKCB;
            NgayBoSung = ngayBoSung;
            NhaXuatBan = nhaXuatBan;
            NamXuatBan = namXuatBan;
            DonGia = donGia;
            DonViTienTe = donViTienTe;
            TinhTrangSach = tinhTrangSach;
            ItemID = itemID;
            SLN = sLN;
            ThanhTien = thanhTien;
        }

        public Temper(int? useCount, int? pOID, string soChungTu, string nhanDe, string iSBN, string ngayChungTu, string dKCB, string ngayBoSung, string nhaXuatBan, string namXuatBan, float donGia, string donViTienTe, string tinhTrangSach, int itemID, int sLN, double thanhTien)
        {
            UseCount = useCount;
            POID = pOID;
            SoChungTu = soChungTu;
            NhanDe = nhanDe;
            ISBN = iSBN;
            NgayChungTu = ngayChungTu;
            DKCB = dKCB;
            NgayBoSung = ngayBoSung;
            NhaXuatBan = nhaXuatBan;
            NamXuatBan = namXuatBan;
            DonGia = donGia;
            DonViTienTe = donViTienTe;
            TinhTrangSach = tinhTrangSach;
            ItemID = itemID;
            SLN = sLN;
            ThanhTien = thanhTien;
        }

        public Temper(int? useCount, string reId, string soChungTu, string nhanDe, string iSBN, string ngayChungTu, string dKCB, string ngayBoSung, string nhaXuatBan, string namXuatBan, float donGia, string donViTienTe, int itemID, int sLN, double thanhTien)
        {
            UseCount = useCount;
            ReId = reId;
            SoChungTu = soChungTu;
            NhanDe = nhanDe;
            ISBN = iSBN;
            NgayChungTu = ngayChungTu;
            DKCB = dKCB;
            NgayBoSung = ngayBoSung;
            NhaXuatBan = nhaXuatBan;
            NamXuatBan = namXuatBan;
            DonGia = donGia;
            DonViTienTe = donViTienTe;
            ItemID = itemID;
            SLN = sLN;
            ThanhTien = thanhTien;
        }
    }
}
