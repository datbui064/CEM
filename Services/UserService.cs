using CEM.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserService
{
    private readonly QlbContext _context;

    public UserService(QlbContext context)
    {
        _context = context;
    }

    public async Task<NguoiDung?> LoginAsync(string tenDangNhap, string matKhau)
    {
        // Kiểm tra giá trị đầu vào
        if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
        {
            throw new ArgumentException("Tên đăng nhập và mật khẩu không được để trống.");
        }

        // Băm mật khẩu
        var passwordHash = HashPassword(matKhau);

        Console.WriteLine($"Trying to login with username: {tenDangNhap} and password hash: {passwordHash}");

        try
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap != null
                                          && u.TenDangNhap == tenDangNhap
                                          && u.MatKhau == passwordHash);

            if (user == null)
            {
                Console.WriteLine("Login failed: invalid username or password.");
                throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            Console.WriteLine($"Login successful for username: {user.TenDangNhap}");
            return user;
        }
        catch (Exception ex)
        {
            // Log lỗi chi tiết
            Console.WriteLine($"Error during login: {ex.Message}");
            throw new Exception("Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại.");
        }
    }
    public async Task<bool> RegisterAsync(NguoiDung newUser)
    {
        // Kiểm tra nếu tên đăng nhập đã tồn tại
        if (await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == newUser.TenDangNhap))
        {
            return false;
        }

        // Băm mật khẩu
        newUser.MatKhau = HashPassword(newUser.MatKhau);

        // Thêm người dùng vào cơ sở dữ liệu
        
            _context.NguoiDungs.Add(newUser);
            await _context.SaveChangesAsync();
        
        

        return true;
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
