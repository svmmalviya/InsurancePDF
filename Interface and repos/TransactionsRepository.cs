using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using PracticalProject.Models;
using iTextSharp.text.pdf.draw;

namespace PracticalProject.Interface_and_repos
{
    public class TransactionsRepository : IPatientTransactions
    {

        public static List<HomeViewModel> patients = new List<HomeViewModel>();
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public TransactionsRepository(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            this._environment = environment;
            patients.Add(new HomeViewModel()
            {
                PolicyNumber = "01234",
                CoverageAmount = 500000,
                DOB = DateTime.Now.AddYears(-20),
                PartyName = "X Patient",
                Id = Guid.NewGuid(),
                Items = new List<PaymentTransaction>
            {
                new PaymentTransaction()
                {
                    amount=1000,
                    Description="Payment Description",
                    Id=Guid.NewGuid(),
                    PaymentDate=DateTime.Now.AddMonths(-6)
                },
            }
            });
        }

        /// <summary>
        /// Add patients
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public bool AddPatient(HomeViewModel patient)
        {
            patients.Add(patient);
            return true;
        }


        /// <summary>
        /// Get patients
        /// </summary>
        /// <returns></returns>
        public List<HomeViewModel> GetPatients()
        {
            return patients.ToList();
        }

        /// <summary>
        /// Get patient by id
        /// </summary>
        /// <returns></returns>
        public HomeViewModel GetPatientById(string id)
        {
            var patient =patients.Where(x => x.Id.ToString() == id).FirstOrDefault();

            //foreach (var item in patient.Items)
            //{
            //    item.PaymentDate = Convert.ToDateTime(item.PaymentDate.Date.ToString("dd/MM/yyyy"));
            //}
            return patient;
        }

        /// <summary>
        /// Edit patient 
        /// </summary>
        /// <returns></returns>
        public HomeViewModel EditPatientById(string id)
        {
            try
            {
                var patient = patients.Where(x => x.Id.ToString() == id.ToString()).FirstOrDefault();
                return patient;
            }
            catch (Exception e)
            {

                throw;
            }
        }


        /// <summary>
        /// Update patient 
        /// </summary>
        /// <returns></returns>
        public HomeViewModel UpdatePatientById(HomeViewModel model)
        {
            try
            {
                var patient = patients.Where(x => x.Id.ToString() == model.Id.ToString()).FirstOrDefault();
                if (patient != null)
                {
                    patients.Remove(patient);

                    patient.PolicyNumber = model.PolicyNumber;
                    patient.PartyName = model.PartyName;
                    patient.Items = model.Items;
                    patient.CoverageAmount = model.CoverageAmount;

                    patients.Add(patient);
                }

                return model;
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public FileContentResult GeneratePDF(HomeViewModel model)
        {
            try
            {

                // Create a Document object
                Document document = new Document(PageSize.A4, 40, 40, 40, 70);

                //MemoryStream
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, PDFData);


                // First, create our fonts
                var titleFont = FontFactory.GetFont("Arial", 14, Font.NORMAL);
                var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD,BaseColor.BLACK);
                var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL,BaseColor.BLACK);

                Rectangle pageSize = writer.PageSize;

                // Open the Document for writing
                document.Open();
                //Add elements to the document here


                // Load and scale the logo image
                string imagePath =  Path.Combine(_environment.WebRootPath, "css","img","logo_black.png");
                Image logoImage = Image.GetInstance(imagePath);
                logoImage.ScaleToFit(80f, 50f); // Adjust the size as needed
                
                // Create a paragraph with the current date
                Paragraph date = new Paragraph(DateTime.Now.ToString("yyyy-MM-dd"), new Font(Font.FontFamily.HELVETICA, 10));
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase(date), document.Right, document.Top, 0);

                // Set the alignment and position of the date in the header
                date.Alignment = Element.ALIGN_RIGHT;
                date.SpacingBefore = 10;
                
                #region Top table

                // Create the header table 
                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SpacingBefore = 20f;

                headertable.SetWidths(new float[] { 4, 2, 4 });  // then set the column's __relative__ widths
                headertable.DefaultCell.Border = Rectangle.NO_BORDER;
                //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
                headertable.SpacingAfter = 80f;

