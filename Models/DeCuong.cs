using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class DeCuong
{
    public string? MaDeCuong { get; set; }

    public string? TieuDe { get; set; }

    public string? MoTa { get; set; }

    public string? DuongDanTep { get; set; }

    public string? MaBaiGiang { get; set; }

    public int MaNguoiDung { get; set; }

    public string? FileName { get; set; }

    public string? BucketName { get; set; }

    private BaiGiang BaiGiang { get; set; }

    public virtual BaiGiang? MaBaiGiangNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
