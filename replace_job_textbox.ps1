$filePath = "AiWpf\Views\MainWindow.xaml"
$lines = @(Get-Content $filePath)
$newLines = [System.Collections.ArrayList]::new()

for ($i = 0; $i -lt $lines.Count; $i++) {
    $line = $lines[$i]
    
    # Look for the exact line with Job Name TextBox in details panel (Grid.Row="3")
    if ($line.Trim() -eq '<TextBox Grid.Row="3" Grid.Column="1" Text="{Binding job_name, Mode=OneWay}"/>') {
        # Replace with ComboBox
      $null = $newLines.Add('<ComboBox Grid.Row="3" Grid.Column="1"')
        $null = $newLines.Add('  SelectedValuePath="job_id"')
        $null = $newLines.Add('      DisplayMemberPath="job_name"')
  $null = $newLines.Add('        SelectedValue="{Binding job_id, Mode=TwoWay}"')
        $null = $newLines.Add('        ItemsSource="{Binding DataContext.Jobs, RelativeSource={RelativeSource AncestorType=Window}}"')
     $null = $newLines.Add('        Margin="0,3,0,3"')
    $null = $newLines.Add('        Background="WhiteSmoke"')
        $null = $newLines.Add('        VerticalAlignment="Center"/>')
    }
  else {
  $null = $newLines.Add($line)
    }
}

# Write back preserving original encoding and line endings
$encoding = [System.Text.Encoding]::UTF8
[System.IO.File]::WriteAllLines($filePath, $newLines, $encoding)
Write-Host "Successfully replaced Job Name TextBox with ComboBox"