                // Define fonts for your PDF content
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                
               var headFont = new Font(baseFont, 11, Font.NORMAL, BaseColor.BLACK);
                Font contentFont = new Font(baseFont, 11,Font.BOLD);

                PdfPTable Tblheadtext = new PdfPTable(1);
                Tblheadtext.SpacingAfter = 20f;

                PdfPCell tblHeadCell = new PdfPCell();
                
                tblHeadCell.BackgroundColor = BaseColor.WHITE;
          
                logoImage.Alignment = Element.ALIGN_RIGHT;

                tblHeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                tblHeadCell.Border = Rectangle.NO_BORDER;
                tblHeadCell.AddElement(logoImage);

                Tblheadtext.AddCell(tblHeadCell);

                document.Add(Tblheadtext);
                LineSeparator line = new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -1);
                
                document.Add(line);
                PdfPTable nested = new PdfPTable(1);


                if (model != null)
                {
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase($"Patient Name : {model.PartyName}", headFont));
                    nextPostCell1.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    nextPostCell1.PaddingLeft = 10f;
                    nextPostCell1.PaddingTop = 10f;
                    nextPostCell1.BorderColor = BaseColor.BLACK;
                    nested.AddCell(nextPostCell1);

                    PdfPCell nextPostCell2 = new PdfPCell(new Phrase($"Policy Number : {model.PolicyNumber}", headFont));
                    nextPostCell2.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    nextPostCell2.PaddingLeft = 10f;
                    nextPostCell2.BorderColor = BaseColor.BLACK;
                    nested.AddCell(nextPostCell2);
                    PdfPCell nextPostCell3 = new PdfPCell(new Phrase($"DOB : {model.DOB.ToString("MM/dd/yyyy")}", headFont));
                    nextPostCell3.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;

                    nextPostCell3.PaddingLeft = 10f;
                    nextPostCell3.BorderColor = BaseColor.BLACK;
                    nested.AddCell(nextPostCell3);

                    PdfPCell nextPostCell4 = new PdfPCell(new Phrase($"Coverage Amount : {model.CoverageAmount.ToString()}", headFont));
                    nextPostCell4.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    nextPostCell4.PaddingLeft = 10f;
                    nextPostCell4.BorderColor = BaseColor.BLACK;
                    nextPostCell4.PaddingBottom = 10f;
                    nested.AddCell(nextPostCell4);

                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Rowspan = 4;
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.Padding = 0f;


                    headertable.AddCell(nesthousing);

                    nested = new PdfPTable(1);

                    nextPostCell1 = new PdfPCell(new Phrase($"ComputerWala.co.in insurance pvt. ltd.  \n \n Address : Post office udaipur \n PinCode : 313001", headFont));
                    nextPostCell1.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                    nextPostCell1.PaddingLeft = 10f;
                    nextPostCell1.PaddingTop = 10f;
                    nextPostCell1.BorderColor = BaseColor.BLACK;

                    nested.AddCell(nextPostCell1);
                    nesthousing = new PdfPCell(nested);
                    nesthousing.Rowspan = 4;
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.Padding = 0f;


                    headertable.AddCell("");
                    headertable.AddCell(nesthousing);

                    document.Add(headertable);
                    document.Add(line);
                    // Add content to the document
                    Paragraph paymenttitle = new Paragraph("Payment Details", titleFont);
                    paymenttitle.Alignment = Element.ALIGN_LEFT;
                    paymenttitle.SpacingAfter = 10;

                    document.Add(paymenttitle);
                    document.Add(line);
                    #endregion

                    #region Items Table
                    //Create body table
                    PdfPTable itemTable = new PdfPTable(4);
                    itemTable.SpacingBefore = 20;
                    itemTable.HorizontalAlignment = 0;
                    itemTable.WidthPercentage = 100;
                    itemTable.SetWidths(new float[] { 10, 40, 20, 30 });  // then set the column's __relative__ widths
                    itemTable.SpacingAfter = 40;
                    itemTable.DefaultCell.Border = Rectangle.BOX;



