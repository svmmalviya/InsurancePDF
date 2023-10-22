namespace PracticalProject.InvoiceUtility
{
    using System;

    public static class InvoiceGenerator
    {
        public static int invoiceNumberCounter = 1;

        public static string GenerateInvoiceNumber()
        {

            // Generate a unique number (you can replace this with your logic)
            int uniqueNumber = GetUniqueNumber();

            // Get the current date and time
            DateTime now = DateTime.Now;

            // invoice number using a prefix, current date, and unique number
            string invoiceNumber = $"INV-{now:yyyyMMddHHmmss}-{uniqueNumber:D5}";

            return invoiceNumber;
        }

        private static int GetUniqueNumber()
        {
            // Simulate generating a unique number (you can replace this with your logic)
            // Here, we increment a counter, but you might use a database sequence, GUID, or other methods.
            lock (typeof(InvoiceGenerator))
            {
                return invoiceNumberCounter++;
            }
        }
    }
}
