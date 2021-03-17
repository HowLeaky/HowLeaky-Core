

using Figgle;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Outputs;
using HowLeaky_SimulationEngine.Tools;
using HowLeaky_IO.Outputs;
using HowLeaky_IO.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;


namespace HowLeaky_IO
{

    public enum HLNodeType
    {
        Root,
        Header,
        Vector,
        Value,
        Switch,
        TimeSeries,
        Enum,
        DayMonth,
        Sequence
    }

    public enum HowLeakyDataType
    {
        Unknown = 0,
        Measured = 1,

        Climate = 2,
        Soil = 3,
        Vegetation = 4,
        Tillage = 5,
        Irrigation = 6,
        Pesticides = 7,
        Phosphorus = 8,
        Nitrates = 9,
        Solutes = 10,
        Simulations = 11,
        Batch = 12,
        Report = 13,
        Package = 14,
        Attachment = 15
    }

    public class ProjectHLK
    {
        public ProjectHLK()
        {

            RemapDictionary = InputParameterDictionary.Create();


            //ClimateData=new List<P51DataFile>();
            //SoilData=new List<ParameterDataSet>();
            //TillageData=new List<ParameterDataSet>();
            //VegData=new List<ParameterDataSet>();
            //PesticideData=new List<ParameterDataSet>();
            //Irrigation=new List<ParameterDataSet>();
            //PhosphorusData=new List<ParameterDataSet>();
            //SoluteData=new List<ParameterDataSet>();

        }

        internal void PrepareReferenceCounts()
        {
            foreach (var climatefile in DataFiles)
            {
                climatefile.ReferenceCount = Simulations.Count(x => x.ClimateData == climatefile);
            }
        }

        public void AddConsoleOutput(string text, bool logtime = true)
        {
            ConsoleOutput.AddConsoleOutput(text, logtime);
        }



        public void AddErrorOutput(string text)
        {
            ConsoleOutput.AddErrorOutput(text);
        }


        internal void CheckZeroArrays(string Name, HowLeakyOutputs outputs)
        {
            foreach (var output in outputs.TimeSeries)
            {
                if (Math.Abs((double)output.DailyValues.Where(x => x != null).Sum()) < 0.000001)
                {
                    AddErrorOutput($"WARNING!! Timeseries is empty ({Name} - {output.OutputDefn.Name})");
                }
            }
        }



        public ConsoleOutputLogger ConsoleOutput { get; set; }
        public Dictionary<string, string> RemapDictionary { get; set; }
        public List<Simulation> Simulations { get; set; }
        public Dictionary<string, List<Simulation>> GroupedSimulations { get; set; }

        public List<ParameterDataSet> Datasets { get; set; }
        public List<P51DataFile> DataFiles { get; set; }

        public string FileName { get; set; }

        public string OutputsDirectory { get; set; }
        public string OutputsCSV { get; set; }