                    PdfPCell cell1 = new PdfPCell(new Phrase("Sr. No", boldTableFont));
                    cell1.HorizontalAlignment = 1;
                    cell1.Padding = 8f;
                    cell1.Border = Rectangle.BOX;
                    itemTable.AddCell(cell1);
                    PdfPCell cell2 = new PdfPCell(new Phrase("Description", boldTableFont));
                    cell2.HorizontalAlignment = 1;
                    cell2.Padding = 8f;
                    cell2.Border = Rectangle.BOX;
                    itemTable.AddCell(cell2);
                    PdfPCell cell3 = new PdfPCell(new Phrase("Payment Date", boldTableFont));
                    cell3.HorizontalAlignment = 1;
                    cell3.Padding = 8f;
                    cell3.Border = Rectangle.BOX;
                    itemTable.AddCell(cell3);
                    PdfPCell cell4 = new PdfPCell(new Phrase("AMOUNT(INR)", boldTableFont));
                    cell4.HorizontalAlignment = 1;
                    cell4.Padding = 8f;
                    cell4.Border = Rectangle.BOX;
                    itemTable.AddCell(cell4);

                    var index = 1;
                    foreach (var row in model.Items)
                    {
                        PdfPCell sr = new PdfPCell(new Phrase(index.ToString(), bodyFont));
                        sr.HorizontalAlignment = 1;
                        sr.Padding = 8f;
                        sr.Border = Rectangle.BOX;
                        itemTable.AddCell(sr);

                        PdfPCell desccell = new PdfPCell(new Phrase(row.Description, bodyFont));
                        desccell.HorizontalAlignment = 1;
                        desccell.Padding = 8f;
                        desccell.Border = Rectangle.BOX;
                        itemTable.AddCell(desccell);

                        PdfPCell datecell = new PdfPCell(new Phrase(row.PaymentDate.ToString("MMddyyyy"), bodyFont));
                        datecell.HorizontalAlignment = 1;
                        datecell.Padding = 8f;
                        datecell.Border = Rectangle.BOX;
                        itemTable.AddCell(datecell);

                        PdfPCell amount = new PdfPCell(new Phrase(row.amount.ToString(), bodyFont));
                        amount.HorizontalAlignment = 1;
                        amount.Padding = 8f;
                        amount.Border = Rectangle.BOX;
                        itemTable.AddCell(amount);

                        index++;

                    }



                    // Table footer
                    PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
                    totalAmtCell1.Border = Rectangle.LIST;
                    totalAmtCell1.Padding = 8f;
                    itemTable.AddCell(totalAmtCell1);
                    PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
                    totalAmtCell2.Border = Rectangle.LIST; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
                    totalAmtCell2.Padding = 8f;
                    itemTable.AddCell(totalAmtCell2);
                    PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Total Amount", boldTableFont));
                    totalAmtStrCell.Border = Rectangle.LIST;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
                    totalAmtStrCell.Padding = 8f;
                    totalAmtStrCell.HorizontalAlignment = 1;
                    itemTable.AddCell(totalAmtStrCell);
                    PdfPCell totalAmtCell = new PdfPCell(new Phrase(model.Items.Sum(x => x.amount).ToString("#,###.00"), boldTableFont));
                    totalAmtCell.HorizontalAlignment = 1;
                    totalAmtCell.Padding = 8f;
                    itemTable.AddCell(totalAmtCell);

                    document.Add(itemTable);
                    document.Add(line);
                    #endregion

                }


                document.Add(Chunk.NEWLINE);

                // Add content to the document
                Paragraph paymenttitle2 = new Paragraph("This is a computer generated copy.", FontFactory.GetFont("Arial", 13, Font.NORMAL,BaseColor.BLACK));
                paymenttitle2.Alignment = Element.ALIGN_LEFT;
                paymenttitle2.SpacingAfter = 10;
                   
                paymenttitle2.SpacingBefore = 50;
                document.Add(paymenttitle2);
                document.Add(line);


                writer.CloseStream = false; //set the closestream property
                                            // Close the Document without closing the underlying stream
                document.Close();


                var pdfData = PDFData.ToArray();
                return new FileContentResult(pdfData, "application/pdf")
                {
                    FileDownloadName = "InsuranceDetails.pdf",
                };

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool RemovePatient(string id)
        {
            var Removed = false;
            try
            {
                patients.Remove(patients.Where(x => x.Id.ToString() == id).FirstOrDefault());
                Removed = true;
                return Removed;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }

  
}
