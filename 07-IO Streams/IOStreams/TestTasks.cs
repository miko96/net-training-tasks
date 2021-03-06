﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace IOStreams
{

    public static class TestTasks
    {
        /// <summary>
        /// Parses Resourses\Planets.xlsx file and returns the planet data: 
        ///   Jupiter     69911.00
        ///   Saturn      58232.00
        ///   Uranus      25362.00
        ///    ...
        /// See Resourses\Planets.xlsx for details
        /// </summary>
        /// <param name="xlsxFileName">source file name</param>
        /// <returns>sequence of PlanetInfo</returns>
        public static IEnumerable<PlanetInfo> ReadPlanetInfoFromXlsx(string xlsxFileName)
        {
            using (Package package = Package.Open(xlsxFileName))
            {
                var sheetUri = new Uri("/xl/worksheets/sheet1.xml", UriKind.Relative);
                var sharedStringsUri = new Uri("/xl/sharedStrings.xml", UriKind.Relative);

                var sheetPackagePart = package.GetPart(sheetUri);
                var stringsPackagePart = package.GetPart(sharedStringsUri);

                var sheet = XDocument.Load(sheetPackagePart.GetStream());
                var sharedStrings = XDocument.Load(stringsPackagePart.GetStream());

                string ns = "{http://schemas.openxmlformats.org/spreadsheetml/2006/main}";
                var meanRadiuses = sheet.Root.Descendants(ns + "v")
                    .Skip(2)
                    .Where((elm, index) => index % 2 != 0)
                    .Select(elm => Convert.ToDouble(elm.Value, new CultureInfo("en-US")));

                return sharedStrings.Descendants(ns + "t")
                    .Select(elm => elm.Value)
                    .Zip(meanRadiuses, (name, radius) => new PlanetInfo { Name = name, MeanRadius = radius });
            }
        }


        /// <summary>
        /// Calculates hash of stream using specifued algorithm
        /// </summary>
        /// <param name="stream">source stream</param>
        /// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
        /// <returns></returns>
        public static string CalculateHash(this Stream stream, string hashAlgorithmName)
        {
            try
            {
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashAlgorithmName);
                byte[] hash = hashAlgorithm.ComputeHash(stream);
                string hashString = "";
                foreach (var chr in hash)
                    hashString += chr.ToString("X2");
                return hashString;
            }
            catch
            { 
                throw new ArgumentException();
            }
        }


        /// <summary>
        /// Returns decompressed strem from file. 
        /// </summary>
        /// <param name="fileName">source file</param>
        /// <param name="method">method used for compression (none, deflate, gzip)</param>
        /// <returns>output stream</returns>
        public static Stream DecompressStream(string fileName, DecompressionMethods method)
        {
           var fileStream = File.Open(fileName, FileMode.Open);

            switch(method)
            {
                case (DecompressionMethods.Deflate):
                    return new DeflateStream(fileStream, CompressionMode.Decompress);
                case(DecompressionMethods.GZip):
                    return new GZipStream(fileStream, CompressionMode.Decompress);
                default:
                    return fileStream;                   
            }
        }


        /// <summary>
        /// Reads file content econded with non Unicode encoding
        /// </summary>
        /// <param name="fileName">source file name</param>
        /// <param name="encoding">encoding name</param>
        /// <returns>Unicoded file content</returns>
        public static string ReadEncodedText(string fileName, string encoding)
        {
            return File.ReadAllText(fileName, Encoding.GetEncoding(encoding));
        }
    }


    public class PlanetInfo : IEquatable<PlanetInfo>
    {
        public string Name { get; set; }
        public double MeanRadius { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, MeanRadius);
        }

        public bool Equals(PlanetInfo other)
        {
            return Name.Equals(other.Name)
                && MeanRadius.Equals(other.MeanRadius);
        }
    }
}