        public Dictionary<string, List<Simulation>> Open(ConsoleOutputLogger consoleoutput, string filename)
        {
            try
            {
                ConsoleOutput = consoleoutput;
                FileName = filename;
                var path = Path.GetDirectoryName(filename);
                OutputsDirectory = Path.Combine(path, "Outputs");
                if (RemapDictionary != null)
                {

                    AddConsoleOutput($"Opening {filename}...");
                    if (File.Exists(filename))
                    {
                        var type = Path.GetExtension(filename.ToLower());
                        if (type == ".hlk")
                        {
                            var datasets = new Dictionary<string, ParameterDataSet>();
                            var datafiles = new Dictionary<string, P51DataFile>();
                            var alldatasets = new List<ParameterDataSet>();
                            XmlDocument Doc = new XmlDocument();
                            Doc.Load(filename);
                            var parameterdatasets = ExtractParameterDataSetFileNames(Doc);

                            foreach (var item in parameterdatasets)
                            {
                                if (IsValidateType(item))
                                {
                                    var itemfilename = Path.GetFileName(item);
                                    string updateditem=item;
                                    if (!File.Exists(item))
                                    {
                                        var oldpathname1 = Path.GetDirectoryName(item);
                                        var oldpathname = Path.GetDirectoryName(oldpathname1);
                                        updateditem = item.Replace(oldpathname, path).ToLower();
                                    }
                                   
                                    if (updateditem.Contains("p51"))
                                    {
                                        var datafile = new P51DataFile(updateditem);
                                        
                                        datafiles.Add(itemfilename, datafile);

                                    }
                                    else
                                    {
                                        var dataset = ExtractParameterDataSet(updateditem);
                                        if (dataset != null)
                                        {

                                            datasets.Add(itemfilename, dataset);
                                            alldatasets.Add(dataset);
                                        }
                                        else
                                        {
                                            AddErrorOutput($"CORRUPTED PARAMETERSET -Couldn't read {item}");
                                        }
                                    }
                                }
                            }
                            DataFiles = datafiles.Select(x => x.Value).ToList();
                            Datasets = alldatasets;
                            Simulations = ExtractSimulations(Doc, datasets, datafiles);
                            GroupedSimulations = GroupSimulations();
                            GenerateInitialConsoleOutput(filename, alldatasets, datafiles);
                            return GroupedSimulations;
                        }
                    }
                    else
                    {
                        AddErrorOutput($"File note found ({filename})");
                    }
                }
                else
                {
                    AddErrorOutput($"Remap dictionary could not be found");
                    throw new Exception("Fatal Error - Remap dictionary not found");
                }
            }
            catch (Exception ex)
            {
                AddErrorOutput($"Exception Caught: {ex}");
            }
            return null;
        }

        private bool IsValidateType(string item)
        {
            var filename=item.ToLower();
            return !String.IsNullOrEmpty(filename) && !filename.Contains(".tee") && !filename.Contains(".sim") && !filename.Contains(".jpg") && !filename.Contains(".sen");
        }

        private void GenerateInitialConsoleOutput(string filename, List<ParameterDataSet> datasets, Dictionary<string, P51DataFile> datafiles)
        {

            var _filename = Path.GetFileName(filename);
            AddConsoleOutput($"Project {_filename} successfully loaded ({filename})");
            AddConsoleOutput($"Climate Data: {GetDataFileSummary(datafiles)}");
            AddConsoleOutput($"Soil Data: {GetDataSetSummary(datasets, HowLeakyDataType.Soil)}");
            AddConsoleOutput($"Tillage Data: {GetDataSetSummary(datasets, HowLeakyDataType.Tillage)}");
            AddConsoleOutput($"Veg Data: {GetDataSetSummary(datasets, HowLeakyDataType.Vegetation)}");
            AddConsoleOutput($"Pesticide Data: {GetDataSetSummary(datasets, HowLeakyDataType.Pesticides)}");
            AddConsoleOutput($"Irrigation Data: {GetDataSetSummary(datasets, HowLeakyDataType.Irrigation)}");
            AddConsoleOutput($"Nitrate Data: {GetDataSetSummary(datasets, HowLeakyDataType.Nitrates)}");
            AddConsoleOutput($"Phosphorus Data: {GetDataSetSummary(datasets, HowLeakyDataType.Phosphorus)}");
            AddConsoleOutput($"Solute Data: {GetDataSetSummary(datasets, HowLeakyDataType.Solutes)}");

            AddConsoleOutput($"Simulations: {Simulations?.Count}");

            AddConsoleOutput($"");
            AddConsoleOutput($"Completed with {ConsoleOutput.ErrorOutputList.Count} errors");


            if (ConsoleOutput.ErrorOutputList.Count == 0)
            {
                UpdateSimulationStatus();
            }
            else
            {
                AddConsoleOutput(FiggleFonts.Standard.Render($"You have {ConsoleOutput.ErrorOutputList.Count} Errors!"), false);
            }

        }

        public string UpdateSimulationStatus()
        {
            var direxists = Directory.Exists(OutputsDirectory);

            var existingcount = direxists ? Directory.GetFiles(OutputsDirectory, "*.hlkbinout", SearchOption.AllDirectories).Count() : 0;
            var remaining = Simulations?.Count - existingcount;
            var text = "";
            if (remaining == 0)
            {
                text = $"All sims completed!";
            }
            else if (existingcount == 0)
            {
                text = $"Ready to start...";
            }
            else
            {
                text = $"{remaining} sims remaining...";
            }
            AddConsoleOutput(FiggleFonts.Ogre.Render(text), false);
            return text;
        }


