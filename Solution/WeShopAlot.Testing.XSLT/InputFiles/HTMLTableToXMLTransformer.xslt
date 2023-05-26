<xsl:stylesheet version="2.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:strip-space elements="*"/>

  <xsl:template match="/table">
    <contrib>
      <xsl:for-each select="tbody/tr[position() > 1]">
        <person>
          <name>
            <xsl:value-of select="td[1]"/>
          </name>
          <surname>
            <xsl:value-of select="td[2]"/>
          </surname>
          <xsl:for-each select="tokenize(td[3], ',')">
            <number>
              <xsl:value-of select="."/>
            </number>
          </xsl:for-each>
        </person>
      </xsl:for-each>
    </contrib>
  </xsl:template>

</xsl:stylesheet>