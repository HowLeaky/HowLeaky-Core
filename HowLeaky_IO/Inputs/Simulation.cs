using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Inputs;
using HowLeaky_SimulationEngine.Tools;
using HowLeaky_IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HowLeaky_SimulationEngine.Errors;

namespace HowLeaky_IO
{
    public class Simulation
    {
        public Simulation(ProjectHLK project, string name)
        {
            Project = project;
            ProjectName = name;
        }

        public ProjectHLK Project { get; set; }
        public string ProjectName { get; set; }
        public int Index { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string SubCatchmentId { get; set; }

        public string GenerateBinaryPathName()
        {
            var subcatchment = (!String.IsNullOrEmpty(SubCatchmentId) ? SubCatchmentId : "NOSUBCATCH").Replace("\n", "");
            var functionunit = (!String.IsNullOrEmpty(FunctionalUnit) ? FunctionalUnit : "NOFU").Replace("\n", "");
            var path1 = CheckCreatePath(Project.OutputsDirectory, $"{subcatchment}_{functionunit}");
            var binaryFilePath = Path.Combine(path1, $"{GenerateOutputName()}.hlkbinout");
            return binaryFilePath;
        }
        public string CheckCreatePath(string path1, string path2)
        {
            var path = Path.Combine(path1, path2);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        public string FunctionalUnit { get; set; }

        public P51DataFile ClimateData { get; set; }

        internal string GenerateOutputName()
        {
            //var subcat=!String.IsNullOrEmpty(SubCatchmentId)
            return $"{ProjectName}_Sim{Index}";
        }

        public ParameterDataSetPtr SoilData { get; set; }
        public List<ParameterDataSetPtr> TillageData { get; set; }
        public List<ParameterDataSetPtr> VegData { get; set; }
        public List<ParameterDataSetPtr> PesticideData { get; set; }
        public ParameterDataSetPtr Irrigation { get; set; }
        public ParameterDataSetPtr PhosphorusData { get; set; }
        public ParameterDataSetPtr SoluteData { get; set; }
        public ParameterDataSetPtr NitrateData { get; set; }

        public List<string> Errors { get; set; }

        public HowLeakyInputsModel InputsModel { get; set; }

        internal void Reset()
        {
            InputsModel = null;
            UpdateClimateFileReferenceCount();
        }
        static readonly object newlock = new object();
        public HowLeakyInputsModel GenerateInputs()
        {
            lock (newlock)
            {
                Errors = new List<string>();
                if (InputsModel == null)
                {
                    InputsModel = new HowLeakyInputsModel(GenerateOutputName(), Index);
                }
                InputsModel.Climate = GenerateClimateInputs(InputsModel.Climate, ClimateData);
                InputsModel.Soil = GenerateSoilInputs(InputsModel.Soil, SoilData);
                InputsModel.Crops = GenerateCropInputs(InputsModel.Crops, VegData);
                InputsModel.Tillage = GenerateTillageInputs(InputsModel.Tillage, TillageData);
                InputsModel.Pesticides = GeneratePesticideInputs(InputsModel.Pesticides, PesticideData);
                InputsModel.Irrigation = GenerateIrrigationInputs(InputsModel.Irrigation, Irrigation);
                InputsModel.Phosphorus = GeneratePhosphorusInputs(InputsModel.Phosphorus, PhosphorusData);
                InputsModel.Nitrate = GeneratNitrateInputs(InputsModel.Nitrate, NitrateData);
                InputsModel.Solutes = GenerateSolutesInputs(InputsModel.Solutes, SoluteData);


                InputsModel.StartDate = StartYear != null ? new BrowserDate((int)StartYear, 1, 1) : null;
                InputsModel.EndDate = EndYear != null ? new BrowserDate((int)EndYear, 12, 31) : null;
                if (InputsModel.StartDate == null)
                {
                    InputsModel.StartDate = ClimateData != null ? ClimateData.StartDate : null;
                }
                if (InputsModel.EndDate == null)
                {
                    InputsModel.EndDate = ClimateData != null ? ClimateData.EndDate : null;
                }
                if (InputsModel.StartDate == null)
                {
                    Errors.Add("Start date is not defined");
                }
                if (InputsModel.EndDate == null)
                {
                    Errors.Add("End date is not defined");
                }

                if (Errors.Count == 0)
                {
                    return InputsModel;
                }
            }
            return null;

        }

        internal void UpdateClimateFileReferenceCount()
        {
            if (ClimateData != null)
            {
                ClimateData.DecrementReferenceCount();
            }
        }

        public bool OutputExists()
        {
            var output = GenerateBinaryPathName();
            return File.Exists(output);
        }

        private HowLeakyInputs_Climate GenerateClimateInputs(HowLeakyInputs_Climate inputs, P51DataFile climateData)
        {
            try
            {
                if (ClimateData != null)
                {
                    if (climateData.HasLoaded == false)
                    {
                        climateData.OpenFull();
                    }
                    if (climateData.HasLoaded)
                    {
                        inputs = new HowLeakyInputs_Climate();
                        inputs.MaxT = climateData.TimeSeries[0];//.Select(x => (double?)x).ToList();//MaxTempValues
                        inputs.MinT = climateData.TimeSeries[1];//.Select(x => (double?)x).ToList();//MinTempValues
                        inputs.Rain = climateData.TimeSeries[2];//.Select(x => (double?)x).ToList(); //RainfallValues
                        inputs.PanEvap = climateData.TimeSeries[3];//.Select(x => (double?)x).ToList();//PanEvapValues
                        inputs.Radiation = climateData.TimeSeries[4];//.Select(x => (double?)x).ToList();//SolarRadValues
                        inputs.StartDate = climateData.StartDate;
                        inputs.EndDate = climateData.EndDate;
                        return inputs;
                    }
                    else
                    {
                        Errors.Add($"Error reading climate data ({climateData.FileName})");

                    }

                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        private HowLeakyInputs_Soil GenerateSoilInputs(HowLeakyInputs_Soil inputs, ParameterDataSetPtr soilData)
        {
            if (soilData != null)
            {
                if (inputs == null)
                {
                    inputs = new HowLeakyInputs_Soil();
                }
                var properties = typeof(HowLeakyInputs_Soil).GetProperties().ToList();
                var errors = LoadValues(soilData, inputs, properties);
                if (errors.Count > 0)
                {
                    Errors.AddRange(errors);
                }
                return inputs;
            }
            return null;
        }

        private List<_CustomCropInputsModel> GenerateCropInputs(List<_CustomCropInputsModel> inputslist, List<ParameterDataSetPtr> vegData)
        {
            if (vegData != null && vegData.Count > 0)
            {
                var list = new List<_CustomCropInputsModel>();
                foreach (var vegdata in vegData)
                {
                    var modeltype = vegdata.DataSet.Parameters.FirstOrDefault(x => x.NameInFile == "ModelType");
                    var isCover = modeltype != null && modeltype.Value == "1";
                    if (!isCover)
                    {
                        var inputs = new HowLeakyInputs_LAIVeg(vegdata.DataSet.Description);
                        var properties = typeof(HowLeakyInputs_LAIVeg).GetProperties().ToList();
                        var errors = LoadValues(vegdata, inputs, properties);
                        if (errors.Count > 0)
                        {
                            Errors.AddRange(errors);
                        }
                        list.Add(inputs);
                    }
                    else
                    {
                        var inputs = new HowLeakyInputs_CoverVeg(vegdata.DataSet.Description);
                        var properties = typeof(HowLeakyInputs_CoverVeg).GetProperties().ToList();
                        var errors = LoadValues(vegdata, inputs, properties);
                        if (errors.Count > 0)
                        {
                            Errors.AddRange(errors);
                        }
                        list.Add(inputs);
                    }
                }
                return list;
            }
            return null;
        }

        private List<HowLeakyInputs_Tillage> GenerateTillageInputs(List<HowLeakyInputs_Tillage> inputslist, List<ParameterDataSetPtr> tillageData)
        {
            if (tillageData != null && tillageData.Count > 0)
            {
                var list = new List<HowLeakyInputs_Tillage>();
                foreach (var tillage in tillageData)
                {
                    var inputs = new HowLeakyInputs_Tillage(tillage.DataSet.Description);
                    var properties = typeof(HowLeakyInputs_Tillage).GetProperties().ToList();
                    var errors = LoadValues(tillage, inputs, properties);
                    if (errors.Count > 0)
                    {
                        Errors.AddRange(errors);
                    }
                    list.Add(inputs);
                }
                return list;
            }
            return null;
        }

        private List<HowLeakyInputs_Pesticide> GeneratePesticideInputs(List<HowLeakyInputs_Pesticide> inputslist, List<ParameterDataSetPtr> pesticideData)
        {
            if (pesticideData != null && pesticideData.Count > 0)
            {
                var list = new List<HowLeakyInputs_Pesticide>();
                foreach (var pesticide in pesticideData)
                {
                    var inputs = new HowLeakyInputs_Pesticide(pesticide.DataSet.Description);
                    var properties = typeof(HowLeakyInputs_Pesticide).GetProperties().ToList();
                    var errors = LoadValues(pesticide, inputs, properties);
                    if (errors.Count > 0)
                    {
                        Errors.AddRange(errors);
                    }
                    list.Add(inputs);
                }
                return list;
            }
            return null;
        }

        private HowLeakyInputs_Irrigation GenerateIrrigationInputs(HowLeakyInputs_Irrigation inputs, ParameterDataSetPtr irrigation)
        {
            if (irrigation != null)
            {
                if (inputs == null)
                {
                    inputs = new HowLeakyInputs_Irrigation();
                }

                var properties = typeof(HowLeakyInputs_Irrigation).GetProperties().ToList();
                var errors = LoadValues(irrigation, inputs, properties);
                if (errors.Count > 0)
                {
                    Errors.AddRange(errors);
                }
                return inputs;
            }
            return null;
        }

        private HowLeakyInputs_Phosphorus GeneratePhosphorusInputs(HowLeakyInputs_Phosphorus inputs, ParameterDataSetPtr phosphorusData)
        {
            if (phosphorusData != null)
            {
                if (inputs == null)
                {
                    inputs = new HowLeakyInputs_Phosphorus();
                }
                var properties = typeof(HowLeakyInputs_Phosphorus).GetProperties().ToList();
                var errors = LoadValues(phosphorusData, inputs, properties);
                if (errors.Count > 0)
                {
                    Errors.AddRange(errors);
                }
                return inputs;

            }
            return null;
        }

        private HowLeakyInputs_Nitrate GeneratNitrateInputs(HowLeakyInputs_Nitrate inputs, ParameterDataSetPtr nitrateData)
        {
            if (nitrateData != null)
            {
                if (inputs == null)
                {
                    inputs = new HowLeakyInputs_Nitrate();
                }
                var properties = typeof(HowLeakyInputs_Nitrate).GetProperties().ToList();
                var errors = LoadValues(nitrateData, inputs, properties);
                if (errors.Count > 0)
                {
                    Errors.AddRange(errors);
                }
                return inputs;
            }
            return null;
        }

        private HowLeakyInputs_Solute GenerateSolutesInputs(HowLeakyInputs_Solute inputs, ParameterDataSetPtr soluteData)
        {
            if (soluteData != null)
            {
                if (inputs == null)
                {
                    inputs = new HowLeakyInputs_Solute();
                }
                var properties = typeof(HowLeakyInputs_Solute).GetProperties().ToList();
                var errors = LoadValues(soluteData, inputs, properties);
                if (errors.Count > 0)
                {
                    Errors.AddRange(errors);
                }
                return inputs;
            }
            return null;
        }


        private List<string> LoadValues(ParameterDataSetPtr source, object inputs, List<PropertyInfo> properties)
        {
            var list = new List<string>();
            var parameters = source.DataSet.Parameters;
            var overrides = source.Overrides;

            foreach (var parameter in parameters)
            {
                var property = properties.FirstOrDefault(x => x.Name == parameter.NameInCode);
                if (property != null)
                {
                    if (TryUpdateParm(inputs, property, parameter.Value) == false)
                    {
                        if (parameter.NameInCode != "PAWC" && parameter.NameInCode != "MaxDailyDrainVolume")
                        {
                            list.Add($"Could not load {parameter.NameInCode}  (type not defined)");
                        }
                    }
                }
                else
                {
                    if (parameter.NameInCode != "PAWC" && 
                        parameter.NameInCode != "MaxDailyDrainVolume" && 
                        parameter.NameInCode != "NitrateSourceData" && 
                        parameter.NameInCode != "WatStressForDeath" &&
                        parameter.NameInCode != "ModelType" &&
                        parameter.NameInCode != "SourceData" &&
                         parameter.NameInCode != "MaxAllowTotalCover" &&
                        parameter.NameInCode != "SoilCrack")
                    {
                        list.Add($"Could not FIND {parameter.NameInCode} parameter in input parameters (from {source.DataSet.SourceFileName})");
                    }
                }
            }

            foreach (var parameter in overrides)
            {
                if (String.IsNullOrEmpty(parameter.CodeName) == false)
                {
                    var property = properties.FirstOrDefault(x => x.Name == parameter.CodeName);
                    if (property != null)
                    {
                        if (TryUpdateParm(inputs, property, parameter.Value) == false)
                        {
                            list.Add($"Could not load override {property.Name}  (type not defined)");
                        }
                    }
                    else
                    {
                        if (parameter.CodeName != "PAWC" &&
                        parameter.CodeName != "MaxDailyDrainVolume" &&
                        parameter.CodeName != "NitrateSourceData" &&
                        parameter.CodeName != "WatStressForDeath" &&
                        parameter.CodeName != "ModelType" &&
                         parameter.CodeName != "SourceData" &&
                         parameter.CodeName != "MaxAllowTotalCover" &&
                        parameter.CodeName != "SoilCrack")
                        {
                            list.Add($"Could not FIND {parameter.CodeName} parameter in input parameters (from {source.DataSet.SourceFileName})");
                        }
                        //list.Add($"Could not FIND {parameter.CodeName} override in input parameters");
                    }
                }
                else
                {
                    list.Add($"Override in Simulation {Index} has no name!!");
                }
            }

            //foreach (var property in properties)
            //{
            //    var param1=overrides.FirstOrDefault(x=>x.CodeName==property.Name);
            //    if(param1!=null)
            //    {
            //        if(TryUpdateParm(inputs,property,param1.Value)==false)
            //        {
            //            list.Add($"Could not load override {property.Name}  (type not defined)");
            //        }
            //    }  
            //    else
            //    {
            //        var param2=parameters.FirstOrDefault(x=>x.NameInFile==property.Name||x.NameInCode==property.Name);
            //        if(param2!=null)
            //        {
            //            if(TryUpdateParm(inputs,property,param2.Value)==false)
            //            {
            //                list.Add($"Could not load {property.Name} value (type not defined)");
            //            }
            //        }
            //        else
            //        {
            //            list.Add($"Could not FIND {property.Name} parameter in datafile");
            //        }
            //    }
            //}
            return list;
        }

        public bool TryUpdateParm(object source, PropertyInfo property, string stringvalue)
        {
            try
            {
                var type = property.PropertyType;
                if (type == typeof(double) || type == typeof(double?))
                {
                    if (type == typeof(double?))
                    {
                        if (string.IsNullOrEmpty(stringvalue))
                        {
                            property.SetValue(source, (double?)null);
                            return true;
                        }
                        else
                        {
                            double value;
                            if (double.TryParse(stringvalue, out value))
                            {
                                property.SetValue(source, value);
                                return true;
                            }
                        }

                    }
                    else
                    {
                        double value;
                        if (double.TryParse(stringvalue, out value))
                        {
                            property.SetValue(source, value);
                            return true;
                        }
                    }

                }
                else if (type == typeof(float) || type == typeof(float?))
                {
                    if (type == typeof(float?))
                    {
                        if (string.IsNullOrEmpty(stringvalue))
                        {
                            property.SetValue(source, (float?)null);
                            return true;
                        }
                        else
                        {
                            float value;
                            if (float.TryParse(stringvalue, out value))
                            {
                                property.SetValue(source, value);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        float value;
                        if (float.TryParse(stringvalue, out value))
                        {
                            property.SetValue(source, value);
                            return true;
                        }
                    }
                }

                else if (type == typeof(int) || type == typeof(int?))
                {
                    if (type == typeof(int?))
                    {
                        if (string.IsNullOrEmpty(stringvalue))
                        {
                            property.SetValue(source, (int?)null);
                            return true;
                        }
                        else
                        {
                            int value;
                            if (int.TryParse(stringvalue, out value))
                            {
                                property.SetValue(source, value);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        int value;
                        if (int.TryParse(stringvalue, out value))
                        {
                            property.SetValue(source, value);
                            return true;
                        }
                    }
                }
                else if (type == typeof(List<double>))
                {
                    var list = stringvalue.Split(',').ToList().Select(x => Double.Parse(x)).ToList();
                    property.SetValue(source, list);
                    return true;


                }
                else if (type == typeof(List<XYXData>))
                {
                    var results = new List<XYXData>();
                    var list = stringvalue.Split(',').ToList();
                    foreach (var item in list)
                    {
                        var values = item.Split('|').ToList();
                        results.Add(new XYXData(values));
                    }

                    property.SetValue(source, results);
                    return true;


                }
                else if (type == typeof(ProfileData))
                {
                    var results = new ProfileData(stringvalue);


                    property.SetValue(source, results);
                    return true;


                }
                else if (type == typeof(TimeSeriesData))
                {
                    var results = new TimeSeriesData(stringvalue);

                    property.SetValue(source, results);
                    return true;


                }
                else if (type == typeof(Sequence))
                {
                    var canInterpolate=property.Name != "FertilizerInputDateSequences";
                    var results = new Sequence(stringvalue, canInterpolate);

                    property.SetValue(source, results);
                    return true;


                }

                else if (type == typeof(bool))
                {
                    var value = stringvalue.ToLower() == "true" ? true : false;
                    property.SetValue(source, value);
                    return true;
                }
                else if (type == typeof(DayMonthData))
                {
                    var results = new DayMonthData(stringvalue);
                    property.SetValue(source, results);
                    return true;
                }
                else if (type == typeof(EPestApplicationPosition))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (EPestApplicationPosition)value);
                        return true;
                    }
                }

                else if (type == typeof(ParameterType) || type == typeof(ParameterType?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (ParameterType)value);
                        return true;
                    }
                }
                else if (type == typeof(IrrigationFormat) || type == typeof(IrrigationFormat?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (IrrigationFormat)value);
                        return true;
                    }
                }
                else if (type == typeof(TargetAmountOptions) || type == typeof(TargetAmountOptions?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (TargetAmountOptions)value);
                        return true;
                    }
                }
                else if (type == typeof(IrrigationEvaporationOptions) || type == typeof(IrrigationEvaporationOptions?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (IrrigationEvaporationOptions)value);
                        return true;
                    }
                }
                else if (type == typeof(IrrigationRunoffOptions) || type == typeof(IrrigationRunoffOptions?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (IrrigationRunoffOptions)value);
                        return true;
                    }
                }
                else if (type == typeof(DissolvedNinLeachingType) || type == typeof(DissolvedNinLeachingType?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (DissolvedNinLeachingType)value);
                        return true;
                    }
                }
                else if (type == typeof(DissolvedNinRunoffType) || type == typeof(DissolvedNinRunoffType?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (DissolvedNinRunoffType)value);
                        return true;
                    }
                }
                else if (type == typeof(ParticulateNinRunoffType) || type == typeof(ParticulateNinRunoffType?))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (ParticulateNinRunoffType)value);
                        return true;
                    }
                }
                else if (type == typeof(DissolvedNinLeachingType))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (DissolvedNinLeachingType)value);
                        return true;
                    }
                }
                
                else if (type == typeof(CropUseOption))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (CropUseOption)value);
                        return true;
                    }
                }
                else if (type == typeof(DenitrificationOption))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (DenitrificationOption)value);
                        return true;
                    }
                }

                else if (type == typeof(string))
                {
                    property.SetValue(source, stringvalue);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update source:{source} with {stringvalue} for {property}", ex);
            }
            return false;
        }
    }
}