        public string GetDataFileSummary(Dictionary<string, P51DataFile> datafiles)
        {
            var list = datafiles.ToList();
            if (list.Count > 0)
            {
                var text = $"{list.Count} files including:";
                var index = 0;
                var maxlength = list.Select(x => x.Value.Name.Length).Max() + 4;
                foreach (var item in list)
                {
                    var text1 = item.Value.Name.ToString().PadRight(maxlength, ' ');
                    var text2 = $"     {++index}.";
                    var text3 = text2.PadRight(10);
                    text = $"{text}\n{text3}{text1}{item.Key}";
                }
                return text;
            }
            return "0 files";

        }

        public string GetDataSetSummary(List<ParameterDataSet> datasets, HowLeakyDataType datatype)
        {

            var list = datasets.Where(x => x.DataType == datatype).ToList();
            if (list.Count > 0)
            {
                var text = $"{list.Count} files including:";
                var index = 0;
                var maxlength = list.Select(x => x.Description.Length).Max() + 4;
                foreach (var item in list)
                {
                    var text1 = item.Description.PadRight(maxlength, ' ');
                    var text2 = $"     {++index}.";
                    var text3 = text2.PadRight(10);
                    text = $"{text}\n{text3}{text1}{item.SourceFileName}";
                }
                return text;
            }
            return "0 files";
        }



        private List<Simulation> ExtractSimulations(XmlDocument doc, Dictionary<string, ParameterDataSet> datasets, Dictionary<string, P51DataFile> datafiles)
        {
            string filename = Path.GetFileNameWithoutExtension(FileName);
            var simulations = new List<Simulation>();
            XmlNodeList xnList = doc.GetElementsByTagName("SimulationObject");
            var index = 0;
            foreach (XmlNode xn in xnList)
            {
                var commentsatt = xn.Attributes["Comments"];
                var comments = commentsatt != null ? commentsatt.Value : "";
                var items = comments.Split(',').ToList();
                var catchmentid = items.Count > 0 && !String.IsNullOrEmpty(items[0]) ? items[0] : null;
                var functionalunit = items.Count > 1 && !String.IsNullOrEmpty(items[1]) ? items[1] : null;

                var simulation = new Simulation(this, filename);
                simulation.Index = ++index;
                simulation.StartYear = ExtractYear(xn, "StartYear");
                simulation.EndYear = ExtractYear(xn, "EndYear");
                simulation.ClimateData = ExtractClimateFile(xn, "ptrStation", datafiles);
                simulation.SoilData = ExtractParameterSet(xn, "ptrSoilType", datasets);
                simulation.TillageData = ExtractParameterSets(xn, "ptrTillage", datasets);
                simulation.VegData = ExtractParameterSets(xn, "ptrVegeOption", datasets);
                simulation.PesticideData = ExtractParameterSets(xn, "ptrPestOption", datasets);
                simulation.Irrigation = ExtractParameterSet(xn, "ptrIrrigation", datasets);
                simulation.PhosphorusData = ExtractParameterSet(xn, "ptrPhosphorus", datasets);
                simulation.SoluteData = ExtractParameterSet(xn, "ptrSolutes", datasets);
                simulation.NitrateData = ExtractParameterSet(xn, "ptrNitrate", datasets);
                simulation.SubCatchmentId = catchmentid;
                simulation.FunctionalUnit = functionalunit;
                simulations.Add(simulation);
            }
            return simulations;
        }

        public Dictionary<string, List<Simulation>> GroupSimulations()
        {
            var dict = new Dictionary<string, List<Simulation>>();
            foreach (var simulation in Simulations)
            {

                var subcatchmentname = !String.IsNullOrEmpty(simulation.SubCatchmentId) ? simulation.SubCatchmentId : "NOSUB";
                var functionunit = !String.IsNullOrEmpty(simulation.FunctionalUnit) ? simulation.FunctionalUnit : "NOFU";
                var uniqueName = $"{subcatchmentname}-{functionunit}-{simulation.ClimateData.Name}";
                if (!dict.ContainsKey(uniqueName))
                {
                    dict.Add(uniqueName, new List<Simulation>());
                }

                dict[uniqueName].Add(simulation);
            }

            return dict;
        }



