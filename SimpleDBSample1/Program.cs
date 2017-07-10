/*******************************************************************************
* Copyright John Boyer. All Rights Reserved.
*   
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/

using System;
using System.Collections.Generic;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;

namespace SimpleDBSample
{
    class Program
    {
        /// <summary>
        /// Customer domain name
        /// </summary>
        private static readonly string DOMAIN = "customer";
        /// <summary>
        /// Amazon Simple DB client 
        /// </summary>
        private static readonly AmazonSimpleDBClient SDB = new AmazonSimpleDBClient();

        public static void Main(string[] args)
        {

            try
            {
                Console.WriteLine("===========================================");
                Console.WriteLine("Getting Started with Amazon SimpleDB");
                Console.WriteLine("===========================================\n");

                // Creating a domain
                Console.WriteLine("Creating domain called customer\n");
                CreateDomain();

                PrintDomains();

                // Putting data into a domain
                Console.WriteLine("Putting data into customer domain.\n");
                batchPutSampleItems();

                PrintAllSampleItems();

                //Deleting a domain
                Console.WriteLine("Deleting customer domain.\n");
                DeleteDomain();

            }
            catch (AmazonSimpleDBException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.Read();
        }

        private static void DeleteDomain()
        {
            var deleteDomainAction = new DeleteDomainRequest(DOMAIN);
            SDB.DeleteDomain(deleteDomainAction);
        }

        private static void PrintAllSampleItems()
        {
            // Getting data from a domain
            Console.WriteLine("Print the attributes\n");
            const string selectExpression = "SELECT * From customer";
            var selectRequestAction = new SelectRequest(selectExpression);
            var selectResponse = SDB.Select(selectRequestAction);

            foreach (var item in selectResponse.Items)
            {
                Console.WriteLine("  Item");
                if (!string.IsNullOrEmpty(item.Name))
                {
                    Console.WriteLine("    Name: {0}", item.Name);
                }
                foreach (var attribute in item.Attributes)
                {
                    Console.WriteLine("      Attribute");
                    if (!string.IsNullOrEmpty(attribute.Name))
                    {
                        Console.WriteLine("        Name: {0}", attribute.Name);
                    }
                    if (!string.IsNullOrEmpty(attribute.Value))
                    {
                        Console.WriteLine("        Value: {0}", attribute.Value);
                    }
                }
            }
            Console.WriteLine();
        }

        private static void batchPutSampleItems()
        {
            List<ReplaceableItem> items = CreateSampleItems();
            //Perform a batch update request
            SDB.BatchPutAttributes(new BatchPutAttributesRequest(DOMAIN, items));
        }

        private static void PrintDomains()
        {
            // Listing domains
            var sdbListDomainsResponse = SDB.ListDomains(new ListDomainsRequest());
            Console.WriteLine("List of domains:\n");
            foreach (var domain in sdbListDomainsResponse.DomainNames)
            {
                Console.WriteLine("  " + domain);
            }

            Console.WriteLine();
        }

        private static void CreateDomain()
        {
            var createDomain = new CreateDomainRequest(DOMAIN);
            SDB.CreateDomain(createDomain);
        }

        private static List<ReplaceableItem> CreateSampleItems()
        {
            var items = new List<ReplaceableItem>();
            var attrs = new List<ReplaceableAttribute>();

            //John Doe
            attrs.Add(new ReplaceableAttribute { Name = "first_name", Value = "John" });
            attrs.Add(new ReplaceableAttribute { Name = "last_name", Value = "Doe" });
            attrs.Add(new ReplaceableAttribute { Name = "email", Value = "john@example.com" });
            items.Add(new ReplaceableItem("cust_001", attrs));

            attrs = new List<ReplaceableAttribute>();

            //Jane Doe
            attrs.Add(new ReplaceableAttribute { Name = "first_name", Value = "Jane" });
            attrs.Add(new ReplaceableAttribute { Name = "last_name", Value = "Doe" });
            attrs.Add(new ReplaceableAttribute { Name = "email", Value = "jane@example.com" });
            items.Add(new ReplaceableItem("cust_002", attrs));

            attrs = new List<ReplaceableAttribute>();

            //Mary Smith
            attrs.Add(new ReplaceableAttribute { Name = "first_name", Value = "Mary" });
            attrs.Add(new ReplaceableAttribute { Name = "last_name", Value = "Smith" });
            attrs.Add(new ReplaceableAttribute { Name = "email", Value = "mary@example.com" });
            items.Add(new ReplaceableItem("cust_003", attrs));

            attrs = new List<ReplaceableAttribute>();

            //Bob Smith
            attrs.Add(new ReplaceableAttribute { Name = "first_name", Value = "Mary" });
            attrs.Add(new ReplaceableAttribute { Name = "last_name", Value = "Smith" });
            attrs.Add(new ReplaceableAttribute { Name = "email", Value = "bob@example.com" });
            items.Add(new ReplaceableItem("cust_004", attrs));

            return items;
        }
    }
}