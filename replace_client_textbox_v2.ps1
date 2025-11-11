$filePath = "AiWpf\Views\MainWindow.xaml"
$lines = @(Get-Content $filePath)
$newLines = [System.Collections.ArrayList]::new()

for ($i = 0; $i -lt $lines.Count; $i++) {
    $line = $lines[$i]
    
    # Look for the exact line with Client Code TextBox in details panel (Grid.Row="1")
    if ($line.Trim() -eq '<TextBox Grid.Row="1" Grid.Column="1" Text="{Binding clt_code, Mode=OneWay}"/>') {
      # Replace with ComboBox
  $null = $newLines.Add('    <ComboBox Grid.Row="1" Grid.Column="1"')
        $null = $newLines.Add(' SelectedValuePath="clt_code"')
        $null = $newLines.Add('           DisplayMemberPath="clt_code"')
    $null = $newLines.Add('       SelectedValue="{Binding clt_code, Mode=TwoWay}"')
        $null = $newLines.Add('            ItemsSource="{Binding DataContext.Clients, RelativeSource={RelativeSource AncestorType=Window}}"')
        $null = $newLines.Add('   Margin="0,3,0,3"')
        $null = $newLines.Add('  Background="WhiteSmoke"')
      $null = $newLines.Add('    VerticalAlignment="Center"/>')
    }
    else {
        $null = $newLines.Add($line)
    }
}

# Write back preserving original encoding and line endings
$encoding = [System.Text.Encoding]::UTF8
[System.IO.File]::WriteAllLines($filePath, $newLines, $encoding)
Write-Host "Successfully replaced Client Code TextBox with ComboBox"