        private List<ParameterDataSetPtr> ExtractParameterSets(XmlNode xn, string elementname, Dictionary<string, ParameterDataSet> datasets)
        {
            var list = new List<ParameterDataSetPtr>();

            var nodes = xn.SelectNodes("*");
            foreach (XmlNode node in nodes)
            {
                if (node.Name.Contains(elementname))
                {
                    var href = node.Attributes["href"].Value;
                    var filename=Path.GetFileName(href);
                    var dataset = datasets[filename];
                    if (dataset != null)
                    {
                        var datasetPtr = new ParameterDataSetPtr(dataset);
                        LoadOverrides(node, datasetPtr);
                        list.Add(datasetPtr);
                    }
                }
            }
            return list;
        }



        private ParameterDataSetPtr ExtractParameterSet(XmlNode xn, string elementname, Dictionary<string, ParameterDataSet> datasets)
        {
            var node = xn.SelectSingleNode(elementname);
            if (node != null)
            {
                var href = node.Attributes["href"].Value;
                var filename=Path.GetFileName(href);
                var dataset = datasets[filename];
                if (dataset != null)
                {
                    var datasetPtr = new ParameterDataSetPtr(dataset);
                    LoadOverrides(node, datasetPtr);
                    return datasetPtr;
                }
            }
            return null;
        }

        private void LoadOverrides(XmlNode xn, ParameterDataSetPtr datasetPtr)
        {
            var nodes = xn.SelectNodes("OverrideParameter");
            foreach (XmlNode node in nodes)
            {
                var overrideParam = new ParameterOverride();
                overrideParam.XMLNode = node.Attributes["Keyword"].Value;
                overrideParam.Active = node.Attributes["Active"].Value == "true";
                if (RemapDictionary.ContainsKey(overrideParam.XMLNode))
                {
                    overrideParam.CodeName = RemapDictionary[overrideParam.XMLNode];
                }
                else
                {
                    throw new Exception($"Key {overrideParam.XMLNode} doesn't exist in Remap Dictioary");
                }
                var valuenode = node.SelectSingleNode("Value");
                if (valuenode != null)
                {
                    overrideParam.Value = valuenode.InnerText;
                }
                datasetPtr.Overrides.Add(overrideParam);
            }
        }

        private P51DataFile ExtractClimateFile(XmlNode xn, string elementname, Dictionary<string, P51DataFile> datafiles)
        {
            var node = xn.SelectSingleNode(elementname);
            if (node != null)
            {
                var href = node.Attributes["href"].Value;
                var filename=Path.GetFileName(href);
                var datafile = datafiles[filename];
                if (datafile != null)
                {
                    return datafile;
                }
            }
            return null;
        }

        public int? ExtractYear(XmlNode xn, string subnodename)
        {
            var node = xn.SelectSingleNode(subnodename);
            if (node != null)
            {
                var text = node.InnerText;
                int value;
                if (int.TryParse(text, out value))
                {
                    return value;
                }


            }
            return null;
        }



        public HashSet<string> ExtractParameterDataSetFileNames(XmlDocument Doc)
        {
            var filenames = new HashSet<string>();
            FetchHref(filenames, Doc.CreateNavigator());
            return filenames;
        }

        private void FetchHref(HashSet<String> paths, XPathNavigator nav)
        {

            XPathNodeIterator nodes = nav.Select("*");
            while (nodes.MoveNext())
            {
                var current = nodes.Current;

                var hasAttributes = current.HasAttributes;
                if (hasAttributes)
                {
                    var href = GetAttribute(current, "href");
                    if (!String.IsNullOrEmpty(href))
                    {
                        paths.Add(href);
                    }
                }
                FetchHref(paths, current);
            }

        }


