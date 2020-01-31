<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:VerificationCodeForEmailChange" >
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
      A request has been made to change your Primary Email Address.
    </p>

    <p>
      To change your Primary Email address, you need to submit following verification code in order to verify that the request was legitimate.
    </p>

    <p>
      Your verification code is: <xsl:value-of select="mstns:VerificationCode"/>.
    </p>

    <p>This is an automatically generated message. Please do not reply to this mail.</p>

    <p>
      Thank you,<br/>
      <xsl:value-of select="mstns:TenantName"/>
    </p>

  </xsl:template>
</xsl:stylesheet>
