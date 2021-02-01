using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19App.Models
{
    public class Result<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int Length { get; set; }

        public Result(T data)
        {
            Code = CODE.SUCCESS;
            Message = MSG.SUCCESS;
            Data = data;
        }
        public Result(string code, string msg, T data)
        {
            Code = code;
            Message = msg;
            Data = data;
        }
        public Result(string code, string msg, T data, int length)
        {
            Code = code;
            Message = msg;
            Data = data;
            Length = length;
        }
    }
    public class CODE
    {
        public const string ERROR = "ERROR";
        public const string SUCCESS = "SUCCESS";
        public const string INVALID = "ERROR";
        public const string EMPTY = "EMPTY";
        public const string INTERNAL_ERROR = "INTERNAL_ERROR";
        public const string NOT_FOUND = "NOT_FOUND";
        public const string UNAUTHORIZE = "UN_AUTHORIZE";
    }

    public class MSG
    {
        public const string NOT_FOUND_STREET = "Không tìm thấy tên đường";
        public const string INTERNAL_ERROR = "Lỗi từ hệ thống";
        public const string INSERT_SUCCESS = "Thêm mới Thành công";
        public const string UPDATE_SUCCESS = "Cập nhật Thành công";
        public const string DELETE_SUCCESS = "Xóa Thành công";
        public const string INSERT_FAIL = "Thêm mới Thất bại";
        public const string UPDATE_FAIL = "Cập nhật Thất bại";
        public const string DELETE_FAIL = "Xóa Thất bại";
        public const string REGISTER_SUCCESS = "Đăng ký thành công";
        public const string CHANGE_PASS_SUCCESS = "Đổi mật khẩu thành công";
        public const string DUPLICATE = "Thông tin nhập đã tồn tại";
        public const string SUCCESS = "Đã thực thi thành công";
        internal static readonly string LOGIN_SUCCESS = "Đăng nhập thành công";
        public static readonly string CAN_NOT_CREATE_USER = "";
        internal static readonly string LOGIN_FAIL = "Đăng nhập thất bại";
        internal static readonly string NOT_FOUND_ADDRESS = "Không tìm thấy địa chỉ";
        internal static readonly string SELECT_SUCCESS = "Thành công!!!";
        public static readonly string NOT_FOUNND = "Không tìm thấy";
        internal static readonly string NO_CHANGE = "Dữ liệu không có gì thay đổi";
        internal static readonly string INVALID_COORDINATE = "Tọa độ không đúng";
    }
}
