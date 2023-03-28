namespace HarrysRestro.Services
{


	public interface ICompanyProfileRepository
    {
        bool AddCompanyProfile(CompanyProfile companyProfile);
        bool DeleteCompany(int id, int UserId);
        IEnumerable<CompanyProfile> GetAllCompanyProfile();
        CompanyProfile GetCompanyProfileById(int id);
        bool UpdateCompanyProfile(CompanyProfile companyProfile);
    }
}