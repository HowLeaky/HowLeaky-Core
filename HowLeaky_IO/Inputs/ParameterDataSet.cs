
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowLeaky_IO
{
    public class ParameterDataSet
    {
        public ParameterDataSet()
        {
            Parameters=new List<DataSetParameter>();
        }
        //public ParameterDataSet(NewApplicationDbContext db, ParameterDataSet dataset, Study study, ApplicationUser appUser)
        //{
        //    Id=Guid.NewGuid();
        //    StudyId=study.Id;
        //    CreatedDate=DateTime.UtcNow;
        //    CreatedBy=appUser.Email;
        //    ModifiedDate=DateTime.UtcNow;
        //    ModifiedBy=appUser.Email;
         
        //    DataSetTemplateId=dataset.DataSetTemplateId;
        //    UserId=new Guid(appUser.Id);
            
        //    Comments=dataset.Comments;
        //    Name=$"Copy of {dataset.Name}";
        //    Type=dataset.Type;
        //    Group=dataset.Group;
        //    Anchors=dataset.Anchors;
        //    ParameterIds="";
        //    ReadOnly=dataset.ReadOnly;
        //    SourceFileName=dataset.SourceFileName;
        //    TypeName=dataset.TypeName;
        //    Description=dataset.Description;
        //    var sourcestudy=db.Studies.FirstOrDefault(x=>Id==dataset.StudyId);
        //    var sourcestudyname=sourcestudy!=null?sourcestudy.Name:"Unknown Study";

        //    Source=$"Copied {DateTime.Now:dd/MM/yyy} from {dataset.Name} ({GetOwnerName(db)}) from study \"{sourcestudyname}\"";
        //    var sourceparams=db.InputParameters.Where(x=>x.ParentId==dataset.Id).ToList();
        //    var newlist=new List<DataSetParameter>();
        //    foreach(var myparam in sourceparams)
        //    {
        //        var newparam=new DataSetParameter(myparam,dataset, appUser);
        //        db.InputParameters.Add(newparam);
        //        newlist.Add(newparam);
        //        db.SaveChanges();
        //    }
        //    ParameterIds=String.Join(",",newlist.Select(x=>x.Id.ToString()).ToList());


        //}

        public Guid Id { get; set; }
        public Guid? DataSetTemplateId { get; set; }
        public Guid UserId { get; set; }
        public Guid? StudyId { get; set; }
        public DateTime?CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime?ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Comments { get; set; }
        public string Name{get;set;}
        //public HowLeakyDataType Type { get; set; }
        public Guid?Group { get; set; }
        public string Anchors { get; set; }
        public string ParameterIds { get; set; }
        public bool ReadOnly { get; set; }
        public string SourceFileName { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string Source{get;set;}
        public List<DataSetParameter>Parameters{get;set;}
        public string CodeName { get; set; }
        public HowLeakyDataType DataType { get; set; }

        internal void AddParameter(DataSetParameter param)
        {
            Parameters.Add(param);
            //var id=param.Id.ToString();
            //if(!String.IsNullOrEmpty(ParameterIds))
            //{
            //    if(ParameterIds.Contains(id)==false)
            //    {
            //        var list=ParameterIds.Split(",").ToList();
            //        list.Add(id);
            //        ParameterIds=string.Join(",",list);
            //    }
            //}
            //else
            //{
            //    ParameterIds=id;
            //}
        }

        internal bool ContainsSearchText(string search)
        {
            if(!String.IsNullOrEmpty(Name)&&Name.ToLower().Contains(search))return true;
            return false;
        }

       
    }
}
