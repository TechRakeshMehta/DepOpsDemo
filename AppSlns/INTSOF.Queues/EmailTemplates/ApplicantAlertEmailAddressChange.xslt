<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:ApplicantAlertEmailAddressChange" >
    <HTML>
      <BODY>
        <xsl:apply-templates select="mstns:Contents"/>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="mstns:Contents">
    <p>
      Dear <xsl:value-of select="mstns:FirstName"/>&#160;<xsl:value-of select="mstns:LastName"/>,
    </p>
    <p>
      This is to inform you that request has been placed for changing your current Primary Email Address.
    </p>
    
    <p>
      Thank you,<br/>
      <xsl:value-of select="mstns:TenantName"/>
    </p>

  </xsl:template>
</xsl:stylesheet>
