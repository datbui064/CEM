using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class BaiGiang
{
    public string? MaBaiGiang { get; set; }

    public string? TieuDe { get; set; }

    public string? NoiDung { get; set; }

    public string? DuongDanTep { get; set; }

    public int? MaNguoiDung { get; set; }

    public string? NguoiLapBaiGiang { get; set; }

    public string? FileName { get; set; }

    public string? BucketName { get; set; }

    public virtual ICollection<CauHoi> CauHois { get; set; } = new List<CauHoi>();

    public virtual ICollection<DeCuong> DeCuongs { get; set; } = new List<DeCuong>();

    public virtual ICollection<KeHoachGiangDay> KeHoachGiangDays { get; set; } = new List<KeHoachGiangDay>();

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual ICollection<PhuongTienCongCu> PhuongTienCongCus { get; set; } = new List<PhuongTienCongCu>();

    public virtual ICollection<TaiLieuHocTap> TaiLieuHocTaps { get; set; } = new List<TaiLieuHocTap>();
}
