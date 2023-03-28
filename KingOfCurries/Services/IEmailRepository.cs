namespace Services
{
    public interface IEmailRepository
    {
        EmailTemplate GetEmailTemplateId(int MessageId);
    }
}