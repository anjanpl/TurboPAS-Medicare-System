
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Hospital Stay Popup 
' Author: Anjan Kumar Paul
' Start Date: 28 Feb 2015
' End Date: 28 Feb 2015
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                28 Feb 2015      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings

Namespace Visitel.UserControl.PayPeriod

    Public Class HospitalizedIndividualsControl
        Inherits System.Web.UI.UserControl

        Private ValidationEnable As Boolean
        Private ValidationGroup As String

        Private HospitalStayValidationGroup As String

        Private GridViewPageSize As Integer = 100

        Private objShared As SharedWebControls

        Private CompanyId As Integer

        Private QueryStringCollection As NameValueCollection

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            QueryStringCollection = Request.QueryString
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewHospitalStay.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        Private Sub GridViewHospitalStay_RowDataBound(sender As Object, e As GridViewRowEventArgs)
            If (e.Row.RowType.Equals(DataControlRowType.DataRow)) Then
                objShared.SetGridViewRowColor(e.Row)
            End If
        End Sub

        Private Sub GridViewHospitalStay_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewHospitalStay.PageIndex = e.NewPageIndex
            BindHospitalStayGridView()
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            GridViewHospitalStay.AutoGenerateColumns = True
            GridViewHospitalStay.ShowHeaderWhenEmpty = True
            GridViewHospitalStay.AllowPaging = True
            GridViewHospitalStay.ShowFooter = False

            If (GridViewHospitalStay.AllowPaging) Then
                GridViewHospitalStay.PageSize = GridViewPageSize
            End If

            AddHandler GridViewHospitalStay.RowDataBound, AddressOf GridViewHospitalStay_RowDataBound
            AddHandler GridViewHospitalStay.PageIndexChanging, AddressOf GridViewHospitalStay_PageIndexChanging

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()

            Try
                BindHospitalStayGridView()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Fetch Data")
            End Try

        End Sub

        ''' <summary>
        ''' Data binding for Hospital Stay GridView
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindHospitalStayGridView()

            Dim objBLHospitalStaySetting As New BLHospitalStaySetting

            Try

                objBLHospitalStaySetting.SelectHospitalizedIndividuals(objShared.VisitelConnectionString, SqlDataSourceHospitalizedIndividuals, objShared.CompanyId,
                                                                       QueryStringCollection)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch hospitalized individuals")
            Finally
                objBLHospitalStaySetting = Nothing
            End Try

            GridViewHospitalStay.DataSource = SqlDataSourceHospitalizedIndividuals
            GridViewHospitalStay.DataBind()

        End Sub

    End Class
End Namespace