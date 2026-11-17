<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>

  <xsl:template match="/Archive">
    <html>
      <head>
        <meta charset="utf-8"/>
        <title>Електронний архів факультету</title>
        <style>
          body { font-family: Arial, sans-serif; margin: 24px; }
          table { border-collapse: collapse; width: 100%; }
          th, td { border: 1px solid #999; padding: 6px 10px; text-align: left; }
          th { background: #e0e0e0; }
          tr:nth-child(even) td { background: #f5f5f5; }
        </style>
      </head>
      <body>
        <h1>Електронний архів факультету</h1>
        <p>Усього матеріалів: <xsl:value-of select="count(Material)"/></p>
        <table>
          <tr>
            <th>№</th>
            <th>П.І.П. автора</th>
            <th>Назва</th>
            <th>Факультет</th>
            <th>Кафедра</th>
            <th>Вид</th>
            <th>Обсяг</th>
            <th>Дата</th>
            <th>Кількість атрибутів</th>
          </tr>
          <xsl:for-each select="Material">
            <xsl:sort select="AuthorFullName"/>
            <tr>
              <td><xsl:value-of select="position()"/></td>
              <td><xsl:value-of select="AuthorFullName"/></td>
              <td><xsl:value-of select="Title"/></td>
              <td><xsl:value-of select="@Faculty"/></td>
              <td><xsl:value-of select="@Department"/></td>
              <td><xsl:value-of select="@MaterialType"/></td>
              <td><xsl:value-of select="@Volume"/></td>
              <td><xsl:value-of select="@CreationDate"/></td>
              <td><xsl:value-of select="count(@*)"/></td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
