using API.Utility;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Repository
{
    public class EmailRepository : IEmailRepository
    {
        private DataTable datatable = new DataTable();


        public EmailRepository()
        {

        }

        public EmailTemplate GetEmailTemplateId(int MessageId)
        {
            EmailTemplate emailTemplate = new EmailTemplate();

            SqlParameter[] param =
                    {

               new SqlParameter("@MessageId", MessageId),
                };


            datatable = SqlHelper.GetTableFromSP("spEmailTemplate_GetByMessageId", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                emailTemplate.MessageId = Convert.ToInt32(dataRow["MessageId"]);
                emailTemplate.MessageFor = dataRow["MessageFor"].ToString();
                emailTemplate.Subject = dataRow["Subject"].ToString();
                emailTemplate.FromEmail = dataRow["From_Email"].ToString();
                emailTemplate.FromName = dataRow["From_Name"].ToString();

            }
            return emailTemplate;
        }


    }
}
