<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:NewAccountForAdmins" >
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
      Welcome to American DataBank! <xsl:value-of select="mstns:TenantName"/>
      has implemented our <b>Complio</b> solution for tracking immunizations, collecting documents and approving supporting documentation.
    </p>
    <p>
      You are now registered as a Complio administrator.  Each administrator has his or her own account to ensure data is kept private and secure.  <b>To get started:</b>
    </p>
    <p style ="margin-left:40px">
      1) Go To http://carmike.dev<br/>

      2) Use your <b>Username</b> and <b>Temporary Password</b> to access your account.<br/>
      <br/>
      <p style ="margin-left:80px">
        Username:&#160;<xsl:value-of select="mstns:LoweredUserName"/><br/>
        Password:&#160;<xsl:value-of select="mstns:DefaultPassword"/><br/>
        <br/>
      </p>
    </p>
    <p style ="margin-left:40px">
      3) You will be instructed to <b>“create a new password.”</b>
    </p>

    <p>
      If you have not yet participated in a Complio training session, please look for a separate message regarding training.  Remember, help is always available – just use the Communication Center at the top left of each screen. Or, you can call us at (800) 200-0853.
    </p>

    <p>Thank you for choosing Complio,</p>

    American DataBank<br/>Account Support<br/>
    (800) 200-0853<br/>complio@americandatabank.com<br/><br/>
    <p>This is an automatically generated message. Replies are not monitored or answered.</p>
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
