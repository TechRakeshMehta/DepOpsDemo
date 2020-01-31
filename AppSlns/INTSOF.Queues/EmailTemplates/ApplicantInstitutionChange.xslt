<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:ApplicantInstitutionChange" >
    <HTML>
      <BODY>
            <xsl:apply-templates select="mstns:Contents"/>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="mstns:Contents">
  <p>Dear <xsl:value-of select="mstns:FirstName"/>&#160;<xsl:value-of select="mstns:LastName"/>, </p>
  <p>This is to confirm that your account has been successfully linked with  <xsl:value-of select="mstns:TenantName"/>.</p>
  <p> You can click <xsl:value-of select="mstns:LoginURL"/> and login using previous institution credentials.</p>
  <p>This is an automatically generated message. Please do not reply to this mail.</p>
  <p>Thank you,<br/>
      <xsl:value-of select="mstns:TenantName"/>
  </p>
   </xsl:template>
</xsl:stylesheet>
