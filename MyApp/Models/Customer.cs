namespace MyApp.Models

{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerRegion { get; set; }
        public string CustomerCounty { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerFax { get; set; }
        public string CustomerSales { get; set; }
        public string CustomerTechnicant { get; set; }
    }

    public class CustomerSummary
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string County { get; set; }
        public string Region { get; set; }
        public string IndustryType { get; set; }
    }

    public class CustomerSelect
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string IndustryType { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Region { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string SalesContact { get; set; }
        public string TechnicalContact { get; set; }
    }
    public class CustomerDeleteRequest
    {
        public int id { get; set; }
    }

}