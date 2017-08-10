Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Web.Services

Namespace Visitel.UserControl.ClientInfo

    Public Class BillingDiagnosisCodeControl
        Inherits BaseUserControl

        Private ControlName As String, InvalidUnitRateMessage As String, ValidationGroup As String

        Private ValidationEnable As Boolean

        Public TextBoxDiagnosis As TextBox

        Private objShared As SharedWebControls

        Public DiagnosisCodeList As New List(Of DiagnosisDataObject)()
        Public DiagnosisDescriptionList As New List(Of DiagnosisDataObject)()


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "BillingDiagnosisCodeControl"
            objShared = New SharedWebControls()
            GetCaptionFromResource()

            objShared.ConnectionOpen()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If (Not IsPostBack) Then
                GetData()
            End If
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
            LoadJS("JavaScript/ClientInfo/" & ControlName & ".js")
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            DropDownListBillingDiagnosisOne.AutoPostBack = objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeOne.AutoPostBack,
                                                        objShared.InlineAssignHelper(DropDownListBillingDiagnosisTwo.AutoPostBack,
                                                        objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeTwo.AutoPostBack,
                                                        objShared.InlineAssignHelper(DropDownListBillingDiagnosisThree.AutoPostBack,
                                                        objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeThree.AutoPostBack,
                                                        objShared.InlineAssignHelper(DropDownListBillingDiagnosisFour.AutoPostBack,
                                                        objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeFour.AutoPostBack,
                                                        True)))))))

            AddHandler DropDownListBillingDiagnosisOne.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisOne_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisCodeOne.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisCodeOne_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisTwo.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisTwo_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisCodeTwo.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisCodeTwo_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisThree.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisThree_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisCodeThree.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisCodeThree_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisFour.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisFour_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagnosisCodeFour.SelectedIndexChanged, AddressOf DropDownListBillingDiagnosisCodeFour_OnSelectedIndexChanged

            DropDownListBillingDiagnosisOne.ClientIDMode = objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeOne.ClientIDMode,
                                                       objShared.InlineAssignHelper(DropDownListBillingDiagnosisTwo.ClientIDMode,
                                                       objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeTwo.ClientIDMode,
                                                       objShared.InlineAssignHelper(DropDownListBillingDiagnosisThree.ClientIDMode,
                                                       objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeThree.ClientIDMode,
                                                       objShared.InlineAssignHelper(DropDownListBillingDiagnosisFour.ClientIDMode,
                                                       objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeFour.ClientIDMode,
                                                       UI.ClientIDMode.Static)))))))


            SetControlTabIndex()

        End Sub

        Private Sub SetControlTabIndex()
            DropDownListBillingDiagnosisOne.TabIndex = 44
            DropDownListBillingDiagnosisTwo.TabIndex = 45
            DropDownListBillingDiagnosisThree.TabIndex = 46
            DropDownListBillingDiagnosisFour.TabIndex = 47

            DropDownListBillingDiagnosisCodeOne.TabIndex = 48
            DropDownListBillingDiagnosisCodeTwo.TabIndex = 49
            DropDownListBillingDiagnosisCodeThree.TabIndex = 50
            DropDownListBillingDiagnosisCodeFour.TabIndex = 51
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            LabelBillingDiagnosisCode.Text = Convert.ToString(ResourceTable("LabelBillingDiagnosisCode"), Nothing).Trim()
            LabelBillingDiagnosisCode.Text = If(String.IsNullOrEmpty(LabelBillingDiagnosisCode.Text), "Billing Diagnosis Code", LabelBillingDiagnosisCode.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Binding Diagnosis Drop Down Lists
        ''' </summary>
        Private Sub BindDiagnosisDropDownLists()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetDiagnosis(objShared.ConVisitel, objShared.CompanyId, DiagnosisCodeList)
            objBLClientInfo = Nothing

            ' Create new stopwatch
            'Dim stopWatch As New Stopwatch()

            ' Begin timing
            'stopWatch.Start()

            DiagnosisCodeList = DiagnosisCodeList.OrderBy(Function(x) x.DiagnosisCode).ToList()
            DiagnosisDescriptionList = DiagnosisCodeList.OrderBy(Function(x) x.DiagnosisDescription).ToList()

            DropDownListBillingDiagnosisCodeOne.DataSource = DiagnosisCodeList
            DropDownListBillingDiagnosisCodeOne.DataTextField = "DiagnosisCode"
            DropDownListBillingDiagnosisCodeOne.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisCodeOne.DataBind()

            DropDownListBillingDiagnosisCodeOne.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisOne.DataSource = DiagnosisDescriptionList
            DropDownListBillingDiagnosisOne.DataTextField = "DiagnosisDescription"
            DropDownListBillingDiagnosisOne.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisOne.DataBind()

            DropDownListBillingDiagnosisOne.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisCodeTwo.DataSource = DiagnosisCodeList
            DropDownListBillingDiagnosisCodeTwo.DataTextField = "DiagnosisCode"
            DropDownListBillingDiagnosisCodeTwo.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisCodeTwo.DataBind()

            DropDownListBillingDiagnosisCodeTwo.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisTwo.DataSource = DiagnosisDescriptionList
            DropDownListBillingDiagnosisTwo.DataTextField = "DiagnosisDescription"
            DropDownListBillingDiagnosisTwo.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisTwo.DataBind()

            DropDownListBillingDiagnosisTwo.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisCodeThree.DataSource = DiagnosisCodeList
            DropDownListBillingDiagnosisCodeThree.DataTextField = "DiagnosisCode"
            DropDownListBillingDiagnosisCodeThree.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisCodeThree.DataBind()

            DropDownListBillingDiagnosisCodeThree.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisThree.DataSource = DiagnosisDescriptionList
            DropDownListBillingDiagnosisThree.DataTextField = "DiagnosisDescription"
            DropDownListBillingDiagnosisThree.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisThree.DataBind()

            DropDownListBillingDiagnosisThree.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisCodeFour.DataSource = DiagnosisCodeList
            DropDownListBillingDiagnosisCodeFour.DataTextField = "DiagnosisCode"
            DropDownListBillingDiagnosisCodeFour.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisCodeFour.DataBind()

            DropDownListBillingDiagnosisCodeFour.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListBillingDiagnosisFour.DataSource = DiagnosisDescriptionList
            DropDownListBillingDiagnosisFour.DataTextField = "DiagnosisDescription"
            DropDownListBillingDiagnosisFour.DataValueField = "DiagnosisId"
            DropDownListBillingDiagnosisFour.DataBind()

            DropDownListBillingDiagnosisFour.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DiagnosisCodeList = Nothing
            DiagnosisDescriptionList = Nothing

            'Threading.Thread.Sleep(500)

            ' Stop timing
            'stopWatch.Stop()

            'Dim ts As TimeSpan
            'ts = stopWatch.Elapsed

            'Label1.Text = Label1.Text & String.Format("Hour{0:00}:Minute{1:00}:Seconds{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)


        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            DropDownListBillingDiagnosisOne.SelectedIndex = objShared.InlineAssignHelper(DropDownListBillingDiagnosisTwo.SelectedIndex,
                                                             objShared.InlineAssignHelper(DropDownListBillingDiagnosisThree.SelectedIndex,
                                                             objShared.InlineAssignHelper(DropDownListBillingDiagnosisFour.SelectedIndex, 0)))

            DropDownListBillingDiagnosisCodeOne.SelectedIndex = objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeTwo.SelectedIndex,
                                                                 objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeThree.SelectedIndex,
                                                                 objShared.InlineAssignHelper(DropDownListBillingDiagnosisCodeFour.SelectedIndex, 0)))
        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetData()
            BindDiagnosisDropDownLists()
        End Sub

        Public Sub DataFoSave(ByRef objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject)
            objClientCaseBillingInfoDataObject.DiagnosisCodeOne = Convert.ToString(DropDownListBillingDiagnosisCodeOne.SelectedItem, Nothing)
            objClientCaseBillingInfoDataObject.DiagnosisCodeTwo = Convert.ToString(DropDownListBillingDiagnosisCodeTwo.SelectedItem, Nothing)
            objClientCaseBillingInfoDataObject.DiagnosisCodeThree = Convert.ToString(DropDownListBillingDiagnosisCodeThree.SelectedItem, Nothing)
            objClientCaseBillingInfoDataObject.DiagnosisCodeFour = Convert.ToString(DropDownListBillingDiagnosisCodeFour.SelectedItem, Nothing)
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseBillingInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject)
            DropDownListBillingDiagnosisCodeOne.SelectedIndex = DropDownListBillingDiagnosisCodeOne.Items.IndexOf(
                        DropDownListBillingDiagnosisCodeOne.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeOne, Nothing)))
            DropDownListBillingDiagnosisCodeOne_OnSelectedIndexChanged(DropDownListBillingDiagnosisCodeOne, Nothing)

            DropDownListBillingDiagnosisCodeTwo.SelectedIndex = DropDownListBillingDiagnosisCodeTwo.Items.IndexOf(
                DropDownListBillingDiagnosisCodeTwo.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeTwo, Nothing)))
            DropDownListBillingDiagnosisCodeTwo_OnSelectedIndexChanged(DropDownListBillingDiagnosisCodeTwo, Nothing)

            DropDownListBillingDiagnosisCodeThree.SelectedIndex = DropDownListBillingDiagnosisCodeThree.Items.IndexOf(
                DropDownListBillingDiagnosisCodeThree.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeThree, Nothing)))
            DropDownListBillingDiagnosisCodeThree_OnSelectedIndexChanged(DropDownListBillingDiagnosisCodeThree, Nothing)

            DropDownListBillingDiagnosisCodeFour.SelectedIndex = DropDownListBillingDiagnosisCodeFour.Items.IndexOf(
                DropDownListBillingDiagnosisCodeFour.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeFour, Nothing)))
            DropDownListBillingDiagnosisCodeFour_OnSelectedIndexChanged(DropDownListBillingDiagnosisCodeFour, Nothing)
        End Sub

        Private Sub DropDownListBillingDiagnosisOne_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisCodeOne.SelectedIndex = DropDownListBillingDiagnosisCodeOne.Items.IndexOf(
                DropDownListBillingDiagnosisCodeOne.Items.FindByValue(Convert.ToString(DropDownListBillingDiagnosisOne.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisCodeOne_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisOne.SelectedIndex = DropDownListBillingDiagnosisOne.Items.IndexOf(DropDownListBillingDiagnosisOne.Items.FindByValue(
                Convert.ToString(DropDownListBillingDiagnosisCodeOne.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisTwo_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisCodeTwo.SelectedIndex = DropDownListBillingDiagnosisCodeTwo.Items.IndexOf(DropDownListBillingDiagnosisCodeTwo.Items.FindByValue(
                                                                 Convert.ToString(DropDownListBillingDiagnosisTwo.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisCodeTwo_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisTwo.SelectedIndex = DropDownListBillingDiagnosisTwo.Items.IndexOf(DropDownListBillingDiagnosisTwo.Items.FindByValue(
                                                             Convert.ToString(DropDownListBillingDiagnosisCodeTwo.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisThree_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisCodeThree.SelectedIndex = DropDownListBillingDiagnosisCodeThree.Items.IndexOf(
                DropDownListBillingDiagnosisCodeThree.Items.FindByValue(Convert.ToString(DropDownListBillingDiagnosisThree.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisCodeThree_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisThree.SelectedIndex = DropDownListBillingDiagnosisThree.Items.IndexOf(DropDownListBillingDiagnosisThree.Items.FindByValue(
                                                               Convert.ToString(DropDownListBillingDiagnosisCodeThree.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisFour_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisCodeFour.SelectedIndex = DropDownListBillingDiagnosisCodeFour.Items.IndexOf(
                DropDownListBillingDiagnosisCodeFour.Items.FindByValue(Convert.ToString(DropDownListBillingDiagnosisFour.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagnosisCodeFour_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagnosisFour.SelectedIndex = DropDownListBillingDiagnosisFour.Items.IndexOf(DropDownListBillingDiagnosisFour.Items.FindByValue(
                                                              Convert.ToString(DropDownListBillingDiagnosisCodeFour.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        ''' <summary>
        ''' This is for filling out diagnosis information
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillDiagnosis()

            TextBoxDiagnosis.Text = String.Empty

            If (Not Convert.ToString(DropDownListBillingDiagnosisOne.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= Convert.ToString(DropDownListBillingDiagnosisOne.SelectedItem, Nothing)
            End If

            If (Not Convert.ToString(DropDownListBillingDiagnosisTwo.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= If(String.IsNullOrEmpty(TextBoxDiagnosis.Text.Trim()),
                                        (Convert.ToString(DropDownListBillingDiagnosisTwo.SelectedItem, Nothing)),
                                        ("," & Environment.NewLine & Convert.ToString(DropDownListBillingDiagnosisTwo.SelectedItem, Nothing)))
            End If

            If (Not Convert.ToString(DropDownListBillingDiagnosisThree.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= If(String.IsNullOrEmpty(TextBoxDiagnosis.Text.Trim()),
                                        (Convert.ToString(DropDownListBillingDiagnosisThree.SelectedItem, Nothing)),
                                        ("," & Environment.NewLine & Convert.ToString(DropDownListBillingDiagnosisThree.SelectedItem, Nothing)))
            End If

            If (Not Convert.ToString(DropDownListBillingDiagnosisFour.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= If(String.IsNullOrEmpty(TextBoxDiagnosis.Text.Trim()),
                                        (Convert.ToString(DropDownListBillingDiagnosisFour.SelectedItem, Nothing)),
                                        ("," & Environment.NewLine & Convert.ToString(DropDownListBillingDiagnosisFour.SelectedItem, Nothing)))
            End If

        End Sub

    End Class
End Namespace
