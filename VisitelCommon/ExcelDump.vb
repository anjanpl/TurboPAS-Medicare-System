Imports System.Data

''' <summary>
''' This class holds all excel dump related methods.
''' </summary>
Public Class ExcelDump

    ''' <summary>
    ''' This will convert data table to excel format.
    ''' </summary>
    ''' <param name="oDataTable"></param>
    ''' <param name="rpt_name"></param>
    ''' <param name="rpt_date"></param>
    ''' <returns></returns>
    ''' 

    Public Function ConvertExcel(oDataTable As DataTable, rpt_name As String, rpt_Criteria As String) As String

        Dim oStringBuilder As New System.Text.StringBuilder()


        Try
        
            '*******************************************************
            '            * Start, check for border width
            '            * *****************************************************

            Dim borderWidth As Integer = 0
            If _ShowExcelTableBorder Then
                borderWidth = 1
            End If
            '*******************************************************
            '            * End, Check for border width
            '            * *****************************************************

            '*******************************************************
            '            * Start, Check for bold heading
            '            * *****************************************************


            Dim boldTagStart As String = ""
            Dim boldTagEnd As String = ""
            If _ExcelHeaderBold Then
                boldTagStart = "<B>"
                boldTagEnd = "</B>"
            End If

            '------------------------------------------For Printing Date-----------------------------
            Dim print_date As New DateTime()
            print_date = DateTime.Today

            Dim _print_date As String = print_date.ToString("dd-MMM-yyyy")
            '----------------------------------------------------------------------------------------

            'string _print_date = rpt_date;

            '-----------------------------------------------------------------------------

            '******************************************************************
            '            * Start, Creating table header
            '            * ****************************************************************


            'oStringBuilder.Append("<TR align=center style=width:100%;>");

            'oStringBuilder.Append("<img alt=" + "''");
            'oStringBuilder.Append("src=~/Images/logo_for_report_1.JPG" + "/>");


            oStringBuilder.Append("<BR/>")
            oStringBuilder.Append("<style>.text { mso-number-format:\@; } </style>")

            oStringBuilder.Append("<Table style='width: 100%' border='0'>")

            oStringBuilder.Append("<TR align=center style='width:100%;font-weight:bold;'>")

            oStringBuilder.Append(Convert.ToString(boldTagStart & rpt_name, Nothing) & boldTagEnd)

            oStringBuilder.Append("</TR>")
            oStringBuilder.Append("<BR/>")

            oStringBuilder.Append("<TR align='center' style='width:100%;font-weight:bold;'>")

            oStringBuilder.Append(Convert.ToString("Report Generation Date: ", Nothing) & _print_date)

            oStringBuilder.Append("</TR>")
            oStringBuilder.Append("<BR/>")

            If rpt_Criteria.Length > 0 Then
                oStringBuilder.Append("<TR align='center' style='width:100%;'>")

                oStringBuilder.Append(rpt_Criteria)


                ' oStringBuilder.Append("<BR/>");
                oStringBuilder.Append("</TR>")
            End If

            oStringBuilder.Append("</Table>")

            '------------------------------------------------------------------------------------          

            '*******************************************************
            '            * End,Check for bold heading
            '            * *****************************************************

            oStringBuilder.Append("<Table border=" & Convert.ToString(borderWidth) & ">")
            'oStringBuilder.Append("<Table border=0 >");
            '******************************************************************
            '            * Start, Creating table header
            '            * ****************************************************************

            oStringBuilder.Append("<TR>")

            For Each oDataColumn As DataColumn In oDataTable.Columns

                oStringBuilder.Append((Convert.ToString((Convert.ToString("<TD>", Nothing) & boldTagStart) + oDataColumn.ColumnName, Nothing) & boldTagEnd) + "</TD>")
            Next

            oStringBuilder.Append("</TR>")
            '******************************************************************
            '            * End, Creating table header
            '            * ****************************************************************

            '******************************************************************
            '            * Start, Creating rows
            '            * ****************************************************************


            For Each oDataRow As DataRow In oDataTable.Rows
                oStringBuilder.Append("<TR>")
                For Each oDataColumn As DataColumn In oDataTable.Columns
                    If oDataColumn.ColumnName.Equals("Account Number") _
                            OrElse oDataColumn.ColumnName.Equals("accountnumber") _
                            OrElse oDataColumn.ColumnName.Equals("OtherBankAccountNo") Then
                        oStringBuilder.Append("<TD class='text'>" + Convert.ToString(oDataRow(oDataColumn.ColumnName), Nothing) + "</TD>")
                    Else
                        oStringBuilder.Append("<TD class='text'>" + Convert.ToString(oDataRow(oDataColumn.ColumnName), Nothing) + "</TD>")

                    End If
                Next
                oStringBuilder.Append("</TR>")
            Next

            '******************************************************************
            '            * End, Creating rows
            '            * ****************************************************************

            'SW.WriteLine(oStringBuilder.ToString());
            'SW.Close();
            oStringBuilder.Append("</Table>")
        Catch ex As Exception
        Finally
        End Try
        Return Convert.ToString(oStringBuilder, Nothing)
    End Function

    Private _ShowExcelTableBorder As Boolean = True

    ''' <summary>
    ''' To show or hide the excel table border
    ''' </summary>
    Public Property ShowExcelTableBorder() As Boolean
        Get
            Return _ShowExcelTableBorder
        End Get
        Set(value As Boolean)
            _ShowExcelTableBorder = value
        End Set
    End Property

    Private _ExcelHeaderBold As Boolean = True


    Public Property ExcelHeaderBold() As Boolean
        Get
            Return _ExcelHeaderBold
        End Get
        Set(value As Boolean)
            ExcelHeaderBold = value
        End Set
    End Property

End Class