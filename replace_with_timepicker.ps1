$filePath = "AiWpf\Views\MainWindow.xaml"
$content = Get-Content $filePath -Raw

# Replace Start Time DateTimePicker with TimePicker
$content = $content -replace '<xctk:DateTimePicker Grid\.Row="8" Grid\.Column="1" Value="\{Binding start_time, Mode=TwoWay\}" Format="Custom" FormatString="HH:mm" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />', '<xctk:TimePicker Grid.Row="8" Grid.Column="1" Value="{Binding start_time, Mode=TwoWay}" Format="Custom" FormatString="HH:mm" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />'

# Replace End Time DateTimePicker with TimePicker
$content = $content -replace '<xctk:DateTimePicker Grid\.Row="9" Grid\.Column="1" Value="\{Binding end_time, Mode=TwoWay\}" Format="Custom" FormatString="HH:mm" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />', '<xctk:TimePicker Grid.Row="9" Grid.Column="1" Value="{Binding end_time, Mode=TwoWay}" Format="Custom" FormatString="HH:mm" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />'

$content | Set-Content $filePath -NoNewline
Write-Host "Successfully replaced DateTimePicker with TimePicker for Start Time and End Time"
