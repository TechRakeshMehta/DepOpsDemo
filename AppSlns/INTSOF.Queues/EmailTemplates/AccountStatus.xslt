﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:AccountStatus" >
    <HTML>
      <BODY>
            <xsl:apply-templates select="mstns:Contents"/>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="mstns:Contents">
    <p> Dear <xsl:value-of select="mstns:FirstName"/>&#160;<xsl:value-of select="mstns:LastName"/>,</p>
    Your ADB account has been <xsl:value-of select="mstns:Message"/>
    <p>This is an automatically generated message. Replies are not monitored or answered.</p>
      Thanks &amp; Regards<br/>ADB Support Group<br/><br/>
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
