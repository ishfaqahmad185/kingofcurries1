using _Helper;
using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using HarrysRestro.Services;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class CompanyProfileRepository : ICompanyProfileRepository
    {
        public CompanyProfileRepository()
        {

        }

        public bool AddCompanyProfile(CompanyProfile companyProfile)
        {


            SqlParameter[] param =
   {
                new SqlParameter("@CompanyTitle", companyProfile.CompanyTitle),
                new SqlParameter("@CompanyLogo", companyProfile.CompanyLogo),
                new SqlParameter("@Companyaddress", companyProfile.Companyaddress),
                new SqlParameter("@CompanyPhoneNo", companyProfile.CompanyPhoneNo),
                new SqlParameter("@CompanyEmail", companyProfile.CompanyEmail),
                new SqlParameter("@CompanyDetail", companyProfile.CompanyDetail),
                new SqlParameter("@CompanyShortname", companyProfile.CompanyShortname),
                new SqlParameter("@UserId", companyProfile.UserId),

            };

              SqlHelper.ExecuteProcedureNonQuery("spCompanyProfile_Insert", param);
            return true;
            
        }
        public IEnumerable<CompanyProfile> GetAllCompanyProfile()
        {
            List<CompanyProfile> Balobj = new List<CompanyProfile>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("spCompanyProfile_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                CompanyProfile item = new CompanyProfile();
                item.CompanyID = dataRow["CompanyId"].ToInt32();
                item.CompanyTitle = dataRow["CompanyTitle"].ToString();
                item.CompanyLogo = dataRow["CompanyLogo"].ToString();
                item.Companyaddress = dataRow["Companyaddress"].ToString();
                item.CompanyPhoneNo = dataRow["CompanyPhoneNo"].ToString();
                item.CompanyEmail = dataRow["CompanyEmail"].ToString();
                item.CompanyDetail = dataRow["CompanyDetail"].ToString();
                item.CompanyShortname = dataRow["CompanyShortname"].ToString();


                Balobj.Add(item);
            }
            return Balobj;

            //return datatable;


        }

		public CompanyProfile GetCompanyProfileById(int id)
		{
			CompanyProfile companyprofile = new CompanyProfile();
			SqlParameter[] param =
			 {
				new SqlParameter("@CompanyId",id)

				};


			DataTable datatable = SqlHelper.GetTableFromSP("spCompanyProfile_GetById", param);

			foreach (DataRow dataRow in datatable.Rows)
			{


                companyprofile.CompanyID = dataRow["CompanyID"].ToInt32();
				companyprofile.CompanyTitle = dataRow["CompanyTitle"].ToString();
				companyprofile.CompanyLogo = dataRow["CompanyLogo"].ToString();
				companyprofile.Companyaddress = dataRow["Companyaddress"].ToString();
				companyprofile.CompanyPhoneNo = dataRow["CompanyPhoneNo"].ToString();
				companyprofile.CompanyEmail = dataRow["CompanyEmail"].ToString();
				companyprofile.CompanyDetail = dataRow["CompanyDetail"].ToString();
				companyprofile.CompanyShortname = dataRow["CompanyShortName"].ToString();
            }
			return companyprofile;

		}


        public Boolean UpdateCompanyProfile(CompanyProfile companyProfile)
        {

            SqlParameter[] param = {
                    new SqlParameter("@CompanyId",companyProfile.CompanyID),
                    new SqlParameter("@CompanyTitle", companyProfile.CompanyTitle),
                    new SqlParameter("@CompanyLogo", companyProfile.CompanyLogo),
                    new SqlParameter("@Companyaddress", companyProfile.Companyaddress),
                    new SqlParameter("@CompanyPhoneNo", companyProfile.CompanyPhoneNo),
                    new SqlParameter("@CompanyEmail", companyProfile.CompanyEmail),
                    new SqlParameter("@CompanyDetail", companyProfile.CompanyDetail),
                    new SqlParameter("@CompanyShortname", companyProfile.CompanyShortname),
                    new SqlParameter("@UserId", companyProfile.UserId),

            };

            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spCompanyProfile_Update", param);



            return data;

        }
        public Boolean DeleteCompany(int id, int UserId)
        {

            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@Company_Id",id),
            new SqlParameter("@UserId",UserId),
            };

            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spCompanyProfile_Delete", param);



            return data;

        }



    }


}
