using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class PhuongTienCongCu
{
    public string MaCongCu { get; set; }

    public string? TenCongCu { get; set; }

    public string? MoTa { get; set; }

    public string? DuongDanTep { get; set; }

    public int MaNguoiDung { get; set; }

    public string? LoaiCongCu { get; set; }

    public string? MaBaiGiang { get; set; }

    public string? FileName { get; set; }

    public string? BucketName { get; set; }

    public virtual BaiGiang? MaBaiGiangNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
