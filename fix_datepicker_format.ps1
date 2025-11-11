$filePath = "AiWpf\Views\MainWindow.xaml"
$content = Get-Content $filePath -Raw

# The simplest approach: Add Culture="en-SE" which uses yyyy-MM-dd format natively
$oldDatePicker = '<DatePicker Grid.Row="7" Grid.Column="1" SelectedDate="{Binding date_display, Mode=TwoWay}" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />'
$newDatePicker = '<DatePicker Grid.Row="7" Grid.Column="1" SelectedDate="{Binding date_display, Mode=TwoWay}" SelectedDateFormat="Short" Language="sv-SE" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />'

$content = $content.Replace($oldDatePicker, $newDatePicker)

$content | Set-Content $filePath -NoNewline
Write-Host "Successfully updated DatePicker to display yyyy-MM-dd format using Swedish culture"
