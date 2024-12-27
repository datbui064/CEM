using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class KeHoachGiangDay
{
    public string MaKeHoach { get; set; }

    public string? TieuDe { get; set; }

    public string? LoaiKeHoach { get; set; }

    public string? ChiTiet { get; set; }

    public string? MaBaiGiang { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual BaiGiang? MaBaiGiangNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
