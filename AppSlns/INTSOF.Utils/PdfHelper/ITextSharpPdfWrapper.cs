using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace INTSOF.Utils.PdfHelper
{
    public class iTextSharpPDFWrapper
    {
        public byte[] DigitallySignPDFDocument(byte[] pdfDocumentDataToBeSigned, byte[] imageToAddToDocument,
            string serverMapPathDigitalCertificate, string signerName, string institutionWebSite)
        {
            byte[] signedDocument = null;

            try
            {
                string alias = null;
                Pkcs12Store pk12 = new Pkcs12Store(new FileStream(serverMapPathDigitalCertificate,
                    FileMode.Open, FileAccess.Read), "".ToCharArray());
                IEnumerator i = pk12.Aliases.GetEnumerator();
                while (i.MoveNext())
                {
                    alias = ((string)i.Current);
                    if (pk12.IsKeyEntry(alias))
                        break;
                }
                AsymmetricKeyParameter akp = pk12.GetKey(alias).Key;
                X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
                X509Certificate[] chain = new X509Certificate[ce.Length];
                for (int k = 0; k < ce.Length; ++k)
                    chain[k] = ce[k].Certificate;
                PdfReader reader = new PdfReader(pdfDocumentDataToBeSigned);
                MemoryStream ms = new MemoryStream();
                PdfStamper stamper = PdfStamper.CreateSignature(reader, ms, '\0');
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;

                //Setup signature
                AcroFields af = stamper.AcroFields;
                iTextSharp.text.pdf.AcroFields.FieldPosition signatureImagePosition = af.GetFieldPositions("SignatureImage")[0];
                left = signatureImagePosition.position.Left;
                right = signatureImagePosition.position.Right;
                top = signatureImagePosition.position.Top;
                heigth = signatureImagePosition.position.Height;
                iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                //Need to fix this for digitial signing with version 5.3 of itextsharp or greater
                //appearance.SetCrypto(akp, chain, null, PdfSignatureAppearance.WINCER_SIGNED);
                appearance.SignDate = DateTime.Now;
                appearance.Contact = signerName;
                appearance.Acro6Layers = true;
                appearance.Reason = signerName + " has signed this Disclosure and Release form.";
                appearance.SignatureGraphic = signatureImage;
                appearance.SignatureGraphic.Alignment = Element.ALIGN_LEFT;
                //Need to fix this for digitial signing with version 5.3 of itextsharp or greater
                //appearance.Render = PdfSignatureAppearance.SignatureRender.GraphicAndDescription;
                appearance.Location = "AMS - " + institutionWebSite;
                appearance.CertificationLevel = PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED;

                //apply the signature to the signature field
                appearance.SetVisibleSignature("UserSignature");
                PdfContentByte contentByte = stamper.GetOverContent(1);
                float currentImageHeigth = 0;
                currentImageHeigth = signatureImage.Height;
                float ratio = 0;
                ratio = heigth / currentImageHeigth;
                float width = signatureImage.Width * ratio;
                signatureImage.ScaleAbsoluteHeight(heigth);
                signatureImage.ScaleAbsoluteWidth(width);
                signatureImage.SetAbsolutePosition(left + ((right - left) - signatureImage.ScaledWidth) / 2, top - signatureImage.ScaledHeight);
                contentByte.AddImage(signatureImage);
                stamper.Close();
                signedDocument = ms.ToArray();
                ms.Close();
            }
            catch { }
            //catch (DocumentException de)
            //{
            //    //Catch 
            //    //Error
            //}
            //catch (IOException ioe)
            //{
            //    //Error
            //}
            return signedDocument;
        }

        public byte[] MergePDFDocuments(byte[] pdfDoc, byte[] mergeDoc)
        {
            MemoryStream pdfDocMS = new MemoryStream();
            PdfConcatenate concat = new PdfConcatenate(pdfDocMS);
            PdfReader pdfDocReader = new PdfReader(pdfDoc);
            concat.AddPages(pdfDocReader);
            byte[] mergedDocument;

            PdfReader mergeReader = new PdfReader(mergeDoc);

            concat.AddPages(mergeReader);

            mergeReader.Close();
            pdfDocReader.Close();
            concat.Close();

            mergedDocument = pdfDocMS.ToArray();
            //Recompress final document to further shrink.
            mergedDocument = CompressPDFDocument(mergedDocument);
            return mergedDocument;
        }

        public byte[] FillInAMSPDFDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument, PDFOrderData pdfOData, List<DocumentPDFMapper> docPDFMapper)
        {
            byte[] signedDocument = null;

            try
            {
                PdfReader reader = new PdfReader(pdfDocumentDataToBeFilledIn);
                MemoryStream ms = new MemoryStream();
                PdfStamper stamper = new PdfStamper(reader, ms);
                AcroFields.FieldPosition signatureImagePosition = null;

                //Fill-in the form values
                AcroFields af = stamper.AcroFields;
                Type pdfODataType = pdfOData.GetType();
                PropertyInfo theProperty = null;

                foreach (DocumentPDFMapper dPDFMapper in docPDFMapper)
                {
                    if (dPDFMapper.DocumentPDFOrderData != null)
                    {
                        theProperty = pdfODataType.GetProperty(dPDFMapper.DocumentPDFOrderData);
                    }
                    if (dPDFMapper.DefaultValue == null)
                    {
                        switch (dPDFMapper.DocumentField)
                        {
                            case "SocialSecurityNumber":
                                af.SetField(dPDFMapper.DocumentField, "XXX-XX-" + Convert.ToString(theProperty.GetValue(pdfOData, null)));
                                break;
                            case "DateSigned":
                                af.SetField(dPDFMapper.DocumentField, DateTime.Now.ToString());
                                break;
                            case "Date":
                                af.SetField(dPDFMapper.DocumentField, DateTime.Now.ToString());
                                break;
                            case "DateDigitallySigned":
                                af.SetField(dPDFMapper.DocumentField, "Date Signed: " + DateTime.Now.ToString());
                                break;
                            case "Location":
                                af.SetField(dPDFMapper.DocumentField, "AMS - " + Convert.ToString(theProperty.GetValue(pdfOData, null)));
                                break;
                            case "DateofBirth":
                                af.SetField(dPDFMapper.DocumentField, "AMS - " + Convert.ToDateTime(theProperty.GetValue(pdfOData, null)).ToShortDateString());
                                break;
                            default:
                                af.SetField(dPDFMapper.DocumentField, Convert.ToString(theProperty.GetValue(pdfOData, null)));
                                break;
                        }
                    }
                    else
                    {
                        af.SetField(dPDFMapper.DocumentField, Convert.ToString(dPDFMapper.DefaultValue));
                    }
                }

                bool bCA = (pdfOData.CurrentState == "CALIFORNIA");
                bool bNY = (pdfOData.CurrentState == "NEW YORK");
                bool bMNOK = (pdfOData.CurrentState == "MINNESOTA" || pdfOData.CurrentState == "OKLAHOMA");

                af.SetFieldProperty("CAResidenceOnly", "setflags", (bCA ? PdfFormField.FLAGS_PRINT : PdfFormField.FLAGS_HIDDEN), null);
                af.SetFieldProperty("NYResidenceOnly", "setflags", (bNY ? PdfFormField.FLAGS_PRINT : PdfFormField.FLAGS_HIDDEN), null);
                af.SetFieldProperty("MNOKResidenceOnly", "setflags", (bMNOK ? PdfFormField.FLAGS_PRINT : PdfFormField.FLAGS_HIDDEN), null);

                af.SetField("CopyRequestedCheckbox", (pdfOData.CopyRequestedInd ? "Yes" : "No"));
                af.SetFieldProperty("CopyRequestedCheckboxText", "setflags", (bCA || bNY || bMNOK ? PdfFormField.FLAGS_PRINT : PdfFormField.FLAGS_HIDDEN), null);
                af.SetFieldProperty("CopyRequestedCheckbox", "setflags", (bCA || bNY || bMNOK ? PdfFormField.FLAGS_PRINT : PdfFormField.FLAGS_HIDDEN), null);

                stamper.FormFlattening = true;
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;

                //Setup signature
                try { signatureImagePosition = af.GetFieldPositions("SignatureImage")[0]; }
                catch { }

                if (signatureImagePosition != null && imageToAddToDocument != null)
                {
                    left = signatureImagePosition.position.Left;
                    right = signatureImagePosition.position.Right;
                    top = signatureImagePosition.position.Top;
                    heigth = signatureImagePosition.position.Height;
                    iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                    PdfContentByte contentByte = stamper.GetOverContent(1);
                    float currentImageHeigth = 0;
                    currentImageHeigth = signatureImage.Height;
                    float ratio = 0;
                    ratio = heigth / currentImageHeigth;
                    float width = signatureImage.Width * ratio;
                    signatureImage.ScaleAbsoluteHeight(heigth);
                    signatureImage.ScaleAbsoluteWidth(width);
                    signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                    contentByte.AddImage(signatureImage);
                }

                stamper.Close();
                signedDocument = ms.ToArray();
                ms.Close();

                //Recompress final document to further shrink.
                signedDocument = CompressPDFDocument(signedDocument);
            }
            catch { }
            //catch (DocumentException de)
            //{
            //    //Error
            //}
            //catch (IOException ioe)
            //{
            //    //Error
            //}
            return signedDocument;
        }

        public static byte[] CompressPDFDocument(byte[] signedDocument)
        {
            PdfReader compressionReader = new PdfReader(signedDocument);
            MemoryStream compressionsMS = new MemoryStream();
            PdfStamper compressionStamper = new PdfStamper(compressionReader, compressionsMS);
            compressionStamper.FormFlattening = true;
            compressionStamper.SetFullCompression();
            compressionStamper.Close();
            signedDocument = compressionsMS.ToArray();
            compressionsMS.Close();
            compressionReader.Close();
            return signedDocument;
        }
    }
}
