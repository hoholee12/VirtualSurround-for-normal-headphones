<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기를 사용하여 수정하지 마십시오.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.VEFX = New System.Windows.Forms.TrackBar()
        Me.LOW_EQ = New System.Windows.Forms.TrackBar()
        Me.HIGH_EQ = New System.Windows.Forms.TrackBar()
        Me.FILTER = New System.Windows.Forms.TrackBar()
        Me.VOLUME = New System.Windows.Forms.TrackBar()
        Me.EFFECT_ON_OFF = New System.Windows.Forms.Button()
        Me.VEFX_CHANGE = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.EFFECTOR_TEXT = New System.Windows.Forms.TextBox()
        CType(Me.VEFX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LOW_EQ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HIGH_EQ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FILTER, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VOLUME, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'VEFX
        '
        Me.VEFX.LargeChange = 1
        Me.VEFX.Location = New System.Drawing.Point(168, 59)
        Me.VEFX.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.VEFX.Maximum = 6
        Me.VEFX.Name = "VEFX"
        Me.VEFX.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.VEFX.Size = New System.Drawing.Size(45, 120)
        Me.VEFX.TabIndex = 0
        Me.VEFX.Value = 3
        '
        'LOW_EQ
        '
        Me.LOW_EQ.LargeChange = 1
        Me.LOW_EQ.Location = New System.Drawing.Point(231, 59)
        Me.LOW_EQ.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.LOW_EQ.Maximum = 6
        Me.LOW_EQ.Name = "LOW_EQ"
        Me.LOW_EQ.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.LOW_EQ.Size = New System.Drawing.Size(45, 120)
        Me.LOW_EQ.TabIndex = 1
        Me.LOW_EQ.Value = 3
        '
        'HIGH_EQ
        '
        Me.HIGH_EQ.LargeChange = 1
        Me.HIGH_EQ.Location = New System.Drawing.Point(295, 59)
        Me.HIGH_EQ.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.HIGH_EQ.Maximum = 6
        Me.HIGH_EQ.Name = "HIGH_EQ"
        Me.HIGH_EQ.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.HIGH_EQ.Size = New System.Drawing.Size(45, 120)
        Me.HIGH_EQ.TabIndex = 2
        Me.HIGH_EQ.Value = 3
        '
        'FILTER
        '
        Me.FILTER.LargeChange = 1
        Me.FILTER.Location = New System.Drawing.Point(361, 59)
        Me.FILTER.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.FILTER.Maximum = 6
        Me.FILTER.Name = "FILTER"
        Me.FILTER.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.FILTER.Size = New System.Drawing.Size(45, 120)
        Me.FILTER.TabIndex = 3
        Me.FILTER.Value = 3
        '
        'VOLUME
        '
        Me.VOLUME.LargeChange = 1
        Me.VOLUME.Location = New System.Drawing.Point(426, 59)
        Me.VOLUME.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.VOLUME.Maximum = 6
        Me.VOLUME.Name = "VOLUME"
        Me.VOLUME.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.VOLUME.Size = New System.Drawing.Size(45, 120)
        Me.VOLUME.TabIndex = 4
        Me.VOLUME.Value = 3
        '
        'EFFECT_ON_OFF
        '
        Me.EFFECT_ON_OFF.Location = New System.Drawing.Point(60, 59)
        Me.EFFECT_ON_OFF.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.EFFECT_ON_OFF.Name = "EFFECT_ON_OFF"
        Me.EFFECT_ON_OFF.Size = New System.Drawing.Size(55, 45)
        Me.EFFECT_ON_OFF.TabIndex = 5
        Me.EFFECT_ON_OFF.UseVisualStyleBackColor = True
        '
        'VEFX_CHANGE
        '
        Me.VEFX_CHANGE.Location = New System.Drawing.Point(60, 158)
        Me.VEFX_CHANGE.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.VEFX_CHANGE.Name = "VEFX_CHANGE"
        Me.VEFX_CHANGE.Size = New System.Drawing.Size(55, 45)
        Me.VEFX_CHANGE.TabIndex = 6
        Me.VEFX_CHANGE.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Location = New System.Drawing.Point(60, 18)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(55, 14)
        Me.TextBox1.TabIndex = 7
        Me.TextBox1.Text = "effect"
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Location = New System.Drawing.Point(60, 38)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(55, 14)
        Me.TextBox2.TabIndex = 8
        Me.TextBox2.Text = "on/off"
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Location = New System.Drawing.Point(60, 118)
        Me.TextBox3.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(55, 14)
        Me.TextBox3.TabIndex = 9
        Me.TextBox3.Text = "VEFX"
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Location = New System.Drawing.Point(60, 138)
        Me.TextBox4.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(55, 14)
        Me.TextBox4.TabIndex = 10
        Me.TextBox4.Text = "change"
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox5.Location = New System.Drawing.Point(161, 38)
        Me.TextBox5.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(55, 14)
        Me.TextBox5.TabIndex = 11
        Me.TextBox5.Text = "VEFX"
        '
        'TextBox6
        '
        Me.TextBox6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox6.Location = New System.Drawing.Point(224, 38)
        Me.TextBox6.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(55, 14)
        Me.TextBox6.TabIndex = 12
        Me.TextBox6.Text = "low-EQ"
        '
        'TextBox7
        '
        Me.TextBox7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox7.Location = New System.Drawing.Point(288, 38)
        Me.TextBox7.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(55, 14)
        Me.TextBox7.TabIndex = 13
        Me.TextBox7.Text = "hi-EQ"
        '
        'TextBox8
        '
        Me.TextBox8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox8.Location = New System.Drawing.Point(354, 38)
        Me.TextBox8.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(55, 14)
        Me.TextBox8.TabIndex = 14
        Me.TextBox8.Text = "filter"
        '
        'TextBox9
        '
        Me.TextBox9.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox9.Location = New System.Drawing.Point(419, 38)
        Me.TextBox9.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(55, 14)
        Me.TextBox9.TabIndex = 15
        Me.TextBox9.Text = "volume"
        '
        'EFFECTOR_TEXT
        '
        Me.EFFECTOR_TEXT.Location = New System.Drawing.Point(207, 202)
        Me.EFFECTOR_TEXT.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.EFFECTOR_TEXT.Name = "EFFECTOR_TEXT"
        Me.EFFECTOR_TEXT.ReadOnly = True
        Me.EFFECTOR_TEXT.Size = New System.Drawing.Size(143, 21)
        Me.EFFECTOR_TEXT.TabIndex = 16
        Me.EFFECTOR_TEXT.Text = "EFFECTOR OFF"
        Me.EFFECTOR_TEXT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(533, 226)
        Me.Controls.Add(Me.EFFECTOR_TEXT)
        Me.Controls.Add(Me.TextBox9)
        Me.Controls.Add(Me.TextBox8)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.TextBox6)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.VEFX_CHANGE)
        Me.Controls.Add(Me.EFFECT_ON_OFF)
        Me.Controls.Add(Me.VOLUME)
        Me.Controls.Add(Me.FILTER)
        Me.Controls.Add(Me.HIGH_EQ)
        Me.Controls.Add(Me.LOW_EQ)
        Me.Controls.Add(Me.VEFX)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "Form1"
        Me.Text = "VEFX Slider"
        CType(Me.VEFX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LOW_EQ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HIGH_EQ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FILTER, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VOLUME, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VEFX As System.Windows.Forms.TrackBar
    Friend WithEvents LOW_EQ As System.Windows.Forms.TrackBar
    Friend WithEvents HIGH_EQ As System.Windows.Forms.TrackBar
    Friend WithEvents FILTER As System.Windows.Forms.TrackBar
    Friend WithEvents VOLUME As System.Windows.Forms.TrackBar
    Friend WithEvents EFFECT_ON_OFF As System.Windows.Forms.Button
    Friend WithEvents VEFX_CHANGE As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents EFFECTOR_TEXT As System.Windows.Forms.TextBox

End Class
