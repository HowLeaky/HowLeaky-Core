using HowLeaky_SimulationEngine.Enums;
using HowLeaky_SimulationEngine.Errors;
using HowLeaky_SimulationEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HowLeaky_SimulationEngine.Inputs
{
    public class HowLeakyInputsModel
    {
        public HowLeakyInputsModel(string identifier, int index)
        {
            Name = identifier;
            SimIndex = index;
            EPanMultiplier = 1;
            RainfallMultiplier = 1;
        }
        public int SimIndex { get; set; }
        public string Name { get; set; }
        public string FunctionalUnit { get; set; }
        public string SubCatchmentId { get; set; }

        public HowLeakyInputs_Climate Climate { get; set; }
        public HowLeakyInputs_Soil Soil { get; set; }
        public List<_CustomCropInputsModel> Crops { get; set; }
        public List<HowLeakyInputs_Tillage> Tillage { get; set; }
        public List<HowLeakyInputs_Pesticide> Pesticides { get; set; }
        public HowLeakyInputs_Irrigation Irrigation { get; set; }
        public HowLeakyInputs_Phosphorus Phosphorus { get; set; }
        public HowLeakyInputs_Nitrate Nitrate { get; set; }
        public HowLeakyInputs_Solute Solutes { get; set; }
        public BrowserDate StartDate { get; set; }
        public BrowserDate EndDate { get; set; }



        public int ResetResidueMassDay { get; set; }
        public int ResetResidueMassMonth { get; set; }
        public double ResetResidueMassValue { get; set; }
        public bool ResetSoilWaterAtDefinedDate { get; set; }
        public int ResetSoilWaterDay { get; set; }
        public int ResetSoilWaterMonth { get; set; }
        public double ResetSoilWaterValue { get; set; }
        public bool ResetSoilWaterAtPlanting { get; set; }
        //public bool CalculateLateralFlow{get;set;}
        //public bool IgnoreCropDepth{get;set;}
        //public bool UsePERFECTDryMatterFn{get;set;}
        //public bool UsePERFECTGroundCoverFn{get;set;}
        //public bool UsePERFECTSoilEvapFn{get;set;}
        //public bool UsePERFECTLeafAreaFn{get;set;}
        //public bool UsePERFECTResidueFn{get;set;}
        //public bool UsePERFECTUSLELSFactor{get;set;}
        //public bool UsePERFECTCNFn{get;set;}
        //public double PAWCStart{get;set;}
        //public int EvaporationOptions{get;set;}
        public double EPanMultiplier { get; set; }
        public double RainfallMultiplier { get; set; }



        public string LoadSoluteParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Solutes == null)
                {
                    Solutes = new HowLeakyInputs_Solute(datasetid, dataname);
                }
                if (TryLoadParameter(Solutes, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadNitrateParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Nitrate == null)
                {
                    Nitrate = new HowLeakyInputs_Nitrate(datasetid, dataname);
                }
                if (TryLoadParameter(Nitrate, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadPhosphorusParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Phosphorus == null)
                {
                    Phosphorus = new HowLeakyInputs_Phosphorus(datasetid, dataname);
                }
                if (TryLoadParameter(Phosphorus, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadPesticideParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Pesticides == null || Pesticides.Count == 0)
                {
                    Pesticides = new List<HowLeakyInputs_Pesticide>();
                }
                var pest = Pesticides.FirstOrDefault(x => x.Id == datasetid);
                if (pest == null)
                {
                    pest = new HowLeakyInputs_Pesticide(datasetid, dataname);
                    Pesticides.Add(pest);
                }
                if (TryLoadParameter(pest, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadIrrigationParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Irrigation == null)
                {
                    Irrigation = new HowLeakyInputs_Irrigation(datasetid, dataname);
                }
                if (TryLoadParameter(Irrigation, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadTillageParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Tillage == null || Tillage.Count == 0)
                {
                    Tillage = new List<HowLeakyInputs_Tillage>();
                }
                var tillage = Tillage.FirstOrDefault(x => x.Id == datasetid);
                if (tillage == null)
                {
                    tillage = new HowLeakyInputs_Tillage(datasetid, dataname);
                    Tillage.Add(tillage);
                }
                if (TryLoadParameter(tillage, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadVegetationParameter(Guid datasetid, string dataname, bool isLAI, string paramName, string value)
        {
            try
            {
                if (Crops == null || Crops.Count == 0)
                {
                    Crops = new List<_CustomCropInputsModel>();
                }
                var crop = Crops.FirstOrDefault(x => x.Id == datasetid);
                if (crop == null)
                {
                    if (isLAI)
                    {
                        crop = new HowLeakyInputs_LAIVeg(datasetid, dataname);
                    }
                    else
                    {
                        crop = new HowLeakyInputs_CoverVeg(datasetid, dataname);
                    }
                    Crops.Add(crop);

                }
                if (TryLoadParameter(crop, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public string LoadSoilParameter(Guid datasetid, string dataname, string paramName, string value)
        {
            try
            {
                if (Soil == null)
                {
                    Soil = new HowLeakyInputs_Soil(datasetid, dataname);
                }
                if (TryLoadParameter(Soil, paramName, value) == false)
                {
                    return paramName;
                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return null;
        }

        public bool TryLoadParameter(object parammodel, string paramName, string value)
        {
            try
            {
                if (String.IsNullOrEmpty(value))
                {
                    return true;
                }
                if (parammodel != null)
                {
                    var property = parammodel.GetType().GetProperty(paramName);
                    if (property != null)
                    {
                        if (TryUpdateParm(parammodel, property, value))
                        {
                            return true;


                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
            return false;
        }

        static public bool TryUpdateParm(object source, PropertyInfo property, string stringvalue)
        {
            try
            {
                var type = property.PropertyType;
                if (type == typeof(double) || type == typeof(double?) || type == typeof(Double))
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
                else if (type == typeof(bool))
                {
                    if (stringvalue.ToLower() == "true")
                    {
                        property.SetValue(source, true);
                    }
                    else
                    {
                        property.SetValue(source, false);
                    }

                    return true;


                }
                else if (type == typeof(List<double>))
                {
                    if (!string.IsNullOrEmpty(stringvalue))
                    {
                        var list = stringvalue.Split(',').ToList().Select(x => Double.Parse(x)).ToList();
                        property.SetValue(source, list);
                        return true;
                    }

                }
                else if (type == typeof(TimeSeriesData))
                {
                    var results = new TimeSeriesData(stringvalue);

                    property.SetValue(source, results);
                    return true;


                }
                else if (type == typeof(ProfileData))
                {
                    var results = new ProfileData(stringvalue);

                    property.SetValue(source, results);
                    return true;


                }
                else if (type == typeof(Sequence))
                {
                    var canInterpolate = property.Name != "FertilizerInputDateSequences";
                    var results = new Sequence(stringvalue, canInterpolate);                    
                    property.SetValue(source, results);
                    return true;


                }
                else if (type == typeof(IrrigationFormat))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (IrrigationFormat)value);
                        return true;
                    }

                }
                else if (type == typeof(TargetAmountOptions))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (TargetAmountOptions)value);
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
                else if (type == typeof(DissolvedNinRunoffType))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (DissolvedNinRunoffType)value);
                        return true;
                    }
                }
                else if (type == typeof(ParticulateNinRunoffType))
                {
                    int value;
                    if (int.TryParse(stringvalue, out value))
                    {
                        property.SetValue(source, (ParticulateNinRunoffType)value);
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



                else if (type == typeof(DayMonthData))
                {
                    var results = new DayMonthData(stringvalue);

                    property.SetValue(source, results);
                    return true;


                }

                else if (type == typeof(string))
                {
                    property.SetValue(source, stringvalue);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ErrorLogger.CreateException(ex);
            }
        }
    }
}
