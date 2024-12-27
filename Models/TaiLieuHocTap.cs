using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class TaiLieuHocTap
{
    public string MaTaiLieu { get; set; }

    public string? TenTaiLieu { get; set; }

    public string? TacGia { get; set; }

    public DateTime? NamXuatBan { get; set; }


    public string? NhaXuatBan { get; set; }

    public string? LoaiTaiLieu { get; set; }

    public string? DuongDan { get; set; }

    public string? MaBaiGiang { get; set; }

    public string? FileName { get; set; }

    public string? BucketName { get; set; }

    public virtual BaiGiang? MaBaiGiangNavigation { get; set; }
}
