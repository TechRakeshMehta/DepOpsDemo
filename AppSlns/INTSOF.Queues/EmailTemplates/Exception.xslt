<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:mstns="http://tempuri.org/ADBEmails.xsd" exclude-result-prefixes="mstns">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="mstns:Exception" >
    <HTML>
      <style>
        body {font: normal 13px Arial,Helvetica,sans-serif;}
        table {font: normal 12px Arial,Helvetica,sans-serif; border:solid 1px black;}
        table td {padding:2; margin:0; border: solid 1px black;}
      </style>
        <body>
          <xsl:apply-templates select="mstns:Contents"/>
        </body>
    </HTML>
  </xsl:template>

  <xsl:template match="mstns:Contents">
    <table width='100%'>
    <b>TIMESTAMP:</b>&#160;<xsl:value-of select="mstns:TimeStamp"/><br />
    <b>QUERYSTRING:</b>&#160;<xsl:value-of select="mstns:QueryString"/><br />
    <b>FILEPATH:</b>&#160;<xsl:value-of select="mstns:FilePath"/><br />
    <b>Request.CurrentExecutionFilePath:</b>&#160; <xsl:value-of select="mstns:CurrentExecutionFilePath"/><br />
    <b>Request.ApplicationPath:</b>&#160;<xsl:value-of select="mstns:ApplicationPath"/><br />
    <b>Request.FilePath:&#160;</b><xsl:value-of select="mstns:Request.FilePath"/><br />
    <b>Request.Path:&#160;</b> <xsl:value-of select="mstns:Path"/><br /><br /><br />
    <b>TYPE:&#160;</b> <xsl:value-of select="mstns:TYPE"/><br />
    <b>MESSAGE: &#160;</b><strong><xsl:value-of select="mstns:MESSAGE"/></strong><br />
    <b>INNEREXCEPTION: &#160;</b><strong><xsl:value-of select="mstns:InnerException"/></strong><br />
    <b>SOURCE: &#160;</b><xsl:value-of select="mstns:SOURCE"/><br />
    <b>TARGETSITE:&#160;</b> <br /><xsl:value-of select="mstns:TARGETSITE"/><br />
    <b> STACKTRACE:&#160;</b> <br /><em><xsl:value-of select="mstns:STACKTRACE"/>
    </em>
    <br />
    <b>DATA:&#160;</b> <strong><xsl:value-of select="mstns:DATA"/></strong><br />
    
    <br />
    <br /><b>Context.Items:</b> <br />
    <xsl:value-of select="mstns:Context.Items" disable-output-escaping="yes"/>
    <br /><br />
    <b>Request.Form:&#160;</b><br /><xsl:value-of select="mstns:Request.Form" disable-output-escaping="yes"/>
    <br /><br />
    <b>Request.ServerVariables:&#160;</b><br />
    <xsl:value-of select="mstns:Request.ServerVariables" disable-output-escaping="yes"/>
    <br /><br />
    <b>Request.PathInfo:&#160;</b><xsl:value-of select="mstns:Request.PathInfo"/><br />
    <b> Request.PhysicalApplicationPath:&#160;</b><xsl:value-of select="mstns:Request.PhysicalApplicationPathh"/><br />
    <b>Request.PhysicalPath:&#160;</b> <xsl:value-of select="mstns:Request.PhysicalPath"/><br />
    <b>Request.RawUrl:&#160;</b><xsl:value-of select="mstns:Request.RawUrl"/><br />
    </table>
  </xsl:template>
  
 </xsl:stylesheet>
