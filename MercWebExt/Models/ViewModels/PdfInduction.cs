using MercWebExt.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MercWebExt.Models.ViewModels
{
    public partial class PdfInduction
    {
        public List<InductionQuestion> InductionQuestions { get; set; }
        public DbSet<InductionInduction> InductionInductions { get; set; }

        public string title { get; set; }
    }
}
