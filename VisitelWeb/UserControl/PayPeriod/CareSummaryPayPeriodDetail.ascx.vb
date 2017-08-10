
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Care Summary Pay Period Detail
' Author: Anjan Kumar Paul
' Start Date: 14 Feb 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                14 Feb 2015     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient

Namespace Visitel.UserControl.PayPeriod
    Public Class CareSummaryPayPeriodDetailControl
        Inherits System.Web.UI.UserControl

        Private ControlName As String

        Private GridViewPageSize As Integer = 100
        Private QueryStringCollection As NameValueCollection
        Private TotalHour As Int32 = 0, TotalMinute As Int32 = 0

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            QueryStringCollection = Request.QueryString
            ControlName = "CareSummaryPayPeriodDetailControl"
            objShared = New SharedWebControls()

            objShared.ConnectionOpen()
            'Master.PageHeaderTitle = "Care SummaryPay Period Detail"
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            GridViewCareSummaryPayPeriodDetail.AutoGenerateColumns = True
            GridViewCareSummaryPayPeriodDetail.ShowHeaderWhenEmpty = True
            GridViewCareSummaryPayPeriodDetail.AllowPaging = True

            If (GridViewCareSummaryPayPeriodDetail.AllowPaging) Then
                GridViewCareSummaryPayPeriodDetail.PageSize = GridViewPageSize
            End If

            AddHandler GridViewCareSummaryPayPeriodDetail.RowDataBound, AddressOf GridViewCareSummaryPayPeriodDetail_RowDataBound
            AddHandler GridViewCareSummaryPayPeriodDetail.PageIndexChanging, AddressOf GridViewCareSummaryPayPeriodDetail_PageIndexChanging

        End Sub

        Private Sub GridViewCareSummaryPayPeriodDetail_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewCareSummaryPayPeriodDetail.PageIndex = e.NewPageIndex
            BindCareSummaryPayPeriodDetailGridView()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewCareSummaryPayPeriodDetail.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()

            Try
                BindCareSummaryPayPeriodDetailGridView()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch Data")
            End Try

        End Sub

        Private Sub GridViewCareSummaryPayPeriodDetail_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            If (e.Row.RowType.Equals(DataControlRowType.DataRow)) Then
                TotalHour = TotalHour + Convert.ToInt32(e.Row.Cells(4).Text.Split(":")(0))
                TotalMinute = TotalMinute + Convert.ToInt32(e.Row.Cells(4).Text.Split(":")(1))

                objShared.SetGridViewRowColor(e.Row)
            End If

            TextBoxTotalTime.Text = TotalHour & ":" & TotalMinute

        End Sub

        ''' <summary>
        ''' Data binding for Pay Period Detail GridView
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindCareSummaryPayPeriodDetailGridView()

            Dim objBLPayPeriodDetail As New BLPayPeriodDetail

            Try

                objBLPayPeriodDetail.SelectCareSummaryPayPeriod(objShared.VisitelConnectionString, SqlDataSourceCareSummaryPayPeriod, QueryStringCollection)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch care summary pay period detail")
            Finally
                objBLPayPeriodDetail = Nothing
            End Try

            GridViewCareSummaryPayPeriodDetail.DataSource = SqlDataSourceCareSummaryPayPeriod
            GridViewCareSummaryPayPeriodDetail.DataBind()

        End Sub
    End Class
End Namespace