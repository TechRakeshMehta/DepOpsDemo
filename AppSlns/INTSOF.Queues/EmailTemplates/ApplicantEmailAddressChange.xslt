<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:ApplicantEmailAddressChange" >
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
      This is to confirm that your existing Primary Email Address  will be updated to <xsl:value-of select="mstns:NewEmailAddress"/> with <xsl:value-of select="mstns:TenantName"/> after verification.
    </p>

    <p>
      Please confirm your new Email Address by clicking on link <xsl:value-of select="mstns:LoginURL"/>.
    </p>

    <p>This is an automatically generated message. Please do not reply to this mail.</p>

    <p>
      Thank you,<br/>
      <xsl:value-of select="mstns:TenantName"/>
    </p>

  </xsl:template>
</xsl:stylesheet>
