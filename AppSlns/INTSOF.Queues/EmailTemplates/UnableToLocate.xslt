<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:UnableToLocate" >
    <HTML>
      <BODY>
        <xsl:apply-templates select="mstns:Contents"/>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="mstns:Contents">
    <p>
      The property has been located for the
      Service Request ID:&#160;<xsl:value-of select="mstns:ServiceRequestId"/>

    </p>
    <p>
      Updated information is listed below:
      Decision Description:&#160;<xsl:value-of select="mstns:DecisionDescription"/>,
      Property Address 1:&#160;<xsl:value-of select="mstns:Address1"/>,
      Property Address 2:&#160;<xsl:value-of select="mstns:Address2"/>,
      City:&#160;<xsl:value-of select="mstns:city"/>,
      State:&#160;<xsl:value-of select="mstns:statename"/>,
      Zip:&#160;<xsl:value-of select="mstns:zipCode"/>,
      County:&#160;<xsl:value-of select="mstns:country"/>
    </p>
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
