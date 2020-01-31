<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:BulkUploadResult" >
    <HTML>
      <BODY>
            <xsl:apply-templates select="mstns:Contents"/>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="mstns:Contents">
    <table>
      <TR>
        <TD>CLIENT ID</TD>
        <TD>LOAN NUMBER</TD>
        <TD>LINE OF BUSINESS SUCCESS</TD>
        <TD>SERVICE REQUEST ID</TD>
      </TR>
      <TR>
    <TD>
      <xsl:value-of select="mstns:ClientId"/>
    </TD>
    <TD>
      <xsl:value-of select="mstns:LoanNumber"/>
    </TD>
    <TD>
      <xsl:value-of select="mstns:BusinessSucess"/>
    </TD>
     <TD>
       <xsl:value-of select="mstns:ServiceRequestID"/>
     </TD>
  </TR>
    </table>
    <p>This is an automatically generated message. Replies are not monitored or answered..</p>
    <span style='font-family:"Courier New"'>
      The information contained in this
      message is proprietary and/or confidential. If you are not the intended
      recipient, please: (i) delete the message and all copies; (ii) do not disclose,
      distribute or use the message in any manner; and (iii) notify the sender
      immediately. In addition, please be aware that any message addressed to our
      domain is subject to archiving and review by persons other than the intended
      recipient. Thank you.
    </span>
  </xsl:template>
</xsl:stylesheet>
