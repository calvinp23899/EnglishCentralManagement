namespace EnglishCentralManagement.Services
{
    public class EmailTemplateService
    {
        private readonly string _templatePath;

        public EmailTemplateService(IWebHostEnvironment env)
        {
            _templatePath = Path.Combine(env.ContentRootPath, "Templates", "Email");
        }

        public async Task<string> RenderAsync(string templateName, Dictionary<string, string> variables)
        {
            var filePath = Path.Combine(_templatePath, $"{templateName}.html");
            var content = await File.ReadAllTextAsync(filePath);

            foreach (var (key, value) in variables)
                content = content.Replace($"{{{{{key}}}}}", value);

            return content;
        }
    }
}
