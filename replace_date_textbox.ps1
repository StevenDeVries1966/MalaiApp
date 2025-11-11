$filePath = "AiWpf\Views\MainWindow.xaml"
$content = Get-Content $filePath -Raw

# Replace Date TextBox with DatePicker
$content = $content -replace '<TextBox Grid\.Row="7" Grid\.Column="1" Text="\{Binding date_display, Mode=OneWay\}" />', '<DatePicker Grid.Row="7" Grid.Column="1" SelectedDate="{Binding date_display, Mode=TwoWay}" Margin="0,3,0,3" Background="WhiteSmoke" VerticalAlignment="Center" />'

$content | Set-Content $filePath -NoNewline
Write-Host "Successfully replaced Date TextBox with DatePicker"
