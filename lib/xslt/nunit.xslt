<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:output method='html'/>

<xsl:template match="/">

<html>

<xsl:apply-templates/>

</html>

</xsl:template>

<xsl:template match="test-results">

<head>

<script type="text/javascript">

function ShowHideDetails(contentControlId)

{

var contentControl = document.getElementById(contentControlId);

if(contentControl)

{

if(contentControl.style.display == 'none')

{

contentControl.style.display = '';

}

else

{

contentControl.style.display = 'none';

}

}

}

</script>

</head>

<body>

<div id="uxHeader" style="font-weight:bold;">

Tests run:
<xsl:value-of select="@total"/>, Failures: <xsl:value-of select="@failures"/>, Not run: <xsl:value-of select="@not-run"/>, Time: <xsl:value-of select="test-suite/@time"/> seconds

</div>

<xsl:if test="//test-case[failure]">

<div style="font-weight:bold;margin-top:10px;border-top:1px solid black;font-size:14pt;">

Failures

</div>

<table>

<xsl:for-each select="//test-suite">

<xsl:if test="results/test-case[@success = 'False']">

<tr>

<td>

<xsl:value-of select="@name" />:

</td>

<td style="width:100%;">

<a href="#" >

<xsl:attribute name="onclick">

ShowHideDetails('uxContent
<xsl:value-of select="@name"/>');

</xsl:attribute>

<xsl:value-of select="count(results/test-case[@success = 'False'])" />

</a>

</td>

</tr>

<tr style="display:none;" >

<xsl:attribute name="id">uxContent<xsl:value-of select="@name"/></xsl:attribute>

<td colspan="2" >

<div style="margin-left:10px;border:1px solid black;width:900px;height:300px;overflow:auto;background-color:silver;">

<xsl:call-template name="failureTemplate" ></xsl:call-template>

</div>

</td>

</tr>

</xsl:if>

</xsl:for-each>

</table>

</xsl:if>

<xsl:if test="//test-case[@executed = 'False']">

<div style="font-weight:bold;margin-top:10px;border-top:1px solid black;font-size:14pt;">

Ignored Tests

</div>

<xsl:for-each select="//test-suite">

<xsl:if test="results/test-case[@executed = 'False']">

<div style="margin-top:5px;font-weight:bold;"><xsl:value-of select="@name" /></div>

<div style="margin-left:10px;">

<xsl:call-template name="ignoreTemplate"></xsl:call-template>

</div>

</xsl:if>

</xsl:for-each> 

</xsl:if>

</body>

</xsl:template>

<xsl:template match="results/test-case[failure]" name="failureTemplate">

<xsl:for-each select="results/test-case[@success = 'False']">

<pre>

<span style="font-weight:bold;font-size:12pt;"><xsl:value-of select="position()"/>)</span><xsl:value-of select="@name"/>:<br/>

<xsl:value-of select="child::node()/message"/>

<xsl:if test="failure"><xsl:value-of select="failure/stack-trace"/></xsl:if>

</pre>

</xsl:for-each>

</xsl:template>

<xsl:template match="results/test-case[reason]" name="ignoreTemplate">

<xsl:for-each select="results/test-case[@executed = 'False']">

<span style="font-weight:bold;font-size:12pt;">

<xsl:value-of select="position()"/>)

</span>

<xsl:value-of select="@name"/>

<div style="font-size:10pt;margin-left:25px;">Reason:

<xsl:choose><xsl:when test="string-length(child::node()/message)=0"> [not defined]</xsl:when>

<xsl:otherwise> "<xsl:value-of select="child::node()/message"/>"</xsl:otherwise>

</xsl:choose>

</div>

</xsl:for-each>

</xsl:template>

</xsl:stylesheet>