        private ParameterDataSet ExtractParameterDataSet(string filename)
        {
            try
            {

                var name = Path.GetFileNameWithoutExtension(filename);
                XmlDocument Doc = new XmlDocument();
                Doc.Load(filename);
                var dataset = new ParameterDataSet();
                dataset.SourceFileName = filename;
                dataset.DataType = ExtractDataType(filename);
                ReadNodes(Doc.CreateNavigator(), dataset);
                return dataset;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            //return null;
        }

        private HowLeakyDataType ExtractDataType(string filename)
        {
            filename = filename.ToLower();
            if (filename.Contains(".soil")) { return HowLeakyDataType.Soil; }
            else if (filename.Contains(".vege")) { return HowLeakyDataType.Vegetation; }
            else if (filename.Contains(".till")) { return HowLeakyDataType.Tillage; }
            else if (filename.Contains(".phos")) { return HowLeakyDataType.Phosphorus; }
            else if (filename.Contains(".pest")) { return HowLeakyDataType.Pesticides; }
            else if (filename.Contains(".nitr")) { return HowLeakyDataType.Nitrates; }
            else if (filename.Contains(".solt")) { return HowLeakyDataType.Solutes; }
            else if (filename.Contains(".p51")) { return HowLeakyDataType.Climate; }
            else if (filename.Contains(".txt")) { return HowLeakyDataType.Measured; }
            else if (filename.Contains(".sim")) { return HowLeakyDataType.Measured; }
            else if (filename.Contains(".csv")) { return HowLeakyDataType.Measured; }
            else if (filename.Contains(".howleakypkg")) { return HowLeakyDataType.Package; }
            return HowLeakyDataType.Unknown;
        }




        private void ReadNodes(XPathNavigator nav, ParameterDataSet dataset)
        {

            XPathNodeIterator nodes = nav.Select("*");
            while (nodes.MoveNext())
            {
                var current = nodes.Current;
                var name = current.Name;
                var isSequence = (name == "PesticideDatesAndRates" || name == "FertilizerInputDateSequences" || name == "IrrigationDates" || name == "IrrigationRunoffSequence" || name == "AdditionalInflowSequence");
                var hasAttributes = current.HasAttributes;
                var hasChildren = current.HasChildren;
                var arrayvalues = GetArrayValues(current);
                var lastmodified = GetAttribute(current, "LastModified");
                var description = GetAttribute(current, "Description");
                var state = GetAttribute(current, "state");
                var index = GetAttribute(current, "index");
                var comments = GetAttribute(current, "Comments");
                var count = GetAttribute(current, "Count");
                var day = GetAttribute(current, "Day");
                var month = GetAttribute(current, "Month");
                var href = GetAttribute(current, "href");
                var value = GetElementValue(current);
                var innerxml = current.InnerXml;

                //var nodetype=ExtractNodeType(name,hasAttributes,hasChildren,innerxml,arrayvalues,timeseries,href,lastmodified,description,state,index,comments,count,value);
                HLNodeType nodetype = HLNodeType.Root;
                if (isSequence) nodetype = HLNodeType.Sequence;
                else if (IsValid(index)) nodetype = HLNodeType.Enum;
                else if (IsValid(day) && IsValid(month)) nodetype = HLNodeType.DayMonth;
                else if (IsValid(state) && (state.ToLower() == "true" || state.ToLower() == "false")) nodetype = HLNodeType.Switch;
                else if (IsValid(arrayvalues)) nodetype = HLNodeType.Vector;
                else if (IsValid(count)) nodetype = HLNodeType.TimeSeries;
                else if (isNumber(value)) nodetype = HLNodeType.Value;
                else if (hasChildren && hasAttributes && (IsValid(href) || IsValid(description))) nodetype = HLNodeType.Header;



                switch (nodetype)
                {
                    case HLNodeType.Root: break;
                    case HLNodeType.Header: dataset.Description = description; dataset.Comments = comments; dataset.Name = name; break;
                    case HLNodeType.Vector: UpdateValue(dataset, name, arrayvalues, comments, lastmodified); break;
                    case HLNodeType.Value: UpdateValue(dataset, name, value, comments, lastmodified); break;
                    case HLNodeType.Switch: UpdateValue(dataset, name, state, comments, lastmodified); break;
                    case HLNodeType.Enum: UpdateValue(dataset, name, index, comments, lastmodified); break;
                    case HLNodeType.DayMonth: UpdateValue(dataset, name, $"{day},{month}", comments, lastmodified); break;
                    case HLNodeType.TimeSeries: UpdateValue(dataset, name, GetTimeSeriesValue(current), comments, lastmodified); break;
                    case HLNodeType.Sequence: UpdateValue(dataset, name, innerxml, comments, lastmodified); break;
                }



                ReadNodes(current, dataset);
            }


        }

        private bool IsValid(string text)
        {
            return !String.IsNullOrEmpty(text);
        }


        private string GetTimeSeriesValue(XPathNavigator current)
        {
            var count = GetAttribute(current, "Count");
            if (!String.IsNullOrEmpty(count))
            {
                var list = new List<string>();
                ReadDataNodes(current, list);
                if (list.Count > 0)
                {
                    return string.Join("|", list);
                }
            }
            return "";

        }
        private void ReadDataNodes(XPathNavigator nav, List<string> list)
        {
            XPathNodeIterator nodes = nav.Select("Data");
            while (nodes.MoveNext())
            {
                var templist = new List<string>();
                var current = nodes.Current;
                var x = current.GetAttribute($"x", string.Empty);
                var y = current.GetAttribute($"y", string.Empty);
                var z = current.GetAttribute($"z", string.Empty);
                var a = current.GetAttribute($"a", string.Empty);
                if (!String.IsNullOrEmpty(x)) templist.Add(x);
                if (!String.IsNullOrEmpty(y)) templist.Add(y);
                if (!String.IsNullOrEmpty(z)) templist.Add(z);
                if (!String.IsNullOrEmpty(a)) templist.Add(a);
                if (templist.Count > 0)
                {
                    list.Add(string.Join(",", templist));
                }
                ReadDataNodes(nodes.Current, list);
            }
        }




        //private HLNodeType ExtractNodeType(string name, bool hasAttributes, bool hasChildren, string innerxml,string arrayvalues, string timeseries,string href,string lastmodified, string description, string state, string index, string comments, string count, string value)
        //{
        //    if(!String.IsNullOrEmpty(index))return HLNodeType.Enum;
        //    if(!String.IsNullOrEmpty(state)&&(state.ToLower()=="true"||state.ToLower()=="false"))return HLNodeType.Switch;
        //    if(!String.IsNullOrEmpty(arrayvalues))return HLNodeType.Vector;
        //    if(!String.IsNullOrEmpty(count))return HLNodeType.TimeSeries;
        //    if(isNumber(value))return HLNodeType.Value;
        //    if(hasChildren&&hasAttributes &&(!String.IsNullOrEmpty(href)||!String.IsNullOrEmpty(description)))return HLNodeType.Header;
        //    return HLNodeType.Root;
        //}

        private bool isNumber(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                float val = 0;
                if (float.TryParse(text, out val))
                {
                    return true;
                }
            }
            return false;

        }


