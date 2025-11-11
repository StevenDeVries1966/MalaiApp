$filePath = "AiWpf\Views\MainWindow.xaml"
$lines = Get-Content $filePath

$newLines = @()
for ($i = 0; $i -lt $lines.Count; $i++) {
    $line = $lines[$i]
    
    # Check if this is the Client Code TextBox line (Grid.Row="1")
    if ($line -match 'Grid\.Row="1"' -and $line -match 'Grid\.Column="1"' -and $line -match 'Text="\{Binding clt_code') {
        # Replace with ComboBox
        $indent = '     '
        $newLines += $indent + '<ComboBox Grid.Row="1" Grid.Column="1"'
        $newLines += $indent + '    SelectedValuePath="clt_code"'
    $newLines += $indent + '    DisplayMemberPath="clt_code"'
        $newLines += $indent + '    SelectedValue="{Binding clt_code, Mode=TwoWay}"'
        $newLines += $indent + '    ItemsSource="{Binding DataContext.Clients, RelativeSource={RelativeSource AncestorType=Window}}"'
    $newLines += $indent + '    Margin="0,3,0,3"'
        $newLines += $indent + '    Background="WhiteSmoke"'
$newLines += $indent + '    VerticalAlignment="Center"/>'
    }
    else {
        $newLines += $line
    }
}

$newLines | Set-Content $filePath
Write-Host "Successfully replaced Client Code TextBox with ComboBox"
