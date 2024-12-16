    using System;
    using System.Collections.Generic;

    namespace MercWebExt.Models.DataBase
    {
        public partial class InductionQuestion
        {
            public int Qid { get; set; }
            public int Qnumber { get; set; }
            public string Question { get; set; }
            public string AnswerLabel1 { get; set; }
            public string AnswerLabel2 { get; set; }
            public bool? IsActivated { get; set; }
            public string Category { get; set; }
            public string AnswerLabel3 { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public bool? IsNotUseFile { get; set; }
            public int isCorrectAnswer { get; set; }
    }
    }