        private void UpdateValue(ParameterDataSet dataset, string name, string value, string comments, string lastmodified)
        {
            if (RemapDictionary.ContainsKey(name))
            {

                var codename = RemapDictionary[name];
                if (codename != "XXXX")
                {
                    var param = new InputParameter()
                    {
                        Id = Guid.NewGuid(),
                        NameInFile = name,
                        Value = value,
                        NameInCode = codename
                    };

                    dataset.AddParameter(param);
                }
                else
                {
                    AddErrorOutput($"Parameter {name} is defined as XXXX in remap dictionary");
                }
            }
            else
            {
                AddErrorOutput($"Parameter {name} not in remap dictionary");
            }

        }





        private string GetElementValue(XPathNavigator current)
        {
            if (current.InnerXml.Contains("<") == false && current.InnerXml.Contains(">") == false)
            {
                if (!String.IsNullOrEmpty(current.Value))
                    return current.Value;
            }
            return null;
        }

        private string GetAttribute(XPathNavigator current, string v)
        {
            var value = current.GetAttribute(v, string.Empty);
            if (!String.IsNullOrEmpty(value))
                return value;
            return null;
        }

        private string GetArrayValues(XPathNavigator current)
        {
            var list = new List<string>();
            for (var i = 1; i <= 10; ++i)
            {
                var value = current.GetAttribute($"value{i}", string.Empty);
                if (!String.IsNullOrEmpty(value))
                {
                    list.Add(value);
                }
            }

            if (list.Count > 0)
            {
                return string.Join(",", list);
            }
            return null;
        }




    }
}
