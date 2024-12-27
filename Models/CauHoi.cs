using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class CauHoi
{
    public string? MaCauHoi { get; set; }

    public string? NoiDung { get; set; }

    public string? DapAn { get; set; }

    public string? MaBaiGiang { get; set; }

    public int MaNguoiDung { get; set; }

    public string? LoaiCauHoi { get; set; }

    public virtual BaiGiang? MaBaiGiangNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
