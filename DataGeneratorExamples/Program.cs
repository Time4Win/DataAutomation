// See https://aka.ms/new-console-template for more information

using DataGenerator.Insurance;

var dataGeneratorService = new DataDirector();

const string pathToFolder = @"C:\DataGenerator\automate\System1-InsuranceCompany\";

dataGeneratorService.GenerateAllData(1000, "MM/dd/yyyy", pathToFolder);