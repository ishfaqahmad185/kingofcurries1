using KingofCurries.Models;

namespace Services
{
    public interface IBankHolidayRepository
    {
        bool DeletebankHoliday(int id, int UserId);
        IEnumerable<BankHoliday> GetAllbankHoliday(); 
        BankHoliday GetAllbankHolidayById(int id);
        bool Insert(BankHoliday bankHoliday);
        bool UpdatebankHoliday(BankHoliday bankHoliday);
    }
}