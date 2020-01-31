<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:NewAccount" >
    <HTML>
      <BODY>
            <xsl:apply-templates select="mstns:Contents"/>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="mstns:Contents">
    <p>Dear <xsl:value-of select="mstns:FirstName"/>&#160;<xsl:value-of select="mstns:LastName"/>, </p>
    <p>This is to confirm that your account has been created with <xsl:value-of select="mstns:TenantName"/>.</p>
    <p>Following are your account details:</p>
    Username:&#160;<xsl:value-of select="mstns:UserName"/><br/>
    Password:&#160;<xsl:value-of select="mstns:UserPassword"/><br/>

    <p>You can login using these credentials after activating your account. Please click <xsl:value-of select="mstns:LoginURL"/> in order to activate your account.</p>

    <p>This is an automatically generated message. Please do not reply to this mail.</p>

    <p>
      Thank you,<br/>
      <xsl:value-of select="mstns:TenantName"/>      
    </p>
      
   </xsl:template>
</xsl:stylesheet>
