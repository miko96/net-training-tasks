using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace LinqToXml
{
    public static class LinqToXml
    {
        /// <summary>
        /// Creates hierarchical data grouped by category
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation (refer to CreateHierarchySourceFile.xml in Resources)</param>
        /// <returns>Xml representation (refer to CreateHierarchyResultFile.xml in Resources)</returns>
        public static string CreateHierarchy(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);

            var resultXml = sourceXml
                .Elements("Data")
                .GroupBy(data => data.Element("Category").Value,
                         data => new XElement("Data", data.Elements().Where(e => e.Name != "Category")))
                .Select(g => new XElement("Group", 
                                new XAttribute("ID", g.Key), g));
            return new XElement("Root", resultXml).ToString();
        }

        /// <summary>
        /// Get list of orders numbers (where shipping state is NY) from xml representation
        /// </summary>
        /// <param name="xmlRepresentation">Orders xml representation (refer to PurchaseOrdersSourceFile.xml in Resources)</param>
        /// <returns>Concatenated orders numbers</returns>
        /// <example>
        /// 99301,99189,99110
        /// </example>
        public static string GetPurchaseOrders(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);
            string ns = "{http://www.adventure-works.com}";
            string state = "NY";

            var orders = sourceXml
                .Elements(ns + "PurchaseOrder")
                .Where(e => e.Element(ns + "Address").Element(ns + "State").Value == state)
                .Select(o => o.Attribute(ns + "PurchaseOrderNumber").Value);
            return string.Join(",", orders);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            var lines = customers.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //var a = new XElement("Root",
            var resultXml = lines.Select(line => line.Split(','))
                 .Select(param =>
                    new XElement("Customer",
                        new XAttribute("CustomerID", param[0]),
                        new XElement("CompanyName", param[1]),
                        new XElement("ContactName", param[2]),
                        new XElement("ContactTitle", param[3]),
                        new XElement("Phone", param[4]),
                        new XElement("FullAddress",
                            new XElement("Address", param[5]),
                            new XElement("City", param[6]),
                            new XElement("Region", param[7]),
                            new XElement("PostalCode", param[8]),
                            new XElement("Country", param[9])))
                    );
            return new XElement("Root", resultXml).ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);
            return sourceXml.Value;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);
            foreach (var elm in sourceXml.Elements("customer"))
                elm.Name = "contact";
            return sourceXml.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);
            string mark = "DELETE";
            return sourceXml.Elements()
                .Where(e => e.Nodes().OfType<XComment>().FirstOrDefault()?.Value == mark)
                .Where(e => e.Elements().Count() >= 2)
                .Select(e => Convert.ToInt32(e.Attribute("id").Value));
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);
            var resultXml = sourceXml.Elements("Customers")
                .OrderBy(e => e.Element("FullAddress").Element("Country").Value)
                .ThenBy(e => e.Element("FullAddress").Element("City").Value);
            return new XElement("Root", resultXml).ToString();
        }

        /// <summary>
        /// Gets XElement flatten string representation to save memory
        /// </summary>
        /// <param name="xmlRepresentation">XElement object</param>
        /// <returns>Flatten string representation</returns>
        /// <example>
        ///     <root><element>something</element></root>
        /// </example>
        public static string GetFlattenString(XElement xmlRepresentation)
        {
            return xmlRepresentation.ToString(SaveOptions.DisableFormatting);   
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            var sourceXml = XElement.Parse(xmlRepresentation);
            return sourceXml.Element("products").Elements("product")
                    .Join(sourceXml.Elements("Orders").Elements("Order"),
                          p => p.Attribute("Id").Value,
                          o => o.Element("product").Value,
                          (p, o) => Convert.ToInt32(p.Attribute("Value").Value))
                    .Sum();
        }
    }
}
