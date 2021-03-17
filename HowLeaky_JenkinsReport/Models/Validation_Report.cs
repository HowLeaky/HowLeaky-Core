using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_ValidationEngine.Models.Validation
{
    public class Validation_Report
    {
        public Validation_Report()
        {

        }



        public Guid Id { get; set; }


        public DateTime? Date { get; set; }

        public string BaseName { get; set; }
        public string BranchName { get; set; }
        public int ProjectsCount { get; set; }

        public string GetReportName()
        {
            return $"{BranchName} vs {BaseName}";
        }

        public bool ContainsSearchText(string search)
        {
            var _search=search.ToLower();
            if(!String.IsNullOrEmpty(BaseName)&&BaseName.ToLower().Contains(_search))return true;
            if (!String.IsNullOrEmpty(BranchName) && BranchName.ToLower().Contains(_search)) return true;
            return false;
        }
    }
}
