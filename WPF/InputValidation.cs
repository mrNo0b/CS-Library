/// <summary> Allow only digits and '.' but not as first character </summary>
/// <param name="sender">TextBox</param>
/// <param name="e">Key info</param>
private void DigitsAndDotOnlyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
{
	// allow ctrl + a
	if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.A) { return; }

	// allow arrow keys
	if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right) { return; }

	// do not allow Ctrl, Shift or Alt keys
	if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ||
		(Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control ||
		(Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
	{ e.Handled = true; return; }

	// if key is dot allow it only once and not as first char
	if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
	{
		TextBox tb = (TextBox)sender;
		tb.Text = tb.Text.Trim();
		
		if (tb.CaretIndex == 0 || tb.Text.Contains(".")) { e.Handled = true; }

		return;
	}

	// allow only digits 
	if ((e.Key < Key.D0 || e.Key > Key.D9) &&
		(e.Key < Key.NumPad0 || e.Key > Key.NumPad9) &&
		e.Key != Key.Delete && e.Key != Key.Back)
	{ e.Handled = true; }
}
